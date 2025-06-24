#include <windows.h>
#include <stdint.h>
#include <stdio.h>

static RECT globalRect;
static PAINTSTRUCT paintstruct;
static HDC hdc;

static int framecounter = 0;
static int width;
static int height;
static int lines_count = 8;
static int font_margin;
static int font_height;

static COLORREF gdi_color_bg = RGB(0, 61, 118);
static COLORREF gdi_color_text = RGB(11, 214, 99);
static COLORREF gdi_color_text2 = RGB(215, 147, 12);

static void gdi_clear(COLORREF color) {
    HBRUSH hBrush = CreateSolidBrush(color);
    FillRect(hdc, &paintstruct.rcPaint, hBrush);
    DeleteObject(hBrush);
}

static void gdi_text(HDC hdc, int x, int y, const char* text) {
    TextOutA(hdc, x, y, text, strlen(text));
}

static void gdi_line(HDC hdc, int x, int y, const char* text) {
    TextOutA(hdc, x, (y * (font_height + font_margin)) + font_margin, text, strlen(text));
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    switch (uMsg) {
        case WM_DESTROY: {
            PostQuitMessage(0);
            return 0;
        }

        case WM_PAINT: {
            hdc = BeginPaint(hwnd, &paintstruct);
            HFONT hFont = CreateFontA(
                font_height,
                0,
                0,
                0,
                FW_NORMAL,
                FALSE,
                FALSE,
                FALSE,
                DEFAULT_CHARSET,
                OUT_DEFAULT_PRECIS,
                CLIP_DEFAULT_PRECIS,
                DEFAULT_QUALITY,
                DEFAULT_PITCH,
                "Arial"
            );
            SelectObject(hdc, hFont);

            gdi_clear(gdi_color_bg);
            SetBkColor(hdc, gdi_color_bg);

            SetTextColor(hdc, gdi_color_text);
            gdi_line(hdc, font_margin, 0, "winbox wingdi example");

            char buff[64];
            for (int line = 2; line < lines_count - 2; line++) {
                sprintf(buff, "%i", line - 2);
                gdi_line(hdc, font_margin, line, buff);
            }

            SetTextColor(hdc, gdi_color_text2);
            sprintf(buff, "hello, %i!", framecounter++);
            gdi_line(hdc, font_margin, lines_count - 1, buff);

            DeleteObject(hFont);
            EndPaint(hwnd, &paintstruct);
            return 0;
        }

        case WM_TIMER: {
            InvalidateRect(hwnd, NULL, TRUE);
            return 0;
        }
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

    width = GetSystemMetrics(SM_CXSCREEN);
    height = GetSystemMetrics(SM_CYSCREEN);
    font_margin = max(width, height) / 128;
    font_height = (height - (font_margin * lines_count)) / lines_count;

    HWND hwnd = CreateWindowExA(
        0, CLASS_NAME, L"Fullscreen Window",
        WS_POPUP,
        0, 0, width, height,
        NULL, NULL, hInstance, NULL
    );
    SetTimer(hwnd, 1, 100, NULL);
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
