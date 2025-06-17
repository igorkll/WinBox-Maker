# WinBox-Maker 1.0
a tool for creating minimal embed versions of windows
takes on the task of modifying the windows image to remove excess and embed software there
the program is perfect for windows builds designed for ATM terminals and other devices that unauthorized people have access to and should not be allowed to leave the specified sandbox
the program needs to be run with administrator rights because it mounts images
the program is primarily aimed at creating Windows images for operation in kiosk mode, that is, the user will have access to only one of your programs that you add to the image and nothing more
however, the program can be used in other usage scenarios (for example, creating a TV set-top box or a Windows-based slot machine)
please note that the program requires the "dism" utility. usually it is built into Windows
the program is recommended to be used with the original English image of windows 10 pro
please note that winbox maker does not provide Windows images, it only provides a tool for reassembling Windows for use in kiosk mode

## what was disabled
* explorer.exe (the desktop is completely inaccessible)
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
* system recovery menu
* automatic entry into recovery mode in case of sudden power outage
* windows boot manager menu
* sfc
* snipping tool
* creating dumps in case of bsod
* system logging
* checking the digital signature of drivers (it may not work on new versions of Windows)
* calculator key
* mail key
* media select key
* messager key
* my computer key
* logitech itouch key
* logitech shopping key
* logitech webcam key
* left/right alt key
* left/right windows key
* power/sleep/wake key
* printscreen key

## project structure
* winbox.wnb - the main project file. contains all settings and paths
* .gitignore - it is created by default in the project if it is not present, so as not to commit unnecessary files if you create the project in the git repository. it won't be overwritten if you make changes there, but it will be created if you delete it
* winbox_build - a folder for saving builds. You don't have to use it, but it's the path to save default images (added to by default .gitignore)
* winbox_temp - it is used during the image build process (added to by default .gitignore)
* winbox_images - directory for basic windows images (added to by default .gitignore)
* winbox_resources - a folder for your resources that are needed to build the system. these files should end up in the git repository
* winbox_resources/files - files from this directory will be moved to the root of the disk on the installed system with replacement
* winbox_resources/program - the directory for your application that will be used in kiosk mode. although this directory is not added to by default .gitignore if you plan to automatically copy your application files here during assembly and build a windows image with your application in post build event, then add this directory to .gitignore
* winbox_resources/drivers - the directory with the drivers with which the image should be built
* winbox_resources/packages - you can add the .cab or .msu packages to this directory

## command line arguments
1. the path to the file .wnb is for automatically starting conversion from the command line. if it points to a directory, it will convert all files .wnb in this directory
2. the path for exporting the output file, if it points to a directory, exports it there under the default name for this *.wnb. if this argument is not specified, it will be exported with the default name to the winbox_build directory next to the *.wnb file. if you just specify the file name here, the file will be created with the specified name in the winbox_build directory (do not specify a specific path here if you specified the first argument as a directory for converting multiple ones *.wnb because this will cause one file to be overwritten by multiple projects)

## command line flags
* /i - exports the .iso installer
* /w - exports the .wim file
* /r - exports the .img partition (with a pre-installed system without a bootloader)