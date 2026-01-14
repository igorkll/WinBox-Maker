#pragma once
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

using json = nlohmann::json;

extern std::string Brain_sysDrive;
extern std::string Brain_windowsDrive;
extern json Brain_inputData;

void Brain_load();