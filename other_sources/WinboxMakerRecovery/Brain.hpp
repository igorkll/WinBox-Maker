#pragma once
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

using json = nlohmann::json;

extern std::string Brain_sysDrive;
extern std::string Brain_windowsDrive;
extern json Brain_inputData;

typedef enum {
	FirmwareType_wim,
	FirmwareType_img,
	FirmwareType_ffu
} FirmwareType;

typedef struct {
	std::string path;
	FirmwareType firmwareType;
} Firmware;

void Brain_load();
Firmware* Brain_getAutoFlashFirmware();
Firmware* Brain_getManualFlashFirmware();