#include <windows.h>
#include <string>
#include <vector>

// Простое текстовое меню
std::vector<std::string> menuItems = {
    "Start Application",
    "Settings",
    "Exit"
};
int selectedItem = 0;

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
    case WM_KEYDOWN:
        switch (wParam) {
        case VK_UP:
            selectedItem = (selectedItem - 1 + menuItems.size()) % menuItems.size();
            InvalidateRect(hwnd, nullptr, TRUE);
            break;
        case VK_DOWN:
            selectedItem = (selectedItem + 1) % menuItems.size();
            InvalidateRect(hwnd, nullptr, TRUE);
            break;
        case VK_RETURN:
            if (selectedItem == (int)menuItems.size() - 1)
                PostQuitMessage(0); // Exit
            break;
        case VK_ESCAPE:
            PostQuitMessage(0);
            break;
        }
        break;
    case WM_PAINT: {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hwnd, &ps);
        RECT rect;
        GetClientRect(hwnd, &rect);

        // Заливка фона
        HBRUSH bg = CreateSolidBrush(RGB(0, 0, 50));
        FillRect(hdc, &rect, bg);
        DeleteObject(bg);

        // Настройка шрифта
        HFONT hFont = CreateFont(48, 0, 0, 0, FW_BOLD, FALSE, FALSE, FALSE,
            DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
            DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, L"Arial");
        SelectObject(hdc, hFont);

        // Рисуем меню
        int y = 100;
        for (size_t i = 0; i < menuItems.size(); i++) {
            SetTextColor(hdc, i == selectedItem ? RGB(255, 255, 0) : RGB(255, 255, 255));
            RECT textRect = RECT{ 50, y, rect.right, y + 60 };
            DrawTextA(hdc, menuItems[i].c_str(), -1, &textRect, DT_LEFT | DT_SINGLELINE);
            y += 80;
        }

        DeleteObject(hFont);
        EndPaint(hwnd, &ps);
        break;
    }
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hwnd, msg, wParam, lParam);
    }
    return 0;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE, LPSTR, int) {
    const wchar_t* className = L"FullscreenMenu";

    WNDCLASS wc = {};
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = className;
    wc.hCursor = LoadCursor(nullptr, IDC_ARROW);

    RegisterClass(&wc);

    int screenWidth = GetSystemMetrics(SM_CXSCREEN);
    int screenHeight = GetSystemMetrics(SM_CYSCREEN);

    HWND hwnd = CreateWindowEx(
        0,
        className,
        L"Kiosk Menu",
        WS_POPUP,
        0, 0, screenWidth, screenHeight,
        nullptr,
        nullptr,
        hInstance,
        nullptr
    );

    ShowWindow(hwnd, SW_SHOW);
    UpdateWindow(hwnd);

    // Цикл сообщений
    MSG msg = {};
    while (GetMessage(&msg, nullptr, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}
