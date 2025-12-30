#pragma once
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

typedef void (*Menu_callback)();

class Menu_menu
{
public:
    std::vector<std::string> menuEntriesNames;
    std::vector<Menu_callback> menuEntries;
    int selected = 0;

    void addMenuEntry_submenu() {

    }

    void addMenuEntry_callback(std::string name, Menu_callback callback) {
        menuEntriesNames.push_back(name);
        menuEntries.push_back(callback);
    }
};

void Menu_select(Menu_menu* menu);
void Menu_start(HINSTANCE hInstance);