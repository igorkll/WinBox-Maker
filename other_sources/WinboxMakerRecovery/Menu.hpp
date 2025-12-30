#pragma once
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

typedef void (*Menu_callback)(void* arg);

static void entry_change_menu(void* _menu) {
    Menu_menu* menu = (Menu_menu*)_menu;
    Menu_select(menu);
}

class Menu_menu
{
public:
    std::vector<std::string> menuEntriesNames;
    std::vector<Menu_callback> menuEntriesCallbacks;
    std::vector<void*> menuEntriesArgs;
    int selected = 0;

    void addMenuEntry_submenu(std::string name, Menu_menu* menu) {
        addMenuEntry_callback(name, entry_change_menu, (void*)menu);
    }

    void addMenuEntry_callback(std::string name, Menu_callback callback, void* arg=nullptr) {
        menuEntriesNames.push_back(name);
        menuEntriesCallbacks.push_back(callback);
        menuEntriesArgs.push_back(arg);
    }
};

void Menu_select(Menu_menu* menu);
void Menu_start(HINSTANCE hInstance);