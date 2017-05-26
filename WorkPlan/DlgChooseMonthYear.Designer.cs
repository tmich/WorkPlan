namespace WorkPlan
{
    partial class DlgChooseMonthYear
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDettaglio = new System.Windows.Forms.RadioButton();
            this.rbSintesi = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbYears = new System.Windows.Forms.ComboBox();
            this.cbMonths = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(275, 217);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 30);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(193, 217);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Annulla";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDettaglio);
            this.groupBox1.Controls.Add(this.rbSintesi);
            this.groupBox1.Location = new System.Drawing.Point(12, 81);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(337, 130);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo report";
            // 
            // rbDettaglio
            // 
            this.rbDettaglio.AutoSize = true;
            this.rbDettaglio.Location = new System.Drawing.Point(5, 65);
            this.rbDettaglio.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbDettaglio.Name = "rbDettaglio";
            this.rbDettaglio.Size = new System.Drawing.Size(321, 38);
            this.rbDettaglio.TabIndex = 13;
            this.rbDettaglio.TabStop = true;
            this.rbDettaglio.Text = "Report dettagliato: una pagina per dipendente\r\ncon la situazione giornaliera.";
            this.rbDettaglio.UseVisualStyleBackColor = true;
            // 
            // rbSintesi
            // 
            this.rbSintesi.AutoSize = true;
            this.rbSintesi.Location = new System.Drawing.Point(5, 21);
            this.rbSintesi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbSintesi.Name = "rbSintesi";
            this.rbSintesi.Size = new System.Drawing.Size(321, 38);
            this.rbSintesi.TabIndex = 12;
            this.rbSintesi.TabStop = true;
            this.rbSintesi.Text = "Report sintetico: una sola pagina riepilogativa \r\ndella situazione mensile. ";
            this.rbSintesi.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbYears);
            this.groupBox2.Controls.Add(this.cbMonths);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(337, 63);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Periodo di riferimento";
            // 
            // cbYears
            // 
            this.cbYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYears.FormattingEnabled = true;
            this.cbYears.Location = new System.Drawing.Point(209, 21);
            this.cbYears.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbYears.Name = "cbYears";
            this.cbYears.Size = new System.Drawing.Size(96, 24);
            this.cbYears.TabIndex = 7;
            // 
            // cbMonths
            // 
            this.cbMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonths.FormattingEnabled = true;
            this.cbMonths.Items.AddRange(new object[] {
            "Gennaio",
            "Febbraio",
            "Marzo",
            "Aprile",
            "Maggio",
            "Giugno",
            "Luglio",
            "Agosto",
            "Settembre",
            "Ottobre",
            "Novembre",
            "Dicembre"});
            this.cbMonths.Location = new System.Drawing.Point(5, 21);
            this.cbMonths.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbMonths.Name = "cbMonths";
            this.cbMonths.Size = new System.Drawing.Size(197, 24);
            this.cbMonths.TabIndex = 6;
            // 
            // DlgChooseMonthYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 264);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgChooseMonthYear";
            this.Text = "Stampa resoconto mensile";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbDettaglio;
        private System.Windows.Forms.RadioButton rbSintesi;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbYears;
        private System.Windows.Forms.ComboBox cbMonths;
    }
}