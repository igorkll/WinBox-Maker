#pragma once
#include <stdint.h>
#include <esp_log.h>
#include <driver/gpio.h>
#include <freertos/FreeRTOS.h>
#include <freertos/task.h>
#include <freertos/event_groups.h>
#include "config.h"

// ---------------------------------------------- display

void hal_display_backlight(bool state);
void hal_display_sendSelect(uint16_t x, uint16_t y, uint16_t width, uint16_t height);
void hal_display_sendReverse(bool reverse);
void hal_display_sendSelectAll();
void hal_display_sendBuffer(const uint8_t* data, size_t size);

// ---------------------------------------------- touchscreen

typedef struct {
    int x;
    int y;
    float z;
} hal_touchscreen_point;

uint8_t hal_touchscreen_touchCount();
hal_touchscreen_point hal_touchscreen_getPoint(uint8_t index);

// ----------------------------------------------

uint16_t hal_rgb888_to_rgb565(uint32_t rgb888);
void hal_delay(size_t time);