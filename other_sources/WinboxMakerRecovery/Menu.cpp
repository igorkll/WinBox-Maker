#include "Menu.hpp"
#include <windows.h>
#include <string>
#include <vector>

// ------------------------------------- consts

COLORREF color_bg = RGB(0, 0, 0);
COLORREF color_title = RGB(255, 0, 0);
COLORREF color_text = RGB(255, 255, 255);
COLORREF color_selectedText = RGB(255, 255, 0);
int lineHeight = 100;
std::string title_text = "Winbox maker recovery";

static void loadConsts() {

}

// ------------------------------------- static

static HBRUSH backgroundBrush;
static HFONT titleFont;
static HFONT menuFont;

static void initStaticObjects() {
    backgroundBrush = CreateSolidBrush(color_bg);
    titleFont = CreateFont(lineHeight * 0.9, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
        DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
        DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, L"Arial");
    menuFont = CreateFont(lineHeight * 0.6, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
        DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
        DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, L"Arial");
}

// ------------------------------------- vars

static std::vector<std::string> menuItems = {
    "Factory reset",
    "Flash .wim",
    "Flash .img",
    "Shutdown"
};
static int selectedItem = 0;

// ------------------------------------- code

static void drawCenterizedText(HDC hdc, int y, const std::string& text) {
    RECT rect;
    GetClientRect(WindowFromDC(hdc), &rect);
    rect.top = y;
    rect.bottom = y + lineHeight;
    rect.left = 0;
    rect.right = rect.right;

    DrawTextA(hdc, text.c_str(), -1, &rect, DT_CENTER | DT_SINGLELINE | DT_VCENTER);
}

static void redrawMenu(HWND hwnd) {
    InvalidateRect(hwnd, nullptr, TRUE);
    
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hwnd, &ps);
    RECT rect;
    GetClientRect(hwnd, &rect);

    SetBkMode(hdc, TRANSPARENT);
    FillRect(hdc, &rect, backgroundBrush);

    SelectObject(hdc, titleFont);
    SetTextColor(hdc, color_title);
    drawCenterizedText(hdc, 0, title_text);

    SelectObject(hdc, menuFont);
    int y = lineHeight;
    for (size_t i = 0; i < menuItems.size(); i++) {
        SetTextColor(hdc, i == selectedItem ? color_selectedText : color_text);
        drawCenterizedText(hdc, y, menuItems[i]);
        y += lineHeight;
    }

    EndPaint(hwnd, &ps);
}

static void handleKeyboard(HWND hwnd, WPARAM key) {
    switch (key) {
    case VK_UP:
        selectedItem = (selectedItem - 1 + menuItems.size()) % menuItems.size();
        redrawMenu(hwnd);
        break;
    case VK_DOWN:
        selectedItem = (selectedItem + 1) % menuItems.size();
        redrawMenu(hwnd);
        break;
    case VK_RETURN:
        if (selectedItem == (int)menuItems.size() - 1)
            PostQuitMessage(0);
        break;
    case VK_ESCAPE:
        PostQuitMessage(0);
        break;
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
    case WM_KEYDOWN:
        handleKeyboard(hwnd, wParam);
        break;
    case WM_PAINT:
        redrawMenu(hwnd);
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hwnd, msg, wParam, lParam);
    }
    return 0;
}

void Menu_start(HINSTANCE hInstance) {
    const wchar_t* className = L"WindowClass";

    WNDCLASS wc = {};
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = className;
    wc.hCursor = LoadCursor(nullptr, IDC_NO);
    RegisterClass(&wc);

    int screenWidth = GetSystemMetrics(SM_CXSCREEN);
    int screenHeight = GetSystemMetrics(SM_CYSCREEN);

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