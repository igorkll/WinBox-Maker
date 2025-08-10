# WinBox-Maker 1.4.0
![preview1](https://raw.githubusercontent.com/igorkll/WinBox-Maker/refs/heads/master/preview.png)  
![preview2](https://raw.githubusercontent.com/igorkll/WinBox-Maker/refs/heads/master/preview2.png)  
![preview3](https://raw.githubusercontent.com/igorkll/WinBox-Maker/refs/heads/master/preview3.png)  
download "blobs" folder: https://drive.google.com/file/d/1xH0g-R7ckmCbhAJV-ED4NHXPXl_dOrfH/view?usp=sharing  
program examples: https://github.com/igorkll/WinBox-Maker-programs  
a tool for creating minimal embed versions of windows  
takes on the task of modifying the windows image to remove excess and embed software there  
the program is perfect for windows builds designed for ATM terminals and other devices that unauthorized people have access to and should not be allowed to leave the specified sandbox  
the program needs to be run with administrator rights because it mounts images  
the program is primarily aimed at creating Windows images for operation in kiosk mode, that is, the user will have access to only one of your programs that you add to the image and nothing more  
however, the program can be used in other usage scenarios (for example, creating a TV set-top box or a Windows-based slot machine)  
please note that the program requires the "dism" utility. usually it is built into Windows  
the program is recommended to be used with the original English image of "Windows 10 Enterprise" or "Windows 10 IoT Enterprise"
please note that winbox maker does not provide Windows images, it only provides a tool for reassembling Windows for use in kiosk mode  
if the application fails, winbox will NOT crash on the Windows desktop. you will just have a black screen. This makes winbox maker safe to use in cases where passersby will have access to the device  
during testing of winbox maker, there was not a single way to get into any system menu (if such behavior is not provided in the application) without using a boot disk or other modification of system files  
you can select the executable file by clicking "select" in the app section, but if you use automatic compilation of the project from the source code when building winbox,  
you will not be able to select the file this way. in this case, just enter the file name manually  
winbox Maker is aimed at creating a windows image with access to one application and without the ability to exit it. it also allows you to change / disable the bootloader logo and change / disable the cursor, which allows you to get a device whose behavior will not make it clear that it runs on windows  
winbox maker also disables key combinations to exclude the possibility of closing the application or switching to any system menu  
however, by changing the configuration of winbox maker, you can achieve some other behavior  
winbox maker provides tools for embedding software into the final windows image. such as the net framework and visual C++ redist and others  

## warnings
* if you are going to build the program thoroughly, you will also need to download the blobs folder separately from google drive (due to the file size limit on github)
* img export is not working at the moment
* when you burn the installation ISO to a USB stick via rufus or a similar program, DO NOT USE the windows installation customization feature, as this will cause conflicts with those tweaks that already exist in winbox and it may work incorrectly
* if you enable "Do not disable hotkeys by changing the layout", then probably many keyboard shortcuts can continue to work! It can be VERY UNSAFE for kiosks in public places. the reason for this: windows
* if your application uses file picker from windows, then this is a backdoor! since if you write "cmd" in the path bar, the command line will open, which is unacceptable for devices in kiosk mode
* the project may contain prebuild and postbuild events, and since the program runs on behalf of the administrator, it can be quite dangerous if you do not fully trust the project you are building. Before building an unknown winbox maker project, be sure to check the contents of the events tab
* after the first launch of the operating system created through winbox maker, let the computer run for about two minutes. do not turn it off at this time and do not touch it. otherwise, it may cause windows to crash and require a system reinstall
* when you first boot up the system, you will probably see the windows logo even if you have disabled/changed it, just let the computer boot for the first time and work for a couple of minutes

## notes
* if you install a script as your application .bat or .cmd then it will run in hidden mode (without console)
* it is recommended to use "Windows 10 Enterprise" or "Windows 10 IoT Enterprise" specifically, otherwise some things probably won't work, such as disabling the login animation and disabling keyboard shortcuts in the system itself
* at the moment, the custom boot logo installation only works on UEFI systems (you also need to turn off secure boot) (it may also not work at all for reasons unknown to me)
* your program from the winbox_resources/program folder on the target system is located in C:\WinboxProgram
* it is better not to use the function of replacing the boot logo in winbox maker, the right solution would be to make your own UEFI with your own boot logo or change an existing one. also, if the computer is in a public place, it is better not to make a UEFI menu
* all build events work from the project folder
* in the post-build event, you get the path to the output file as an argument to your script
* the win-mounted event is triggered when the Windows image is mounted and all winbox maker patches have already been applied to it. this event can be used to apply additional patches to Windows through the winbox_temp/wim_mount directory
* in the "build" tab, you can configure the automatic build of your application along with winbox. for this, its source code must be located in the "winbox_resources/sources" folder (subfolders are allowed) please note that the build system is selected by switching tabs in the submenu

## menu description
* base - select the base Windows image that will be used to create a custom Windows image
* description - enter the names of the project and its description, this information will be included in the final *.wim file and will also serve as the name for the output file
* app - choose what your Windows image will do. This can be the launch of a single web page or a custom application
* settings - configure your windows image and embed the components that are necessary for your application to work
* post install - install the scripts and registry files that will be applied to the system when it is first turned on. This can be used to change the system configuration or install software
* activation - enter the windows activation key that will be embedded in the image. the system will work without this, however, it will be inactive and despite the fact that there will be no activation sign, such a system will not be considered legal and is suitable only for testing. You can include the activation key immediately in the image or activate the system during installation using standard installer tools
* events - execute cmd commands on the host machine during the build process. this can be used, for example, to copy files to the project directory or for anything else. to make this work, don't forget to activate the events you use in the checkmark!
* interpreters - embed some interpreters into the Windows image immediately at the build stage
* build - build your app together with Winbox. when using this, you can make the "winbox_resources/program" directory empty and specify a name *.exe file in "app" tab manually. in order for this to work, don't forget to activate the checkmark function near the "add" button!
* downloading - allows you to download files during the build stage. It allows you to unpack archives automatically. please note that the download path is set relative to the project folder. don't forget to add download paths to ".gitignore". in order for this to work, don't forget to activate the checkmark function near the "add" button!

## what was disabled
* explorer.exe (the desktop is completely inaccessible)
* alt+f4
* ctrl+alt+del
* all keyboard shortcuts with Windows button
* telemetry
* windows defender
* windows updates
* UAC
* task manager
* creating screenshots
* sticky keys
* check disk
* hibernation and fast loading
* oobe
* firewall
* system recovery menu
* automatic entry into recovery mode in case of sudden power outage
* windows boot manager menu
* sfc
* snipping tool
* creating dumps in case of bsod
* system logging
* system sounds
* smart screen
* the following services are disabled: wbengine, wuauserv, RemoteRegistry, WSearch, SysMain, WerSvc, shellhwdetection, SSDPSRV, TermService, lanmanserver, napagent
* checking the digital signature of drivers (it may not work on new versions of Windows)
* lock screen
* logon animation (it only works normally in the enterprise version)
* animation of opening and closing windows

### these keys were disabled by changing the keyboard layout
* calculator key
* mail key
* media select key
* messager key
* my computer key
* logitech itouch key
* logitech shopping key
* logitech webcam key
* left/right alt keys
* left/right windows keys
* power/sleep/wake key
* printscreen key
* f1 - f24 keys
* web back, favorites, forward, home, refresh, search, stop keys

### the following keys and combinations have been disabled at the system level
* Alt+F4
* Alt+Space
* Alt+Tab
* Alt+Win
* Application
* BrowserBack
* BrowserFavorites
* BrowserForward
* BrowserHome
* BrowserRefresh
* BrowserSearch
* BrowserStop
* Ctrl+Alt+Del
* Ctrl+Esc
* Ctrl+F4
* Ctrl+Tab
* Ctrl+Win
* Ctrl+Win+F
* F21
* LaunchApp1
* LaunchApp2
* LaunchMail
* LaunchMediaSelect
* LShift+LAlt+NumLock
* LShift+LAlt+PrintScrn
* Shift+Ctrl+Esc
* Shift+Win
* Windows

## changes in the edge browser
* disabled all hotkeys
* disabled updates
* page translation is disabled
* synchronization is disabled

## project structure
* winbox.wnb - the main project file. contains all settings and paths
* .gitignore - it is created by default in the project if it is not present, so as not to commit unnecessary files if you create the project in the git repository. it won't be overwritten if you make changes there, but it will be created if you delete it
* winbox_build - a folder for saving builds. You don't have to use it, but it's the path to save default images (added to by default .gitignore)
* winbox_temp - it is used during the image build process (added to by default .gitignore)
* winbox_images - directory for basic windows images (added to by default .gitignore)
* winbox_resources - a folder for your resources that are needed to build the system. these files should end up in the git repository
* winbox_resources/files - files from this directory will be moved to the root of the disk on the installed system with replacement
* winbox_resources/program - the directory for your application that will be used in kiosk mode. make this directory empty if you are using automatic compilation of the project from the source code using winbox maker
* winbox_resources/drivers - the directory with the drivers with which the image should be built
* winbox_resources/nvidia_drivers - put the driver installers for your nvidia graphics cards here
* winbox_resources/amd_drivers - put the driver installers for your AMD graphics cards here
* winbox_resources/packages - you can add the .cab or .msu packages to this directory
* winbox_resources/cursor - the directory where you can upload custom cursor files (.cur for different states)
* winbox_resources/sources - the source code of your application for building using winbox maker

## custom cursor files (winbox_resources/cursor)
* AppStarting.ani
* Arrow.cur
* Crosshair.cur
* Hand.cur
* Help.cur
* IBeam.cur
* No.cur
* NWPen.cur
* SizeAll.cur
* SizeNESW.cur
* SizeNS.cur
* SizeNWSE.cur
* SizeWE.cur
* UpArrow.cur
* Wait.ani

## command line arguments
1. the path to the file .wnb is for automatically starting conversion from the command line. if it points to a directory, it will convert all files .wnb in this directory
2. the path for exporting the output file, if it points to a directory, exports it there under the default name for this *.wnb. if this argument is not specified, it will be exported with the default name to the winbox_build directory next to the *.wnb file. if you just specify the file name here, the file will be created with the specified name in the winbox_build directory (do not specify a specific path here if you specified the first argument as a directory for converting multiple ones *.wnb because this will cause one file to be overwritten by multiple projects)

## command line flags
* /i - exports the .iso installer
* /w - exports the .wim file