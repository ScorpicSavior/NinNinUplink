/*
 * Copyright (C) 2016 ScorpicSavior
 * 
 * This file is part of NinNinUplink.
 * 
 * NinNinUplink is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * NinNinUplink is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with NinNinUplink.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Net.Http;
using NinNinUplink.Properties;

namespace NinNinUplink
{
    public partial class MainForm : Form
    {
        private int m_sentRecords;
        private string m_tosPath;
        private NinNinApi m_api;
        private IDisposable m_subscription;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Settings.Default.MainWindowLocation != null)
                this.Location = Settings.Default.MainWindowLocation;

            if (Settings.Default.MainWindowSize != null)
                this.Size = Settings.Default.MainWindowSize;

            this.ApiKeyTextBox.Text = Settings.Default.ApiKey;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Settings.Default.GamePath == null || !IsToSAtPath(Settings.Default.GamePath)) {
                // Try auto detecting and guessing
                var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 372000");
                if (key == null)
                    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 372000");
                if (key != null) {
                    var val = key.GetValue("InstallLocation") as string;
                    if (val != null && IsToSAtPath(val))
                        this.m_tosPath = val;
                }
                if (this.m_tosPath == null) {
                    key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
                    if (key != null) {
                        var val = key.GetValue("SteamPath") as string;
                        if (val != null) {
                            val = Path.Combine(Path.GetFullPath(val), @"SteamApps\common\TreeOfSavior");
                            if (IsToSAtPath(val))
                                this.m_tosPath = val;
                        }
                    }
                }
                if (this.m_tosPath == null) {
                    var val = @"C:\Program Files (x86)\Steam\SteamApps\common\TreeOfSavior";
                    if (IsToSAtPath(val))
                        this.m_tosPath = val;
                }

                // Ask the user
                if (this.m_tosPath == null) {
                    var dialog = new FolderBrowserDialog();
                    dialog.ShowNewFolderButton = false;
                    dialog.Description = "Please point to your TreeOfSavior installation folder:";
                    if (dialog.ShowDialog() == DialogResult.OK) {
                        if (IsToSAtPath(dialog.SelectedPath))
                            this.m_tosPath = dialog.SelectedPath;
                    }
                }

                if (this.m_tosPath != null) {
                    Settings.Default.GamePath = this.m_tosPath;
                    Settings.Default.Save();
                }
            }
            else {
                this.m_tosPath = Settings.Default.GamePath;
            }
            
            if (this.m_tosPath == null) {
                MessageBox.Show("Can't find your ToS installation location!", "x.x", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else if (this.ApiKeyTextBox.Text != "") {
                this.StartCheckBox.Focus();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.MainWindowLocation = this.Location;

            if (this.WindowState == FormWindowState.Normal)
                Settings.Default.MainWindowSize = this.Size;
            else
                Settings.Default.MainWindowSize = this.RestoreBounds.Size;

            Settings.Default.ApiKey = this.ApiKeyTextBox.Text;
            Settings.Default.Save();
        }

        private async void chkStart_Click(object sender, EventArgs e)
        {
            if (this.StartCheckBox.Checked) {
                var apiKey = this.ApiKeyTextBox.Text.Trim();
                if (apiKey.Length == 0) {
                    this.StartCheckBox.Checked = false;
                    MessageBox.Show("You need to enter your API Key!", "ò.ó", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.m_api = new NinNinApi(Settings.Default.BaseAddress, apiKey);

                this.StartCheckBox.Enabled = false;
                this.ApiKeyTextBox.Enabled = false;
                this.StatusLabel.Text = "Checking API key...";

                HttpResponseMessage res = await this.m_api.CheckApiKey();
                switch (res.StatusCode) {
                    case System.Net.HttpStatusCode.Unauthorized:
                        MessageBox.Show("Invalid API Key!", "ò.ó", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.StopTail("Invalid API Key");
                        break;
                    case System.Net.HttpStatusCode.OK:
                        this.StartTail();
                        break;
                    default:
                        MessageBox.Show("HTTP error! Maybe server down or something.", "x.x", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.StopTail("HTTP error");
                        break;
                }
            }
            else {
                this.StopTail("Stopped");
            }
        }

        private void StartTail()
        {
            var logFolder = Path.Combine(this.m_tosPath, @"addons\nnchannel");
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            var logPath = Path.Combine(logFolder, "population_log.csv");
            this.m_subscription = SimpleLogTailer.Tail(logPath).Subscribe(this.Tail_ReadLine, this.Tail_Error);
            this.ApiKeyTextBox.Enabled = false;
            this.StartCheckBox.Text = "Stop";
            this.StartCheckBox.Enabled = true;
            this.StartCheckBox.Checked = true;
            this.StatusLabel.Text = "Waiting for data...";
        }

        private void StopTail(string message)
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.StopTail(message); });
                return;
            }

            if (this.m_subscription != null) {
                this.m_subscription.Dispose();
                this.m_subscription = null;
            }

            if (this.m_api != null) {
                this.m_api.Dispose();
                this.m_api = null;
            }

            this.StatusLabel.Text = message;
            this.ApiKeyTextBox.Enabled = true;
            this.StartCheckBox.Text = "Start";
            this.StartCheckBox.Enabled = true;
            this.StartCheckBox.Checked = false;
        }

        private void IncreaseSentRecords()
        {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { this.IncreaseSentRecords(); });
                return;
            }

            this.m_sentRecords++;
            this.StatusLabel.Text = string.Format("Sent records: {0}", this.m_sentRecords);
        }

        private void Tail_ReadLine(string line)
        {
            var record = Regex.Split(line, ";(?=(?:[^\"]|\"[^\"]*\")*$)").Select(s => Regex.Replace(s, "^\"|\"$", "")).ToArray();
            if (record.Length >= 4) {
                var populations = new int[record.Length - 3];
                for (int i = 3; i < record.Length; i++)
                    populations[i - 3] = int.Parse(record[i]);

                this.m_api.PostPopulations(record[0], record[1], record[2], populations).ContinueWith(t => this.IncreaseSentRecords());
            }
        }

        private void Tail_Error(Exception e)
        {
            Console.WriteLine(e.ToString());
            this.StopTail("Logfile error");
        }

        private static bool IsToSAtPath(string path)
        {
            return File.Exists(Path.Combine(path, @"release\Client_tos.exe"));
        }
    }
}
