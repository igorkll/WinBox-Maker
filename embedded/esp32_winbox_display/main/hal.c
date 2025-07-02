#include <driver/spi_master.h>
#include <esp_heap_caps.h>
#include <esp_vfs.h>
#include <esp_vfs_fat.h>
#include <driver/gptimer.h>
#include <freertos/semphr.h>
#include <esp_timer.h>
#include <esp_random.h>
#include <esp_lcd_io_spi.h>
#include <esp_lcd_panel_io.h>
#include <string.h>
#include <math.h>
#include <esp_system.h>
#include <nvs_flash.h>
#include "hal.h"
#include "main.h"

const char* HAL_LOG_TAG = "esp32_winbox_display_hal";

// ---------------------------------------------- display

#define BYTES_PER_COLOR 2

static spi_device_handle_t display;

typedef struct {
	uint8_t cmd;
	uint8_t data[16];
	uint8_t datalen;
	int16_t delay; //-1 = end of commands
} _command;

typedef struct {
	_command list[8];
	size_t count;
} _commandList;

typedef struct {
    bool state;
} spi_pretransfer_info;

static void _spi_pre_transfer_callback(spi_transaction_t* t) {
    spi_pretransfer_info* pretransfer_info = (spi_pretransfer_info*)t->user;
    gpio_set_level(DISPLAY_DC, pretransfer_info->state);
}

static const _command display_enable = {0x29, {0}, 0, 0};
static const _command display_invert = {0x21, {0}, 0, 0};

static const _command display_init[] = {
	/* rgb 565 - big endian */
	{0x3A, {0x05}, 1, 0},
	/* Porch Setting */
	{0xB2, {0x0c, 0x0c, 0x00, 0x33, 0x33}, 5, 0},
	/* Gate Control, Vgh=13.65V, Vgl=-10.43V */
	{0xB7, {0x45}, 1, 0},
	/* VCOM Setting, VCOM=1.175V */
	{0xBB, {0x2B}, 1, 0},
	/* LCM Control, XOR: BGR, MX, MH */
	{0xC0, {0x2C}, 1, 0},
	/* VDV and VRH Command Enable, enable=1 */
	{0xC2, {0x01, 0xff}, 2, 0},
	/* VRH Set, Vap=4.4+... */
	{0xC3, {0x11}, 1, 0},
	/* VDV Set, VDV=0 */
	{0xC4, {0x20}, 1, 0},
	/* Power Control 1, AVDD=6.8V, AVCL=-4.8V, VDDS=2.3V */
	{0xD0, {0xA4, 0xA1}, 1, 0},
	/* Positive Voltage Gamma Control */
	{0xE0, {0xD0, 0x00, 0x05, 0x0E, 0x15, 0x0D, 0x37, 0x43, 0x47, 0x09, 0x15, 0x12, 0x16, 0x19}, 14, 0},
	/* Negative Voltage Gamma Control */
	{0xE1, {0xD0, 0x00, 0x05, 0x0D, 0x0C, 0x06, 0x2D, 0x44, 0x40, 0x0E, 0x1C, 0x18, 0x16, 0x19}, 14, 0},
	/* Sleep Out */
	{0x11, {0}, 0, 100},
	/* Idle mode off */
	{0x38, {0}, 0, -1},
};

// X+ Y+
//#define _ROTATION_0 0
//#define _ROTATION_1 (1<<5) | (1<<6) | (1<<2)
//#define _ROTATION_2 (1<<6) | (1<<7) | (1<<2) | (1<<4)
//#define _ROTATION_3 (1<<5) | (1<<7) | (1<<4)
// Y+ X+
#define _ROTATION_0 (1<<5)
#define _ROTATION_1 (1<<6) | (1<<2)
#define _ROTATION_2 (1<<5) | (1<<6) | (1<<7) | (1<<2) | (1<<4)
#define _ROTATION_3 (1<<7) | (1<<4)
static _command _rotate(uint8_t rotation) {
	uint8_t regvalue = 0;
	switch (rotation) {
		default:
			regvalue = _ROTATION_0;
			break;

		case 1:
			regvalue = _ROTATION_1;
			break;

		case 2:
			regvalue = _ROTATION_2;
			break;

		case 3:
			regvalue = _ROTATION_3;
			break;
	}

	if (DISPLAY_SWAP_RGB) regvalue ^= (1 << 3);
	if (DISPLAY_FLIP_X) {
		regvalue ^= (1 << 6);
		regvalue ^= (1 << 2);
	}
	if (DISPLAY_FLIP_Y) {
		regvalue ^= (1 << 7);
		regvalue ^= (1 << 4);
	}
	if (DISPLAY_SWAP_XY) {
		regvalue ^= (1 << 5);
	}

	return (_command) {0x36, {regvalue}, 1, -1};
}

static _commandList _select(uint16_t x, uint16_t y, uint16_t x2, uint16_t y2) {
	/*
	return (_commandList) {
		.list = {
			{0x2A, {x >> 8, x & 0xff, x2 >> 8, x2 & 0xff}, 4},
			{0x2B, {y >> 8, y & 0xff, y2 >> 8, y2 & 0xff}, 4},
			{0x2C, {0}, 0, -1}
		}
	};
	*/
	return (_commandList) {
		.count = 3,
		.list = {
			{0x2A, {y >> 8, y & 0xff, y2 >> 8, y2 & 0xff}, 4},
			{0x2B, {x >> 8, x & 0xff, x2 >> 8, x2 & 0xff}, 4},
			{0x2C, {0}, 0, -1}
		}
	};
}

static _commandList _selectFrame(uint16_t x, uint16_t y, uint16_t width, uint16_t height) {
	return _select(
		DISPLAY_OFFSET_X + x,
		DISPLAY_OFFSET_Y + y,
		(DISPLAY_OFFSET_X + x + width) - 1,
		(DISPLAY_OFFSET_Y + y + height) - 1
	);
}

static void _sendCommand(const uint8_t cmd) {
    spi_pretransfer_info pre_transfer_info = {
        .state = false
    };

    spi_transaction_t t = {
        .length = 8,
        .tx_buffer = &cmd,
        .user = (void*)(&pre_transfer_info)
    };

    ESP_ERROR_CHECK(spi_device_transmit(display, &t));
}

static void _sendData(const uint8_t* data, size_t size) {
    spi_pretransfer_info pre_transfer_info = {
        .state = true
    };

    spi_transaction_t t = {
        .length = size * 8,
        .tx_buffer = data,
        .user = (void*)(&pre_transfer_info)
    };
    
    ESP_ERROR_CHECK(spi_device_transmit(display, &t));
}

static bool _doCommand(const _command command) {
    _sendCommand(command.cmd);
    if (command.datalen > 0) _sendData(command.data, command.datalen);

	if (command.delay > 0) {
		vTaskDelay(command.delay / portTICK_PERIOD_MS);
	} else if (command.delay < 0) {
		return true;
	}
	return false;
}

static void _doCommands(const _command* list, size_t count) {
	for (size_t i = 0; i < count; i++) {
		_doCommand(list[i]);
	}
}

static void _doCommandList(const _commandList* list) {
	_doCommands(list->list, list->count);
}

static void _sendSelect(uint16_t x, uint16_t y, uint16_t width, uint16_t height) {
	_commandList list = _selectFrame(x, y, width, height);
	_doCommandList(&list);
}

static void _sendSelectAll() {
	_sendSelect(0, 0, DISPLAY_WIDTH, DISPLAY_HEIGHT);
}

#define CLEAR_BUFFER_SIZE 2048
static uint16_t clear_buffer[CLEAR_BUFFER_SIZE] = {0};
static void _clear() {
	_sendSelectAll();
	for (size_t i = 0; i < (DISPLAY_WIDTH * DISPLAY_HEIGHT * BYTES_PER_COLOR) / sizeof(clear_buffer); i++) {
		_sendData(clear_buffer, sizeof(clear_buffer));
	}
}

static uint16_t swap_endian(uint16_t value) {
    return (value << 8) | (value >> 8);
}

uint16_t rgb888_to_rgb565(uint32_t rgb888) {
    uint8_t r = (rgb888 >> 16) & 0xFF;
    uint8_t g = (rgb888 >> 8) & 0xFF;
    uint8_t b = rgb888 & 0xFF;

    uint16_t r5 = (r >> 3) & 0x1F;
    uint16_t g6 = (g >> 2) & 0x3F;
    uint16_t b5 = (b >> 3) & 0x1F;

    uint16_t out = (r5 << 11) | (g6 << 5) | b5;
    #ifdef DISPLAY_SWAP_ENDIAN
        out = swap_endian(out);
    #endif
    return out;
}

// ----------------------------------------------

static void _initDisplay() {
    uint16_t clear_color = rgb888_to_rgb565(BOOT_BACKGROUND);
    for (size_t i = 0; i < CLEAR_BUFFER_SIZE; i++) {
		clear_buffer[i] = clear_color;
	}

	// ---- init spi bus
	spi_bus_config_t buscfg={
		#ifdef DISPLAY_MISO
			.miso_io_num=DISPLAY_MISO,
		#else
			.miso_io_num=-1,
		#endif
		.mosi_io_num=DISPLAY_MOSI,
		.sclk_io_num=DISPLAY_CLK,
		.quadwp_io_num=-1,
		.quadhd_io_num=-1,
		.max_transfer_sz = DISPLAY_WIDTH * DISPLAY_HEIGHT * BYTES_PER_COLOR
	};
	ESP_ERROR_CHECK(spi_bus_initialize(DISPLAY_HOST, &buscfg, SPI_DMA_CH_AUTO));

	// ---- init spi device
    spi_device_interface_config_t devcfg = {
        .clock_speed_hz = DISPLAY_FREQ,
        .mode = 0,
        #ifdef DISPLAY_CS
			.spics_io_num=DISPLAY_CS,
		#else
			.spics_io_num=-1,
		#endif
        .input_delay_ns = 0,
        .queue_size = 1,
        .pre_cb = _spi_pre_transfer_callback,
        .flags = SPI_DEVICE_NO_DUMMY
    };

    ESP_ERROR_CHECK(spi_bus_add_device(DISPLAY_HOST, &devcfg, &display));

	// ---- init display
	gpio_config_t io_conf = {};
	io_conf.pin_bit_mask |= 1ULL << DISPLAY_DC;
	#ifdef DISPLAY_RST
		io_conf.pin_bit_mask |= 1ULL << DISPLAY_RST;
	#endif
	io_conf.mode = GPIO_MODE_OUTPUT;
	gpio_config(&io_conf);

	#ifdef DISPLAY_RST
		gpio_set_level(DISPLAY_RST, false);
		hal_delay(100);
		gpio_set_level(DISPLAY_RST, true);
		hal_delay(100);
	#endif

	_doCommands(display_init, sizeof(display_init) / sizeof(*display_init));
	#ifdef DISPLAY_INVERT
		_doCommand(display_invert);
	#endif
	_doCommand(_rotate(DISPLAY_ROTATION));
	_clear();
	_doCommand(display_enable);
	_sendSelectAll();
}

void hal_display_backlight(bool state) {
	#ifdef DISPLAY_BL
		gpio_set_level(DISPLAY_BL, DISPLAY_INVERT_BL ? !state : state);
	#endif
}

void hal_display_sendBuffer(const uint8_t* data, size_t size) {
    _sendData(data, size);
}

// ---------------------------------------------- touchscreen

#ifdef TOUCHSCREEN_FT6336U
#include <driver/i2c.h>

static uint8_t i2c_readReg(uint8_t addr) {
    uint8_t val = 0;
    i2c_master_write_read_device(TOUCHSCREEN_HOST, TOUCHSCREEN_ADDR, &addr, 1, &val, 1, 100 / portTICK_PERIOD_MS);
    return val;
}

static int i2c_readDualReg(uint8_t addr) {
    uint8_t read_buf[2];
    read_buf[0] = i2c_readReg(addr);
    read_buf[1] = i2c_readReg(addr + 1);
    return ((read_buf[0] & 0x0f) << 8) | read_buf[1];
}

static void _initTouchscreen() {
	i2c_config_t config = {
		.mode = I2C_MODE_MASTER,
		.sda_io_num = TOUCHSCREEN_SDA,
		.scl_io_num = TOUCHSCREEN_SCL,
		.sda_pullup_en = GPIO_PULLUP_ENABLE,
		.scl_pullup_en = GPIO_PULLUP_ENABLE,
		.master.clk_speed = 400000,
	};

	ESP_ERROR_CHECK(i2c_param_config(TOUCHSCREEN_HOST, &config));
	ESP_ERROR_CHECK(i2c_driver_install(TOUCHSCREEN_HOST, config.mode, 0, 0, 0));

	#ifdef TOUCHSCREEN_RST
		gpio_config_t io_conf = {};
		io_conf.pin_bit_mask |= 1ULL << TOUCHSCREEN_RST;
		io_conf.mode = GPIO_MODE_OUTPUT;
		gpio_config(&io_conf);

		gpio_set_level(TOUCHSCREEN_RST, false);
		vTaskDelay(100 / portTICK_PERIOD_MS);
		gpio_set_level(TOUCHSCREEN_RST, true);
		vTaskDelay(100 / portTICK_PERIOD_MS);
	#endif
}

uint8_t hal_touchscreen_touchCount() {
	return i2c_readReg(0x02) & 0x0F;
}

hal_touchscreen_point hal_touchscreen_getPoint(uint8_t index) {
	float x = 0;
    float y = 0;
    float z = 0;

	switch (index) {
		case 0:
			x = i2c_readDualReg(0x03);
			y = i2c_readDualReg(0x05);
			z = 1;
			break;

		case 1:
			x = i2c_readDualReg(0x09);
			y = i2c_readDualReg(0x0B);
			z = 1;
			break;
	}

    if (TOUCHSCREEN_SWAP_XY) {
        int t = x;
        x = y;
        y = t;
    }

    x *= TOUCHSCREEN_MUL_X;
    y *= TOUCHSCREEN_MUL_Y;
    x += TOUCHSCREEN_OFFSET_X;
    y += TOUCHSCREEN_OFFSET_Y;

    if (x < 0) {
        x = 0;
    } else if (x >= TOUCHSCREEN_WIDTH) {
        x = TOUCHSCREEN_WIDTH - 1;
    }

    if (y < 0) {
        y = 0;
    } else if (y >= TOUCHSCREEN_HEIGHT) {
        y = TOUCHSCREEN_HEIGHT - 1;
    }

    bool flipFlip = TOUCHSCREEN_ROTATION == 2 || TOUCHSCREEN_ROTATION == 3;
    if (TOUCHSCREEN_FLIP_X ^ flipFlip) x = TOUCHSCREEN_WIDTH - 1 - x;
    if (TOUCHSCREEN_FLIP_Y ^ flipFlip) y = TOUCHSCREEN_HEIGHT - 1 - y;

    switch (TOUCHSCREEN_ROTATION) {
        case 1:
        case 3:
            int t = x;
            x = y;
            y = TOUCHSCREEN_WIDTH - t;
            break;
    }

    return (hal_touchscreen_point) {
        .x = x + 0.5,
        .y = y + 0.5,
        .z = z
    };
}
#else
static void _initTouchscreen() {
}

uint8_t hal_touchscreen_touchCount() {
	return 0;
}

hal_touchscreen_point hal_touchscreen_getPoint(uint8_t index) {
	return (hal_touchscreen_point) {.x = 0, .y = 0, .z = 0};
}
#endif

// ---------------------------------------------- filesystem

static void _initFilesystem() {
    static wl_handle_t s_wl_handle = WL_INVALID_HANDLE;
    esp_vfs_fat_mount_config_t fs_config = {
        .max_files = 2,
        .format_if_mount_failed = false,
        .allocation_unit_size = CONFIG_WL_SECTOR_SIZE
    };

    ESP_ERROR_CHECK(esp_vfs_fat_spiflash_mount_rw_wl("/storage", "storage", &fs_config, &s_wl_handle));
}

// ----------------------------------------------

void hal_delay(size_t time) {
    size_t ticks = time / portTICK_PERIOD_MS;
    if (ticks <= 0) ticks = 1;
    vTaskDelay(ticks);
}

void app_main() {
    #ifdef DISPLAY_BL
		gpio_config_t io_conf = {};
		io_conf.pin_bit_mask |= 1ULL << DISPLAY_BL;
		io_conf.mode = GPIO_MODE_OUTPUT;
		gpio_config(&io_conf);
	#endif

	hal_display_backlight(false);

	_initDisplay();
	_initTouchscreen();
	_initFilesystem();

	_main();
}