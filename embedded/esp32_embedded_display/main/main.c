#include <stdint.h>
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <string.h>
#include <driver/uart.h>
#include "hal.h"
#include "bmp.h"

#define LOGO_PATH "/storage/logo.bmp"
#define UART_NUM UART_NUM_0
#define BUF_SIZE 1024

static hal_touchscreen_point touchInfo[TOUCHSCREEN_MAXTOUCH];

static void uart_init() {
    const uart_config_t uart_config = {
        .baud_rate = 115200,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
    };

    ESP_ERROR_CHECK(uart_driver_install(UART_NUM, BUF_SIZE, BUF_SIZE, 0, NULL, 0));
    ESP_ERROR_CHECK(uart_param_config(UART_NUM, &uart_config));
    ESP_ERROR_CHECK(uart_set_pin(UART_NUM, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE));
}

static void sendTouch(char prefix, hal_touchscreen_point touchPoint) {
    char buffer[64];
    sprintf(buffer, "%c:%i:%i\n", prefix, touchPoint.x, touchPoint.y);
    uart_write_bytes(UART_NUM, buffer, strlen(buffer));
}

void _main() {
    if (!BOOT_DISABLE_LOGO) {
        ImageInfo imageInfo = bmp_readImageInfo(LOGO_PATH);
        int offsetX = (DISPLAY_WIDTH / 2) - (imageInfo.width / 2);
        int offsetY = (DISPLAY_HEIGHT / 2) - (imageInfo.height / 2);
        if (imageInfo.reverseLines) hal_display_sendReverse(true);
        hal_display_sendSelect(offsetX, offsetY, imageInfo.width, imageInfo.height);
        bmp_draw(LOGO_PATH);
        if (imageInfo.reverseLines) hal_display_sendReverse(false);
        hal_display_sendSelectAll();
    }
    hal_display_backlight(true);

    uart_init();
    
    while (true) {
        uint8_t touchCount = hal_touchscreen_touchCount();
        if (touchCount > TOUCHSCREEN_MAXTOUCH) touchCount = TOUCHSCREEN_MAXTOUCH;
        for (size_t i = 0; i < TOUCHSCREEN_MAXTOUCH; i++) {
            hal_touchscreen_point touchPoint = {};
            if (i < touchCount) touchPoint = hal_touchscreen_getPoint(i);
            hal_touchscreen_point oldTouchPoint = touchInfo[i];
            if (touchPoint.z > 0 && oldTouchPoint.z == 0) {
                sendTouch('P', touchPoint);
            } else if (touchPoint.z == 0 && oldTouchPoint.z > 0) {
                sendTouch('R', oldTouchPoint);
            } else if (touchPoint.x != oldTouchPoint.x || touchPoint.y != oldTouchPoint.y) {
                sendTouch('D', touchPoint);
            }
            touchInfo[i] = touchPoint;
        }
    }
}