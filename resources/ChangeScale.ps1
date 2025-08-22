param (
    [int]$Scaling = 100
)

# --- Apply scaling (DPI) ---
$regPath = "HKCU:\Control Panel\Desktop"
$regPath2 = "HKLM:\DEFAULT_USER\Control Panel\Desktop"

try {
    Set-ItemProperty -Path $regPath -Name "LogPixels" -Type DWord -Value ([int](96 * $Scaling / 100)) -Force
    Set-ItemProperty -Path $regPath -Name "Win8DpiScaling" -Type DWord -Value 1 -Force
    Set-ItemProperty -Path $regPath -Name "DpiScalingVer" -Type DWord -Value 0x00001000 -Force
} catch {
    # normally, exception handling of this kind is not required, but I did it anyway just in case
    Write-Host "the current user has not been loaded"
}

try {
    Set-ItemProperty -Path $regPath2 -Name "LogPixels" -Type DWord -Value ([int](96 * $Scaling / 100)) -Force
    Set-ItemProperty -Path $regPath2 -Name "Win8DpiScaling" -Type DWord -Value 1 -Force
    Set-ItemProperty -Path $regPath2 -Name "DpiScalingVer" -Type DWord -Value 0x00001000 -Force
} catch {
    # normally, exception handling of this kind is not required, but I did it anyway just in case
    Write-Host "the default user has not been loaded"
}

Write-Host "Scaling set to ${Scaling}% (requires logoff/login or explorer.exe restart)"
