#pragma once
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

extern std::string Brain_windowsDrive;
extern std::string Brain_sysDrive;
extern json Brain_inputData;

void Brain_load();