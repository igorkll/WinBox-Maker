#include "Brain.hpp"
#include "Menu.hpp"

std::string Brain_sysDrive;
std::string Brain_windowsDrive;
json Brain_inputData;

static std::string firmwareFlashError = "";

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

static std::string getFirmwarePathAtDrive(std::string drive, std::string firmwareName) {
    std::string path = drive + "\\" + firmwareName;
    if (GetFileAttributesA(path.c_str()) != INVALID_FILE_ATTRIBUTES) {
        return path;
    }
    return "";
}

static Firmware* getFirmwareAtDrive(std::string drive) {
    FirmwareType firmwareType;
    std::string path = "";

    if (Brain_inputData.value("allowFlashWim", false)) {
        firmwareType = FirmwareType_wim;
        path = getFirmwarePathAtDrive(drive, Brain_inputData.value("wimName", "firmware.wim"));
    }

    if (path.size() == 0 && Brain_inputData.value("allowFlashImg", false)) {
        firmwareType = FirmwareType_img;
        path = getFirmwarePathAtDrive(drive, Brain_inputData.value("imgName", "firmware.img"));
    }

    if (path.size() == 0 && Brain_inputData.value("allowFlashFfu", false)) {
        firmwareType = FirmwareType_ffu;
        path = getFirmwarePathAtDrive(drive, Brain_inputData.value("ffuName", "firmware.ffu"));
    }

    if (path.size() != 0) {
        Firmware* firmware = new Firmware;
        firmware->firmwareType = firmwareType;
        firmware->path = path;
        return firmware;
    }
    return nullptr;
}

Firmware* Brain_getAutoFlashFirmware() {
    return getFirmwareAtDrive(Brain_windowsDrive);
}

Firmware* Brain_getManualFlashFirmware() {
    char drives[512];
    DWORD len = GetLogicalDriveStringsA(sizeof(drives), drives);

    for (size_t i = 0; i < len; i += strlen(&drives[i]) + 1) {
        std::string drive = &drives[i];
        if (!drive.empty()) drive.pop_back();
        if (drive != Brain_windowsDrive && drive != Brain_sysDrive) {
            Firmware* firmware = getFirmwareAtDrive(drive);
            if (firmware) return firmware;
        }
    }

    return nullptr;
}

static std::wstring NormalizePath(const std::wstring& raw) {
    std::wstring p = raw;

    std::replace(p.begin(), p.end(), L'/', L'\\');

    while (!p.empty() && p[0] == L'\\')
    p.erase(p.begin());

    if (p.find(L"..") != std::wstring::npos)
        return L"";

    std::wstring windowsDrive(Brain_windowsDrive.begin(), Brain_windowsDrive.end());
    return windowsDrive + L"\\" + p;
}

void Brain_deletePath(const std::wstring& path) {
    DWORD attr = GetFileAttributesW(path.c_str());
    if (attr == INVALID_FILE_ATTRIBUTES)
        return;

    if (attr & FILE_ATTRIBUTE_DIRECTORY) {
        WIN32_FIND_DATAW fd;
        std::wstring searchPath = path + L"\\*";
        HANDLE hFind = FindFirstFileW(searchPath.c_str(), &fd);
        if (hFind != INVALID_HANDLE_VALUE) {
            do {
                std::wstring name = fd.cFileName;
                if (name != L"." && name != L"..") {
                    Brain_deletePath(path + L"\\" + name);
                }
            } while (FindNextFileW(hFind, &fd));
            FindClose(hFind);
        }
        RemoveDirectoryW(path.c_str());
    }
    else {
        DeleteFileW(path.c_str());
    }
}

void Brain_factoryReset() {
    std::string dataPaths = Brain_inputData["dataPaths"];
    std::wstring dataPathsW(dataPaths.begin(), dataPaths.end());
    auto lines = Brain_splitLines(dataPathsW);

    for (const auto& l : lines) {
        std::wstring path = NormalizePath(l);

        if (path.empty())
            continue;

        if (path.rfind(L"C:\\", 0) != 0)
            continue;

        Brain_deletePath(path);
    }
}

bool Brain_flashFirmware(Firmware* firmware, FlashQuietMode flashQuietMode, bool trySaveData) {
    firmwareFlashError = "ASD\nqwe";

    return false;
}

std::string Brain_getFlashError() {
    return firmwareFlashError;
}
