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

typedef enum {
	FlashQuietMode_DontHide,
	FlashQuietMode_OnlyLogo,
	FlashQuietMode_BlackScreen
} FlashQuietMode;

typedef struct {
	std::string path;
	FirmwareType firmwareType;
} Firmware;

template <typename StringType>
std::vector<StringType> Brain_splitLines(const StringType& s) {
    std::vector<StringType> lines;
    size_t start = 0, end;

    while ((end = s.find(typename StringType::value_type('\n'), start)) != StringType::npos) {
        lines.push_back(s.substr(start, end - start));
        start = end + 1;
    }

    if (start <= s.size())
        lines.push_back(s.substr(start));

    return lines;
}

void Brain_deletePath(const std::wstring& path);

void Brain_load();
Firmware* Brain_getAutoFlashFirmware();
Firmware* Brain_getManualFlashFirmware();

void Brain_factoryReset();
bool Brain_flashFirmware(Firmware* firmware, FlashQuietMode flashQuietMode, bool trySaveData);
std::string Brain_getFlashError();