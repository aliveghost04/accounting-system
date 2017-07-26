namespace ProyectoPropietaria
{
    partial class Contabilidad
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.configSTMI = new System.Windows.Forms.ToolStripMenuItem();
            this.parametrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnCountablesAccounts = new System.Windows.Forms.Button();
            this.btnAccountTypes = new System.Windows.Forms.Button();
            this.btnCurrenciesTypes = new System.Windows.Forms.Button();
            this.btnPlacements = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.configSTMI,
            this.reportesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem,
            this.salirToolStripMenuItem1});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.salirToolStripMenuItem.Text = "Cerrar Sesión";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem1
            // 
            this.salirToolStripMenuItem1.Name = "salirToolStripMenuItem1";
            this.salirToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.salirToolStripMenuItem1.Text = "Salir";
            this.salirToolStripMenuItem1.Click += new System.EventHandler(this.salirToolStripMenuItem1_Click);
            // 
            // configSTMI
            // 
            this.configSTMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parametrosToolStripMenuItem,
            this.usuariosToolStripMenuItem});
            this.configSTMI.Name = "configSTMI";
            this.configSTMI.Size = new System.Drawing.Size(95, 20);
            this.configSTMI.Text = "Configuración";
            this.configSTMI.Click += new System.EventHandler(this.cRUDToolStripMenuItem_Click);
            // 
            // parametrosToolStripMenuItem
            // 
            this.parametrosToolStripMenuItem.Name = "parametrosToolStripMenuItem";
            this.parametrosToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.parametrosToolStripMenuItem.Text = "Parametros";
            this.parametrosToolStripMenuItem.Click += new System.EventHandler(this.parametrosToolStripMenuItem_Click);
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.usuariosToolStripMenuItem.Text = "Usuarios";
            this.usuariosToolStripMenuItem.Click += new System.EventHandler(this.usuariosToolStripMenuItem_Click);
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.reportesToolStripMenuItem.Text = "Reportes";
            this.reportesToolStripMenuItem.Click += new System.EventHandler(this.reportesToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(357, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Bienvenido";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(324, 79);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(77, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Fulanito de Tal";
            // 
            // btnCountablesAccounts
            // 
            this.btnCountablesAccounts.Location = new System.Drawing.Point(12, 47);
            this.btnCountablesAccounts.Name = "btnCountablesAccounts";
            this.btnCountablesAccounts.Size = new System.Drawing.Size(119, 45);
            this.btnCountablesAccounts.TabIndex = 5;
            this.btnCountablesAccounts.Text = "Cuentas Contables";
            this.btnCountablesAccounts.UseVisualStyleBackColor = true;
            this.btnCountablesAccounts.Click += new System.EventHandler(this.btnCountablesAccounts_Click);
            // 
            // btnAccountTypes
            // 
            this.btnAccountTypes.Location = new System.Drawing.Point(12, 114);
            this.btnAccountTypes.Name = "btnAccountTypes";
            this.btnAccountTypes.Size = new System.Drawing.Size(119, 45);
            this.btnAccountTypes.TabIndex = 6;
            this.btnAccountTypes.Text = "Tipos de Cuentas";
            this.btnAccountTypes.UseVisualStyleBackColor = true;
            this.btnAccountTypes.Click += new System.EventHandler(this.btnAccountTypes_Click);
            // 
            // btnCurrenciesTypes
            // 
            this.btnCurrenciesTypes.Location = new System.Drawing.Point(172, 114);
            this.btnCurrenciesTypes.Name = "btnCurrenciesTypes";
            this.btnCurrenciesTypes.Size = new System.Drawing.Size(119, 45);
            this.btnCurrenciesTypes.TabIndex = 7;
            this.btnCurrenciesTypes.Text = "Tipos de Monedas";
            this.btnCurrenciesTypes.UseVisualStyleBackColor = true;
            this.btnCurrenciesTypes.Click += new System.EventHandler(this.btnCurrenciesTypes_Click);
            // 
            // btnPlacements
            // 
            this.btnPlacements.Location = new System.Drawing.Point(172, 47);
            this.btnPlacements.Name = "btnPlacements";
            this.btnPlacements.Size = new System.Drawing.Size(119, 45);
            this.btnPlacements.TabIndex = 8;
            this.btnPlacements.Text = "Entrada Contable";
            this.btnPlacements.UseVisualStyleBackColor = true;
            this.btnPlacements.Click += new System.EventHandler(this.btnPlacements_Click);
            // 
            // Contabilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 182);
            this.Controls.Add(this.btnPlacements);
            this.Controls.Add(this.btnCurrenciesTypes);
            this.Controls.Add(this.btnAccountTypes);
            this.Controls.Add(this.btnCountablesAccounts);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 220);
            this.Name = "Contabilidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Contabilidad";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Contabilidad_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configSTMI;
        private System.Windows.Forms.ToolStripMenuItem parametrosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnCountablesAccounts;
        private System.Windows.Forms.Button btnAccountTypes;
        private System.Windows.Forms.Button btnCurrenciesTypes;
        private System.Windows.Forms.Button btnPlacements;
        private System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usuariosToolStripMenuItem;
    }
}

