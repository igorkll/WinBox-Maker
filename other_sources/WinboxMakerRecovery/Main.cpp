#include "Menu.hpp"
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

using json = nlohmann::json;

static std::string sysDrive;

static void loadConsts() {
    char sysDriveCStr[MAX_PATH];
    GetEnvironmentVariableA("SystemDrive", sysDriveCStr, MAX_PATH);
    sysDrive = std::string(sysDriveCStr);

    std::ifstream inFile(sysDrive + "\\WinboxMakerRecovery\\settings.json");
    if (inFile) {
        json j;
        inFile >> j;

        
    }
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE, LPSTR, int) {
    loadConsts();
    Menu_start(hInstance);
    return 0;
}
