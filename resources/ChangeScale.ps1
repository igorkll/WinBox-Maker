param (
    [int]$Scaling = 100
)

# --- Apply scaling (DPI) ---
$regPath = "HKCU:\Control Panel\Desktop"

Set-ItemProperty -Path $regPath -Name "LogPixels" -Type DWord -Value ([int](96 * $Scaling / 100)) -Force
Set-ItemProperty -Path $regPath -Name "Win8DpiScaling" -Type DWord -Value 1 -Force
Set-ItemProperty -Path $regPath -Name "DpiScalingVer" -Type DWord -Value 0x00001000 -Force

Write-Host "Scaling set to ${Scaling}% (requires logoff/login or explorer.exe restart)"
