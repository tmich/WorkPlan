namespace WorkPlan
{
    partial class DlgChooseEmployee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgChooseEmployee));
            this.lstEmployees = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblTotPagine = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstEmployees
            // 
            this.lstEmployees.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEmployees.CheckOnClick = true;
            this.lstEmployees.FormattingEnabled = true;
            this.lstEmployees.Location = new System.Drawing.Point(13, 13);
            this.lstEmployees.Name = "lstEmployees";
            this.lstEmployees.Size = new System.Drawing.Size(538, 412);
            this.lstEmployees.TabIndex = 0;
            this.lstEmployees.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstEmployees_ItemCheck);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(461, 436);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lblTotPagine
            // 
            this.lblTotPagine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotPagine.AutoSize = true;
            this.lblTotPagine.Location = new System.Drawing.Point(13, 436);
            this.lblTotPagine.Name = "lblTotPagine";
            this.lblTotPagine.Size = new System.Drawing.Size(87, 17);
            this.lblTotPagine.TabIndex = 2;
            this.lblTotPagine.Text = "lblTotPagine";
            // 
            // DlgChooseEmployee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 481);
            this.Controls.Add(this.lblTotPagine);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstEmployees);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgChooseEmployee";
            this.Text = "Scegli dipendente";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgChooseEmployee_FormClosing);
            this.Load += new System.EventHandler(this.DlgChooseEmployee_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lstEmployees;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblTotPagine;
    }
}