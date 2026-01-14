#include "Menu.hpp"
#include "json.hpp"
#include "Brain.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

using json = nlohmann::json;

// ---------------------------------------------------------

static Menu_menu mainMenu;

static void entry_factory_reset(void* _) {
    Menu_select(&mainMenu);
}

static void entry_system_info(void* _) {
    Menu_select(&mainMenu);
}

static void entry_reboot_to_system(void* _) {
    PostQuitMessage(0);
}

static void loadRecoveryMenu(HINSTANCE hInstance) {
    if (!Brain_inputData.value("allowMenu", false)) return;

    if (Brain_inputData.value("allowFactoryReset", false)) {
        mainMenu.addMenuEntry_noNoNoYesNo_callback("Factory reset", entry_factory_reset);
    }

    if (Brain_inputData.value("textOnInfoPage_en", false)) {
        mainMenu.addMenuEntry_callback("System info", entry_system_info);
    }

    mainMenu.addMenuEntry_callback("Reboot to the system now", entry_reboot_to_system);

    Menu_select(&mainMenu);
    Menu_start(hInstance);
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE, LPSTR, int) {
    Brain_load();

    Firmware* autoFlashFirmware = Brain_getAutoFlashFirmware();
    if (autoFlashFirmware) {

    }

    loadRecoveryMenu(hInstance);
    return 0;
}
