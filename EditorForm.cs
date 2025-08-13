using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        const string defaultProcessName = "not busy";
        WinBoxProject winBoxProject;
        WindowsDescription[]? windowsDescriptions;
        bool softwareCheck = true;
        TaskbarManager taskbarManager;
        int currentBuildItemIndex = -1;
        int currentDownloadItemIndex = -1;
        DownloadItem? currentDownloadItem;
        BuildItem? currentBuildItem;
        bool guiEventsLock = false;
        bool loadingWindowsTask = false;
        bool windowsImagePathChanged = false;

        public EditorForm(WinBoxProject winBoxProject)
        {
            InitializeComponent();
            this.Text = $"{WinBox_Maker.Program.version} - {this.Text} ({winBoxProject.GetName()})";
            this.winBoxProject = winBoxProject;
            this.taskbarManager = TaskbarManager.Instance;

            ArchitectureSelect.Items.Clear();
            ArchitectureSelect.Items.Add("x64");
            ArchitectureSelect.Items.Add("x86");
            ArchitectureSelect.Items.Add("arm64");

            ClearPythonList();

            OpenEmbeddedFolder.Visible = false;
            //ExportImg.Visible = false;
            tabControl1.TabPages.Remove(tabPage7);

            UpdateDownloadItemsList();
            UpdateBuildItemsList();

            softwareCheck = true;
            TweakList.Items.Clear();
            AddTweakToList("Integrate microsoft edge");
            AddTweakToList("Integrate vc redist");
            AddTweakToList("Integrate vc redist (compatible architectures)");
            AddTweakToList("Integrate net 9.0.6");
            AddTweakToList("Integrate net 8.0.17");
            AddTweakToList("Integrate net 4.8.1");
            AddTweakToList("Integrate net 4.7.2");
            AddTweakToList("Integrate app runtime 1.7.3");
            AddTweakToList("Hide Cursor");
            AddTweakToList("Disable boot circle");
            AddTweakToList("Disable boot logo");
            AddTweakToList("Disable boot messages");
            AddTweakToList("Do not disable hotkeys by changing the layout");
            softwareCheck = false;

            UnlockForm();
            if (winBoxProject.NeedLoadWindows())
            {
                UpdateGui();
                LoadWindowsTask();
            }
            else
            {
                UpdateWindowsVersionsList();
                UpdateGui();
            }

            eventWarningDelay();
        }

        void eventWarningDelay()
        {
            if (!winBoxProject.winBoxConfig.isBuildEventsUsed()) return;
            MessageBox.Show(Program.buildEventsWarning, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void UpdateDownloadItemsList()
        {
            softwareCheck = true;
            DownloadItem? lastDownloadItem = null;
            int lastItemIndex = -1;
            DownloadItems.Items.Clear();
            foreach (DownloadItem downloadItem in winBoxProject.winBoxConfig.DownloadItems)
            {
                lastItemIndex = DownloadItems.Items.Add(downloadItem.name);
                lastDownloadItem = downloadItem;
            }
            if (lastItemIndex >= 0)
            {
                DownloadItems.SetItemChecked(lastItemIndex, true);
            }
            softwareCheck = false;
            currentDownloadItemIndex = lastItemIndex;
            UpdateSelectedDownloadItem(lastDownloadItem);
        }

        void UpdateBuildItemsList()
        {
            softwareCheck = true;
            BuildItem? lastBuildItem = null;
            int lastItemIndex = -1;
            BuildItems.Items.Clear();
            foreach (BuildItem buildItem in winBoxProject.winBoxConfig.BuildItems)
            {
                lastItemIndex = BuildItems.Items.Add(buildItem.name);
                lastBuildItem = buildItem;
            }
            if (lastItemIndex >= 0)
            {
                BuildItems.SetItemChecked(lastItemIndex, true);
            }
            softwareCheck = false;
            currentBuildItemIndex = lastItemIndex;
            UpdateSelectedBuildItem(lastBuildItem);
        }

        void UpdateSelectedBuildItem(BuildItem? buildItem)
        {
            currentBuildItem = buildItem;
            if (buildItem == null)
            {
                bl_panel.Visible = false;
            }
            else
            {
                bl_panel.Visible = true;
                guiEventsLock = true;
                bl_title.Text = buildItem.name ?? "";
                bl_path.Text = buildItem.msbuild_path ?? "";
                bl_conf.Text = buildItem.msbuild_configuration ?? "";
                cmake_path.Text = buildItem.cmake_path ?? "";
                cargo_path.Text = buildItem.cargo_path ?? "";
                cmake_configuration.Text = buildItem.cmake_configuration ?? "";
                bl_tabcontrol.SelectedIndex = (int)currentBuildItem.type;
                bl_folder.Text = buildItem.subdirectory ?? "";
                bl_folder_enable.CheckState = buildItem.subdirectory_enabled == true ? CheckState.Checked : CheckState.Unchecked;
                guiEventsLock = false;
            }
        }

        void UpdateSelectedDownloadItem(DownloadItem? downloadItem)
        {
            currentDownloadItem = downloadItem;
            if (downloadItem == null)
            {
                dl_panel.Visible = false;
            }
            else
            {
                dl_panel.Visible = true;
                guiEventsLock = true;
                dl_name.Text = downloadItem.name ?? "";
                dl_url.Text = downloadItem.url ?? "";
                dl_path.Text = downloadItem.path ?? "";
                dl_cache.CheckState = downloadItem.cache == true ? CheckState.Checked : CheckState.Unchecked;
                dl_unpack.CheckState = downloadItem.unpack == true ? CheckState.Checked : CheckState.Unchecked;
                guiEventsLock = false;
            }
        }

        void ClearPythonList()
        {
            pythonVersion.Items.Clear();
            pythonVersion.Items.Add("none");
        }

        void AddTweakToList(String tweak)
        {
            TweakList.Items.Add(tweak, Program.isTweakEnabled(winBoxProject.winBoxConfig, tweak));
        }

        void UnlockFormRecursion(Control parent)
        {
            ProcessName.Text = defaultProcessName;
            ProcessValue.Value = 0;
            foreach (Control control in parent.Controls)
            {
                control.Enabled = true;

                if (control.HasChildren)
                {
                    UnlockFormRecursion(control);
                }
            }
            UpdateGui();
        }

        void LockFormRecursion(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (
                    (control.Name != "LICENSE" &&
                    control.Name != "README" &&
                    control.Name != "OpenProjectFolder" &&
                    control.Name != "OpenEmbeddedFolder" &&
                    control.Name != "openProgramData" &&
                    control.Name != "EmbedDisplayReadme" &&
                    control.Name != "BuildItems" &&
                    control.Name != "DownloadItems" &&
                    !(control is ProgressBar) &&
                    !(control is Label) &&
                    !(control is PictureBox) &&
                    !(control is TabControl) &&
                    !(control is Panel) &&
                    !(control is TabPage)) ||
                    control.Name == "pictureBox3" ||
                    control.Name == "bl_tabcontrol")
                {
                    control.Enabled = false;
                }

                if (control.HasChildren)
                {
                    LockFormRecursion(control);
                }
            }
        }

        void UnlockForm()
        {
            UnlockFormRecursion(this);
        }

        void LockForm()
        {
            LockFormRecursion(this);
        }

        async void LoadWindowsTask()
        {
            if (loadingWindowsTask) return;

            loadingWindowsTask = true;
            LockForm();
            await winBoxProject.LoadWindowsImageAsync(UpdateProcessName, UpdateProcessValue);
            UnlockForm();
            UpdateWindowsVersionsList();
            UpdateGui();
            loadingWindowsTask = false;
        }

        private async void ExportIsoInstaller_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installer (*.iso)|*.iso";
                saveFileDialog.Title = $"Save you WinBox installer ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildIsoAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private async void ExportInstallWim_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox (*.wim)|*.wim";
                saveFileDialog.Title = $"Save you WinBox install.wim ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildWimAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private async void ExportImg_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installed (*.img)|*.img";
                saveFileDialog.Title = $"Save you WinBox installed .img ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "img";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildImgAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private async void WindowsSelect_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Windows image (*.iso)|*.iso", winBoxProject.imagesDirectoryPath, false);
            UnlockForm();

            if (name != null)
            {
                winBoxProject.UnloadWindowsImage();
                winBoxProject.winBoxConfig.BaseWindowsImage = name;
                winBoxProject.SaveConfig();
                UpdateGui();
                LoadWindowsTask();
            }
        }

        private void WindowsName_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            windowsImagePathChanged = true;
            winBoxProject.UnloadWindowsImage();
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsImage = WindowsName.Text;
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        void windowsReload()
        {
            if (guiEventsLock || loadingWindowsTask) return;

            if (windowsImagePathChanged)
            {
                LoadWindowsTask();
                windowsImagePathChanged = false;
            }
        }

        private void WindowsName_Leave(object sender, EventArgs e)
        {
            windowsReload();
        }

        private void WindowsName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                windowsReload();
                e.SuppressKeyPress = true; // предотвращает звуковой сигнал при нажатии Enter
            }
        }

        /*
        private void WindowsClear_Click(object sender, EventArgs e)
        {
            winBoxProject.UnloadWindowsImage();
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.winBoxConfig.BaseWindowsImage = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }
        */

        private void WindowsVersionSelect_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (winBoxProject.winBoxConfig.BaseWindowsImage == null)
            {
                WindowsVersionSelect.Text = null;
                return;
            }

            winBoxProject.winBoxConfig.BaseWindowsVersion = WindowsVersionSelect.Text;
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void ArchitectureSelect_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.Architecture = ArchitectureSelect.Text;
            winBoxProject.SaveConfig();
        }

        private void WindowsVersionUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindowsVersionsList();
            UpdateGui();
        }

        void UpdateGuiWithoutWindowsVersion()
        {
            guiEventsLock = true;
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "not selected";

            WinboxName.Text = winBoxProject.winBoxConfig.WinboxName;
            WinboxDescription.Text = winBoxProject.winBoxConfig.WinboxDescription;

            WindowsDescription.Text = "";
            if (windowsDescriptions != null && winBoxProject.winBoxConfig.BaseWindowsVersion != null)
            {
                foreach (WindowsDescription windowsDescription in windowsDescriptions)
                {
                    if (windowsDescription.name == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        WindowsDescription.Text = windowsDescription.description;
                        break;
                    }
                }
            }

            bool canExport = winBoxProject.canExport();
            ExportIsoInstaller.Enabled = canExport;
            ExportInstallWim.Enabled = canExport;
            ExportImg.Enabled = canExport;
            guiEventsLock = false;
        }

        void UpdateGui()
        {
            guiEventsLock = true;

            pythonVersion.Text = winBoxProject.winBoxConfig.pythonVersion ?? "none";

            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion ?? "";
            ArchitectureSelect.Text = winBoxProject.winBoxConfig.Architecture ?? "";

            OemKey.Text = winBoxProject.winBoxConfig.OemKey ?? "";
            UseOemKey.CheckState = winBoxProject.winBoxConfig.UseOemKey == true ? CheckState.Checked : CheckState.Unchecked;

            ProgramName.Text = winBoxProject.winBoxConfig.ProgramName ?? "";
            ProgramArgs.Text = winBoxProject.winBoxConfig.ProgramArgs ?? "";
            RawCommand.Text = winBoxProject.winBoxConfig.RawCommand ?? "";

            WebSite.Text = winBoxProject.winBoxConfig.WebSite ?? "";
            WebSessionTimeout.Text = winBoxProject.winBoxConfig.WebSessionTimeout.ToString();

            postinstall_bat.Text = winBoxProject.winBoxConfig.PostInstall_bat ?? "not selected";
            postinstall_reg.Text = winBoxProject.winBoxConfig.PostInstall_reg ?? "not selected";
            postinstall_user_bat.Text = winBoxProject.winBoxConfig.PostInstall_user_bat ?? "not selected";
            postinstall_user_reg.Text = winBoxProject.winBoxConfig.PostInstall_user_reg ?? "not selected";
            CustomBootLogo.Text = winBoxProject.winBoxConfig.CustomBootLogo ?? "not selected";

            AddVirtualDisplay.CheckState = winBoxProject.winBoxConfig.AddVirtualDisplay == true ? CheckState.Checked : CheckState.Unchecked;
            UseEmbeddedDisplay.CheckState = winBoxProject.winBoxConfig.UseEmbeddedDisplay == true ? CheckState.Checked : CheckState.Unchecked;
            CustomBootLogo_centering.CheckState = winBoxProject.winBoxConfig.CustomBootLogo_centering == true ? CheckState.Checked : CheckState.Unchecked;
            VirtualDisplayWidth.Text = winBoxProject.winBoxConfig.VirtualDisplayWidth.ToString();
            VirtualDisplayHeight.Text = winBoxProject.winBoxConfig.VirtualDisplayHeight.ToString();

            ScreenTimeout.Text = winBoxProject.winBoxConfig.ScreenTimeout.ToString();

            prebuildEnabled.CheckState = winBoxProject.winBoxConfig.prebuildEnabled == true ? CheckState.Checked : CheckState.Unchecked;
            prebuildEvent.Text = winBoxProject.winBoxConfig.prebuildEvent ?? "";

            postbuildEnabled.CheckState = winBoxProject.winBoxConfig.postbuildEnabled == true ? CheckState.Checked : CheckState.Unchecked;
            postbuildEvent.Text = winBoxProject.winBoxConfig.postbuildEvent ?? "";

            winmountedEnabled.CheckState = winBoxProject.winBoxConfig.winmountedEnabled == true ? CheckState.Checked : CheckState.Unchecked;
            winmountedEvent.Text = winBoxProject.winBoxConfig.winmountedEvent ?? "";

            buildEnabled.CheckState = winBoxProject.winBoxConfig.buildEnabled == true ? CheckState.Checked : CheckState.Unchecked;
            downloadEnabled.CheckState = winBoxProject.winBoxConfig.downloadEnabled == true ? CheckState.Checked : CheckState.Unchecked;

            switch (winBoxProject.winBoxConfig.ProgramType)
            {
                case ProgramTypeEnum.ExecutableFile:
                    ProgramType_ExecutableFile.Checked = true;
                    break;

                case ProgramTypeEnum.RawCommand:
                    ProgramType_RawCommand.Checked = true;
                    break;

                case ProgramTypeEnum.WebSite:
                    ProgramType_WebSite.Checked = true;
                    break;

                case ProgramTypeEnum.None:
                    ProgramType_None.Checked = true;
                    break;
            }

            switch (winBoxProject.winBoxConfig.LaunchMode)
            {
                case ProgramLaunchModeEnum.insteadDesktop:
                    insteadDesktop.Checked = true;
                    break;

                case ProgramLaunchModeEnum.afterDesktop:
                    afterDesktop.Checked = true;
                    break;
            }
            guiEventsLock = false;

            UpdateGuiWithoutWindowsVersion();
        }

        void UpdateWindowsVersionsList()
        {
            if (winBoxProject.winBoxConfig.BaseWindowsImage == null)
            {
                winBoxProject.winBoxConfig.BaseWindowsVersion = null;
                winBoxProject.SaveConfig();
                return;
            }

            try
            {
                windowsDescriptions = winBoxProject.GetWindowsDescriptions();
                WindowsVersionSelect.Items.Clear();
                bool exists = false;
                foreach (WindowsDescription item in windowsDescriptions)
                {
                    WindowsVersionSelect.Items.Add(item.name);
                    if (item.name == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    if (WindowsVersionSelect.Items.Count > 0)
                    {
                        bool findedTarget = false;

                        foreach (WindowsDescription item in windowsDescriptions)
                        {
                            if (item.name.EndsWith("enterprise", StringComparison.OrdinalIgnoreCase))
                            {
                                winBoxProject.winBoxConfig.BaseWindowsVersion = item.name;
                                winBoxProject.SaveConfig();
                                findedTarget = true;
                                break;
                            }
                        }

                        if (!findedTarget)
                        {
                            foreach (WindowsDescription item in windowsDescriptions)
                            {
                                if (item.name.EndsWith("pro", StringComparison.OrdinalIgnoreCase))
                                {
                                    winBoxProject.winBoxConfig.BaseWindowsVersion = item.name;
                                    winBoxProject.SaveConfig();
                                    findedTarget = true;
                                    break;
                                }
                            }
                        }

                        if (!findedTarget)
                        {
                            winBoxProject.winBoxConfig.BaseWindowsVersion = windowsDescriptions[0].name;
                            winBoxProject.SaveConfig();
                        }
                    }
                    else
                    {
                        winBoxProject.winBoxConfig.BaseWindowsVersion = null;
                        winBoxProject.SaveConfig();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"couldn't read the list of versions from the image: {ex}", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WindowsVersionClear_Click(object sender, EventArgs e)
        {
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WinboxName_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.WinboxName = WinboxName.Text;
            winBoxProject.SaveConfig();
        }

        private void WinboxDescription_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.WinboxDescription = WinboxDescription.Text;
            winBoxProject.SaveConfig();
        }

        private void OemKey_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.OemKey = OemKey.Text;
            winBoxProject.SaveConfig();
        }

        private void UseOemKey_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.UseOemKey = UseOemKey.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void ProgramArgs_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ProgramArgs = ProgramArgs.Text;
            winBoxProject.SaveConfig();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.OpenWebPage(WinBox_Maker.Program.logichubUrl + "#winbox");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.OpenWebPage(WinBox_Maker.Program.logichubUrl);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            LockForm();
            Form form = new ProgramSettings(UnlockForm);
            form.Show();
        }

        private void UpdateProcessName(string text)
        {
            ProcessName.Text = text;
        }

        private void UpdateProcessValue(int Value)
        {
            ProcessValue.Value = Value;
            //taskbarManager.SetProgressState(Value == 0 ? TaskbarProgressBarState.NoProgress : TaskbarProgressBarState.Normal);
            taskbarManager.SetProgressState(TaskbarProgressBarState.Normal);
            taskbarManager.SetProgressValue(Value, 100);
        }

        private void back_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.SwitchForm(this, WinBox_Maker.Program.openProjectForm);
        }

        private void README_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer(Program.ResourcePath("README.md"));
            form.Show();
        }

        private void LICENSE_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer(Program.ResourcePath("LICENSE.txt"));
            form.Show();
        }

        private async void AppSelect_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Executable files (*.exe;*.bat;*.cmd)|*.exe;*.bat;*.cmd|All files (*.*)|*.*", Path.Combine(winBoxProject.resourcesDirectoryPath, "program"), true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.ProgramName = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void AppClear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.ProgramName = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void ProgramType_ExecutableFile_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_ExecutableFile.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.ExecutableFile;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void ProgramType_RawCommand_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_RawCommand.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.RawCommand;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void ProgramType_WebSite_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_WebSite.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.WebSite;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void ProgramType_None_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_None.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.None;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void RawCommand_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.RawCommand = RawCommand.Text;
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void OpenProjectFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", winBoxProject.baseDirectoryPath);
            }
            catch (Exception ex)
            {
            }
        }

        private void WebSite_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.WebSite = WebSite.Text;
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void ResetWebSessionTimeout(int value)
        {
            winBoxProject.winBoxConfig.WebSessionTimeout = value;
            guiEventsLock = true;
            WebSessionTimeout.Text = winBoxProject.winBoxConfig.WebSessionTimeout.ToString();
            guiEventsLock = false;
        }

        private void WebSessionTimeout_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            try
            {
                winBoxProject.winBoxConfig.WebSessionTimeout = int.Parse(WebSessionTimeout.Text);
                if (winBoxProject.winBoxConfig.WebSessionTimeout < 0)
                {
                    ResetWebSessionTimeout(0);
                }
                else if (winBoxProject.winBoxConfig.WebSessionTimeout > 1440)
                {
                    ResetWebSessionTimeout(1440);
                }
            }
            catch (FormatException)
            {
                ResetWebSessionTimeout(winBoxProject.winBoxConfig.WebSessionTimeout ?? 0);
            }
            catch (OverflowException)
            {
                ResetWebSessionTimeout(winBoxProject.winBoxConfig.WebSessionTimeout ?? 0);
            }
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private async void OpenLocalHtml_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Local html page (*.html)|*.html", Path.Combine(winBoxProject.resourcesDirectoryPath, "program"), true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.WebSite = @$"C:\WinboxProgram\{name}";
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private async void postinstall_bat_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Bat scripts (*.bat;*.cmd)|*.bat;*.cmd|All files (*.*)|*.*", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.PostInstall_bat = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void postinstall_bat_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.PostInstall_bat = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void postinstall_reg_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Registry files (*.reg)|*.reg|All files (*.*)|*.*", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.PostInstall_reg = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void postinstall_reg_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.PostInstall_reg = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void postinstall_user_bat_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Bat scripts (*.bat;*.cmd)|*.bat;*.cmd|All files (*.*)|*.*", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.PostInstall_user_bat = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void postinstall_user_bat_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.PostInstall_user_bat = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void postinstall_user_reg_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Registry files (*.reg)|*.reg|All files (*.*)|*.*", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.PostInstall_user_reg = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void postinstall_user_reg_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.PostInstall_user_reg = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void ResetScreenTimeout(int value)
        {
            winBoxProject.winBoxConfig.ScreenTimeout = value;
            ScreenTimeout.Text = winBoxProject.winBoxConfig.ScreenTimeout.ToString();
        }

        private void ScreenTimeout_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            try
            {
                winBoxProject.winBoxConfig.ScreenTimeout = int.Parse(ScreenTimeout.Text);
                if (winBoxProject.winBoxConfig.ScreenTimeout < 0)
                {
                    ResetScreenTimeout(0);
                }
                else if (winBoxProject.winBoxConfig.WebSessionTimeout > 360)
                {
                    ResetScreenTimeout(360);
                }
            }
            catch (FormatException)
            {
                ResetScreenTimeout(winBoxProject.winBoxConfig.ScreenTimeout ?? 0);
            }
            catch (OverflowException)
            {
                ResetScreenTimeout(winBoxProject.winBoxConfig.ScreenTimeout ?? 0);
            }
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void TweakList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (softwareCheck) return;

            int index = e.Index;
            string title = TweakList.Items[index].ToString();
            bool state = e.NewValue == CheckState.Checked;
            Program.setTweakEnabled(winBoxProject.winBoxConfig, title, state);
            winBoxProject.SaveConfig();
        }

        private void DownloadItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (softwareCheck) return;

            softwareCheck = true;
            bool state = e.NewValue == CheckState.Checked;
            if (state)
            {
                int index = e.Index;
                for (int i = 0; i < DownloadItems.Items.Count; i++)
                {
                    DownloadItems.SetItemChecked(i, index == i);
                }
                currentDownloadItemIndex = index;
                UpdateSelectedDownloadItem(winBoxProject.winBoxConfig.DownloadItems[index]);
            }
            else
            {
                currentDownloadItemIndex = -1;
                UpdateSelectedDownloadItem(null);
            }
            softwareCheck = false;
        }

        private async void CustomBootLogo_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff"
, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.CustomBootLogo = name;
                //ImageConverter.ConvertToBmp_54_24(Path.Combine(winBoxProject.resourcesDirectoryPath, winBoxProject.winBoxConfig.CustomBootLogo), Path.Combine(winBoxProject.baseDirectoryPath, "winbox_temp", "debug.bmp"));
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void CustomBootLogo_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.CustomBootLogo = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WindowsDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddVirtualDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.AddVirtualDisplay = AddVirtualDisplay.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void ResetVirtualDisplayWidth(int value)
        {
            winBoxProject.winBoxConfig.VirtualDisplayWidth = value;
            VirtualDisplayWidth.Text = winBoxProject.winBoxConfig.VirtualDisplayWidth.ToString();
        }

        private void VirtualDisplayWidth_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            try
            {
                winBoxProject.winBoxConfig.VirtualDisplayWidth = int.Parse(VirtualDisplayWidth.Text);
                if (winBoxProject.winBoxConfig.VirtualDisplayWidth < 0)
                {
                    ResetVirtualDisplayWidth(0);
                }
                else if (winBoxProject.winBoxConfig.VirtualDisplayWidth > 4096)
                {
                    ResetVirtualDisplayWidth(4096);
                }
            }
            catch (FormatException)
            {
                ResetVirtualDisplayWidth(winBoxProject.winBoxConfig.VirtualDisplayWidth ?? 0);
            }
            catch (OverflowException)
            {
                ResetVirtualDisplayWidth(winBoxProject.winBoxConfig.VirtualDisplayWidth ?? 0);
            }
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void ResetVirtualDisplayHeight(int value)
        {
            winBoxProject.winBoxConfig.VirtualDisplayHeight = value;
            VirtualDisplayHeight.Text = winBoxProject.winBoxConfig.VirtualDisplayHeight.ToString();
        }

        private void VirtualDisplayHeight_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            try
            {
                winBoxProject.winBoxConfig.VirtualDisplayHeight = int.Parse(VirtualDisplayHeight.Text);
                if (winBoxProject.winBoxConfig.VirtualDisplayHeight < 0)
                {
                    ResetVirtualDisplayHeight(0);
                }
                else if (winBoxProject.winBoxConfig.VirtualDisplayHeight > 4096)
                {
                    ResetVirtualDisplayHeight(4096);
                }
            }
            catch (FormatException)
            {
                ResetVirtualDisplayHeight(winBoxProject.winBoxConfig.VirtualDisplayHeight ?? 0);
            }
            catch (OverflowException)
            {
                ResetVirtualDisplayHeight(winBoxProject.winBoxConfig.VirtualDisplayHeight ?? 0);
            }
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void OpenEmbeddedFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Program.ResourcePath("embedded"));
            }
            catch (Exception ex)
            {
            }
        }

        private void EmbedDisplayReadme_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer(Program.ResourcePath("embeddedDisplay.txt"));
            form.Show();
        }

        private void UseEmbeddedDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.UseEmbeddedDisplay = UseEmbeddedDisplay.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void CustomBootLogo_centering_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.CustomBootLogo_centering = CustomBootLogo_centering.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void prebuildEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.prebuildEnabled = prebuildEnabled.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void prebuildEvent_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.prebuildEvent = prebuildEvent.Text;
            winBoxProject.SaveConfig();
        }

        private void postbuildEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.postbuildEnabled = postbuildEnabled.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void postbuildEvent_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.postbuildEvent = postbuildEvent.Text;
            winBoxProject.SaveConfig();
        }

        private void winmountedEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmountedEnabled = winmountedEnabled.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void winmountedEvent_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmountedEvent = winmountedEvent.Text;
            winBoxProject.SaveConfig();
        }

        private void pythonVersionsUpdate_Click(object sender, EventArgs e)
        {
            ClearPythonList();

        }

        private void pythonVersion_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.pythonVersion = pythonVersion.Text;
            winBoxProject.SaveConfig();
        }

        private void ProgramName_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ProgramName = ProgramName.Text;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void insteadDesktop_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (insteadDesktop.Checked)
            {
                winBoxProject.winBoxConfig.LaunchMode = ProgramLaunchModeEnum.insteadDesktop;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void afterDesktop_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (afterDesktop.Checked)
            {
                winBoxProject.winBoxConfig.LaunchMode = ProgramLaunchModeEnum.afterDesktop;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void downloadEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.downloadEnabled = downloadEnabled.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void addDownload_Click(object sender, EventArgs e)
        {
            DownloadItem downloadItem = new DownloadItem();
            downloadItem.name = $"download item {winBoxProject.winBoxConfig.DownloadItems.Count() + 1}";
            downloadItem.url = "https://raw.githubusercontent.com/igorkll/trashfolder/refs/heads/main/sound3/1.mp3";
            downloadItem.path = "winbox_temp/files/DIRECTORIES ARE/CREATED AUTOMATICALLY/example.mp3";
            downloadItem.cache = true;
            downloadItem.unpack = false;
            winBoxProject.winBoxConfig.DownloadItems.Add(downloadItem);
            winBoxProject.SaveConfig();
            UpdateDownloadItemsList();
        }

        private void dl_delete_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.DownloadItems.Remove(currentDownloadItem);
            winBoxProject.SaveConfig();
            UpdateDownloadItemsList();
        }

        private void dl_name_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentDownloadItem.name = dl_name.Text;
            DownloadItems.Items[currentDownloadItemIndex] = dl_name.Text;
            winBoxProject.SaveConfig();
        }

        private void dl_url_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentDownloadItem.url = dl_url.Text;
            winBoxProject.SaveConfig();
        }

        private void dl_path_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentDownloadItem.path = dl_path.Text;
            winBoxProject.SaveConfig();
        }

        private void dl_cache_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentDownloadItem.cache = dl_cache.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void dl_unpack_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentDownloadItem.unpack = dl_unpack.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void openProgramData_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Program.appdataPath);
            }
            catch (Exception ex)
            {
            }
        }

        private void buildEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.buildEnabled = buildEnabled.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void bl_tabcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = bl_tabcontrol.SelectedTab;
            currentBuildItem.type = (BuildItemType)bl_tabcontrol.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void addBuild_Click(object sender, EventArgs e)
        {
            BuildItem buildItem = new BuildItem();
            buildItem.name = $"build item {winBoxProject.winBoxConfig.BuildItems.Count() + 1}";
            buildItem.type = BuildItemType.msbuild;
            buildItem.subdirectory = "";
            buildItem.subdirectory_enabled = false;
            buildItem.msbuild_path = "";
            buildItem.msbuild_configuration = "Release";
            buildItem.cmake_path = "";
            buildItem.cmake_configuration = "Release";

            winBoxProject.winBoxConfig.BuildItems.Add(buildItem);
            winBoxProject.SaveConfig();
            UpdateBuildItemsList();
        }

        private async void bl_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue,
                "Msbuild project (*.sln)|*.sln|All files (*.*)|*.*",
                winBoxProject.sourcesDirectoryPath,
                true
            );
            if (name != null)
            {
                currentBuildItem.msbuild_path = name;
                guiEventsLock = true;
                bl_path.Text = currentBuildItem.msbuild_path;
                guiEventsLock = false;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void bl_clear_Click(object sender, EventArgs e)
        {
            currentBuildItem.msbuild_path = "";
            guiEventsLock = true;
            bl_path.Text = "";
            guiEventsLock = false;
            winBoxProject.SaveConfig();
        }

        private void bl_delete_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.BuildItems.Remove(currentBuildItem);
            winBoxProject.SaveConfig();
            UpdateBuildItemsList();
        }

        private void BuildItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (softwareCheck) return;

            softwareCheck = true;
            bool state = e.NewValue == CheckState.Checked;
            if (state)
            {
                int index = e.Index;
                for (int i = 0; i < BuildItems.Items.Count; i++)
                {
                    BuildItems.SetItemChecked(i, index == i);
                }
                currentBuildItemIndex = index;
                UpdateSelectedBuildItem(winBoxProject.winBoxConfig.BuildItems[index]);
            }
            else
            {
                currentBuildItemIndex = -1;
                UpdateSelectedBuildItem(null);
            }
            softwareCheck = false;
        }

        private void bl_title_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.name = bl_title.Text;
            BuildItems.Items[currentBuildItemIndex] = bl_title.Text;
            winBoxProject.SaveConfig();
        }

        private void bl_conf_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.msbuild_configuration = bl_conf.Text;
            winBoxProject.SaveConfig();
        }

        private void bl_path_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.msbuild_path = bl_path.Text;
            winBoxProject.SaveConfig();
        }

        private void BuildItems_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bl_folder_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.subdirectory = bl_folder.Text;
            winBoxProject.SaveConfig();
        }

        private void bl_folder_enable_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.subdirectory_enabled = bl_folder_enable.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void cmake_path_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.cmake_path = cmake_path.Text;
            winBoxProject.SaveConfig();
        }

        private async void cmake_path_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue,
                "CMake project (*.txt)|*.txt|All files (*.*)|*.*",
                winBoxProject.sourcesDirectoryPath,
                true
            );
            if (name != null)
            {
                currentBuildItem.cmake_path = name;
                guiEventsLock = true;
                cmake_path.Text = currentBuildItem.cmake_path;
                guiEventsLock = false;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void cmake_path_clear_Click(object sender, EventArgs e)
        {
            currentBuildItem.cmake_path = "";
            guiEventsLock = true;
            cmake_path.Text = "";
            guiEventsLock = false;
            winBoxProject.SaveConfig();
        }

        private void cargo_path_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.cargo_path = cargo_path.Text;
            winBoxProject.SaveConfig();
        }

        private async void cargo_path_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue,
                "Cargo project (*.toml)|*.toml|All files (*.*)|*.*",
                winBoxProject.sourcesDirectoryPath,
                true
            );
            if (name != null)
            {
                currentBuildItem.cargo_path = name;
                guiEventsLock = true;
                cargo_path.Text = currentBuildItem.cargo_path;
                guiEventsLock = false;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void cargo_path_clear_Click(object sender, EventArgs e)
        {
            currentBuildItem.cargo_path = "";
            guiEventsLock = true;
            cargo_path.Text = "";
            guiEventsLock = false;
            winBoxProject.SaveConfig();
        }
    }
}
