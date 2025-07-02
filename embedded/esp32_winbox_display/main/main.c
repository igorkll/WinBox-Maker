#include <stdint.h>
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <string.h>
#include "hal.h"

void _main() {
    int a = 0;
    hal_display_backlight(true);
    while (true) {
        hal_display_sendBuffer(&a, 1024);
    }
}