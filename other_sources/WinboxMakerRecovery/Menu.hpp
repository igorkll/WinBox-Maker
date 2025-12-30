#pragma once
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

class Menu_menu
{
public:
    std::vector<std::string> menuEntries;
    int selected = 0;
};

void Menu_start(HINSTANCE hInstance);