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
static COLORREF color_selectedTextShadow = RGB(255, 64, 0);
static int lineHeight;
static int textShadowWidth;
static int screenWidth;
static int screenHeight;

static int progressbar_barHeight;
static int progressbar_offset;
static int progressbar_frameThickness;
static int progressbar_innerPadding;

static COLORREF progressbar_frameColor = RGB(200, 200, 200);
static COLORREF progressbar_bgColor = RGB(40, 40, 40);
static COLORREF progressbar_fillColor = RGB(0, 120, 215);

static void calculateConsts() {
    lineHeight = screenHeight / 12;
    textShadowWidth = screenHeight / 400;

    progressbar_barHeight = screenHeight / 6;
    progressbar_offset = screenHeight / 24;
    progressbar_frameThickness = screenHeight / 600;
    progressbar_innerPadding = screenHeight / 100;
}

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

static bool messageEnabled = false;
static bool messageAllowManualClose = false;
static std::string messageText = "";
static float messageProgress = -1;
static MenuMessageQuietMode messageMenuMessageQuietMode = MenuMessageQuietMode_BlackScreen;

static int menuLockReturn = -9999;

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

static void drawCenterizedTextWithShadow(HDC hdc, int y, const std::string& text, COLORREF color, COLORREF shadowColor) {
    SetTextColor(hdc, shadowColor);
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
    return !menu || messageEnabled;
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

static void drawProgress(HDC hdc, RECT clientRect, float progress)
{
    // защита от мусора
    if (progress < 0.0f) progress = 0.0f;
    if (progress > 1) progress = 1.0f;

    // ===== РАМКА ПРОГРЕССБАРА =====
    RECT frameRect;
    frameRect.left = progressbar_offset;
    frameRect.right = clientRect.right - progressbar_offset;
    frameRect.bottom = clientRect.bottom - progressbar_offset;
    frameRect.top = frameRect.bottom - progressbar_barHeight;

    // фон рамки
    HBRUSH bgBrush = CreateSolidBrush(progressbar_bgColor);
    FillRect(hdc, &frameRect, bgBrush);
    DeleteObject(bgBrush);

    // рамка
    HPEN framePen = CreatePen(PS_SOLID, progressbar_frameThickness, progressbar_frameColor);
    HPEN oldPen = (HPEN)SelectObject(hdc, framePen);
    HBRUSH oldBrush = (HBRUSH)SelectObject(hdc, GetStockObject(NULL_BRUSH));

    Rectangle(
        hdc,
        frameRect.left,
        frameRect.top,
        frameRect.right,
        frameRect.bottom
    );

    SelectObject(hdc, oldBrush);
    SelectObject(hdc, oldPen);
    DeleteObject(framePen);

    // ===== ВНУТРЕННИЙ ПРОГРЕСС =====
    RECT fillRect;
    fillRect.left = frameRect.left + progressbar_innerPadding;
    fillRect.top = frameRect.top + progressbar_innerPadding;
    fillRect.bottom = frameRect.bottom - progressbar_innerPadding;

    int maxWidth = (frameRect.right - frameRect.left) - progressbar_innerPadding * 2;
    fillRect.right = fillRect.left + (int)(maxWidth * progress);

    if (fillRect.right > fillRect.left)
    {
        HBRUSH fillBrush = CreateSolidBrush(progressbar_fillColor);
        FillRect(hdc, &fillRect, fillBrush);
        DeleteObject(fillBrush);
    }
}

static void redrawMenu() {
    InvalidateRect(hwnd, nullptr, TRUE);

    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hwnd, &ps);
    RECT rect;
    GetClientRect(hwnd, &rect);

    bool isMessage = messageEnabled;
    bool isMenu = !isMenuDisabled();

    SetBkMode(hdc, TRANSPARENT);

    if (isMessage || isMenu) {
        if (isMessage && messageMenuMessageQuietMode == MenuMessageQuietMode_BlackScreen) {
            FillRect(hdc, &rect, blackBrush);
        } else {
            FillRect(hdc, &rect, backgroundBrush);
            drawLogo(hdc, menuLogo);

            if (!isMessage || messageMenuMessageQuietMode == MenuMessageQuietMode_DontHide) {
                SelectObject(hdc, titleFont);
                SetTextColor(hdc, color_title);
                if (isMenu && menu->titleOverride.size() > 0) {
                    drawCenterizedText(hdc, 0, menu->titleOverride);
                }
                else
                {
                    drawCenterizedText(hdc, 0, Brain_inputData.value("title", "Winbox maker recovery"));
                }
            }
        }
    } else {
        FillRect(hdc, &rect, blackBrush);
    }

    if (isMessage) {
        if (messageMenuMessageQuietMode == MenuMessageQuietMode_DontHide) {
            SelectObject(hdc, menuFont);
            int y = lineHeight;
            auto lines = split_lines(messageText);
            for (const auto& line : lines) {
                drawCenterizedTextWithShadow(hdc, y, line, color_text, color_textShadow);
                y += lineHeight;
            }

            if (messageProgress >= 0) {
                drawProgress(hdc, rect, messageProgress);
            }
        }
    } else if (isMenu) {
        SelectObject(hdc, menuFont);
        int y = lineHeight;
        for (size_t i = 0; i < menu->menuEntriesNames.size(); i++) {
            drawCenterizedTextWithShadow(hdc, y, menu->menuEntriesNames[i],
                i == menu->selected ? color_selectedText : color_text,
                i == menu->selected ? color_selectedTextShadow : color_textShadow);
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
    if (messageEnabled) {
        if (messageAllowManualClose) {
            messageEnabled = false;
            redrawMenu();
        }
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

    calculateConsts();
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

void Menu_status(std::string text, float progress, MenuMessageQuietMode menuMessageQuietMode) {
    messageEnabled = true;
    messageAllowManualClose = false;
    messageText = text;
    messageProgress = progress;
    messageMenuMessageQuietMode = menuMessageQuietMode;
    redrawMenu();
}

void Menu_hideStatus() {
    messageEnabled = false;
}

void Menu_message(std::string text, float progress, MenuMessageQuietMode menuMessageQuietMode) {
    Menu_status(text, progress, menuMessageQuietMode);
    messageAllowManualClose = true;
    
    while (messageEnabled)
        Menu_process();
}

void Menu_process() {
    MSG msg = {};
    if (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}

int Menu_lock() {
    menuLockReturn = -9999;

    MSG msg = {};
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
        if (menuLockReturn != -9999) return menuLockReturn;
    }

    return -9999;
}

void Menu_unlock(int unlockCode) {
    menuLockReturn = unlockCode;
}
