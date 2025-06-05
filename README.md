# WinBox-Maker
a tool for creating minimal embed versions of windows
takes on the task of modifying the windows image to remove excess and embed software there
the program needs to be run with administrator rights because it mounts images

## project structure
* winbox.wnb - the main project file. contains all settings and paths
* .gitignore - it is created by default in the project if it is not present, so as not to commit unnecessary files if you create the project in the git repository. it won't be overwritten if you make changes there, but it will be created if you delete it
* winbox_resources - a folder for your resources that are needed to build the system. these files should end up in the git repository
* winbox_build - a folder for saving builds. You don't have to use it, but it's the path to save default images (added to by default .gitignore)
* winbox_temp - it is used during the image build process (added to by default .gitignore)
* winbox_bigResources - a directory for large resource files that are not unique but are downloaded from the outside, such as initial windows images that should not be included in git repository (added to by default .gitignore)