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

void hal_delay(size_t time);