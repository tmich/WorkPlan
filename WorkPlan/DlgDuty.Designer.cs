﻿namespace WorkPlan
{
    partial class DlgDuty
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
            this.label1 = new System.Windows.Forms.Label();
            this.dtPickerStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtPickerEnd = new System.Windows.Forms.DateTimePicker();
            this.tmPickerStart = new System.Windows.Forms.DateTimePicker();
            this.tmPickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPositions = new System.Windows.Forms.ComboBox();
            this.txNotes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inizio";
            // 
            // dtPickerStart
            // 
            this.dtPickerStart.Location = new System.Drawing.Point(101, 15);
            this.dtPickerStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtPickerStart.Name = "dtPickerStart";
            this.dtPickerStart.Size = new System.Drawing.Size(231, 22);
            this.dtPickerStart.TabIndex = 1;
            this.dtPickerStart.ValueChanged += new System.EventHandler(this.dtPickerStart_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fine";
            // 
            // dtPickerEnd
            // 
            this.dtPickerEnd.Enabled = false;
            this.dtPickerEnd.Location = new System.Drawing.Point(101, 47);
            this.dtPickerEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtPickerEnd.Name = "dtPickerEnd";
            this.dtPickerEnd.Size = new System.Drawing.Size(231, 22);
            this.dtPickerEnd.TabIndex = 3;
            // 
            // tmPickerStart
            // 
            this.tmPickerStart.CustomFormat = "HH:mm";
            this.tmPickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmPickerStart.Location = new System.Drawing.Point(341, 15);
            this.tmPickerStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tmPickerStart.Name = "tmPickerStart";
            this.tmPickerStart.ShowUpDown = true;
            this.tmPickerStart.Size = new System.Drawing.Size(92, 22);
            this.tmPickerStart.TabIndex = 4;
            // 
            // tmPickerEnd
            // 
            this.tmPickerEnd.CustomFormat = "HH:mm";
            this.tmPickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmPickerEnd.Location = new System.Drawing.Point(341, 47);
            this.tmPickerEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tmPickerEnd.Name = "tmPickerEnd";
            this.tmPickerEnd.ShowUpDown = true;
            this.tmPickerEnd.Size = new System.Drawing.Size(92, 22);
            this.tmPickerEnd.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Posizione";
            // 
            // cbPositions
            // 
            this.cbPositions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPositions.FormattingEnabled = true;
            this.cbPositions.Location = new System.Drawing.Point(101, 79);
            this.cbPositions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPositions.Name = "cbPositions";
            this.cbPositions.Size = new System.Drawing.Size(332, 24);
            this.cbPositions.TabIndex = 7;
            // 
            // txNotes
            // 
            this.txNotes.Location = new System.Drawing.Point(101, 112);
            this.txNotes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txNotes.Multiline = true;
            this.txNotes.Name = "txNotes";
            this.txNotes.Size = new System.Drawing.Size(332, 130);
            this.txNotes.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 116);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Note";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(227, 250);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(335, 250);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 11;
            this.button2.Text = "&Annulla";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // DlgDuty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 289);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txNotes);
            this.Controls.Add(this.cbPositions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tmPickerEnd);
            this.Controls.Add(this.tmPickerStart);
            this.Controls.Add(this.dtPickerEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtPickerStart);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgDuty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DlgDuty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgDuty_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtPickerStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtPickerEnd;
        private System.Windows.Forms.DateTimePicker tmPickerStart;
        private System.Windows.Forms.DateTimePicker tmPickerEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbPositions;
        private System.Windows.Forms.TextBox txNotes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}