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

void uart_init() {
    const uart_config_t uart_config = {
        .baud_rate = 5000000,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
    };

    ESP_ERROR_CHECK(uart_driver_install(UART_NUM, BUF_SIZE, BUF_SIZE, 0, NULL, 0));
    ESP_ERROR_CHECK(uart_param_config(UART_NUM, &uart_config));
    ESP_ERROR_CHECK(uart_set_pin(UART_NUM, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE));
}

void _main() {
    hal_display_backlight(true);
    if (!BOOT_DISABLE_LOGO) {
        ImageInfo imageInfo = bmp_readImageInfo(LOGO_PATH);
        int offsetX = 0;
        int offsetY = 0;
        if (!BOOT_DISABLE_LOGO_CENTERING) {
            offsetX = (DISPLAY_WIDTH / 2) - (imageInfo.width / 2);
            offsetY = (DISPLAY_HEIGHT / 2) - (imageInfo.height / 2);
        }
        hal_display_sendSelect(offsetX, offsetY, imageInfo.width, imageInfo.height);
        bmp_draw(LOGO_PATH);
        hal_display_sendSelectAll();
    }
    hal_display_backlight(true);

    uart_init();
    uart_write_bytes(UART_NUM, "hello", 6);
    

}