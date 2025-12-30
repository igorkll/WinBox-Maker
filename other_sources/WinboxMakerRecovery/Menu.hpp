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

class Menu_menu
{
public:
    std::vector<std::string> menuEntriesNames;
    std::vector<Menu_callback> menuEntriesCallbacks;
    std::vector<void*> menuEntriesArgs;
    int selected = 0;
    std::string titleOverride;

    void addMenuEntry_noNoNoYesNo_callback(std::string name, Menu_callback callback, void* arg = nullptr, int _recurtionCounter = 3, Menu_menu* backTo = nullptr) {
        if (backTo == nullptr) backTo = this;

        Menu_menu menu;
        menu.titleOverride = name + " (" + std::to_string(3 - _recurtionCounter) + "/3)";

        for (int i = 0; i < 4; i++) menu.addMenuEntry_submenu("No", backTo);
        if (_recurtionCounter <= 0) {
            menu.addMenuEntry_callback("Yes", callback, arg);
        }
        else
        {
            menu.addMenuEntry_noNoNoYesNo_callback(_recurtionCounter == 3 ? name : "Yes", callback, arg, _recurtionCounter - 1, backTo);
        }
        for (int i = 0; i < 2; i++) menu.addMenuEntry_submenu("No", backTo);

        addMenuEntry_submenu(name, &menu);
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
    Menu_select(menu);
}

class Menu_menu;
void Menu_select(Menu_menu* menu);
void Menu_start(HINSTANCE hInstance);