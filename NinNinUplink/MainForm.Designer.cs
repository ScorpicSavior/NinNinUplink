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

namespace NinNinUplink
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ApiKeyLabel = new System.Windows.Forms.Label();
            this.ApiKeyTextBox = new System.Windows.Forms.TextBox();
            this.StartCheckBox = new System.Windows.Forms.CheckBox();
            this.StatusGroupBox = new System.Windows.Forms.GroupBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.StatusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ApiKeyLabel
            // 
            this.ApiKeyLabel.AutoSize = true;
            this.ApiKeyLabel.Location = new System.Drawing.Point(12, 11);
            this.ApiKeyLabel.Name = "ApiKeyLabel";
            this.ApiKeyLabel.Size = new System.Drawing.Size(86, 13);
            this.ApiKeyLabel.TabIndex = 0;
            this.ApiKeyLabel.Text = "Nin-Nin API Key:";
            // 
            // ApiKeyTextBox
            // 
            this.ApiKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApiKeyTextBox.Location = new System.Drawing.Point(104, 8);
            this.ApiKeyTextBox.Name = "ApiKeyTextBox";
            this.ApiKeyTextBox.Size = new System.Drawing.Size(218, 20);
            this.ApiKeyTextBox.TabIndex = 1;
            // 
            // StartCheckBox
            // 
            this.StartCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StartCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.StartCheckBox.AutoSize = true;
            this.StartCheckBox.Location = new System.Drawing.Point(8, 16);
            this.StartCheckBox.Name = "StartCheckBox";
            this.StartCheckBox.Size = new System.Drawing.Size(39, 23);
            this.StartCheckBox.TabIndex = 2;
            this.StartCheckBox.Text = "Start";
            this.StartCheckBox.UseVisualStyleBackColor = true;
            this.StartCheckBox.Click += new System.EventHandler(this.chkStart_Click);
            // 
            // StatusGroupBox
            // 
            this.StatusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusGroupBox.Controls.Add(this.StatusLabel);
            this.StatusGroupBox.Controls.Add(this.StartCheckBox);
            this.StatusGroupBox.Location = new System.Drawing.Point(15, 32);
            this.StatusGroupBox.Name = "StatusGroupBox";
            this.StatusGroupBox.Size = new System.Drawing.Size(307, 47);
            this.StatusGroupBox.TabIndex = 3;
            this.StatusGroupBox.TabStop = false;
            this.StatusGroupBox.Text = "NNChannelTracker";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(54, 21);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(24, 13);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "Idle";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 91);
            this.Controls.Add(this.StatusGroupBox);
            this.Controls.Add(this.ApiKeyTextBox);
            this.Controls.Add(this.ApiKeyLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 130);
            this.Name = "MainForm";
            this.Text = "Nin-Nin Uplink";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.StatusGroupBox.ResumeLayout(false);
            this.StatusGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ApiKeyLabel;
        private System.Windows.Forms.TextBox ApiKeyTextBox;
        private System.Windows.Forms.CheckBox StartCheckBox;
        private System.Windows.Forms.GroupBox StatusGroupBox;
        private System.Windows.Forms.Label StatusLabel;
    }
}
