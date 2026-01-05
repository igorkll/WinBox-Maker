#include "Menu.hpp"
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>

using json = nlohmann::json;

// ------------------------------------- consts

static COLORREF color_bg = RGB(0, 0, 0);
static COLORREF color_title = RGB(255, 0, 0);
static COLORREF color_text = RGB(255, 255, 255);
static COLORREF color_textShadow = RGB(64, 64, 64);
static COLORREF color_selectedText = RGB(255, 255, 0);
static std::string title_text = "Winbox maker recovery";
static int lineHeight = 100;
static int textShadowWidth = 3;
static int screenWidth;
static int screenHeight;
static std::string sysDrive;

static void loadConsts() {
    char sysDriveCStr[MAX_PATH];
    GetEnvironmentVariableA("SystemDrive", sysDriveCStr, MAX_PATH);
    sysDrive = std::string(sysDriveCStr);

    std::ifstream inFile(sysDrive + "\\WinboxMakerRecovery\\settings.json");
    if (inFile) {
        json j;
        inFile >> j;

        if (j.contains("title")) title_text = j["title"];
    }

    lineHeight = screenHeight / 8;
}

// ------------------------------------- static

static HBRUSH backgroundBrush;
static HFONT titleFont;
static HFONT menuFont;
static HBITMAP menuLogo;

static HFONT createMenuFont(int cHeight) {
    return CreateFontA(cHeight, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
        DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
        DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, "Arial");
}

static void initStaticObjects() {
    backgroundBrush = CreateSolidBrush(color_bg);
    titleFont = createMenuFont(lineHeight * 0.9);
    menuFont = createMenuFont(lineHeight * 0.6);
    menuLogo = (HBITMAP)LoadImageA(nullptr, (sysDrive + "\\WinboxMakerRecovery\\logo.bmp").c_str(), IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
}

// ------------------------------------- vars

static Menu_menu* menu;

// ------------------------------------- code

static void drawCenterizedText(HDC hdc, int y, const std::string& text, int xOffset = 0) {
    RECT rect;
    GetClientRect(WindowFromDC(hdc), &rect);
    rect.top = y;
    rect.bottom = y + lineHeight;
    rect.left = xOffset;
    rect.right = xOffset + rect.right;

    DrawTextA(hdc, text.c_str(), -1, &rect, DT_CENTER | DT_SINGLELINE | DT_VCENTER);
}

static void drawCenterizedTextWithShadow(HDC hdc, int y, const std::string& text, COLORREF color) {
    SetTextColor(hdc, color_textShadow);
    for (int ix = -textShadowWidth; ix <= textShadowWidth; ix += textShadowWidth) {
        for (int iy = -textShadowWidth; iy <= textShadowWidth; iy += textShadowWidth) {
            drawCenterizedText(hdc, y + iy, text, ix);
        }
    }

    SetTextColor(hdc, color);
    drawCenterizedText(hdc, y, text);
}

static void drawLogo(HWND hwnd, HDC hdc, HBITMAP logo) {
    RECT rc;
    GetClientRect(hwnd, &rc);
    int winW = rc.right - rc.left;
    int winH = rc.bottom - rc.top;

    BITMAP bmp;
    GetObject(logo, sizeof(bmp), &bmp);
    int imgW = bmp.bmWidth;
    int imgH = bmp.bmHeight;

    float scale = min((float)winW / imgW, (float)winH / imgH);
    int drawW = (int)(imgW * scale);
    int drawH = (int)(imgH * scale);

    int x = (winW - drawW) / 2;
    int y = (winH - drawH) / 2;

    HDC hMemDC = CreateCompatibleDC(hdc);
    HBITMAP hOld = (HBITMAP)SelectObject(hMemDC, logo);

    StretchBlt(hdc, x, y, drawW, drawH, hMemDC, 0, 0, imgW, imgH, SRCCOPY);

    SelectObject(hMemDC, hOld);
    DeleteDC(hMemDC);
}

static void redrawMenu(HWND hwnd) {
    InvalidateRect(hwnd, nullptr, TRUE);
    
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hwnd, &ps);
    RECT rect;
    GetClientRect(hwnd, &rect);

    SetBkMode(hdc, TRANSPARENT);
    FillRect(hdc, &rect, backgroundBrush);
    drawLogo(hwnd, hdc, menuLogo);

    SelectObject(hdc, titleFont);
    SetTextColor(hdc, color_title);
    if (menu->titleOverride.size() > 0) {
        drawCenterizedText(hdc, 0, menu->titleOverride);
    }
    else
    {
        drawCenterizedText(hdc, 0, title_text);
    }

    SelectObject(hdc, menuFont);
    int y = lineHeight;
    for (size_t i = 0; i < menu->menuEntriesNames.size(); i++) {
        drawCenterizedTextWithShadow(hdc, y, menu->menuEntriesNames[i], i == menu->selected ? color_selectedText : color_text);

        y += lineHeight;
    }

    EndPaint(hwnd, &ps);
}

static void pointerMove(HWND hwnd, bool up) {
    if (up) {
        menu->selected = (menu->selected - 1 + menu->menuEntriesNames.size()) % menu->menuEntriesNames.size();
    }
    else
    {
        menu->selected = (menu->selected + 1) % menu->menuEntriesNames.size();
    }
    redrawMenu(hwnd);
}

static void pointerAccept(HWND hwnd) {
    Menu_callback callback = menu->menuEntriesCallbacks[menu->selected];
    callback(menu->menuEntriesArgs[menu->selected]);
    redrawMenu(hwnd);
}

static void handleKeyboard(HWND hwnd, WPARAM key) {
    switch (key) {
    case VK_UP:
        pointerMove(hwnd, true);
        break;

    case VK_DOWN:
    case VK_VOLUME_DOWN: //volume down - down
        pointerMove(hwnd, false);
        break;
    
    case VK_RETURN:
    case VK_VOLUME_UP: //volume up - accept
        pointerAccept(hwnd);
        break;
    
    case VK_ESCAPE:
        PostQuitMessage(0);
        break;
    }
}

static void handleAppCommand(HWND hwnd, WPARAM lParam) {
    switch (GET_APPCOMMAND_LPARAM(lParam)) {
    case APPCOMMAND_VOLUME_UP: //volume up - accept
        pointerAccept(hwnd);
        break;
    case APPCOMMAND_VOLUME_DOWN: //volume down - down
        pointerMove(hwnd, false);
        break;
    }
}

static void mouseHandle(HWND hwnd, WPARAM lParam) {
    int x = GET_X_LPARAM(lParam);
    int y = GET_Y_LPARAM(lParam);
    int lineIndex = (y / lineHeight) - 1;
    if (lineIndex == menu->selected) {
        pointerAccept(hwnd);
    }
    else if (lineIndex >= 0 && lineIndex < menu->menuEntriesNames.size())
    {
        menu->selected = lineIndex;
        redrawMenu(hwnd);
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
    case WM_KEYDOWN:
        handleKeyboard(hwnd, wParam);
        return 0;
    case WM_PAINT:
        redrawMenu(hwnd);
        return 0;
    case WM_APPCOMMAND:
        handleAppCommand(hwnd, wParam);
        return 0;
    case WM_DESTROY:
        PostQuitMessage(0);
        return 0;
    case WM_LBUTTONDOWN:
        mouseHandle(hwnd, lParam);
        return 0;
    case WM_SYSCOMMAND:
        if ((wParam & 0xFFF0) == SC_CLOSE) { //disable alt+f4. use esc
            return 0;
        }
    }

    return DefWindowProc(hwnd, msg, wParam, lParam);
}

void Menu_select(Menu_menu* _menu) {
    menu = _menu;
}

void Menu_start(HINSTANCE hInstance) {
    const wchar_t* className = L"WindowClass";

    ShowCursor(FALSE);

    WNDCLASS wc = {};
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = className;
    wc.hCursor = LoadCursor(nullptr, IDC_ARROW);
    RegisterClass(&wc);

    screenWidth = GetSystemMetrics(SM_CXSCREEN);
    screenHeight = GetSystemMetrics(SM_CYSCREEN);

    HWND hwnd = CreateWindowEx(
        0,
        className,
        L"Recovery",
        WS_POPUP,
        0, 0, screenWidth, screenHeight,
        nullptr,
        nullptr,
        hInstance,
        nullptr
    );

    loadConsts();
    initStaticObjects();

    ShowWindow(hwnd, SW_SHOW);
    UpdateWindow(hwnd);

    MSG msg = {};
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}