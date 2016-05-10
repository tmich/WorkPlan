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
            this.btNewEmployee = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvEmployees = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSurname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMatr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btNewEmployee
            // 
            this.btNewEmployee.Location = new System.Drawing.Point(3, 3);
            this.btNewEmployee.Name = "btNewEmployee";
            this.btNewEmployee.Size = new System.Drawing.Size(151, 23);
            this.btNewEmployee.TabIndex = 1;
            this.btNewEmployee.Text = "&Nuovo";
            this.btNewEmployee.UseVisualStyleBackColor = true;
            this.btNewEmployee.Click += new System.EventHandler(this.btNewEmployee_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btNewEmployee);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvEmployees);
            this.splitContainer1.Size = new System.Drawing.Size(640, 608);
            this.splitContainer1.SplitterDistance = 157;
            this.splitContainer1.TabIndex = 2;
            // 
            // lvEmployees
            // 
            this.lvEmployees.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lvEmployees.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMatr,
            this.colName,
            this.colSurname});
            this.lvEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvEmployees.FullRowSelect = true;
            this.lvEmployees.Location = new System.Drawing.Point(0, 0);
            this.lvEmployees.MultiSelect = false;
            this.lvEmployees.Name = "lvEmployees";
            this.lvEmployees.Size = new System.Drawing.Size(479, 608);
            this.lvEmployees.TabIndex = 1;
            this.lvEmployees.UseCompatibleStateImageBehavior = false;
            this.lvEmployees.View = System.Windows.Forms.View.Details;
            this.lvEmployees.ItemActivate += new System.EventHandler(this.lvEmployees_ItemActivate);
            // 
            // colName
            // 
            this.colName.DisplayIndex = 0;
            this.colName.Text = "Nome";
            this.colName.Width = 70;
            // 
            // colSurname
            // 
            this.colSurname.DisplayIndex = 1;
            this.colSurname.Text = "Cognome";
            this.colSurname.Width = 82;
            // 
            // colMatr
            // 
            this.colMatr.DisplayIndex = 2;
            this.colMatr.Text = "Matricola";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // EmployeeListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "EmployeeListView";
            this.Size = new System.Drawing.Size(640, 608);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btNewEmployee;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvEmployees;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSurname;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader colMatr;
    }
}