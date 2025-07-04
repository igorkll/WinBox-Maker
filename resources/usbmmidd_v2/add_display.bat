@cd /d "%~dp0"

@goto %PROCESSOR_ARCHITECTURE%
@exit

:AMD64
@cmd /c deviceinstaller64.exe enableidd 1
sc.exe create usbmmidd binPath="C:\WinboxResources\usbmmidd_v2\deviceinstaller64.exe enableidd 1"
@goto end

:x86
@cmd /c deviceinstaller.exe enableidd 1
sc.exe create usbmmidd binPath="C:\WinboxResources\usbmmidd_v2\deviceinstaller.exe enableidd 1"

:end
