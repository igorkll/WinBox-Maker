#include "Menu.hpp"
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

using json = nlohmann::json;

static std::string sysDrive;
static json inputData;

static void loadConsts() {
    char sysDriveCStr[MAX_PATH];
    GetEnvironmentVariableA("SystemDrive", sysDriveCStr, MAX_PATH);
    sysDrive = std::string(sysDriveCStr);

    std::ifstream inFile(sysDrive + "\\WinboxMakerRecovery\\settings.json");
    if (inFile) {
        inFile >> inputData;
    }
}

// ---------------------------------------------------------

static Menu_menu mainMenu;

static void entry_factory_reset(void* _) {
    Menu_select(&mainMenu);
}

static void entry_reboot_to_system(void* _) {
    PostQuitMessage(0);
}

static void entry_system_info(void* _) {
    Menu_select(&mainMenu);
}

static void loadRecoveryMenu(HINSTANCE hInstance) {
    if (!inputData.value("allowMenu", false)) return;

    if (inputData.value("allowFactoryReset", false)) {
        mainMenu.addMenuEntry_noNoNoYesNo_callback("Factory reset", entry_factory_reset);
    }

    if (inputData.value("textOnInfoPage_en", false)) {
        mainMenu.addMenuEntry_noNoNoYesNo_callback("System info", entry_system_info);
    }

    mainMenu.addMenuEntry_callback("Reboot to the system now", entry_reboot_to_system);

    Menu_select(&mainMenu);
    Menu_start(hInstance);
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE, LPSTR, int) {
    loadConsts();

    loadRecoveryMenu(hInstance);
    return 0;
}
