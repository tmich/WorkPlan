namespace WorkPlan
{
    partial class DlgCausAss
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
            this.lbCausali = new System.Windows.Forms.ListBox();
            this.cmdElimina = new System.Windows.Forms.Button();
            this.cmdNuovo = new System.Windows.Forms.Button();
            this.txtDescrizione = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCodice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdSalva = new System.Windows.Forms.Button();
            this.cmdAnnulla = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbCausali
            // 
            this.lbCausali.FormattingEnabled = true;
            this.lbCausali.ItemHeight = 16;
            this.lbCausali.Location = new System.Drawing.Point(12, 12);
            this.lbCausali.Name = "lbCausali";
            this.lbCausali.Size = new System.Drawing.Size(404, 180);
            this.lbCausali.TabIndex = 29;
            this.lbCausali.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbCausali_MouseDoubleClick);
            // 
            // cmdElimina
            // 
            this.cmdElimina.Location = new System.Drawing.Point(245, 198);
            this.cmdElimina.Name = "cmdElimina";
            this.cmdElimina.Size = new System.Drawing.Size(84, 30);
            this.cmdElimina.TabIndex = 28;
            this.cmdElimina.Text = "Elimina";
            this.cmdElimina.UseVisualStyleBackColor = true;
            this.cmdElimina.Visible = false;
            // 
            // cmdNuovo
            // 
            this.cmdNuovo.Location = new System.Drawing.Point(332, 198);
            this.cmdNuovo.Name = "cmdNuovo";
            this.cmdNuovo.Size = new System.Drawing.Size(84, 30);
            this.cmdNuovo.TabIndex = 27;
            this.cmdNuovo.Text = "Aggiungi";
            this.cmdNuovo.UseVisualStyleBackColor = true;
            this.cmdNuovo.Click += new System.EventHandler(this.cmdNuovo_Click);
            // 
            // txtDescrizione
            // 
            this.txtDescrizione.Enabled = false;
            this.txtDescrizione.Location = new System.Drawing.Point(117, 273);
            this.txtDescrizione.MaxLength = 50;
            this.txtDescrizione.Name = "txtDescrizione";
            this.txtDescrizione.Size = new System.Drawing.Size(299, 22);
            this.txtDescrizione.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 278);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 24;
            this.label3.Text = "Descrizione";
            // 
            // txtCodice
            // 
            this.txtCodice.Enabled = false;
            this.txtCodice.Location = new System.Drawing.Point(117, 244);
            this.txtCodice.MaxLength = 5;
            this.txtCodice.Name = "txtCodice";
            this.txtCodice.Size = new System.Drawing.Size(75, 22);
            this.txtCodice.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 249);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "Codice";
            // 
            // txtId
            // 
            this.txtId.Enabled = false;
            this.txtId.Location = new System.Drawing.Point(374, 244);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(42, 22);
            this.txtId.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(349, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Id";
            // 
            // cmdSalva
            // 
            this.cmdSalva.Enabled = false;
            this.cmdSalva.Location = new System.Drawing.Point(332, 312);
            this.cmdSalva.Name = "cmdSalva";
            this.cmdSalva.Size = new System.Drawing.Size(84, 30);
            this.cmdSalva.TabIndex = 30;
            this.cmdSalva.Text = "Salva";
            this.cmdSalva.UseVisualStyleBackColor = true;
            this.cmdSalva.Click += new System.EventHandler(this.cmdSalva_Click);
            // 
            // cmdAnnulla
            // 
            this.cmdAnnulla.Enabled = false;
            this.cmdAnnulla.Location = new System.Drawing.Point(242, 312);
            this.cmdAnnulla.Name = "cmdAnnulla";
            this.cmdAnnulla.Size = new System.Drawing.Size(84, 30);
            this.cmdAnnulla.TabIndex = 31;
            this.cmdAnnulla.Text = "Annulla";
            this.cmdAnnulla.UseVisualStyleBackColor = true;
            this.cmdAnnulla.Click += new System.EventHandler(this.cmdAnnulla_Click);
            // 
            // DlgCausAss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 354);
            this.Controls.Add(this.cmdAnnulla);
            this.Controls.Add(this.cmdSalva);
            this.Controls.Add(this.lbCausali);
            this.Controls.Add(this.cmdElimina);
            this.Controls.Add(this.cmdNuovo);
            this.Controls.Add(this.txtDescrizione);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCodice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.Name = "DlgCausAss";
            this.Text = "Causali Assenza";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbCausali;
        private System.Windows.Forms.Button cmdElimina;
        private System.Windows.Forms.Button cmdNuovo;
        private System.Windows.Forms.TextBox txtDescrizione;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCodice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdSalva;
        private System.Windows.Forms.Button cmdAnnulla;
    }
}