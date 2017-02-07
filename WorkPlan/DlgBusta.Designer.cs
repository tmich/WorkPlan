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
            this.btnOk = new System.Windows.Forms.Button();
            this.lvVoci = new System.Windows.Forms.ListView();
            this.colDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colImp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnNuovaVoce = new System.Windows.Forms.Button();
            this.btnEliminaVoce = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtImporto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbMese
            // 
            this.cmbMese.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMese.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cmbMese.Location = new System.Drawing.Point(87, 10);
            this.cmbMese.Name = "cmbMese";
            this.cmbMese.Size = new System.Drawing.Size(178, 28);
            this.cmbMese.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Periodo";
            // 
            // cmbAnno
            // 
            this.cmbAnno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAnno.FormattingEnabled = true;
            this.cmbAnno.Location = new System.Drawing.Point(271, 10);
            this.cmbAnno.Name = "cmbAnno";
            this.cmbAnno.Size = new System.Drawing.Size(121, 28);
            this.cmbAnno.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(464, 415);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(97, 34);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Salva";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lvVoci
            // 
            this.lvVoci.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDesc,
            this.colImp});
            this.lvVoci.FullRowSelect = true;
            this.lvVoci.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvVoci.Location = new System.Drawing.Point(12, 58);
            this.lvVoci.Name = "lvVoci";
            this.lvVoci.Size = new System.Drawing.Size(446, 391);
            this.lvVoci.TabIndex = 6;
            this.lvVoci.UseCompatibleStateImageBehavior = false;
            this.lvVoci.View = System.Windows.Forms.View.Details;
            this.lvVoci.DoubleClick += new System.EventHandler(this.lvVoci_DoubleClick);
            // 
            // colDesc
            // 
            this.colDesc.Text = "Descrizione";
            this.colDesc.Width = 250;
            // 
            // colImp
            // 
            this.colImp.Text = "Importo";
            this.colImp.Width = 120;
            // 
            // btnNuovaVoce
            // 
            this.btnNuovaVoce.Location = new System.Drawing.Point(464, 58);
            this.btnNuovaVoce.Name = "btnNuovaVoce";
            this.btnNuovaVoce.Size = new System.Drawing.Size(97, 34);
            this.btnNuovaVoce.TabIndex = 7;
            this.btnNuovaVoce.Text = "&Nuova voce";
            this.btnNuovaVoce.UseVisualStyleBackColor = true;
            this.btnNuovaVoce.Click += new System.EventHandler(this.btnNuovaVoce_Click);
            // 
            // btnEliminaVoce
            // 
            this.btnEliminaVoce.Location = new System.Drawing.Point(464, 98);
            this.btnEliminaVoce.Name = "btnEliminaVoce";
            this.btnEliminaVoce.Size = new System.Drawing.Size(97, 34);
            this.btnEliminaVoce.TabIndex = 8;
            this.btnEliminaVoce.Text = "&Elimina voce";
            this.btnEliminaVoce.UseVisualStyleBackColor = true;
            this.btnEliminaVoce.Click += new System.EventHandler(this.btnEliminaVoce_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 459);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 24);
            this.label3.TabIndex = 9;
            this.label3.Text = "Totale busta";
            // 
            // txtImporto
            // 
            this.txtImporto.BackColor = System.Drawing.SystemColors.Info;
            this.txtImporto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImporto.ForeColor = System.Drawing.Color.ForestGreen;
            this.txtImporto.Location = new System.Drawing.Point(142, 458);
            this.txtImporto.Name = "txtImporto";
            this.txtImporto.Size = new System.Drawing.Size(138, 27);
            this.txtImporto.TabIndex = 10;
            this.txtImporto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(286, 459);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 24);
            this.label4.TabIndex = 11;
            this.label4.Text = "€";
            // 
            // DlgBusta
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 506);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtImporto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnEliminaVoce);
            this.Controls.Add(this.btnNuovaVoce);
            this.Controls.Add(this.lvVoci);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbAnno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMese);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DlgBusta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dettaglio busta paga";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgBusta_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMese;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAnno;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListView lvVoci;
        private System.Windows.Forms.ColumnHeader colDesc;
        private System.Windows.Forms.ColumnHeader colImp;
        private System.Windows.Forms.Button btnNuovaVoce;
        private System.Windows.Forms.Button btnEliminaVoce;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtImporto;
        private System.Windows.Forms.Label label4;
    }
}