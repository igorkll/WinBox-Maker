#include "bmp.h"
#include "hal.h"

#pragma pack(push, 1)

typedef struct {
    char bfTypeB;
    char bfTypeM;
    int32_t bfSize;
    int16_t bfReserved1;
    int16_t bfReserved2;
    int32_t bfOffBits;
} BITMAPFILEHEADER_struct;

typedef struct {
    uint16_t bcWidth;
    uint16_t bcHeight;
    uint16_t bcPlanes;
    uint16_t bcBitCount;
} BITMAPCOREHEADER_struct;

typedef struct {
    int32_t biWidth;
    int32_t biHeight;
    uint16_t biPlanes;
    uint16_t biBitCount;
    uint32_t biCompression;
    uint32_t biSizeImage;
    int32_t biXPelsPerMeter;
    int32_t biYPelsPerMeter;
    uint32_t biClrUsed;
    uint32_t biClrImportant;
} BITMAPINFOHEADER_struct;

typedef struct {
    int32_t biWidth;
    int32_t biHeight;
    uint16_t biPlanes;
    uint16_t biBitCount;
    uint32_t biCompression;
    uint32_t biSizeImage;
    int32_t biXPelsPerMeter;
    int32_t biYPelsPerMeter;
    uint32_t biClrUsed;
    uint32_t biClrImportant;
    uint32_t bV4RedMask;
    uint32_t bV4GreenMask;
    uint32_t bV4BlueMask;
    uint32_t bV4AlphaMask;
    uint32_t bV4CSType;
    uint32_t stub1;
    uint32_t stub2;
    uint32_t stub3;
    uint32_t stub4;
    uint32_t stub5;
    uint32_t stub6;
    uint32_t stub7;
    uint32_t stub8;
    uint32_t stub9;
    uint32_t bV4GammaRed;
    uint32_t bV4GammaGreen;
    uint32_t bV4GammaBlue;
} BITMAPV4HEADER_struct;

typedef struct {
    int32_t biWidth;
    int32_t biHeight;
    uint16_t biPlanes;
    uint16_t biBitCount;
    uint32_t biCompression;
    uint32_t biSizeImage;
    int32_t biXPelsPerMeter;
    int32_t biYPelsPerMeter;
    uint32_t biClrUsed;
    uint32_t biClrImportant;
    uint32_t bV4RedMask;
    uint32_t bV4GreenMask;
    uint32_t bV4BlueMask;
    uint32_t bV4AlphaMask;
    uint32_t bV4CSType;
    uint32_t stub1;
    uint32_t stub2;
    uint32_t stub3;
    uint32_t stub4;
    uint32_t stub5;
    uint32_t stub6;
    uint32_t stub7;
    uint32_t stub8;
    uint32_t stub9;
    uint32_t bV4GammaRed;
    uint32_t bV4GammaGreen;
    uint32_t bV4GammaBlue;
    uint32_t bV5Intent;
    uint32_t bV5ProfileData;
    uint32_t bV5ProfileSize;
    uint32_t bV5Reserved;
} BITMAPV5HEADER_struct;

#pragma pack(pop)

static ImageInfo _parse(const char* path, bool pushToDisplay) {
    ImageInfo info = {};

    FILE *file = fopen(path, "rb");
    if (file == NULL) return info;

    // check & read header
    BITMAPFILEHEADER_struct BITMAPFILEHEADER;
    fread(&BITMAPFILEHEADER, 1, sizeof(BITMAPFILEHEADER), file);
    if (BITMAPFILEHEADER.bfTypeB != 'B' || BITMAPFILEHEADER.bfTypeM != 'M') {
        printf("BMP ERROR: invalid bmp signature: %c%c\n", BITMAPFILEHEADER.bfTypeB, BITMAPFILEHEADER.bfTypeM);
        fclose(file);
        return info;
    }

    // read info
    uint32_t bcSize;
    fread(&bcSize, sizeof(uint32_t), 1, file);
    switch (bcSize) {
        case 12 : {
            BITMAPCOREHEADER_struct BITMAPINFO;
            fread(&BITMAPINFO, 1, sizeof(BITMAPINFO), file);
            info.width = BITMAPINFO.bcWidth;
            info.height = BITMAPINFO.bcHeight;
            info.bits = BITMAPINFO.bcBitCount;
            break;
        }

        case 40 : {
            BITMAPINFOHEADER_struct BITMAPINFO;
            fread(&BITMAPINFO, 1, sizeof(BITMAPINFO), file);
            info.width = BITMAPINFO.biWidth;
            info.height = BITMAPINFO.biHeight;
            info.bits = BITMAPINFO.biBitCount;
            break;
        }

        case 108 : {
            BITMAPV4HEADER_struct BITMAPINFO;
            fread(&BITMAPINFO, 1, sizeof(BITMAPINFO), file);
            info.width = BITMAPINFO.biWidth;
            info.height = BITMAPINFO.biHeight;
            info.bits = BITMAPINFO.biBitCount;
            break;
        }

        case 124 : {
            BITMAPV5HEADER_struct BITMAPINFO;
            fread(&BITMAPINFO, 1, sizeof(BITMAPINFO), file);
            info.width = BITMAPINFO.biWidth;
            info.height = BITMAPINFO.biHeight;
            info.bits = BITMAPINFO.biBitCount;
            break;
        }

        default : {
            printf("BMP ERROR: unsupported BITMAPINFO: %li\n", bcSize);
            fclose(file);
            return info;
        }
    }

    info.reverseLines = info.height > 0;
    info.height = abs(info.height);

    if (pushToDisplay) {
        fseek(file, BITMAPFILEHEADER.bfOffBits, SEEK_SET);

        uint16_t* buffer = malloc(DRAW_BUFFER_SIZE * 2);
        uint8_t* bmpBuffer = malloc(BMP_BUFFER_SIZE);
        size_t bufferPos = 0;
        size_t bmpBufferPos = BMP_BUFFER_SIZE;

        uint8_t bmpRead() {
            if (bmpBufferPos >= BMP_BUFFER_SIZE) {
                fread(bmpBuffer, 1, BMP_BUFFER_SIZE, file);
                bmpBufferPos = 0;
            }
            return bmpBuffer[bmpBufferPos++];
        }

        for (int iy = 0; iy < info.height; iy++) {
            for (int ix = 0; ix < info.width; ix++) {
                uint8_t blue = bmpRead();
                uint8_t green = bmpRead();
                uint8_t red = bmpRead();
                uint8_t alpha = 255;
                if (info.bits == 32) {
                    alpha = bmpRead();
                }

                uint32_t color;
                if (alpha > 0) {
                    color = (red << 16) | (green << 8) | blue;
                } else {
                    color = BOOT_BACKGROUND;
                }

                if (bufferPos >= DRAW_BUFFER_SIZE) {
                    hal_display_sendBuffer((uint8_t*)buffer, bufferPos * 2);
                    bufferPos = 0;
                }
                buffer[bufferPos++] = hal_rgb888_to_rgb565(color);
            }
        }
        if (bufferPos > 0) {
            hal_display_sendBuffer((uint8_t*)buffer, bufferPos * 2);
            bufferPos = 0;
        }
        free(buffer);
        free(bmpBuffer);
    }

    fclose(file);
    return info;
}

ImageInfo bmp_readImageInfo(const char* path) {
    return _parse(path, false);
}

ImageInfo bmp_draw(const char* path) {
    return _parse(path, true);
}