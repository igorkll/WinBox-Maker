#include <windows.h>
#include <stdint.h>

static RECT globalRect;
static PAINTSTRUCT paintstruct;
static HDC hdc;

static COLORREF gdi_color_bg = RGB(0, 61, 118);
static COLORREF gdi_color_warn = RGB(215, 147, 12);
static COLORREF gdi_color_text = RGB(11, 214, 99);

static void gdi_clear(COLORREF color) {
    HBRUSH hBrush = CreateSolidBrush(color);
    FillRect(hdc, &paintstruct.rcPaint, hBrush);
    DeleteObject(hBrush);
}

static void gdi_text(HDC hdc, int x, int y, const char* text) {
    TextOutA(hdc, x, y, text, strlen(text));
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    switch (uMsg) {
        case WM_DESTROY: {
            PostQuitMessage(0);
            return 0;
        }

        case WM_PAINT: {
            hdc = BeginPaint(hwnd, &paintstruct);

            gdi_clear(gdi_color_bg);

            SetBkColor(hdc, gdi_color_bg);
            SetTextColor(hdc, gdi_color_warn);
            gdi_text(hdc, 5, 5, "Hello, Fullscreen!");

            EndPaint(hwnd, &paintstruct);
            break;
        }

        return 0;
    }

    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd) {
    const char CLASS_NAME[] = "FullscreenWindowClass";
    WNDCLASS wc = { 0 };
    wc.lpfnWndProc = WindowProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = CLASS_NAME;
    RegisterClassA(&wc);

    HWND hwnd = CreateWindowExA(
        0, CLASS_NAME, "Fullscreen Window",
        WS_POPUP,
        0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN),
        NULL, NULL, hInstance, NULL
    );
    ShowWindow(hwnd, nShowCmd);
    UpdateWindow(hwnd);

    GetClientRect(hwnd, &globalRect);

    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}
