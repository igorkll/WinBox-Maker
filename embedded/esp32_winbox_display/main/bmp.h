#include <stdint.h>
#include <stdbool.h>
#include <stdio.h>
#include <stddef.h>

typedef struct {
    uint16_t width;
    uint16_t height;
    uint8_t bits;
} ImageInfo;

ImageInfo bmp_readImageInfo(const char* path);
ImageInfo bmp_draw(const char* path, int x, int y);