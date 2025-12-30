using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;
using static System.Net.Mime.MediaTypeNames;
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
        public TaskbarManager taskbarManager;
        bool guiEventsLock = false;
        bool loadingWindowsTask = false;
        bool windowsImagePathChanged = false;

        int currentBuildItemIndex = -1;
        BuildItem? currentBuildItem;

        int currentDownloadItemIndex = -1;
        DownloadItem? currentDownloadItem;

        TwoStrings? current_keyboard_layout;

        public EditorForm(WinBoxProject winBoxProject)
        {
            InitializeComponent();
            this.Text = $"{WinBox_Maker.Program.version} - {this.Text} ({winBoxProject.GetName()})";
            this.winBoxProject = winBoxProject;
            Program.winBoxProject = winBoxProject;
            this.taskbarManager = TaskbarManager.Instance;

            ArchitectureSelect.Items.Clear();
            ArchitectureSelect.Items.Add("x64");
            ArchitectureSelect.Items.Add("x86");
            ArchitectureSelect.Items.Add("arm64");

            ClearPythonList();

            OpenEmbeddedFolder.Visible = false;
            mainTabControl.TabPages.Remove(tabPage7);
            mainTabControl.TabPages.Remove(tabPage9);

            UpdateDownloadItemsList();
            UpdateBuildItemsList();
            UpdateKeyboardLayoutsList();

            softwareCheck = true;
            TweakList.Items.Clear();
            AddTweakToList("Integrate microsoft edge");
            AddTweakToList("Integrate vc redist");
            AddTweakToList("Integrate vc redist (compatible architectures)");
            AddTweakToList("Integrate nircmd");
            AddTweakToList("Integrate PSTools");
            AddTweakToList("Integrate net 9.0.6");
            AddTweakToList("Integrate net 8.0.17");
            AddTweakToList("Integrate net 4.8.1");
            AddTweakToList("Integrate net 4.7.2");
            AddTweakToList("Integrate app runtime 1.7.3");
            AddTweakToList("Hide Cursor");
            AddTweakToList("Hide Touchscreen Visualization");
            AddTweakToList("Disable boot circle");
            AddTweakToList("Disable boot logo");
            AddTweakToList("Disable boot messages");
            AddTweakToList("Disable all boot UI");
            AddTweakToList("Disable security mitigations (performance boost)");
            AddTweakToList("Hide bootmgr errors");
            AddTweakToList("Enable CrashOnCtrlScroll (BSOD)");
            AddTweakToList("Do not disable hotkeys by changing the layout");
            AddTweakToList("Do not disable hotkeys by keyboard filter");
            AddTweakToList("completely remove explorer.exe");
            AddTweakToList("completely remove system audio/images");
            AddTweakToList("removing Windows/System apps (breaks the default shell)");
            AddTweakToList("removal of the subsystem SysWOW64");
            AddTweakToList("remove windows defender files");
            AddTweakToList("remove OneDrive");
            AddTweakToList("Allow check-disk");
            AddTweakToList("Disable system integrity checks");
            AddTweakToList("Disable HyperV / VSM / ELAM");
            AddTweakToList("Hide system errors");
            softwareCheck = false;

            resetKeyboardFilterBlockList();

            UnlockForm();
            if (winBoxProject.NeedLoadWindows())
            {
                UpdateGui();
                LoadWindowsTask();
            }
            else
            {
                UpdateWindowsVersionsList();
                UpdateGuiAfterWindowsLoaded();
            }

            eventWarningDelay();
        }

        void resetKeyboardFilterBlockList()
        {
            softwareCheck = true;
            keyboard_filter_blockList.Items.Clear();
            AddBlockedHotkeyToList("Alt");
            AddBlockedHotkeyToList("Alt+F4");
            AddBlockedHotkeyToList("Alt+Space");
            AddBlockedHotkeyToList("Alt+Tab");
            AddBlockedHotkeyToList("Alt+Win");
            AddBlockedHotkeyToList("Application");
            AddBlockedHotkeyToList("BrowserBack");
            AddBlockedHotkeyToList("BrowserFavorites");
            AddBlockedHotkeyToList("BrowserForward");
            AddBlockedHotkeyToList("BrowserHome");
            AddBlockedHotkeyToList("BrowserRefresh");
            AddBlockedHotkeyToList("BrowserSearch");
            AddBlockedHotkeyToList("BrowserStop");
            AddBlockedHotkeyToList("Ctrl");
            AddBlockedHotkeyToList("Ctrl+Alt+Del");
            AddBlockedHotkeyToList("Ctrl+Esc");
            AddBlockedHotkeyToList("Ctrl+F4");
            AddBlockedHotkeyToList("Ctrl+Tab");
            AddBlockedHotkeyToList("Ctrl+Win");
            AddBlockedHotkeyToList("Ctrl+Win+F");
            AddBlockedHotkeyToList("Escape");
            AddBlockedHotkeyToList("F21");
            AddBlockedHotkeyToList("LaunchApp1");
            AddBlockedHotkeyToList("LaunchApp2");
            AddBlockedHotkeyToList("LaunchMail");
            AddBlockedHotkeyToList("LaunchMediaSelect");
            AddBlockedHotkeyToList("LShift+LAlt+NumLock");
            AddBlockedHotkeyToList("LShift+LAlt+PrintScrn");
            AddBlockedHotkeyToList("MediaNext");
            AddBlockedHotkeyToList("MediaPlayPause");
            AddBlockedHotkeyToList("MediaPrev");
            AddBlockedHotkeyToList("MediaStop");
            AddBlockedHotkeyToList("Shift");
            AddBlockedHotkeyToList("Shift+Ctrl+Esc");
            AddBlockedHotkeyToList("Shift+Win");
            AddBlockedHotkeyToList("VolumeDown");
            AddBlockedHotkeyToList("VolumeMute");
            AddBlockedHotkeyToList("VolumeUp");
            AddBlockedHotkeyToList("Win++");
            AddBlockedHotkeyToList("Win+,");
            AddBlockedHotkeyToList("Win+-");
            AddBlockedHotkeyToList("Win+.");
            AddBlockedHotkeyToList("Win+/");
            AddBlockedHotkeyToList("Win+B");
            AddBlockedHotkeyToList("Win+Break");
            AddBlockedHotkeyToList("Win+C");
            AddBlockedHotkeyToList("Win+D");
            AddBlockedHotkeyToList("Win+Down");
            AddBlockedHotkeyToList("Win+E");
            AddBlockedHotkeyToList("Win+Enter");
            AddBlockedHotkeyToList("Win+Esc");
            AddBlockedHotkeyToList("Win+F");
            AddBlockedHotkeyToList("Win+F1");
            AddBlockedHotkeyToList("Win+H");
            AddBlockedHotkeyToList("Win+Home");
            AddBlockedHotkeyToList("Win+I");
            AddBlockedHotkeyToList("Win+J");
            AddBlockedHotkeyToList("Win+K");
            AddBlockedHotkeyToList("Win+L");
            AddBlockedHotkeyToList("Win+Left");
            AddBlockedHotkeyToList("Win+M");
            AddBlockedHotkeyToList("Win+O");
            AddBlockedHotkeyToList("Win+P");
            AddBlockedHotkeyToList("Win+PageDown");
            AddBlockedHotkeyToList("Win+PageUp");
            AddBlockedHotkeyToList("Win+Q");
            AddBlockedHotkeyToList("Win+R");
            AddBlockedHotkeyToList("Win+Right");
            AddBlockedHotkeyToList("Win+Shift+Down");
            AddBlockedHotkeyToList("Win+Shift+Left");
            AddBlockedHotkeyToList("Win+Shift+Right");
            AddBlockedHotkeyToList("Win+Shift+Up");
            AddBlockedHotkeyToList("Win+Space");
            AddBlockedHotkeyToList("Win+T");
            AddBlockedHotkeyToList("Win+Tab");
            AddBlockedHotkeyToList("Win+U");
            AddBlockedHotkeyToList("Win+Up");
            AddBlockedHotkeyToList("Win+V");
            AddBlockedHotkeyToList("Win+W");
            AddBlockedHotkeyToList("Win+Z");
            AddBlockedHotkeyToList("Windows");
            softwareCheck = false;
        }

        void eventWarningDelay()
        {
            if (!winBoxProject.winBoxConfig.isBuildEventsUsed()) return;
            MessageBox.Show(Program.buildEventsWarning, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        void AddBlockedHotkeyToList(String tweak)
        {
            keyboard_filter_blockList.Items.Add(tweak, Program.isCheckEnabled(winBoxProject.winBoxConfig.keyboard_filter_blockList, tweak));
        }

        void UnlockFormRecursion(Control parent)
        {
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
            UpdateProcessName(defaultProcessName);
            UpdateProcessValue(0);
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
            OnWindowsLoadedFirst();
            UpdateGuiAfterWindowsLoaded();
            loadingWindowsTask = false;

            winBoxProject.SaveConfig();
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
                saveFileDialog.DefaultExt = "wim";
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

        private async void ExportInstallEsd_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox (*.esd)|*.esd";
                saveFileDialog.Title = $"Save you WinBox install.esd ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "esd";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildEsdAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
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
            UpdateGuiAfterWindowsLoaded();
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
            UpdateGuiAfterWindowsLoaded();
        }

        void UpdateGuiWithoutWindowsVersion()
        {
            guiEventsLock = true;
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "";

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
            ExportImgUefi.Enabled = canExport;
            ExportInstallEsd.Enabled = canExport;
            guiEventsLock = false;
        }

        void UpdateGuiCurrentServices()
        {
            services_stop_view.Text = string.Join("\n", winBoxProject.getStopServicesList());
            services_start_view.Text = string.Join("\n", winBoxProject.getStartServicesList());
        }

        void UpdateGuiCurrentSchtasks()
        {
            schtasks_stopOrDelete_view.Text = string.Join("\n", winBoxProject.getStopOrDeleteSchtasksList());
        }

        void OnWindowsLoadedFirst()
        {
            winBoxProject.winBoxConfig.forceIot = winBoxProject.winBoxConfig.BaseWindowsVersion.Contains("enterprise", StringComparison.OrdinalIgnoreCase);

            UpdateKeyboardLayoutsList();
        }

        void UpdateGuiAfterWindowsLoaded()
        {
            guiEventsLock = true;

            TimeZoneKeyName.Items.Clear();
            TimeZoneKeyName.Items.AddRange(winBoxProject.GetWindowsTimeZones());

            keyboard_layouts_available.Items.Clear();
            keyboard_layouts_available.Items.AddRange(winBoxProject.GetWindowsKeyboardLayoutNames());
            if (keyboard_layouts_available.Items.Count > 0)
            {
                keyboard_layouts_available.SelectedIndex = 0;
            }

            UpdateGui();
        }

        void UpdateGui()
        {
            guiEventsLock = true;

            keyboard_filter_enabled.Enabled = !Program.isTweakEnabled(winBoxProject.winBoxConfig, "Do not disable hotkeys by keyboard filter");
            keyboard_filter_enabled.Checked = winBoxProject.winBoxConfig.keyboard_filter_enabled == true;
            keyboard_filter_panel.Enabled = keyboard_filter_enabled.Enabled && keyboard_filter_enabled.Checked;

            TimeZoneKeyName.Text = winBoxProject.winBoxConfig.TimeZoneKeyName ?? "";

            pythonVersion.Text = winBoxProject.winBoxConfig.pythonVersion ?? "none";

            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion ?? "";
            ArchitectureSelect.Text = winBoxProject.winBoxConfig.Architecture ?? "";

            OemKey.Text = winBoxProject.winBoxConfig.OemKey ?? "";

            ProgramName.Text = winBoxProject.winBoxConfig.ProgramName ?? "";
            ProgramArgs.Text = winBoxProject.winBoxConfig.ProgramArgs ?? "";
            RawCommand.Text = winBoxProject.winBoxConfig.RawCommand ?? "";

            WebSite.Text = winBoxProject.winBoxConfig.WebSite ?? "";
            WebSessionTimeout.Text = winBoxProject.winBoxConfig.WebSessionTimeout.ToString();

            onbuild_reg.Text = winBoxProject.winBoxConfig.onbuild_reg ?? "not selected";
            postinstall_bat.Text = winBoxProject.winBoxConfig.PostInstall_bat ?? "not selected";
            postinstall_reg.Text = winBoxProject.winBoxConfig.PostInstall_reg ?? "not selected";
            postinstall_user_bat.Text = winBoxProject.winBoxConfig.PostInstall_user_bat ?? "not selected";
            postinstall_user_reg.Text = winBoxProject.winBoxConfig.PostInstall_user_reg ?? "not selected";
            CustomBootLogo.Text = winBoxProject.winBoxConfig.CustomBootLogo ?? "not selected";

            AddVirtualDisplay.Checked = winBoxProject.winBoxConfig.AddVirtualDisplay == true;
            UseEmbeddedDisplay.Checked = winBoxProject.winBoxConfig.UseEmbeddedDisplay == true;
            CustomBootLogo_centering.Checked = winBoxProject.winBoxConfig.CustomBootLogo_centering == true;
            VirtualDisplayWidth.Text = winBoxProject.winBoxConfig.VirtualDisplayWidth.ToString();
            VirtualDisplayHeight.Text = winBoxProject.winBoxConfig.VirtualDisplayHeight.ToString();

            ScreenTimeout.Text = winBoxProject.winBoxConfig.ScreenTimeout.ToString();
            StandbyTimeout.Text = winBoxProject.winBoxConfig.StandbyTimeout.ToString();
            HibernateTimeout.Text = winBoxProject.winBoxConfig.HibernateTimeout.ToString();
            DiskTimeout.Text = winBoxProject.winBoxConfig.DiskTimeout.ToString();
            ScreenTimeout_dc.Text = winBoxProject.winBoxConfig.ScreenTimeout_dc.ToString();
            StandbyTimeout_dc.Text = winBoxProject.winBoxConfig.StandbyTimeout_dc.ToString();
            HibernateTimeout_dc.Text = winBoxProject.winBoxConfig.HibernateTimeout_dc.ToString();
            DiskTimeout_dc.Text = winBoxProject.winBoxConfig.DiskTimeout_dc.ToString();

            keyboard_filter_ForceOffAccessibility.Checked = winBoxProject.winBoxConfig.keyboard_filter_ForceOffAccessibility == true;
            keyboard_filter_DisableKeyboardFilterForAdministrators.Checked = winBoxProject.winBoxConfig.keyboard_filter_DisableKeyboardFilterForAdministrators == true;
            keyboard_filter_BreakoutKeyScanCode.Text = winBoxProject.winBoxConfig.keyboard_filter_BreakoutKeyScanCode.ToString();

            regtweak_overwrite_en.Checked = winBoxProject.winBoxConfig.regtweak_overwrite_en == true;
            regtweak_overwrite.Text = winBoxProject.winBoxConfig.regtweak_overwrite ?? "";
            regtweak_overwrite.Enabled = winBoxProject.winBoxConfig.regtweak_overwrite_en == true;

            cds_width.Text = winBoxProject.winBoxConfig.cds_width.ToString();
            cds_height.Text = winBoxProject.winBoxConfig.cds_height.ToString();
            cds_bitDepth.Text = winBoxProject.winBoxConfig.cds_bitDepth.ToString();
            cds_refreshRate.Text = winBoxProject.winBoxConfig.cds_refreshRate.ToString();
            cds_scaling.Text = winBoxProject.winBoxConfig.cds_scaling.ToString();

            prebuildEnabled.Checked = winBoxProject.winBoxConfig.prebuildEnabled == true;
            prebuildEvent.Text = winBoxProject.winBoxConfig.prebuildEvent ?? "";
            prebuildEvent.Enabled = winBoxProject.winBoxConfig.prebuildEnabled == true;
            prebuild_breakbefore.Checked = winBoxProject.winBoxConfig.prebuild_breakbefore == true;
            prebuild_breakafter.Checked = winBoxProject.winBoxConfig.prebuild_breakafter == true;

            postbuildEnabled.Checked = winBoxProject.winBoxConfig.postbuildEnabled == true;
            postbuildEvent.Text = winBoxProject.winBoxConfig.postbuildEvent ?? "";
            postbuildEvent.Enabled = winBoxProject.winBoxConfig.postbuildEnabled == true;
            postbuild_breakbefore.Checked = winBoxProject.winBoxConfig.postbuild_breakbefore == true;
            postbuild_breakafter.Checked = winBoxProject.winBoxConfig.postbuild_breakafter == true;

            winmountedEnabled.Checked = winBoxProject.winBoxConfig.winmountedEnabled == true;
            winmountedEvent.Text = winBoxProject.winBoxConfig.winmountedEvent ?? "";
            winmountedEvent.Enabled = winBoxProject.winBoxConfig.winmountedEnabled == true;
            winmounted_breakbefore.Checked = winBoxProject.winBoxConfig.winmounted_breakbefore == true;
            winmounted_breakafter.Checked = winBoxProject.winBoxConfig.winmounted_breakafter == true;

            winmountedEarlyEnabled.Checked = winBoxProject.winBoxConfig.winmountedEarlyEnabled == true;
            winmountedEarlyEvent.Text = winBoxProject.winBoxConfig.winmountedEarlyEvent ?? "";
            winmountedEarlyEvent.Enabled = winBoxProject.winBoxConfig.winmountedEarlyEnabled == true;
            winmountedEarly_breakbefore.Checked = winBoxProject.winBoxConfig.winmountedEarly_breakbefore == true;
            winmountedEarly_breakafter.Checked = winBoxProject.winBoxConfig.winmountedEarly_breakafter == true;

            recoveryMountedEarlyEnabled.Checked = winBoxProject.winBoxConfig.recoveryMountedEarlyEnabled == true;
            recoveryMountedEarlyEvent.Text = winBoxProject.winBoxConfig.recoveryMountedEarlyEvent ?? "";
            recoveryMountedEarlyEvent.Enabled = winBoxProject.winBoxConfig.recoveryMountedEarlyEnabled == true;
            recoveryMountedEarly_breakbefore.Checked = winBoxProject.winBoxConfig.recoveryMountedEarly_breakbefore == true;
            recoveryMountedEarly_breakafter.Checked = winBoxProject.winBoxConfig.recoveryMountedEarly_breakafter == true;

            schtasks_stopOrDelete.Text = winBoxProject.winBoxConfig.schtasks_stopOrDelete ?? "";
            schtasks_stopOrDelete_deleteFromList.Text = winBoxProject.winBoxConfig.schtasks_stopOrDelete_deleteFromList ?? "";
            schtasks_stopOrDeleteOnlyFromList.Checked = winBoxProject.winBoxConfig.schtasks_stopOrDeleteOnlyFromList == true;

            customdism_commands.Text = winBoxProject.winBoxConfig.customdism_commands ?? "";
            customdism_features.Text = winBoxProject.winBoxConfig.customdism_features ?? "";

            buildEnabled.Checked = winBoxProject.winBoxConfig.buildEnabled == true;
            downloadEnabled.Checked = winBoxProject.winBoxConfig.downloadEnabled == true;

            forceIot.Checked = winBoxProject.winBoxConfig.forceIot == true;
            enable_hibernation.Checked = winBoxProject.winBoxConfig.enable_hibernation == true;
            enable_hiberboot.Enabled = winBoxProject.winBoxConfig.enable_hibernation == true;
            enable_hiberboot.Checked = winBoxProject.winBoxConfig.enable_hiberboot == true;
            dc_use.Checked = winBoxProject.winBoxConfig.dc_use == true;
            UseCustomDisplaySettings.Checked = winBoxProject.winBoxConfig.UseCustomDisplaySettings == true;
            UseCustomDisplaySettings_scale.Checked = winBoxProject.winBoxConfig.UseCustomDisplaySettings_scale == true;

            cds_width_use.Checked = winBoxProject.winBoxConfig.cds_width_use == true;
            cds_height_use.Checked = winBoxProject.winBoxConfig.cds_height_use == true;
            cds_orientation_use.Checked = winBoxProject.winBoxConfig.cds_orientation_use == true;
            cds_bitDepth_use.Checked = winBoxProject.winBoxConfig.cds_bitDepth_use == true;
            cds_refreshRate_use.Checked = winBoxProject.winBoxConfig.cds_refreshRate_use == true;

            cds_orientation.SelectedIndex = winBoxProject.winBoxConfig.cds_orientation ?? 0;
            firstBootAction.SelectedIndex = (int)(winBoxProject.winBoxConfig.firstBootAction ?? 0);
            comboBox1.SelectedIndex = (int)(winBoxProject.winBoxConfig.powerScheme ?? 0);

            action_closingLaptop.SelectedIndex = (int)(winBoxProject.winBoxConfig.action_closingLaptop ?? 0);
            action_powerButton.SelectedIndex = (int)(winBoxProject.winBoxConfig.action_powerButton ?? 0);
            action_sleepButton.SelectedIndex = (int)(winBoxProject.winBoxConfig.action_sleepButton ?? 0);
            action_closingLaptop_dc.SelectedIndex = (int)(winBoxProject.winBoxConfig.action_closingLaptop_dc ?? 0);
            action_powerButton_dc.SelectedIndex = (int)(winBoxProject.winBoxConfig.action_powerButton_dc ?? 0);
            action_sleepButton_dc.SelectedIndex = (int)(winBoxProject.winBoxConfig.action_sleepButton_dc ?? 0);

            img_size.Text = winBoxProject.winBoxConfig.img_size.ToString();
            img_install_ram.Text = winBoxProject.winBoxConfig.img_install_ram.ToString();
            img_install_cpu.Text = winBoxProject.winBoxConfig.img_install_cpu.ToString();

            img_shutdownAfterInstall.Checked = winBoxProject.winBoxConfig.img_shutdownAfterInstall == true;
            img_runningPostinstallOnFirstRealStartup.Checked = winBoxProject.winBoxConfig.img_runningPostinstallOnFirstRealStartup == true;
            img_generalizeAfterInstall.Checked = winBoxProject.winBoxConfig.img_generalizeAfterInstall == true;

            img_runningPostinstallOnFirstRealStartup.Enabled = winBoxProject.winBoxConfig.img_shutdownAfterInstall == true;
            img_generalizeAfterInstall.Enabled = winBoxProject.winBoxConfig.img_shutdownAfterInstall == true;

            dc_panel.Enabled = winBoxProject.winBoxConfig.dc_use == true;
            HibernateTimeout.Enabled = winBoxProject.winBoxConfig.enable_hibernation == true;
            HibernateTimeout_dc.Enabled = winBoxProject.winBoxConfig.enable_hibernation == true;
            CustomDisplaySettings_panel.Enabled = winBoxProject.winBoxConfig.UseCustomDisplaySettings == true;
            UseCustomDisplaySettings_scale_panel.Enabled = winBoxProject.winBoxConfig.UseCustomDisplaySettings_scale == true;

            cds_width.Enabled = winBoxProject.winBoxConfig.cds_width_use == true;
            cds_height.Enabled = winBoxProject.winBoxConfig.cds_height_use == true;
            cds_orientation.Enabled = winBoxProject.winBoxConfig.cds_orientation_use == true;
            cds_bitDepth.Enabled = winBoxProject.winBoxConfig.cds_bitDepth_use == true;
            cds_refreshRate.Enabled = winBoxProject.winBoxConfig.cds_refreshRate_use == true;

            computername.Text = winBoxProject.winBoxConfig.computername;
            actionAtEndOfApplication_command.Text = winBoxProject.winBoxConfig.actionAtEndOfApplication_command;
            computername_use.Checked = winBoxProject.winBoxConfig.computername_use == true;
            computername.Enabled = winBoxProject.winBoxConfig.computername_use == true;
            actionAtEndOfApplication_command.Enabled = winBoxProject.winBoxConfig.actionAtEndOfApplication == ActionAtEndOfApplication.execute_command;

            appdelay_time.Checked = winBoxProject.winBoxConfig.appdelay_time == true;
            appdelay_internet.Checked = winBoxProject.winBoxConfig.appdelay_internet == true;

            appdelay_time_value.Text = winBoxProject.winBoxConfig.appdelay_time_value.ToString();
            appcrash_time_value.Text = winBoxProject.winBoxConfig.appcrash_time_value.ToString();
            appdelay_internet_requestdelay.Text = winBoxProject.winBoxConfig.appdelay_internet_requestdelay.ToString();
            appdelay_internet_checkurl.Text = winBoxProject.winBoxConfig.appdelay_internet_checkurl;

            appdelay_time_value.Enabled = winBoxProject.winBoxConfig.appdelay_time == true;
            appcrash_time_value.Enabled = winBoxProject.winBoxConfig.appcrash_time == true;
            appdelay_internet_checkurl.Enabled = winBoxProject.winBoxConfig.appdelay_internet == true;
            appdelay_internet_requestdelay.Enabled = winBoxProject.winBoxConfig.appdelay_internet == true;

            CustomBootLogo_UseLogoBeforeApp.Checked = winBoxProject.winBoxConfig.CustomBootLogo_UseLogoBeforeApp == true;
            wait_before_app_logo.Checked = winBoxProject.winBoxConfig.wait_before_app_logo == true;
            logoBeforeApp_panel.Enabled = winBoxProject.winBoxConfig.CustomBootLogo_UseLogoBeforeApp != true;

            logoBeforeApp.Text = winBoxProject.winBoxConfig.logoBeforeApp ?? "not selected";
            logoBeforeApp_stretch.SelectedIndex = (int)(winBoxProject.winBoxConfig.logoBeforeApp_stretch ?? 0);

            delete_paths.Text = winBoxProject.winBoxConfig.delete_paths ?? "";
            delete_dism.Text = winBoxProject.winBoxConfig.delete_dism ?? "";
            delete_dism_universal.Text = winBoxProject.winBoxConfig.delete_dism_universal ?? "";
            delete_dism_remove_package.Text = winBoxProject.winBoxConfig.delete_dism_remove_package ?? "";
            delete_dism_remove_appx_package.Text = winBoxProject.winBoxConfig.delete_dism_remove_appx_package ?? "";

            aaf_readme_iso.Checked = winBoxProject.winBoxConfig.aaf_readme_iso == true;
            aaf_readme_system.Checked = winBoxProject.winBoxConfig.aaf_readme_system == true;
            aaf_readme_boot.Checked = winBoxProject.winBoxConfig.aaf_readme_boot == true;
            aaf_readme_recovery.Checked = winBoxProject.winBoxConfig.aaf_readme_recovery == true;
            aaf_info_iso.Checked = winBoxProject.winBoxConfig.aaf_info_iso == true;
            aaf_info_system.Checked = winBoxProject.winBoxConfig.aaf_info_system == true;
            aaf_info_boot.Checked = winBoxProject.winBoxConfig.aaf_info_boot == true;
            aaf_info_recovery.Checked = winBoxProject.winBoxConfig.aaf_info_recovery == true;

            oemkey_installer.Checked = winBoxProject.winBoxConfig.oemkey_installer == true;
            oemkey_dism.Checked = winBoxProject.winBoxConfig.oemkey_dism == true;
            oemkey_slmgr.Checked = winBoxProject.winBoxConfig.oemkey_slmgr == true;

            DynamicDaylightTimeDisabled.Checked = winBoxProject.winBoxConfig.DynamicDaylightTimeDisabled == true;
            DisableNtp.Checked = winBoxProject.winBoxConfig.DisableNtp == true;
            RealTimeIsUniversal.Checked = winBoxProject.winBoxConfig.RealTimeIsUniversal == true;

            recoveryMenuAction.SelectedIndex = (int)(winBoxProject.winBoxConfig.recoveryMenuAction ?? 0);
            bool replaceRecoveryFlag = winBoxProject.winBoxConfig.recoveryMenuAction == RecoveryMenuAction.Replace;
            ReplaceRecovery.Enabled = replaceRecoveryFlag;
            ReplaceRecovery_sel.Enabled = replaceRecoveryFlag;
            ReplaceRecovery_clr.Enabled = replaceRecoveryFlag;

            bool manual = winBoxProject.winBoxConfig.manual_setup == true;
            manual_setup.Checked = manual;
            manual_setup_complete.Text = winBoxProject.winBoxConfig.manual_setup_complete ?? "not selected";
            manual_setup_error.Text = winBoxProject.winBoxConfig.manual_setup_error ?? "not selected";
            manual_setup_autounattend.Text = winBoxProject.winBoxConfig.manual_setup_autounattend ?? "not selected";
            manual_setup_sysunattend.Text = winBoxProject.winBoxConfig.manual_setup_sysunattend ?? "not selected";
            manual_setup_panel.Enabled = manual;

            recoverymod_manual_allow.Checked = winBoxProject.winBoxConfig.recoverymod_manual_allow == true;
            installermod_manual_allow.Checked = winBoxProject.winBoxConfig.installermod_manual_allow == true;
            recoverypanel.Enabled = !manual || winBoxProject.winBoxConfig.recoverymod_manual_allow == true;
            installerpanel.Enabled = !manual || winBoxProject.winBoxConfig.installermod_manual_allow == true;

            UpdateGuiCurrentServices();
            UpdateGuiCurrentSchtasks();

            services_stop.Text = winBoxProject.winBoxConfig.services_stop ?? "";
            services_start.Text = winBoxProject.winBoxConfig.services_start ?? "";
            services_deleteFromList.Text = winBoxProject.winBoxConfig.services_deleteFromList ?? "";
            services_stopOnlyList.Checked = winBoxProject.winBoxConfig.services_stopOnlyList == true;
            services_startOnlyList.Checked = winBoxProject.winBoxConfig.services_startOnlyList == true;

            bsod_autoreboot.Checked = winBoxProject.winBoxConfig.bsod_autoreboot == true;
            bsod_disabledisplay.Checked = winBoxProject.winBoxConfig.bsod_disabledisplay == true;
            ChangeTimezone.Checked = winBoxProject.winBoxConfig.ChangeTimezone == true;

            TimeZoneKeyName.Enabled = ChangeTimezone.Checked;

            ReplaceRecovery.Text = winBoxProject.winBoxConfig.ReplaceRecovery ?? "";
            EnableRecovery.Checked = winBoxProject.winBoxConfig.EnableRecovery == true;

            CustomBootLogo_UseOnBootres.Checked = winBoxProject.winBoxConfig.CustomBootLogo_UseOnBootres == true;
            bootresRepacking_logoPath.Text = winBoxProject.winBoxConfig.bootresRepacking_logoPath ?? "not selected";

            bootresRepacking_panel.Enabled = winBoxProject.winBoxConfig.CustomBootLogo_UseOnBootres != true;

            recovery_winPE_mod_en.Checked = winBoxProject.winBoxConfig.recovery_winPE_mod.enabled == true;
            recovery_winPE_mod.Enabled = winBoxProject.winBoxConfig.recovery_winPE_mod.enabled == true;
            installer_winPE_mod_en.Checked = winBoxProject.winBoxConfig.installer_winPE_mod.enabled == true;
            installer_winPE_mod.Enabled = winBoxProject.winBoxConfig.installer_winPE_mod.enabled == true;

            install_bypass.Checked = winBoxProject.winBoxConfig.install_bypass == true;
            checkBox1.Checked = winBoxProject.winBoxConfig.AllowStartRecoveryFromBootloader == true;

            tab_app.Enabled = !manual;
            tab_settings.Enabled = !manual;
            postinstall_panel_system.Enabled = !manual;
            postinstall_panel_user.Enabled = !manual;
            img_shutdownAfterInstall.Enabled = !manual;
            if (manual)
            {
                img_runningPostinstallOnFirstRealStartup.Enabled = false;
                img_generalizeAfterInstall.Enabled = false;
            }

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

            switch (winBoxProject.winBoxConfig.actionAtEndOfApplication)
            {
                case ActionAtEndOfApplication.none:
                    ActionAtEndOfApplication_none.Checked = true;
                    break;

                case ActionAtEndOfApplication.restart_app:
                    ActionAtEndOfApplication_restart_app.Checked = true;
                    break;

                case ActionAtEndOfApplication.execute_command:
                    ActionAtEndOfApplication_execute_command.Checked = true;
                    break;

                case ActionAtEndOfApplication.shutdown_computer:
                    ActionAtEndOfApplication_shutdown_computer.Checked = true;
                    break;

                case ActionAtEndOfApplication.reboot_computer:
                    ActionAtEndOfApplication_reboot_computer.Checked = true;
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
            UpdateGuiAfterWindowsLoaded();
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
            taskbarManager.SetProgressState(Value == 0 ? TaskbarProgressBarState.NoProgress : TaskbarProgressBarState.Normal, this.Handle);
            //taskbarManager.SetProgressState(TaskbarProgressBarState.Normal);
            taskbarManager.SetProgressValue(Value, 100, this.Handle);
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
                winBoxProject.updateActionAtEndOfApplication();
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ProgramType_RawCommand_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_RawCommand.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.RawCommand;
                winBoxProject.updateActionAtEndOfApplication();
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ProgramType_WebSite_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_WebSite.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.WebSite;
                winBoxProject.updateActionAtEndOfApplication();
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ProgramType_None_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (ProgramType_None.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.None;
                winBoxProject.updateActionAtEndOfApplication();
                winBoxProject.SaveConfig();
                UpdateGui();
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
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.batFilter, winBoxProject.resourcesDirectoryPath, true);
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
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.batFilter, winBoxProject.resourcesDirectoryPath, true);
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

        int checkPowercfgTimeoutNumber(int oldNumber, string text)
        {
            try
            {
                return Math.Clamp(int.Parse(text), 0, 2147483647);
            }
            catch (FormatException) { }
            catch (OverflowException) { }
            return oldNumber;
        }

        private void ScreenTimeout_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ScreenTimeout = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.ScreenTimeout ?? 0, ScreenTimeout.Text);
            if (winBoxProject.winBoxConfig.ScreenTimeout.ToString() != ScreenTimeout.Text)
                ScreenTimeout.Text = winBoxProject.winBoxConfig.ScreenTimeout.ToString();
            winBoxProject.SaveConfig();
        }

        private void StandbyTimeout_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.StandbyTimeout = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.StandbyTimeout ?? 0, StandbyTimeout.Text);
            if (winBoxProject.winBoxConfig.StandbyTimeout.ToString() != StandbyTimeout.Text)
                StandbyTimeout.Text = winBoxProject.winBoxConfig.StandbyTimeout.ToString();
            winBoxProject.SaveConfig();
        }

        private void HibernateTimeout_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.HibernateTimeout = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.HibernateTimeout ?? 0, HibernateTimeout.Text);
            if (winBoxProject.winBoxConfig.HibernateTimeout.ToString() != HibernateTimeout.Text)
                HibernateTimeout.Text = winBoxProject.winBoxConfig.HibernateTimeout.ToString();
            winBoxProject.SaveConfig();
        }

        private void ScreenTimeout_dc_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ScreenTimeout_dc = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.ScreenTimeout_dc ?? 0, ScreenTimeout_dc.Text);
            if (winBoxProject.winBoxConfig.ScreenTimeout_dc.ToString() != ScreenTimeout_dc.Text)
                ScreenTimeout_dc.Text = winBoxProject.winBoxConfig.ScreenTimeout_dc.ToString();
            winBoxProject.SaveConfig();
        }

        private void StandbyTimeout_dc_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.StandbyTimeout_dc = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.StandbyTimeout_dc ?? 0, StandbyTimeout_dc.Text);
            if (winBoxProject.winBoxConfig.StandbyTimeout_dc.ToString() != StandbyTimeout_dc.Text)
                StandbyTimeout_dc.Text = winBoxProject.winBoxConfig.StandbyTimeout_dc.ToString();
            winBoxProject.SaveConfig();
        }

        private void HibernateTimeout_dc_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.HibernateTimeout_dc = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.HibernateTimeout_dc ?? 0, HibernateTimeout_dc.Text);
            if (winBoxProject.winBoxConfig.HibernateTimeout_dc.ToString() != HibernateTimeout_dc.Text)
                HibernateTimeout_dc.Text = winBoxProject.winBoxConfig.HibernateTimeout_dc.ToString();
            winBoxProject.SaveConfig();
        }

        private void TweakList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (softwareCheck) return;

            int index = e.Index;
            string title = TweakList.Items[index].ToString();
            bool state = e.NewValue == CheckState.Checked;
            Program.setTweakEnabled(winBoxProject.winBoxConfig, title, state);
            winBoxProject.SaveConfig();
            UpdateGuiCurrentServices();
            UpdateGui();
        }

        private void keyboard_filter_blockList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (softwareCheck) return;

            int index = e.Index;
            string title = keyboard_filter_blockList.Items[index].ToString();
            bool state = e.NewValue == CheckState.Checked;
            Program.setCheckEnabled(winBoxProject.winBoxConfig.keyboard_filter_blockList, title, state);
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void keyboard_filter_blockList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void CustomBootLogo_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.imageFilter, winBoxProject.resourcesDirectoryPath, true);
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

            winBoxProject.winBoxConfig.AddVirtualDisplay = AddVirtualDisplay.Checked;
            winBoxProject.SaveConfig();
        }

        int checkResolutionNumber(int oldNumber, string text)
        {
            try
            {
                return Math.Clamp(int.Parse(text), 0, 65535);
            }
            catch (FormatException) { }
            catch (OverflowException) { }
            return oldNumber;
        }

        int checkSizeNumber(int oldNumber, string text, int maxVal = 1024 * 1024 * 1024)
        {
            try
            {
                return Math.Clamp(int.Parse(text), 0, maxVal);
            }
            catch (FormatException) { }
            catch (OverflowException) { }
            return oldNumber;
        }

        private void VirtualDisplayWidth_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.VirtualDisplayWidth = checkResolutionNumber(winBoxProject.winBoxConfig.VirtualDisplayWidth ?? 0, VirtualDisplayWidth.Text);
            if (winBoxProject.winBoxConfig.VirtualDisplayWidth.ToString() != VirtualDisplayWidth.Text)
                VirtualDisplayWidth.Text = winBoxProject.winBoxConfig.VirtualDisplayWidth.ToString();
            winBoxProject.SaveConfig();
        }

        private void VirtualDisplayHeight_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.VirtualDisplayHeight = checkResolutionNumber(winBoxProject.winBoxConfig.VirtualDisplayHeight ?? 0, VirtualDisplayHeight.Text);
            if (winBoxProject.winBoxConfig.VirtualDisplayHeight.ToString() != VirtualDisplayHeight.Text)
                VirtualDisplayHeight.Text = winBoxProject.winBoxConfig.VirtualDisplayHeight.ToString();
            winBoxProject.SaveConfig();
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

            winBoxProject.winBoxConfig.UseEmbeddedDisplay = UseEmbeddedDisplay.Checked;
            winBoxProject.SaveConfig();
        }

        private void CustomBootLogo_centering_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.CustomBootLogo_centering = CustomBootLogo_centering.Checked;
            winBoxProject.SaveConfig();
        }

        private void prebuildEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.prebuildEnabled = prebuildEnabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
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

            winBoxProject.winBoxConfig.postbuildEnabled = postbuildEnabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
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

            winBoxProject.winBoxConfig.winmountedEnabled = winmountedEnabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
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

            winBoxProject.winBoxConfig.downloadEnabled = downloadEnabled.Checked;
            winBoxProject.SaveConfig();
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

            currentDownloadItem.cache = dl_cache.Checked;
            winBoxProject.SaveConfig();
        }

        private void dl_unpack_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentDownloadItem.unpack = dl_unpack.Checked;
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

            winBoxProject.winBoxConfig.buildEnabled = buildEnabled.Checked;
            winBoxProject.SaveConfig();
        }

        private void bl_tabcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = bl_tabcontrol.SelectedTab;
            currentBuildItem.type = (BuildItemType)bl_tabcontrol.SelectedIndex;
            winBoxProject.SaveConfig();
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

        private void bl_folderInProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.folderInProject = bl_folderInProject.Text;
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

            currentBuildItem.subdirectory_enabled = bl_folder_enable.Checked;
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

        private void custom_path_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.custom_path = custom_path.Text;
            winBoxProject.SaveConfig();
        }

        private async void custom_path_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceFolderAsync(UpdateProcessName, UpdateProcessValue, winBoxProject.sourcesDirectoryPath);
            if (name != null)
            {
                currentBuildItem.custom_path = name;
                guiEventsLock = true;
                custom_path.Text = currentBuildItem.custom_path;
                guiEventsLock = false;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void custom_path_clear_Click(object sender, EventArgs e)
        {
            currentBuildItem.custom_path = "";
            guiEventsLock = true;
            custom_path.Text = "";
            guiEventsLock = false;
            winBoxProject.SaveConfig();
        }

        private void custom_command_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.custom_command = custom_command.Text;
            winBoxProject.SaveConfig();
        }

        private void forceIot_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.forceIot = forceIot.Checked;
            winBoxProject.SaveConfig();
        }

        private void enable_hibernation_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.enable_hibernation = enable_hibernation.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void dc_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.dc_use = dc_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void electron_packager_path_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.electron_packager_path = electron_packager_path.Text;
            winBoxProject.SaveConfig();
        }

        private async void electron_packager_path_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue,
                "Electron project (package.json) (*.json)|*.json|All files (*.*)|*.*",
                winBoxProject.sourcesDirectoryPath,
                true
            );
            if (name != null)
            {
                currentBuildItem.electron_packager_path = name;
                guiEventsLock = true;
                electron_packager_path.Text = currentBuildItem.electron_packager_path;
                guiEventsLock = false;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void electron_packager_path_clear_Click(object sender, EventArgs e)
        {
            currentBuildItem.electron_packager_path = "";
            guiEventsLock = true;
            electron_packager_path.Text = "";
            guiEventsLock = false;
            winBoxProject.SaveConfig();
        }

        private void electron_packager_name_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            currentBuildItem.electron_packager_name = electron_packager_name.Text;
            winBoxProject.SaveConfig();
        }

        private async void debugBuild_Click(object sender, EventArgs e)
        {
            LockForm();
            await winBoxProject.debugBuildProgramsAsync(UpdateProcessName, UpdateProcessValue);
            try
            {
                Process.Start("explorer.exe", winBoxProject.debugBuildProgramsPath);
            }
            catch (Exception ex)
            {
            }
            UnlockForm();
        }

        private void UseCustomDisplaySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.UseCustomDisplaySettings = UseCustomDisplaySettings.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_width_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_width = checkResolutionNumber(winBoxProject.winBoxConfig.cds_width ?? 0, cds_width.Text);
            if (winBoxProject.winBoxConfig.cds_width.ToString() != cds_width.Text)
                cds_width.Text = winBoxProject.winBoxConfig.cds_width.ToString();
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_height_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_height = checkResolutionNumber(winBoxProject.winBoxConfig.cds_height ?? 0, cds_height.Text);
            if (winBoxProject.winBoxConfig.cds_height.ToString() != cds_height.Text)
                cds_height.Text = winBoxProject.winBoxConfig.cds_height.ToString();
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_bitDepth_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_bitDepth = checkResolutionNumber(winBoxProject.winBoxConfig.cds_bitDepth ?? 0, cds_bitDepth.Text);
            if (winBoxProject.winBoxConfig.cds_bitDepth.ToString() != cds_bitDepth.Text)
                cds_bitDepth.Text = winBoxProject.winBoxConfig.cds_bitDepth.ToString();
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_refreshRate_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_refreshRate = checkResolutionNumber(winBoxProject.winBoxConfig.cds_refreshRate ?? 0, cds_refreshRate.Text);
            if (winBoxProject.winBoxConfig.cds_refreshRate.ToString() != cds_refreshRate.Text)
                cds_refreshRate.Text = winBoxProject.winBoxConfig.cds_refreshRate.ToString();
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_scaling_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_scaling = checkResolutionNumber(winBoxProject.winBoxConfig.cds_scaling ?? 0, cds_scaling.Text);
            if (winBoxProject.winBoxConfig.cds_scaling.ToString() != cds_scaling.Text)
                cds_scaling.Text = winBoxProject.winBoxConfig.cds_scaling.ToString();
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_orientation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_orientation = cds_orientation.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void firstBootAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.firstBootAction = (FirstBootActionEnum)firstBootAction.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void UseCustomDisplaySettings_scale_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.UseCustomDisplaySettings_scale = UseCustomDisplaySettings_scale.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void action_closingLaptop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.action_closingLaptop = (ButtonAction)action_closingLaptop.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void action_powerButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.action_powerButton = (ButtonAction)action_powerButton.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void action_sleepButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.action_sleepButton = (ButtonAction)action_sleepButton.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void action_closingLaptop_dc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.action_closingLaptop_dc = (ButtonAction)action_closingLaptop_dc.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void action_powerButton_dc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.action_powerButton_dc = (ButtonAction)action_powerButton_dc.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void action_sleepButton_dc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.action_sleepButton_dc = (ButtonAction)action_sleepButton_dc.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void cds_width_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_width_use = cds_width_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_height_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_height_use = cds_height_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_orientation_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_orientation_use = cds_orientation_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_bitDepth_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_bitDepth_use = cds_bitDepth_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void cds_refreshRate_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.cds_refreshRate_use = cds_refreshRate_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        async Task exportImgWindow(bool useUefi)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installed (*.img)|*.img";
                saveFileDialog.Title = $"Save you WinBox installed .img ({(useUefi ? "UEFI" : "BIOS")}) ({winBoxProject.winBoxConfig.WinboxName})";
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
                    await winBoxProject.BuildImgAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription, useUefi);
                }
            }
            UnlockForm();
        }

        private async void ExportImg_Click(object sender, EventArgs e)
        {
            await exportImgWindow(false);
        }

        private async void ExportImgUefi_Click(object sender, EventArgs e)
        {
            await exportImgWindow(true);
        }

        private void img_size_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.img_size = checkSizeNumber(winBoxProject.winBoxConfig.img_size ?? 0, img_size.Text);
            if (winBoxProject.winBoxConfig.img_size.ToString() != img_size.Text)
                img_size.Text = winBoxProject.winBoxConfig.img_size.ToString();
            winBoxProject.SaveConfig();
        }

        private void img_install_ram_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.img_install_ram = checkSizeNumber(winBoxProject.winBoxConfig.img_install_ram ?? 0, img_install_ram.Text);
            if (winBoxProject.winBoxConfig.img_install_ram.ToString() != img_install_ram.Text)
                img_install_ram.Text = winBoxProject.winBoxConfig.img_install_ram.ToString();
            winBoxProject.SaveConfig();
        }

        private void img_install_cpu_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.img_install_cpu = checkSizeNumber(winBoxProject.winBoxConfig.img_install_cpu ?? 0, img_install_cpu.Text);
            if (winBoxProject.winBoxConfig.img_install_cpu.ToString() != img_install_cpu.Text)
                img_install_cpu.Text = winBoxProject.winBoxConfig.img_install_cpu.ToString();
            winBoxProject.SaveConfig();
        }

        private void img_shutdownAfterInstall_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.img_shutdownAfterInstall = img_shutdownAfterInstall.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void img_runningPostinstallOnFirstRealStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.img_runningPostinstallOnFirstRealStartup = img_runningPostinstallOnFirstRealStartup.Checked;
            winBoxProject.SaveConfig();
        }

        private void computername_use_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.computername_use = computername_use.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void computername_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.computername = computername.Text;
            winBoxProject.SaveConfig();
        }

        private void img_generalizeAfterInstall_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.img_generalizeAfterInstall = img_generalizeAfterInstall.Checked;
            winBoxProject.SaveConfig();
        }

        private void actionAtEndOfApplication_command_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.actionAtEndOfApplication_command = actionAtEndOfApplication_command.Text;
            winBoxProject.SaveConfig();
        }

        private void ActionAtEndOfApplication_none_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked)
            {
                winBoxProject.winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.none;
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ActionAtEndOfApplication_restart_app_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked)
            {
                winBoxProject.winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.restart_app;
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ActionAtEndOfApplication_execute_command_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked)
            {
                winBoxProject.winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.execute_command;
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ActionAtEndOfApplication_reboot_computer_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked)
            {
                winBoxProject.winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.reboot_computer;
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void ActionAtEndOfApplication_shutdown_computer_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            RadioButton checkBox = (RadioButton)sender;
            if (checkBox.Checked)
            {
                winBoxProject.winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.shutdown_computer;
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private void appdelay_time_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appdelay_time = appdelay_time.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void appdelay_internet_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appdelay_internet = appdelay_internet.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void appdelay_time_value_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appdelay_time_value = checkSizeNumber(winBoxProject.winBoxConfig.appdelay_time_value ?? 0, appdelay_time_value.Text, 99999);
            if (winBoxProject.winBoxConfig.appdelay_time_value.ToString() != appdelay_time_value.Text)
                appdelay_time_value.Text = winBoxProject.winBoxConfig.appdelay_time_value.ToString();
            winBoxProject.SaveConfig();
        }

        private void appdelay_internet_checkurl_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appdelay_internet_checkurl = appdelay_internet_checkurl.Text;
            winBoxProject.SaveConfig();
        }

        private async void logoBeforeApp_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.imageFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.logoBeforeApp = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void logoBeforeApp_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.logoBeforeApp = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void logoBeforeApp_stretch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.logoBeforeApp_stretch = (StretchMode)logoBeforeApp_stretch.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void CustomBootLogo_UseLogoBeforeApp_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.CustomBootLogo_UseLogoBeforeApp = CustomBootLogo_UseLogoBeforeApp.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void appdelay_internet_requestdelay_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appdelay_internet_requestdelay = checkSizeNumber(winBoxProject.winBoxConfig.appdelay_internet_requestdelay ?? 0, appdelay_internet_requestdelay.Text, 99999);
            if (winBoxProject.winBoxConfig.appdelay_internet_requestdelay.ToString() != appdelay_internet_requestdelay.Text)
                appdelay_internet_requestdelay.Text = winBoxProject.winBoxConfig.appdelay_internet_requestdelay.ToString();
            winBoxProject.SaveConfig();
        }

        private void wait_before_app_logo_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.wait_before_app_logo = wait_before_app_logo.Checked;
            winBoxProject.SaveConfig();
        }

        private void DiskTimeout_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.DiskTimeout = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.DiskTimeout ?? 0, DiskTimeout.Text);
            if (winBoxProject.winBoxConfig.DiskTimeout.ToString() != DiskTimeout.Text)
                DiskTimeout.Text = winBoxProject.winBoxConfig.DiskTimeout.ToString();
            winBoxProject.SaveConfig();
        }

        private void DiskTimeout_dc_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.DiskTimeout_dc = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.DiskTimeout_dc ?? 0, DiskTimeout_dc.Text);
            if (winBoxProject.winBoxConfig.DiskTimeout_dc.ToString() != DiskTimeout_dc.Text)
                DiskTimeout_dc.Text = winBoxProject.winBoxConfig.DiskTimeout_dc.ToString();
            winBoxProject.SaveConfig();
        }

        private void delete_paths_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.delete_paths = delete_paths.Text;
            winBoxProject.SaveConfig();
        }

        private void delete_dism_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.delete_dism = delete_dism.Text;
            winBoxProject.SaveConfig();
        }

        private void delete_dism_remove_package_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.delete_dism_remove_package = delete_dism_remove_package.Text;
            winBoxProject.SaveConfig();
        }

        private void delete_dism_remove_appx_package_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.delete_dism_remove_appx_package = delete_dism_remove_appx_package.Text;
            winBoxProject.SaveConfig();
        }

        private void delete_dism_universal_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.delete_dism_universal = delete_dism_universal.Text;
            winBoxProject.SaveConfig();
        }

        private void manual_setup_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.manual_setup = manual_setup.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void manual_setup_complete_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.batFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.manual_setup_complete = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void manual_setup_complete_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.manual_setup_complete = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void manual_setup_error_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.batFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.manual_setup_error = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void manual_setup_error_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.manual_setup_error = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void manual_setup_autounattend_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.xmlFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.manual_setup_autounattend = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void manual_setup_autounattend_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.manual_setup_autounattend = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void aaf_readme_iso_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_readme_iso = aaf_readme_iso.Checked;
            winBoxProject.SaveConfig();
        }

        private void aaf_readme_system_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_readme_system = aaf_readme_system.Checked;
            winBoxProject.SaveConfig();
        }

        private void aaf_readme_boot_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_readme_boot = aaf_readme_boot.Checked;
            winBoxProject.SaveConfig();
        }

        private void aaf_info_iso_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_info_iso = aaf_info_iso.Checked;
            winBoxProject.SaveConfig();
        }

        private void aaf_info_system_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_info_system = aaf_info_system.Checked;
            winBoxProject.SaveConfig();
        }

        private void aaf_info_boot_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_info_boot = aaf_info_boot.Checked;
            winBoxProject.SaveConfig();
        }

        private async void onbuild_reg_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Registry files (*.reg)|*.reg|All files (*.*)|*.*", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.onbuild_reg = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void onbuild_reg_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.onbuild_reg = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void oemkey_installer_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.oemkey_installer = oemkey_installer.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void oemkey_dism_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.oemkey_dism = oemkey_dism.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void oemkey_slmgr_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.oemkey_slmgr = oemkey_slmgr.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void manual_setup_sysunattend_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.xmlFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.manual_setup_sysunattend = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void manual_setup_sysunattend_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.manual_setup_sysunattend = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void manual_setup_sysunattend_Click(object sender, EventArgs e)
        {

        }

        private void DynamicDaylightTimeDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.DynamicDaylightTimeDisabled = DynamicDaylightTimeDisabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void DisableNtp_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.DisableNtp = DisableNtp.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void services_stop_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.services_stop = services_stop.Text;
            winBoxProject.SaveConfig();
            UpdateGuiCurrentServices();
        }

        private void services_start_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.services_start = services_start.Text;
            winBoxProject.SaveConfig();
            UpdateGuiCurrentServices();
        }

        private void services_stopOnlyList_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.services_stopOnlyList = services_stopOnlyList.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void services_startOnlyList_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.services_startOnlyList = services_startOnlyList.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void RealTimeIsUniversal_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.RealTimeIsUniversal = RealTimeIsUniversal.Checked;
            winBoxProject.SaveConfig();
        }

        void updateTimeZoneKeyInConfig()
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.TimeZoneKeyName = TimeZoneKeyName.Text;
            winBoxProject.SaveConfig();
        }

        private void TimeZoneKeyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTimeZoneKeyInConfig();
        }

        private void TimeZoneKeyName_TextChanged(object sender, EventArgs e)
        {
            updateTimeZoneKeyInConfig();
        }

        private void customdism_commands_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.customdism_commands = customdism_commands.Text;
            winBoxProject.SaveConfig();
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void enable_hiberboot_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.enable_hiberboot = enable_hiberboot.Checked;
            winBoxProject.SaveConfig();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.powerScheme = (PowerScheme)comboBox1.SelectedIndex;
            winBoxProject.SaveConfig();
        }

        private void aaf_readme_recovery_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_readme_recovery = aaf_readme_recovery.Checked;
            winBoxProject.SaveConfig();
        }

        private void aaf_info_recovery_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.aaf_info_recovery = aaf_info_recovery.Checked;
            winBoxProject.SaveConfig();
        }

        private void recoverymod_manual_allow_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recoverymod_manual_allow = recoverymod_manual_allow.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // installermod_manual_allow
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.installermod_manual_allow = installermod_manual_allow.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recoveryMenuAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recoveryMenuAction = (RecoveryMenuAction)recoveryMenuAction.SelectedIndex;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void services_deleteFromList_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.services_deleteFromList = services_deleteFromList.Text;
            winBoxProject.SaveConfig();
            UpdateGuiCurrentServices();
        }

        private void customdism_features_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.customdism_features = customdism_features.Text;
            winBoxProject.SaveConfig();
        }

        private void bsod_autoreboot_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.bsod_autoreboot = bsod_autoreboot.Checked;
            winBoxProject.SaveConfig();
        }

        private void bsod_disabledisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.bsod_disabledisplay = bsod_disabledisplay.Checked;
            winBoxProject.SaveConfig();
        }

        private void ChangeTimezone_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ChangeTimezone = ChangeTimezone.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void ReplaceRecovery_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ReplaceRecovery = ReplaceRecovery.Text;
            winBoxProject.SaveConfig();
        }

        private async void ReplaceRecovery_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? recoveryPath = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.wimFilter, winBoxProject.resourcesDirectoryPath, false);
            if (recoveryPath != null)
            {
                winBoxProject.winBoxConfig.ReplaceRecovery = recoveryPath;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void ReplaceRecovery_clr_Click(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.ReplaceRecovery = "";
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void appcrash_time_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appcrash_time = appcrash_time.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void appcrash_time_value_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.appcrash_time_value = checkSizeNumber(winBoxProject.winBoxConfig.appcrash_time_value ?? 0, appcrash_time_value.Text, 99999);
            if (winBoxProject.winBoxConfig.appcrash_time_value.ToString() != appcrash_time_value.Text)
                appcrash_time_value.Text = winBoxProject.winBoxConfig.appcrash_time_value.ToString();
            winBoxProject.SaveConfig();
        }

        private void EnableRecovery_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.EnableRecovery = EnableRecovery.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void CustomBootLogo_UseOnBootres_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.CustomBootLogo_UseOnBootres = CustomBootLogo_UseOnBootres.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void bootresRepacking_logoPath_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.imageFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.bootresRepacking_logoPath = name;
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void bootresRepacking_logoPath_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.bootresRepacking_logoPath = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void prebuild_breakbefore_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.prebuild_breakbefore = prebuild_breakbefore.Checked;
            winBoxProject.SaveConfig();
        }

        private void prebuild_breakafter_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.prebuild_breakafter = prebuild_breakafter.Checked;
            winBoxProject.SaveConfig();
        }

        private void postbuild_breakbefore_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.postbuild_breakbefore = postbuild_breakbefore.Checked;
            winBoxProject.SaveConfig();
        }

        private void postbuild_breakafter_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.postbuild_breakafter = postbuild_breakafter.Checked;
            winBoxProject.SaveConfig();
        }

        private void winmounted_breakbefore_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmounted_breakbefore = winmounted_breakbefore.Checked;
            winBoxProject.SaveConfig();
        }

        private void winmounted_breakafter_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmounted_breakafter = winmounted_breakafter.Checked;
            winBoxProject.SaveConfig();
        }

        private void winmountedEarlyEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmountedEarlyEnabled = winmountedEarlyEnabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void winmountedEarlyEvent_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmountedEarlyEvent = winmountedEarlyEvent.Text;
            winBoxProject.SaveConfig();
        }

        private void winmountedEarly_breakbefore_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmountedEarly_breakbefore = winmountedEarly_breakbefore.Checked;
            winBoxProject.SaveConfig();
        }

        private void winmountedEarly_breakafter_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.winmountedEarly_breakafter = winmountedEarly_breakafter.Checked;
            winBoxProject.SaveConfig();
        }

        private void recoveryMountedEarlyEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recoveryMountedEarlyEnabled = recoveryMountedEarlyEnabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recoveryMountedEarlyEvent_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recoveryMountedEarlyEvent = recoveryMountedEarlyEvent.Text;
            winBoxProject.SaveConfig();
        }

        private void recoveryMountedEarly_breakbefore_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recoveryMountedEarly_breakbefore = recoveryMountedEarly_breakbefore.Checked;
            winBoxProject.SaveConfig();
        }

        private void recoveryMountedEarly_breakafter_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recoveryMountedEarly_breakafter = recoveryMountedEarly_breakafter.Checked;
            winBoxProject.SaveConfig();
        }

        private void installerMountedEarlyEvent_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.installerMountedEarlyEvent = installerMountedEarlyEvent.Text;
            winBoxProject.SaveConfig();
        }

        private void installerMountedEarlyEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.installerMountedEarlyEnabled = installerMountedEarlyEnabled.Checked;
            winBoxProject.SaveConfig();
        }

        private void installerMountedEarly_breakbefore_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.installerMountedEarly_breakbefore = installerMountedEarly_breakbefore.Checked;
            winBoxProject.SaveConfig();
        }

        private void tabPage57_Click(object sender, EventArgs e)
        {

        }

        private void installerMountedEarly_breakafter_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.installerMountedEarly_breakafter = installerMountedEarly_breakafter.Checked;
            winBoxProject.SaveConfig();
        }

        private void schtasks_stopOrDeleteOnlyFromList_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.schtasks_stopOrDeleteOnlyFromList = schtasks_stopOrDeleteOnlyFromList.Checked;
            winBoxProject.SaveConfig();
            UpdateGuiCurrentSchtasks();
        }

        private void schtasks_stopOrDelete_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.schtasks_stopOrDelete = schtasks_stopOrDelete.Text;
            winBoxProject.SaveConfig();
            UpdateGuiCurrentSchtasks();
        }

        private void schtasks_stopOrDelete_deleteFromList_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.schtasks_stopOrDelete_deleteFromList = schtasks_stopOrDelete_deleteFromList.Text;
            winBoxProject.SaveConfig();
            UpdateGuiCurrentSchtasks();
        }

        private void keyboard_layouts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void recovery_winPE_mod_en_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.recovery_winPE_mod.enabled = recovery_winPE_mod_en.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void installer_winPE_mod_en_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.installer_winPE_mod.enabled = installer_winPE_mod_en.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void installer_winPE_mod_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.installer_winPE_mod.openGui("Windows installer");
        }

        private void recovery_winPE_mod_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.recovery_winPE_mod.openGui("Windows recovery");
        }

        private void install_bypass_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.install_bypass = install_bypass.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.AllowStartRecoveryFromBootloader = checkBox1.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void keyboard_filter_blockList_reset_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.keyboard_filter_blockList = Program.default_keyboard_filter_blockList.ToList();
            resetKeyboardFilterBlockList();
        }

        private void keyboard_filter_enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.keyboard_filter_enabled = keyboard_filter_enabled.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void keyboard_filter_DisableKeyboardFilterForAdministrators_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.keyboard_filter_DisableKeyboardFilterForAdministrators = keyboard_filter_DisableKeyboardFilterForAdministrators.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void keyboard_filter_ForceOffAccessibility_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.keyboard_filter_ForceOffAccessibility = keyboard_filter_ForceOffAccessibility.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        void resetRegTweakOverwrite()
        {
            winBoxProject.winBoxConfig.regtweak_overwrite = System.IO.File.ReadAllText("resources/tweak.reg");
        }

        private void regtweak_overwrite_en_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (regtweak_overwrite_en.Checked && winBoxProject.winBoxConfig.regtweak_overwrite == null)
            {
                resetRegTweakOverwrite();
            }

            winBoxProject.winBoxConfig.regtweak_overwrite_en = regtweak_overwrite_en.Checked;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void regtweak_overwrite_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.regtweak_overwrite = regtweak_overwrite.Text;
            winBoxProject.SaveConfig();
        }

        private void regtweak_overwrite_reset_Click(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            resetRegTweakOverwrite();
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void keyboard_filter_BreakoutKeyScanCode_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.keyboard_filter_BreakoutKeyScanCode = checkPowercfgTimeoutNumber(winBoxProject.winBoxConfig.keyboard_filter_BreakoutKeyScanCode ?? 0, keyboard_filter_BreakoutKeyScanCode.Text);
            if (winBoxProject.winBoxConfig.keyboard_filter_BreakoutKeyScanCode.ToString() != keyboard_filter_BreakoutKeyScanCode.Text)
                keyboard_filter_BreakoutKeyScanCode.Text = winBoxProject.winBoxConfig.keyboard_filter_BreakoutKeyScanCode.ToString();
            winBoxProject.SaveConfig();
        }

        // -------------------------------------------- item lists

        // ------------- service functions

        void UpdateItemsList<T>(
            CheckedListBox listControl,
            List<T> items,
            ref int currentIndex,
            Action<T?> updateSelectedItem,
            string nameFieldName)
            where T : class
        {
            softwareCheck = true;

            T? lastItem = null;
            int lastIndex = -1;

            listControl.Items.Clear();
            foreach (T item in items)
            {
                string name = (item.GetType().GetProperty(nameFieldName)?.GetValue(item) as string) ?? "(no name)";
                lastIndex = listControl.Items.Add(name);
                lastItem = item;
            }
            if (lastIndex >= 0)
                listControl.SetItemChecked(lastIndex, true);

            softwareCheck = false;

            currentIndex = lastIndex;
            updateSelectedItem(lastItem);
        }

        void ItemCheckHandler<T>(
            CheckedListBox listControl,
            List<T> items,
            ref int currentIndex,
            Action<T?> updateSelectedItem,
            ItemCheckEventArgs e)
            where T : class
        {
            if (softwareCheck) return;

            softwareCheck = true;
            bool state = e.NewValue == CheckState.Checked;

            if (state)
            {
                int index = e.Index;
                for (int i = 0; i < listControl.Items.Count; i++)
                {
                    listControl.SetItemChecked(i, i == index);
                }

                currentIndex = index;
                updateSelectedItem(items[index]);
            }
            else
            {
                currentIndex = -1;
                updateSelectedItem(null);
            }

            softwareCheck = false;
        }

        enum ItemAction
        {
            Remove,
            MoveToTop,
            MoveToBottom,
            MoveUp,
            MoveDown
        }

        void doItemAction<T>(CheckedListBox listControl, List<T> list, T item, ItemAction action, Action saveConfig, Action updateList)
        {
            if (!list.Contains(item)) return;

            switch (action)
            {
                case ItemAction.Remove:
                    list.Remove(item);
                    break;

                case ItemAction.MoveToTop:
                    list.Remove(item);
                    list.Insert(0, item);
                    break;

                case ItemAction.MoveToBottom:
                    list.Remove(item);
                    list.Add(item);
                    break;

                case ItemAction.MoveUp:
                    {
                        int index = list.IndexOf(item);
                        if (index > 0)
                        {
                            list.RemoveAt(index);
                            list.Insert(index - 1, item);
                        }
                    }
                    break;

                case ItemAction.MoveDown:
                    {
                        int index = list.IndexOf(item);
                        if (index >= 0 && index < list.Count - 1)
                        {
                            list.RemoveAt(index);
                            list.Insert(index + 1, item);
                        }
                    }
                    break;
            }

            saveConfig?.Invoke();
            updateList?.Invoke();

            if (action != ItemAction.Remove)
            {
                int newIndex = list.IndexOf(item);
                for (int i = 0; i < listControl.Items.Count; i++)
                {
                    listControl.SetItemChecked(i, i == newIndex);
                }
            }
        }

        // ------------- update lists

        void UpdateBuildItemsList()
        {
            UpdateItemsList(
                BuildItems,
                winBoxProject.winBoxConfig.BuildItems,
                ref currentBuildItemIndex,
                UpdateSelectedBuildItem,
                "name"
            );
        }

        void UpdateDownloadItemsList()
        {
            UpdateItemsList(
                DownloadItems,
                winBoxProject.winBoxConfig.DownloadItems,
                ref currentDownloadItemIndex,
                UpdateSelectedDownloadItem,
                "name"
            );
        }

        void UpdateKeyboardLayoutsList()
        {
            int currentIndex = -1;
            UpdateItemsList(
                keyboard_layouts,
                winBoxProject.winBoxConfig.keyboard_layouts,
                ref currentIndex,
                UpdateSelectedKeyboardLayout,
                "string1"
            );
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
                bl_folderInProject.Text = buildItem.folderInProject ?? "";

                cmake_path.Text = buildItem.cmake_path ?? "";
                cargo_path.Text = buildItem.cargo_path ?? "";
                electron_packager_path.Text = buildItem.electron_packager_path ?? "";
                electron_packager_name.Text = buildItem.electron_packager_name ?? "";
                cmake_configuration.Text = buildItem.cmake_configuration ?? "";
                custom_path.Text = buildItem.custom_path ?? "";
                custom_command.Text = buildItem.custom_command ?? "";
                bl_tabcontrol.SelectedIndex = (int)currentBuildItem.type;
                bl_folder.Text = buildItem.subdirectory ?? "";
                bl_folder_enable.Checked = buildItem.subdirectory_enabled == true;
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
                dl_cache.Checked = downloadItem.cache == true;
                dl_unpack.Checked = downloadItem.unpack == true;
                guiEventsLock = false;
            }
        }

        void UpdateSelectedKeyboardLayout(TwoStrings? twoString)
        {
            current_keyboard_layout = twoString;
            if (twoString == null)
            {
                keyboard_layouts_setupPanel.Visible = false;
            }
            else
            {
                keyboard_layouts_setupPanel.Visible = true;
                guiEventsLock = true;
                keyboard_layouts_name.Text = twoString.string1 ?? "";
                keyboard_layouts_id.Text = twoString.string2 ?? "";
                guiEventsLock = false;
            }
        }

        // ------------- deletion

        void bl_delete_Click(object sender, EventArgs e)
        {
            doItemAction(
                BuildItems,
                winBoxProject.winBoxConfig.BuildItems,
                currentBuildItem,
                ItemAction.Remove,
                winBoxProject.SaveConfig,
                UpdateBuildItemsList
            );
        }

        private void dl_delete_Click(object sender, EventArgs e)
        {
            doItemAction(
                DownloadItems,
                winBoxProject.winBoxConfig.DownloadItems,
                currentDownloadItem,
                ItemAction.Remove,
                winBoxProject.SaveConfig,
                UpdateDownloadItemsList
            );
        }

        private void keyboard_layouts_remove_Click(object sender, EventArgs e)
        {
            doItemAction(
                keyboard_layouts,
                winBoxProject.winBoxConfig.keyboard_layouts,
                current_keyboard_layout,
                ItemAction.Remove,
                winBoxProject.SaveConfig,
                UpdateKeyboardLayoutsList
            );
        }

        // ------------- item check

        private void BuildItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ItemCheckHandler(
                BuildItems,
                winBoxProject.winBoxConfig.BuildItems,
                ref currentBuildItemIndex,
                UpdateSelectedBuildItem,
                e
            );
        }

        private void DownloadItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ItemCheckHandler(
                DownloadItems,
                winBoxProject.winBoxConfig.DownloadItems,
                ref currentDownloadItemIndex,
                UpdateSelectedDownloadItem,
                e
            );
        }

        private void keyboard_layouts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int currentIndex = -1;
            ItemCheckHandler(
                keyboard_layouts,
                winBoxProject.winBoxConfig.keyboard_layouts,
                ref currentIndex,
                UpdateSelectedKeyboardLayout,
                e
            );
        }

        // ------------- add

        private void addBuild_Click(object sender, EventArgs e)
        {
            BuildItem buildItem = new BuildItem();
            buildItem.initDefaults();
            buildItem.name = $"build item {winBoxProject.winBoxConfig.BuildItems.Count() + 1}";

            winBoxProject.winBoxConfig.buildEnabled = true;
            winBoxProject.winBoxConfig.BuildItems.Add(buildItem);
            winBoxProject.SaveConfig();
            UpdateBuildItemsList();
            UpdateGui();
        }

        private void addDownload_Click(object sender, EventArgs e)
        {
            DownloadItem downloadItem = new DownloadItem();
            downloadItem.name = $"download item {winBoxProject.winBoxConfig.DownloadItems.Count() + 1}";
            downloadItem.url = "https://raw.githubusercontent.com/igorkll/trashfolder/refs/heads/main/sound3/1.mp3";
            downloadItem.path = "winbox_temp/files/DIRECTORIES ARE/CREATED AUTOMATICALLY/example.mp3";
            downloadItem.cache = true;
            downloadItem.unpack = false;

            winBoxProject.winBoxConfig.downloadEnabled = true;
            winBoxProject.winBoxConfig.DownloadItems.Add(downloadItem);
            winBoxProject.SaveConfig();
            UpdateDownloadItemsList();
            UpdateGui();
        }

        private void keyboard_layouts_add_Click(object sender, EventArgs e)
        {
            TwoStrings[] all_keyboard_layouts = winBoxProject.GetWindowsKeyboardLayouts();
            TwoStrings twoStrings = all_keyboard_layouts[keyboard_layouts_available.SelectedIndex];

            winBoxProject.winBoxConfig.keyboard_layouts.Add(twoStrings);
            winBoxProject.SaveConfig();
            UpdateKeyboardLayoutsList();
        }

        // ------------- control

        private void keyboard_layouts_makeDefault_Click(object sender, EventArgs e)
        {
            doItemAction(
                keyboard_layouts,
                winBoxProject.winBoxConfig.keyboard_layouts,
                current_keyboard_layout,
                ItemAction.MoveToTop,
                winBoxProject.SaveConfig,
                UpdateKeyboardLayoutsList
            );
        }
    }
}
