using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        const string defaultProcessName = "not busy";
        WinBoxProject winBoxProject;
        WindowsDescription[]? windowsDescriptions;
        bool softwareCheck = true;
        TaskbarManager taskbarManager;

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

            TweakList.Items.Clear();
            AddTweakToList("Integrate microsoft edge");
            AddTweakToList("Hide Cursor");
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
        }

        void AddTweakToList(String tweak)
        {
            TweakList.Items.Add(tweak, Program.isTweakEnabled(winBoxProject.winBoxConfig, tweak));
        }

        void UnlockForm()
        {
            ProcessName.Text = defaultProcessName;
            ProcessValue.Value = 0;
            foreach (Control control in this.Controls)
            {
                control.Enabled = true;
            }
            UpdateGui();
        }

        void LockForm()
        {
            foreach (Control control in this.Controls)
            {
                if (
                    control.Name != "LICENSE" &&
                    control.Name != "README" &&
                    control.Name != "OpenProjectFolder" &&
                    !(control is ProgressBar) && !(control is Label) && !(control is PictureBox))
                {
                    control.Enabled = false;
                }
            }
        }

        async void LoadWindowsTask()
        {
            LockForm();
            await winBoxProject.LoadWindowsImageAsync(UpdateProcessName, UpdateProcessValue);
            UnlockForm();
            UpdateWindowsVersionsList();
            UpdateGui();
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

        private async void ExportImgPartition_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installed partition (*.img)|*.img";
                saveFileDialog.Title = $"Save you WinBox installed .img partition ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName + " (partition)";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildImgPartitionAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
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

        private void WindowsClear_Click(object sender, EventArgs e)
        {
            winBoxProject.UnloadWindowsImage();
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.winBoxConfig.BaseWindowsImage = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WindowsVersionSelect_TextChanged(object sender, EventArgs e)
        {
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

            bool canExport = true;
            if (winBoxProject.winBoxConfig.BaseWindowsImage == null || winBoxProject.winBoxConfig.BaseWindowsVersion == null)
            {
                canExport = false;
            }
            if (canExport)
            {
                switch (winBoxProject.winBoxConfig.ProgramType)
                {
                    case ProgramTypeEnum.ExecutableFile:
                        if (winBoxProject.winBoxConfig.ProgramName == null || winBoxProject.winBoxConfig.ProgramName.Length == 0)
                        {
                            canExport = false;
                        }
                        break;

                    case ProgramTypeEnum.RawCommand:
                        if (winBoxProject.winBoxConfig.RawCommand == null || winBoxProject.winBoxConfig.RawCommand.Length == 0)
                        {
                            canExport = false;
                        }
                        break;

                    case ProgramTypeEnum.WebSite:
                        if (winBoxProject.winBoxConfig.WebSite == null || winBoxProject.winBoxConfig.WebSite.Length == 0)
                        {
                            canExport = false;
                        }
                        break;
                }
            }
            if (canExport)
            {
                bool exists = false;
                WindowsDescription[] localWindowsDescriptions = winBoxProject.GetWindowsDescriptions();
                foreach (WindowsDescription item in localWindowsDescriptions)
                {
                    if (item.name == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    canExport = false;
                }
            }

            ExportIsoInstaller.Enabled = canExport;
            ExportInstallWim.Enabled = canExport;
            ExportImgPartition.Enabled = canExport;
        }

        void UpdateGui()
        {
            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion ?? "";
            ArchitectureSelect.Text = winBoxProject.winBoxConfig.Architecture ?? "";

            OemKey.Text = winBoxProject.winBoxConfig.OemKey ?? "";
            UseOemKey.CheckState = winBoxProject.winBoxConfig.UseOemKey == true ? CheckState.Checked : CheckState.Unchecked;

            ProgramName.Text = winBoxProject.winBoxConfig.ProgramName ?? "not selected";
            ProgramArgs.Text = winBoxProject.winBoxConfig.ProgramArgs ?? "";
            RawCommand.Text = winBoxProject.winBoxConfig.RawCommand ?? "";

            WebSite.Text = winBoxProject.winBoxConfig.WebSite ?? "";
            WebSessionTimeout.Text = winBoxProject.winBoxConfig.WebSessionTimeout.ToString();

            postinstall_bat.Text = winBoxProject.winBoxConfig.PostInstall_bat ?? "not selected";
            postinstall_reg.Text = winBoxProject.winBoxConfig.PostInstall_reg ?? "not selected";
            CustomBootLogo.Text = winBoxProject.winBoxConfig.CustomBootLogo ?? "not selected";

            ScreenTimeout.Text = winBoxProject.winBoxConfig.ScreenTimeout.ToString();

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
            }
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
                                    findedTarget = true;
                                    break;
                                }
                            }
                        }

                        if (!findedTarget)
                        {
                            winBoxProject.winBoxConfig.BaseWindowsVersion = windowsDescriptions[0].name;
                        }
                    }
                    else
                    {
                        winBoxProject.winBoxConfig.BaseWindowsVersion = null;
                    }
                }

                winBoxProject.SaveConfig();
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
            winBoxProject.winBoxConfig.WinboxName = WinboxName.Text;
            winBoxProject.SaveConfig();
        }

        private void WinboxDescription_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.WinboxDescription = WinboxDescription.Text;
            winBoxProject.SaveConfig();
        }

        private void OemKey_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.OemKey = OemKey.Text;
            winBoxProject.SaveConfig();
        }

        private void UseOemKey_CheckedChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.UseOemKey = UseOemKey.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void ProgramArgs_TextChanged(object sender, EventArgs e)
        {
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

        private void UpdateProcessName(string text)
        {
            ProcessName.Text = text;
        }

        private void UpdateProcessValue(int Value)
        {
            ProcessValue.Value = Value;
            taskbarManager.SetProgressState(Value == 0 ? TaskbarProgressBarState.NoProgress : TaskbarProgressBarState.Normal);
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
            }
            winBoxProject.SaveConfig();
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
            if (ProgramType_ExecutableFile.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.ExecutableFile;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void ProgramType_RawCommand_CheckedChanged(object sender, EventArgs e)
        {
            if (ProgramType_RawCommand.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.RawCommand;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void ProgramType_WebSite_CheckedChanged(object sender, EventArgs e)
        {
            if (ProgramType_WebSite.Checked)
            {
                winBoxProject.winBoxConfig.ProgramType = ProgramTypeEnum.WebSite;
                winBoxProject.SaveConfig();
                UpdateGuiWithoutWindowsVersion();
            }
        }

        private void RawCommand_TextChanged(object sender, EventArgs e)
        {
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
            winBoxProject.winBoxConfig.WebSite = WebSite.Text;
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void ResetWebSessionTimeout(int value)
        {
            winBoxProject.winBoxConfig.WebSessionTimeout = value;
            WebSessionTimeout.Text = winBoxProject.winBoxConfig.WebSessionTimeout.ToString();
        }

        private void WebSessionTimeout_TextChanged(object sender, EventArgs e)
        {
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
            UnlockForm();
            if (name != null)
            {
                winBoxProject.winBoxConfig.WebSite = @$"C:\WinboxProgram\{name}";
                winBoxProject.SaveConfig();
                UpdateGui();
            }
        }

        private async void postinstall_bat_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Bat scripts (*.bat;*.cmd)|*.bat;*.cmd|All files (*.*)|*.*", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.PostInstall_bat = name;
            }
            winBoxProject.SaveConfig();
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
            }
            winBoxProject.SaveConfig();
            UnlockForm();
        }

        private void postinstall_reg_clr_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.PostInstall_reg = null;
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

        private async void CustomBootLogo_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "BMP files (*.bmp)|*.bmp", winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.CustomBootLogo = name;
            }
            winBoxProject.SaveConfig();
            UnlockForm();
        }

        private void CustomBootLogo_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.CustomBootLogo = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }
    }
}
