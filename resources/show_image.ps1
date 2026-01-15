param(
    [string]$path,
    [string]$stretch,
    [int]$offsetX = 0,
    [int]$offsetY = 0,
    [int]$topmost = 0,
    [string]$stopFileFlag = $null
)

Add-Type -AssemblyName PresentationFramework
Add-Type -AssemblyName WindowsBase
Add-Type -AssemblyName PresentationCore

# Win32 API
Add-Type @"
using System;
using System.Runtime.InteropServices;
public class Win32 {
    [DllImport("user32.dll", SetLastError=true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError=true)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    public const int GWL_EXSTYLE = -20;
    public const int WS_EX_TOOLWINDOW = 0x00000080;
    public const int WS_EX_NOACTIVATE = 0x08000000;
    public const int WS_EX_TRANSPARENT = 0x00000020;
    public static readonly IntPtr HWND_BOTTOM = (IntPtr)1;
    public static readonly IntPtr HWND_TOPMOST = (IntPtr)(-1);
    public static readonly IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
    public const UInt32 SWP_NOMOVE = 0x0001;
    public const UInt32 SWP_NOSIZE = 0x0002;
    public const UInt32 SWP_NOACTIVATE = 0x0010;
}
"@

$w = New-Object System.Windows.Window

$w.add_ContentRendered({
    Set-Content "C:\WinboxResources\show_image.flag" ""
})

$w.WindowStyle = 'None'
$w.ResizeMode = 'NoResize'
$w.WindowState = 'Maximized'
$w.Topmost = [bool]$topmost
$w.ShowInTaskbar = $false
$w.SizeToContent = 'Manual'
$w.Background = [System.Windows.Media.Brushes]::Black
$w.Cursor = [System.Windows.Input.Cursors]::None

# Картинка
$img = New-Object System.Windows.Controls.Image
$uri = New-Object System.Uri((Resolve-Path $path).ProviderPath)
$img.Source = New-Object Windows.Media.Imaging.BitmapImage $uri

if ($stretch -ceq "None") {
    $img.Stretch = 'None'
    $img.HorizontalAlignment = 'Center'
    $img.VerticalAlignment = 'Center'
    $img.Margin = [System.Windows.Thickness]::new($offsetX, $offsetY, 0, 0)
} else {
    $img.Stretch = $stretch
    $img.HorizontalAlignment = 'Stretch'
    $img.VerticalAlignment = 'Stretch'
}

$w.Content = $img

# Показываем окно
$w.Show()

# Получаем HWND
$hwnd = (New-Object System.Windows.Interop.WindowInteropHelper($w)).Handle

# Применяем стили: неактивное, прозрачное, не в Alt+Tab
$exStyle = [Win32]::GetWindowLong($hwnd, [Win32]::GWL_EXSTYLE)
$exStyle = $exStyle -bor [Win32]::WS_EX_NOACTIVATE -bor [Win32]::WS_EX_TOOLWINDOW -bor [Win32]::WS_EX_TRANSPARENT
[Win32]::SetWindowLong($hwnd, [Win32]::GWL_EXSTYLE, $exStyle)

# Ставим окно в самый низ/верх
$insertAfter = if ([bool]$topmost) {
    [Win32]::HWND_TOPMOST
} else {
    [Win32]::HWND_BOTTOM
}

[Win32]::SetWindowPos(
    $hwnd,
    $insertAfter,
    0, 0, 0, 0,
    [Win32]::SWP_NOMOVE -bor
    [Win32]::SWP_NOSIZE -bor
    [Win32]::SWP_NOACTIVATE
)

# таймер чтобы зафиксировать слой окна
$timer = New-Object System.Windows.Threading.DispatcherTimer
$timer.Interval = [TimeSpan]::FromMilliseconds(200)
$timer.Add_Tick({
    [Win32]::SetWindowPos(
        $hwnd,
        $insertAfter,
        0, 0, 0, 0,
        [Win32]::SWP_NOMOVE -bor
        [Win32]::SWP_NOSIZE -bor
        [Win32]::SWP_NOACTIVATE
    )
})
$timer.Start()

# если stopFileFlag появится - закрываем
if ($null -ne $stopFileFlag) {

    $fullPath = [System.IO.Path]::GetFullPath($stopFileFlag)
    $dir  = [System.IO.Path]::GetDirectoryName($fullPath)
    $file = [System.IO.Path]::GetFileName($fullPath)

    $fsw = New-Object System.IO.FileSystemWatcher
    $fsw.Path = $dir
    $fsw.Filter = $file
    $fsw.EnableRaisingEvents = $true

    Register-ObjectEvent $fsw Created -Action {
        # ВАЖНО: всё, что трогает WPF — через Dispatcher
        $w.Dispatcher.Invoke({
            $w.Close()
            [System.Windows.Threading.Dispatcher]::CurrentDispatcher.InvokeShutdown()
        })
    }
}

# Удерживаем окно открытым
[System.Windows.Threading.Dispatcher]::Run()
