namespace WorkPlan
{
    partial class EmployeeListView
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.colMatr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSurname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbNuovo = new System.Windows.Forms.ToolStripButton();
            this.tbEdit = new System.Windows.Forms.ToolStripButton();
            this.tbDelete = new System.Windows.Forms.ToolStripButton();
            this.colPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(4, 29);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvEmployees);
            this.splitContainer1.Size = new System.Drawing.Size(845, 532);
            this.splitContainer1.SplitterDistance = 381;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // lvEmployees
            // 
            this.lvEmployees.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lvEmployees.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMatr,
            this.colSurname,
            this.colName,
            this.colPosition});
            this.lvEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.Location = new System.Drawing.Point(0, 0);
            this.lvEmployees.Margin = new System.Windows.Forms.Padding(4);
            this.lvEmployees.MultiSelect = false;
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(381, 532);
            this.lvEmployees.TabIndex = 2;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ItemActivate += new System.EventHandler(this.lvEmployees_ItemActivate);
            // 
            // colMatr
            // 
            this.colMatr.Text = "Matricola";
            this.colMatr.Width = 96;
            // 
            // colName
            // 
            this.colName.Text = "Nome";
            this.colName.Width = 70;
            // 
            // colSurname
            // 
            this.colSurname.Text = "Cognome";
            this.colSurname.Width = 82;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNuovo,
            this.tbEdit,
            this.tbDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(853, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbNuovo
            // 
            this.tbNuovo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbNuovo.Image = global::WorkPlan.Resources._new;
            this.tbNuovo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbNuovo.Name = "tbNuovo";
            this.tbNuovo.Size = new System.Drawing.Size(24, 24);
            this.tbNuovo.Text = "&Nuovo";
            this.tbNuovo.Click += new System.EventHandler(this.tbNuovo_Click);
            // 
            // tbEdit
            // 
            this.tbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEdit.Image = global::WorkPlan.Resources.edit;
            this.tbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(24, 24);
            this.tbEdit.Text = "&Modifica";
            this.tbEdit.Click += new System.EventHandler(this.tbEdit_Click);
            // 
            // tbDelete
            // 
            this.tbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbDelete.Image = global::WorkPlan.Resources.delete;
            this.tbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDelete.Name = "tbDelete";
            this.tbDelete.Size = new System.Drawing.Size(24, 24);
            this.tbDelete.Text = "Elimina";
            this.tbDelete.Click += new System.EventHandler(this.tbDelete_Click);
            // 
            // colPosition
            // 
            this.colPosition.Text = "Reparto";
            // 
            // EmployeeListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EmployeeListView";
            this.Size = new System.Drawing.Size(853, 748);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbNuovo;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.ColumnHeader colMatr;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSurname;
        private System.Windows.Forms.ToolStripButton tbEdit;
        private System.Windows.Forms.ToolStripButton tbDelete;
        private System.Windows.Forms.ColumnHeader colPosition;
    }
}