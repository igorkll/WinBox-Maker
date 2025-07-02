#define BOOT_BACKGROUND 0xff00ff
#define BOOT_DISABLE_LOGO false
#define BOOT_DISABLE_LOGO_CENTERING false

// ---------------------------------------------- display settings

#define DISPLAY_FREQ 80000000
#define DISPLAY_HOST SPI2_HOST
#define DISPLAY_MISO 12 //optional
#define DISPLAY_MOSI 13
#define DISPLAY_CLK  14
#define DISPLAY_DC   21
#define DISPLAY_CS   22 //optional
#define DISPLAY_RST  33 //optional. comment if you connected this pin to the microcontroller RST
#define DISPLAY_BL   4  //optional

#define DISPLAY_WIDTH   480
#define DISPLAY_HEIGHT  320

#define DISPLAY_SWAP_ENDIAN  true
#define DISPLAY_SWAP_RGB     true
#define DISPLAY_INVERT       true
#define DISPLAY_INVERT_BL    false

#define DISPLAY_FLIP_X    true
#define DISPLAY_FLIP_Y    false
#define DISPLAY_SWAP_XY   false
#define DISPLAY_ROTATION  1
#define DISPLAY_OFFSET_X  0
#define DISPLAY_OFFSET_Y  0

// ---------------------------------------------- touchscreen settings

#define TOUCHSCREEN_FT6336U
#define TOUCHSCREEN_SDA   5
#define TOUCHSCREEN_SCL   27
#define TOUCHSCREEN_HOST  I2C_NUM_0
#define TOUCHSCREEN_ADDR  0x38
#define TOUCHSCREEN_RST   23 //optional

#define TOUCHSCREEN_WIDTH   320 //required parameters. the width and height of the touchscreen in pixels
#define TOUCHSCREEN_HEIGHT  480

#define TOUCHSCREEN_MUL_X 1
#define TOUCHSCREEN_MUL_Y 1

#define TOUCHSCREEN_FLIP_X    false
#define TOUCHSCREEN_FLIP_Y    false
#define TOUCHSCREEN_SWAP_XY   false
#define TOUCHSCREEN_ROTATION  1
#define TOUCHSCREEN_OFFSET_X  0
#define TOUCHSCREEN_OFFSET_Y  0