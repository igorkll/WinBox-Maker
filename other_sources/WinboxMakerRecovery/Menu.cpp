#include "Menu.hpp"
#include "json.hpp"
#include <windows.h>
#include <windowsx.h>
#include <string>
#include <vector>
#include <fstream>
#include "Brain.hpp"

using json = nlohmann::json;

// ------------------------------------- consts

static COLORREF color_black = RGB(0, 0, 0);
static COLORREF color_bg = RGB(0, 0, 0);
static COLORREF color_title = RGB(255, 0, 0);
static COLORREF color_text = RGB(255, 255, 255);
static COLORREF color_textShadow = RGB(64, 64, 64);
static COLORREF color_selectedText = RGB(255, 255, 0);
static int lineHeight = 100;
static int textShadowWidth = 3;
static int screenWidth;
static int screenHeight;

// ------------------------------------- static

static HBRUSH backgroundBrush;
static HBRUSH blackBrush;
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
    blackBrush = CreateSolidBrush(color_black);
    titleFont = createMenuFont(lineHeight * 0.9);
    menuFont = createMenuFont(lineHeight * 0.6);
    menuLogo = (HBITMAP)LoadImageA(nullptr, (Brain_sysDrive + "\\WinboxMakerRecovery\\logo.bmp").c_str(), IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
}

// ------------------------------------- vars

static HWND hwnd;
static Menu_menu* menu;
static bool exitLock = false;
static bool menuInited = false;
static std::string messagetext = "";

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

static void drawLogo(HDC hdc, HBITMAP logo) {
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

static bool isMenuDisabled() {
    return !menu || messagetext.size() > 0;
}

static std::vector<std::string> split_lines(const std::string& s) {
    std::vector<std::string> lines;
    size_t start = 0, end;

    while ((end = s.find('\n', start)) != std::string::npos) {
        lines.push_back(s.substr(start, end - start));
        start = end + 1;
    }

    if (start <= s.size())
        lines.push_back(s.substr(start));

    return lines;
}

static void redrawMenu() {
    InvalidateRect(hwnd, nullptr, TRUE);

    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hwnd, &ps);
    RECT rect;
    GetClientRect(hwnd, &rect);

    bool isMessage = messagetext.size() > 0;
    bool isMenu = !isMenuDisabled();

    SetBkMode(hdc, TRANSPARENT);

    if (isMessage || isMenu) {
        FillRect(hdc, &rect, backgroundBrush);
        drawLogo(hdc, menuLogo);

        SelectObject(hdc, titleFont);
        SetTextColor(hdc, color_title);
        if (isMenu && menu->titleOverride.size() > 0) {
            drawCenterizedText(hdc, 0, menu->titleOverride);
        }
        else
        {
            drawCenterizedText(hdc, 0, Brain_inputData.value("title", "Winbox maker recovery"));
        }
    } else {
        FillRect(hdc, &rect, blackBrush);
    }

    if (isMessage) {
        SelectObject(hdc, menuFont);
        int y = lineHeight;
        auto lines = split_lines(messagetext);
        for (const auto& line : lines) {
            drawCenterizedTextWithShadow(hdc, y, line, color_text);
            y += lineHeight;
        }
    } else if (isMenu) {
        SelectObject(hdc, menuFont);
        int y = lineHeight;
        for (size_t i = 0; i < menu->menuEntriesNames.size(); i++) {
            drawCenterizedTextWithShadow(hdc, y, menu->menuEntriesNames[i], i == menu->selected ? color_selectedText : color_text);
            y += lineHeight;
        }
    }

    EndPaint(hwnd, &ps);
}

static void pointerMove(bool up) {
    if (isMenuDisabled()) return;
    if (up) {
        menu->selected = (menu->selected - 1 + menu->menuEntriesNames.size()) % menu->menuEntriesNames.size();
    }
    else
    {
        menu->selected = (menu->selected + 1) % menu->menuEntriesNames.size();
    }
    redrawMenu();
}

static void pointerAccept() {
    if (messagetext.size() > 0) {
        messagetext = "";
        redrawMenu();
    } else if (!isMenuDisabled()) {
        Menu_callback callback = menu->menuEntriesCallbacks[menu->selected];
        callback(menu->menuEntriesArgs[menu->selected]);
        redrawMenu();
    }
}

static void handleKeyboard(WPARAM key) {
    switch (key) {
    case VK_UP:
        pointerMove(true);
        break;
    
    case VK_RETURN:
    case VK_VOLUME_UP: //volume up - accept
        pointerAccept();
        break;

    case VK_DOWN:
    case VK_VOLUME_DOWN: //volume down - down
        pointerMove(false);
        break;
    
    case VK_ESCAPE:
        if (!exitLock) ExitProcess(0);
        break;
    }
}

static void handleAppCommand(WPARAM lParam) {
    switch (GET_APPCOMMAND_LPARAM(lParam)) {
    case APPCOMMAND_VOLUME_UP: //volume up - accept
        pointerAccept();
        break;
    case APPCOMMAND_VOLUME_DOWN: //volume down - down
        pointerMove(false);
        break;
    }
}

static void mouseHandle(WPARAM lParam) {
    if (isMenuDisabled()) return;

    int x = GET_X_LPARAM(lParam);
    int y = GET_Y_LPARAM(lParam);
    int lineIndex = (y / lineHeight) - 1;
    if (lineIndex == menu->selected) {
        pointerAccept();
    }
    else if (lineIndex >= 0 && lineIndex < menu->menuEntriesNames.size())
    {
        menu->selected = lineIndex;
        redrawMenu();
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
    case WM_KEYDOWN:
        handleKeyboard(wParam);
        return 0;
    case WM_PAINT:
        redrawMenu();
        return 0;
    case WM_APPCOMMAND:
        handleAppCommand(wParam);
        return 0;
    case WM_DESTROY:
        ExitProcess(0);
        return 0;
    case WM_LBUTTONDOWN:
        mouseHandle(lParam);
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
    redrawMenu();
}

void Menu_init(HINSTANCE hInstance) {
    if (menuInited) return;
    menuInited = true;

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

    hwnd = CreateWindowEx(
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

    initStaticObjects();
    ShowWindow(hwnd, SW_SHOW);
    UpdateWindow(hwnd);
}

void Menu_start(HINSTANCE hInstance) {
    Menu_init(hInstance);

    MSG msg = {};
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}

void Menu_enableExitLock(bool _exitLock) {
    exitLock = _exitLock;
}

void Menu_message(std::string text) {
    messagetext = text;
    redrawMenu();
    while (messagetext.size() > 0)
        Menu_process();
}

void Menu_process() {
    MSG msg = {};
    if (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}