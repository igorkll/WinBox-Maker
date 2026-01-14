#include "Brain.hpp"

std::string Brain_sysDrive;
std::string Brain_windowsDrive;
json Brain_inputData;

static std::string GetWinPeDrive() {
    char driveCStr[MAX_PATH];
    GetEnvironmentVariableA("SystemDrive", driveCStr, MAX_PATH);
    return std::string(driveCStr);
}

static std::string FindWindowsDrive() {
    char drives[512];
    DWORD len = GetLogicalDriveStringsA(sizeof(drives), drives);

    for (size_t i = 0; i < len; i += strlen(&drives[i]) + 1) {
        std::string path = std::string(&drives[i]) + "Windows\\System32\\config\\SYSTEM";
        if (GetFileAttributesA(path.c_str()) != INVALID_FILE_ATTRIBUTES) {
            drives[strlen(&drives[i]) - 1] = '\0';
            return std::string(&drives[i]);
        }
    }
    return "";
}

void Brain_load() {
    Brain_sysDrive = GetWinPeDrive();
    Brain_windowsDrive = FindWindowsDrive();

    std::ifstream inFile(Brain_sysDrive + "\\WinboxMakerRecovery\\settings.json");
    if (inFile) {
        Brain_inputData = json::parse(inFile);
    }
}

static Firmware* getFirmwareAtDrive(std::string drive) {
    Firmware* firmware = new Firmware;
    return firmware;
}

Firmware* Brain_getAutoFlashFirmware() {
    return getFirmwareAtDrive(Brain_windowsDrive);
}

Firmware* Brain_getManualFlashFirmware() {
    return nullptr;
}