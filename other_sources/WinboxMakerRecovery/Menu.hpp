#pragma once
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

class Menu_menu;
void Menu_select(Menu_menu* menu);
static void _entry_change_menu(void* _menu);

typedef void (*Menu_callback)(void* arg);

static int _yesPositions[] = { 1, 4, 3 };

class Menu_menu
{
public:
    std::vector<std::string> menuEntriesNames;
    std::vector<Menu_callback> menuEntriesCallbacks;
    std::vector<void*> menuEntriesArgs;
    int selected = 0;
    bool alwaysResetSelect = false;
    std::string titleOverride;

    void addMenuEntry_noNoNoYesNo_callback(std::string name, Menu_callback callback, void* arg = nullptr, int _recurtionCounter = 2, Menu_menu* backTo = nullptr, std::string title = "") {
        if (backTo == nullptr) backTo = this;
        if (title.size() == 0) title = name;

        int recursionIndex = 2 - _recurtionCounter;

        Menu_menu* menu = new Menu_menu();
        menu->titleOverride = title + " (" + std::to_string(recursionIndex + 1) + "/3)";
        menu->alwaysResetSelect = true;

        int yesPosition = _yesPositions[recursionIndex];
        int pointsCount = 6;

        for (int i = 0; i < yesPosition; i++) menu->addMenuEntry_submenu("No", backTo);
        if (_recurtionCounter <= 0) {
            menu->addMenuEntry_callback("Yes", callback, arg);
        }
        else
        {
            menu->addMenuEntry_noNoNoYesNo_callback("Yes", callback, arg, _recurtionCounter - 1, backTo, title);
        }
        for (int i = 0; i < (pointsCount - 1 - yesPosition); i++) menu->addMenuEntry_submenu("No", backTo);

        addMenuEntry_submenu(name, menu);
    }

    void addMenuEntry_submenu(std::string name, Menu_menu* menu) {
        addMenuEntry_callback(name, _entry_change_menu, (void*)menu);
    }

    void addMenuEntry_callback(std::string name, Menu_callback callback, void* arg=nullptr) {
        menuEntriesNames.push_back(name);
        menuEntriesCallbacks.push_back(callback);
        menuEntriesArgs.push_back(arg);
    }
};

static void _entry_change_menu(void* _menu) {
    Menu_menu* menu = (Menu_menu*)_menu;
    if (menu->alwaysResetSelect) menu->selected = 0;
    Menu_select(menu);
}

class Menu_menu;
void Menu_select(Menu_menu* menu);
void Menu_init(HINSTANCE hInstance);
void Menu_start(HINSTANCE hInstance);
void Menu_enableExitLock(bool exitLock);
void Menu_message(std::string text);
void Menu_process();
