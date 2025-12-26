#include <windows.h>
#include <string>
#include <vector>

static std::vector<std::string> menuItems = {
    "Start Application",
    "Settings",
    "Exit"
};
static int selectedItem = 0;

static void redrawMenu(HWND hwnd) {
    InvalidateRect(hwnd, nullptr, TRUE);
    
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hwnd, &ps);
    RECT rect;
    GetClientRect(hwnd, &rect);

    HBRUSH bg = CreateSolidBrush(RGB(0, 0, 50));
    FillRect(hdc, &rect, bg);
    DeleteObject(bg);

    HFONT hFont = CreateFont(48, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
        DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
        DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, L"Arial");
    SelectObject(hdc, hFont);

    int y = 100;
    for (size_t i = 0; i < menuItems.size(); i++) {
        SetTextColor(hdc, i == selectedItem ? RGB(255, 255, 0) : RGB(255, 255, 255));
        RECT textRect = RECT{ 50, y, rect.right, y + 60 };
        DrawTextA(hdc, menuItems[i].c_str(), -1, &textRect, DT_LEFT | DT_SINGLELINE);
        y += 80;
    }

    DeleteObject(hFont);
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

    ShowWindow(hwnd, SW_SHOW);
    UpdateWindow(hwnd);

    MSG msg = {};
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}