namespace WorkPlan
{
    partial class DlgBusta
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
            this.cmbMese = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAnno = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtImporto = new System.Windows.Forms.MaskedTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbMese
            // 
            this.cmbMese.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMese.FormattingEnabled = true;
            this.cmbMese.Items.AddRange(new object[] {
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
            "Dicembre",
            "Tredicesima",
            "Quattordicesima"});
            this.cmbMese.Location = new System.Drawing.Point(76, 10);
            this.cmbMese.Name = "cmbMese";
            this.cmbMese.Size = new System.Drawing.Size(178, 24);
            this.cmbMese.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Periodo";
            // 
            // cmbAnno
            // 
            this.cmbAnno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnno.FormattingEnabled = true;
            this.cmbAnno.Location = new System.Drawing.Point(260, 10);
            this.cmbAnno.Name = "cmbAnno";
            this.cmbAnno.Size = new System.Drawing.Size(121, 24);
            this.cmbAnno.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Importo";
            // 
            // txtImporto
            // 
            this.txtImporto.Location = new System.Drawing.Point(76, 63);
            this.txtImporto.Mask = "9999.99 $";
            this.txtImporto.Name = "txtImporto";
            this.txtImporto.Size = new System.Drawing.Size(98, 22);
            this.txtImporto.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(284, 129);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(97, 34);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // DlgBusta
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 175);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtImporto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbAnno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMese);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgBusta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DlgBusta";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgBusta_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMese;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAnno;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtImporto;
        private System.Windows.Forms.Button btnOk;
    }
}