#include "Menu.hpp"
#include "json.hpp"
#include "Brain.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

//сорян ребят, один из первых опытов в C++
//тут особо не чистил память ибо мне пока сложно работать с new и я не совсем понимаю местные правила работы с памятью
//с "C" знаком через embedded разработку под esp32, но в плюсах все как то странно
//однако выделяется тут настолько мало что плевать
//но утечки тут в коде есть, хоть ли вообще не критически для проги, все пашет

using json = nlohmann::json;

// ---------------------------------------------------------

static const int flashWithoutFactoryReset = 0;
static const int flashWithFactoryReset = 1;
static const int flashFactoryResetQuestion = 2;

static const int flashCancelUnlockCode = 0;
static const int flashAcceptUnlockCode = 1;

static Menu_menu mainMenu;

static void entry_menu_unlock(void* _unlockCode) {
    int unlockCode = *((int*)_unlockCode);
    Menu_unlock(unlockCode);
}

static void entry_flash_firmware(void* _factoryResetMode) {
    Firmware* firmware = Brain_getManualFlashFirmware();
    if (!firmware) {
        Menu_message(std::string("Couldn't find the firmware on the external drive\n") + "");
        Menu_select(&mainMenu);
        return;
    }

    bool factoryResetMode = *((int*)_factoryResetMode);
    bool trySaveData = factoryResetMode == flashWithoutFactoryReset;

    if (factoryResetMode == flashFactoryResetQuestion) {
        Menu_menu* factoryResetSelectMenu = new Menu_menu();
        factoryResetSelectMenu->addMenuEntry_callback("Flash without factory reset", entry_menu_unlock, (void*)&flashWithoutFactoryReset);
        factoryResetSelectMenu->addMenuEntry_callback("Flash with factory reset", entry_menu_unlock, (void*)&flashWithFactoryReset);
        factoryResetSelectMenu->addMenuEntry_callback("Cancel", entry_menu_unlock, (void*)&flashFactoryResetQuestion);
        Menu_select(factoryResetSelectMenu);

        int factoryResetMode = Menu_lock();
        trySaveData = factoryResetMode == flashWithoutFactoryReset;
        delete factoryResetSelectMenu;

        if (factoryResetMode == flashFactoryResetQuestion) {
            delete firmware;
            return;
        }
    }

    Menu_menu* acceptMenu = new Menu_menu();
    std::string flashAcceptStr = std::string("Flash ") + firmware->path + (trySaveData ? " with save data" : " with factory reset");
    acceptMenu->addMenuEntry_noNoNoYesNo_callback(flashAcceptStr, entry_menu_unlock, (void*)&flashAcceptUnlockCode);
    acceptMenu->addMenuEntry_callback("Cancel flashing", entry_menu_unlock, (void*)&flashCancelUnlockCode);
    Menu_select(acceptMenu);

    if (Menu_lock() == 1) {
        delete acceptMenu;
        if (trySaveData) {
            Menu_message("The device's firmware has been updated\nThe data has been saved");
        }
        else {
            Menu_message("The device's firmware has been updated\nDevice settings have been reset");
        }
    } else {
        delete acceptMenu;
    }
    Menu_select(&mainMenu);

    delete firmware;
}

static void entry_factory_reset(void* _) {
    Brain_factoryReset();
    Menu_message("Device settings have been reset");
    Menu_select(&mainMenu);
}

static void entry_system_info(void* _) {
    Menu_message(Brain_inputData.value("textOnInfoPage", ""));
}

static void entry_reboot_to_system(void* _) {
    ExitProcess(0);
}

static void loadRecoveryMenu(HINSTANCE hInstance) {
    if (!Brain_inputData.value("allowMenu", false)) return;

    if (Brain_inputData.value("allowFactoryReset", false)) {
        mainMenu.addMenuEntry_noNoNoYesNo_callback("Factory reset", entry_factory_reset);
    }

    bool allowFlashWithoutFactoryReset = Brain_inputData.value("allowFlashWithoutFactoryReset", false);
    bool allowFlashWithFactoryReset = Brain_inputData.value("allowFlashWithFactoryReset", false);
    if (Brain_inputData.value("allowManualFlash", false) && (allowFlashWithoutFactoryReset || allowFlashWithFactoryReset)) {
        if (allowFlashWithoutFactoryReset && allowFlashWithFactoryReset) {
            mainMenu.addMenuEntry_callback("Flash firmware from external drive", entry_flash_firmware, (void*)&flashFactoryResetQuestion);
        } else if (allowFlashWithoutFactoryReset) {
            mainMenu.addMenuEntry_callback("Flash firmware from external drive", entry_flash_firmware, (void*)&flashWithoutFactoryReset);
        } else if (allowFlashWithFactoryReset) {
            mainMenu.addMenuEntry_callback("Flash firmware from external drive", entry_flash_firmware, (void*)&flashWithFactoryReset);
        }
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

    if (Brain_inputData.value("allowAutoFlash", false)) {
        Firmware* autoFlashFirmware = Brain_getAutoFlashFirmware();
        if (autoFlashFirmware) {
            Menu_init(hInstance);

            FlashQuietMode flashQuietMode = (FlashQuietMode)Brain_inputData.value("autoFlashQuietMode", FlashQuietMode_BlackScreen);

            //если автоматическая прошивка прошла успешно - перезагрузка в систему
            if (Brain_flashFirmware(autoFlashFirmware, flashQuietMode, true)) return 0;

            //если автоматическая прошивка завершилась с ошибкой
            //но стоит режим автоматической прошивка как скрытый или только логотип, все равно перезагружаемся в систему
            if (flashQuietMode != FlashQuietMode_DontHide) return 0;

            //если автоматическая прошивка проходит не в скрытом режиме, показываем ошибку и после нажатия enter пускаем в recovery
            //но если recovery меню отключено полностью то все равно произойдет перезагрузка
            std::string err = Brain_getFlashError();
            if (err.size() > 0) Menu_message(err);
        }
        delete autoFlashFirmware;
    }

    loadRecoveryMenu(hInstance);
    return 0;
}
