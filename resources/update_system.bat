reagentc.exe /disable
chkntfs /x *

bcdedit /set {current} bootstatuspolicy ignoreallfailures
bcdedit /set {current} recoveryenabled no
bcdedit /set {bootmgr} displaybootmenu no
bcdedit /set {bootmgr} timeout 0
bcdedit /set loadoptions DISABLE_INTEGRITY_CHECKS
bcdedit /set NOINTEGRITYCHECKS ON
bcdedit /set TESTSIGNING ON

sc config wbengine start= disabled
sc config wuauserv start= disabled
sc config RemoteRegistry start= disabled
sc config WSearch start= disabled
sc config SysMain start= disabled
sc config WerSvc start= disabled
sc config shellhwdetection start= disabled
sc config SSDPSRV start= disabled
sc config TermService start= disabled
sc config lanmanserver start= disabled
sc config napagent start= disabled
net stop wbengine
net stop wuauserv
net stop RemoteRegistry
net stop WSearch
net stop SysMain
net stop WerSvc
net stop shellhwdetection
net stop SSDPSRV
net stop TermService
net stop lanmanserver
net stop napagent

reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layout" /v "Scancode Map" /t REG_BINARY /d 000000000000000030000000000021e000006ce000006de0000011e000006be000003b0000004400000057000000580000006400000065000000660000006700000068000000690000006a0000003c0000006b0000006c0000006d0000006e0000006f0000003d0000003e0000003f0000004000000041000000420000004300000013e0000014e0000012e00000380000005be000005ee0000037e0000038e000005ce000005fe0000063e000006ae0000066e0000069e0000032e0000067e0000065e0000068e000000000 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl" /v AutoReboot /t REG_DWORD /d 1 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl" /v CrashDumpEnabled /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\HardwareEvents" /v MaxSize /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application" /v MaxSize /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Security" /v MaxSize /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System" /v MaxSize /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager" /v AutoChkTimeout /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power" /v HibernateEnabledDefault /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile" /v EnableFirewall /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile" /v EnableFirewall /t REG_DWORD /d 0 /f

reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData" /v AllowLockScreen /t REG_DWORD /d 0 /f
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon" /v HideAutoLogonUI /t REG_DWORD /d 1 /f
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon" /v HideFirstLogonAnimation /t REG_DWORD /d 1 /f
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon" /v BrandingNeutral /t REG_DWORD /d 1 /f