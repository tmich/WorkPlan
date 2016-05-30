namespace WorkPlan
{
    partial class DlgNowork
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
            this.tmPickerEnd = new System.Windows.Forms.DateTimePicker();
            this.tmPickerStart = new System.Windows.Forms.DateTimePicker();
            this.dtPickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtPickerStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cbReasons = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txNotes = new System.Windows.Forms.TextBox();
            this.chkFullDay = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tmPickerEnd
            // 
            this.tmPickerEnd.CustomFormat = "HH:mm";
            this.tmPickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmPickerEnd.Location = new System.Drawing.Point(341, 47);
            this.tmPickerEnd.Margin = new System.Windows.Forms.Padding(4);
            this.tmPickerEnd.Name = "tmPickerEnd";
            this.tmPickerEnd.ShowUpDown = true;
            this.tmPickerEnd.Size = new System.Drawing.Size(92, 22);
            this.tmPickerEnd.TabIndex = 11;
            // 
            // tmPickerStart
            // 
            this.tmPickerStart.CustomFormat = "HH:mm";
            this.tmPickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tmPickerStart.Location = new System.Drawing.Point(341, 15);
            this.tmPickerStart.Margin = new System.Windows.Forms.Padding(4);
            this.tmPickerStart.Name = "tmPickerStart";
            this.tmPickerStart.ShowUpDown = true;
            this.tmPickerStart.Size = new System.Drawing.Size(92, 22);
            this.tmPickerStart.TabIndex = 10;
            // 
            // dtPickerEnd
            // 
            this.dtPickerEnd.Location = new System.Drawing.Point(101, 47);
            this.dtPickerEnd.Margin = new System.Windows.Forms.Padding(4);
            this.dtPickerEnd.Name = "dtPickerEnd";
            this.dtPickerEnd.Size = new System.Drawing.Size(231, 22);
            this.dtPickerEnd.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Fine";
            // 
            // dtPickerStart
            // 
            this.dtPickerStart.Location = new System.Drawing.Point(101, 15);
            this.dtPickerStart.Margin = new System.Windows.Forms.Padding(4);
            this.dtPickerStart.Name = "dtPickerStart";
            this.dtPickerStart.Size = new System.Drawing.Size(231, 22);
            this.dtPickerStart.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Inizio";
            // 
            // cbReasons
            // 
            this.cbReasons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReasons.FormattingEnabled = true;
            this.cbReasons.Location = new System.Drawing.Point(101, 114);
            this.cbReasons.Margin = new System.Windows.Forms.Padding(4);
            this.cbReasons.Name = "cbReasons";
            this.cbReasons.Size = new System.Drawing.Size(332, 24);
            this.cbReasons.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Motivazione";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(335, 285);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 17;
            this.button2.Text = "&Annulla";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(227, 285);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 16;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 151);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Note";
            // 
            // txNotes
            // 
            this.txNotes.Location = new System.Drawing.Point(101, 147);
            this.txNotes.Margin = new System.Windows.Forms.Padding(4);
            this.txNotes.Multiline = true;
            this.txNotes.Name = "txNotes";
            this.txNotes.Size = new System.Drawing.Size(332, 130);
            this.txNotes.TabIndex = 14;
            // 
            // chkFullDay
            // 
            this.chkFullDay.AutoSize = true;
            this.chkFullDay.Location = new System.Drawing.Point(308, 86);
            this.chkFullDay.Name = "chkFullDay";
            this.chkFullDay.Size = new System.Drawing.Size(125, 21);
            this.chkFullDay.TabIndex = 18;
            this.chkFullDay.Text = "Giornata Intera";
            this.chkFullDay.UseVisualStyleBackColor = true;
            this.chkFullDay.CheckedChanged += new System.EventHandler(this.chkFullDay_CheckedChanged);
            // 
            // DlgNowork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 350);
            this.Controls.Add(this.chkFullDay);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txNotes);
            this.Controls.Add(this.cbReasons);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tmPickerEnd);
            this.Controls.Add(this.tmPickerStart);
            this.Controls.Add(this.dtPickerEnd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtPickerStart);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DlgNowork";
            this.Text = "DlgNowork";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgNowork_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker tmPickerEnd;
        private System.Windows.Forms.DateTimePicker tmPickerStart;
        private System.Windows.Forms.DateTimePicker dtPickerEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtPickerStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbReasons;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txNotes;
        private System.Windows.Forms.CheckBox chkFullDay;
    }
}