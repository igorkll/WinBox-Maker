# WinBox-Maker
a tool for creating minimal embed versions of windows
takes on the task of modifying the windows image to remove excess and embed software there
the program is perfect for windows builds designed for ATM terminals and other devices that unauthorized people have access to and should not be allowed to leave the specified sandbox
the program needs to be run with administrator rights because it mounts images
the program is primarily aimed at creating Windows images for operation in kiosk mode, that is, the user will have access to only one of your programs that you add to the image and nothing more. however, the program can be used in other usage scenarios

## project structure
* winbox.wnb - the main project file. contains all settings and paths
* .gitignore - it is created by default in the project if it is not present, so as not to commit unnecessary files if you create the project in the git repository. it won't be overwritten if you make changes there, but it will be created if you delete it
* winbox_resources - a folder for your resources that are needed to build the system. these files should end up in the git repository
* winbox_build - a folder for saving builds. You don't have to use it, but it's the path to save default images (added to by default .gitignore)
* winbox_temp - it is used during the image build process (added to by default .gitignore)
* winbox_bigResources - a directory for large resource files that are not unique but are downloaded from the outside, such as initial windows images that should not be included in git repository (added to by default .gitignore)

## command line arguments
1. the path to the file .wnb is for automatically starting conversion from the command line. if it points to a directory, it will convert all files .wnb in this directory
2. the path for exporting the output file, if it points to a directory, exports it there under the default name for this *.wnb. if this argument is not specified, it will be exported with the default name to the winbox_build directory next to the *.wnb file (do not specify a specific path here if you specified the first argument as a directory for converting multiple ones *.wnb because this will cause one file to be overwritten by multiple projects)

## command line flags
* /i - exports the installer .iso
* /w - exports the .wim file
* /r - exports the .img partition