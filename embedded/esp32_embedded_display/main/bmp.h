#include <stdint.h>
#include <stdbool.h>
#include <stdio.h>
#include <stddef.h>

//pixels, not bytes
#define DRAW_BUFFER_SIZE (1024 * 16)
//bytes, not pixels
#define BMP_BUFFER_SIZE (1024 * 64)

typedef struct {
    int32_t width;
    int32_t height;
    uint8_t bits;
    bool reverseLines;
} ImageInfo;

ImageInfo bmp_readImageInfo(const char* path);
ImageInfo bmp_draw(const char* path);