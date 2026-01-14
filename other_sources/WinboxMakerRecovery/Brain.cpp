#include "Brain.hpp"

using json = nlohmann::json;

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
    GetLogicalDriveStringsA(sizeof(drives), drives);

    for (char* d = drives; *d; d += strlen(d) + 1) {
        std::string path = std::string(d) + "Windows\\System32\\config\\SYSTEM";
        if (GetFileAttributesA(path.c_str()) != INVALID_FILE_ATTRIBUTES) {
            return std::string(d);
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