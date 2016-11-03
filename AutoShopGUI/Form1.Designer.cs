namespace AutoShopGUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.genericToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jungleADToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jungleADTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jungleAPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jungleAPTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.midADToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.midAPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supportADTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supportAPBurstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supportAPHealToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supportAPTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topADToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topADTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topAPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topAPTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxHealthPotions = new System.Windows.Forms.GroupBox();
            this.maxHealthPotionsLevel = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.maxHealthPotionCount = new System.Windows.Forms.NumericUpDown();
            this.checkBoxBuyHealthPotion = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxBuild = new System.Windows.Forms.GroupBox();
            this.buildTree = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader2 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader3 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader7 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader8 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader4 = new DevComponents.AdvTree.ColumnHeader();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.unreadEmailStyle = new DevComponents.DotNetBar.ElementStyle();
            this.buttonBuildMoveUp = new System.Windows.Forms.Button();
            this.buttonBuildMoveDown = new System.Windows.Forms.Button();
            this.buttonBuildRemove = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSellItem = new System.Windows.Forms.Button();
            this.buttonAddItem = new System.Windows.Forms.Button();
            this.textBoxFilterItemsDatabase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridItemDatabase = new System.Windows.Forms.DataGridView();
            this.ItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemGold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.groupBoxHealthPotions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxHealthPotionsLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxHealthPotionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxBuild.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buildTree)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridItemDatabase)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(775, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.genericToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.fileToolStripMenuItem1.Text = "File";
            this.fileToolStripMenuItem1.Click += new System.EventHandler(this.fileToolStripMenuItem1_Click);
            // 
            // genericToolStripMenuItem
            // 
            this.genericToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aDCToolStripMenuItem,
            this.jungleADToolStripMenuItem,
            this.jungleADTankToolStripMenuItem,
            this.jungleAPToolStripMenuItem,
            this.jungleAPTankToolStripMenuItem,
            this.midADToolStripMenuItem,
            this.midAPToolStripMenuItem,
            this.supportADTankToolStripMenuItem,
            this.supportAPBurstToolStripMenuItem,
            this.supportAPHealToolStripMenuItem,
            this.supportAPTankToolStripMenuItem,
            this.topADToolStripMenuItem,
            this.topADTankToolStripMenuItem,
            this.topAPToolStripMenuItem,
            this.topAPTankToolStripMenuItem});
            this.genericToolStripMenuItem.Name = "genericToolStripMenuItem";
            this.genericToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.genericToolStripMenuItem.Text = "Generic";
            // 
            // aDCToolStripMenuItem
            // 
            this.aDCToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aDCToolStripMenuItem.Image")));
            this.aDCToolStripMenuItem.Name = "aDCToolStripMenuItem";
            this.aDCToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.aDCToolStripMenuItem.Text = "ADC";
            this.aDCToolStripMenuItem.Click += new System.EventHandler(this.aDCToolStripMenuItem_Click);
            // 
            // jungleADToolStripMenuItem
            // 
            this.jungleADToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jungleADToolStripMenuItem.Image")));
            this.jungleADToolStripMenuItem.Name = "jungleADToolStripMenuItem";
            this.jungleADToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.jungleADToolStripMenuItem.Text = "Jungle AD";
            this.jungleADToolStripMenuItem.Click += new System.EventHandler(this.jungleADToolStripMenuItem_Click);
            // 
            // jungleADTankToolStripMenuItem
            // 
            this.jungleADTankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jungleADTankToolStripMenuItem.Image")));
            this.jungleADTankToolStripMenuItem.Name = "jungleADTankToolStripMenuItem";
            this.jungleADTankToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.jungleADTankToolStripMenuItem.Text = "Jungle AD Tank";
            this.jungleADTankToolStripMenuItem.Click += new System.EventHandler(this.jungleADTankToolStripMenuItem_Click);
            // 
            // jungleAPToolStripMenuItem
            // 
            this.jungleAPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jungleAPToolStripMenuItem.Image")));
            this.jungleAPToolStripMenuItem.Name = "jungleAPToolStripMenuItem";
            this.jungleAPToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.jungleAPToolStripMenuItem.Text = "Jungle AP";
            this.jungleAPToolStripMenuItem.Click += new System.EventHandler(this.jungleAPToolStripMenuItem_Click);
            // 
            // jungleAPTankToolStripMenuItem
            // 
            this.jungleAPTankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jungleAPTankToolStripMenuItem.Image")));
            this.jungleAPTankToolStripMenuItem.Name = "jungleAPTankToolStripMenuItem";
            this.jungleAPTankToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.jungleAPTankToolStripMenuItem.Text = "Jungle AP Tank";
            this.jungleAPTankToolStripMenuItem.Click += new System.EventHandler(this.jungleAPTankToolStripMenuItem_Click);
            // 
            // midADToolStripMenuItem
            // 
            this.midADToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("midADToolStripMenuItem.Image")));
            this.midADToolStripMenuItem.Name = "midADToolStripMenuItem";
            this.midADToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.midADToolStripMenuItem.Text = "Mid AD";
            this.midADToolStripMenuItem.Click += new System.EventHandler(this.midADToolStripMenuItem_Click);
            // 
            // midAPToolStripMenuItem
            // 
            this.midAPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("midAPToolStripMenuItem.Image")));
            this.midAPToolStripMenuItem.Name = "midAPToolStripMenuItem";
            this.midAPToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.midAPToolStripMenuItem.Text = "Mid AP";
            this.midAPToolStripMenuItem.Click += new System.EventHandler(this.midAPToolStripMenuItem_Click);
            // 
            // supportADTankToolStripMenuItem
            // 
            this.supportADTankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("supportADTankToolStripMenuItem.Image")));
            this.supportADTankToolStripMenuItem.Name = "supportADTankToolStripMenuItem";
            this.supportADTankToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.supportADTankToolStripMenuItem.Text = "Support AD Tank";
            this.supportADTankToolStripMenuItem.Click += new System.EventHandler(this.supportADTankToolStripMenuItem_Click);
            // 
            // supportAPBurstToolStripMenuItem
            // 
            this.supportAPBurstToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("supportAPBurstToolStripMenuItem.Image")));
            this.supportAPBurstToolStripMenuItem.Name = "supportAPBurstToolStripMenuItem";
            this.supportAPBurstToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.supportAPBurstToolStripMenuItem.Text = "Support AP Burst";
            this.supportAPBurstToolStripMenuItem.Click += new System.EventHandler(this.supportAPBurstToolStripMenuItem_Click);
            // 
            // supportAPHealToolStripMenuItem
            // 
            this.supportAPHealToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("supportAPHealToolStripMenuItem.Image")));
            this.supportAPHealToolStripMenuItem.Name = "supportAPHealToolStripMenuItem";
            this.supportAPHealToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.supportAPHealToolStripMenuItem.Text = "Support AP Heal";
            this.supportAPHealToolStripMenuItem.Click += new System.EventHandler(this.supportAPHealToolStripMenuItem_Click);
            // 
            // supportAPTankToolStripMenuItem
            // 
            this.supportAPTankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("supportAPTankToolStripMenuItem.Image")));
            this.supportAPTankToolStripMenuItem.Name = "supportAPTankToolStripMenuItem";
            this.supportAPTankToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.supportAPTankToolStripMenuItem.Text = "Support AP Tank";
            this.supportAPTankToolStripMenuItem.Click += new System.EventHandler(this.supportAPTankToolStripMenuItem_Click);
            // 
            // topADToolStripMenuItem
            // 
            this.topADToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("topADToolStripMenuItem.Image")));
            this.topADToolStripMenuItem.Name = "topADToolStripMenuItem";
            this.topADToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.topADToolStripMenuItem.Text = "Top AD";
            this.topADToolStripMenuItem.Click += new System.EventHandler(this.topADToolStripMenuItem_Click);
            // 
            // topADTankToolStripMenuItem
            // 
            this.topADTankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("topADTankToolStripMenuItem.Image")));
            this.topADTankToolStripMenuItem.Name = "topADTankToolStripMenuItem";
            this.topADTankToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.topADTankToolStripMenuItem.Text = "Top AD Tank";
            this.topADTankToolStripMenuItem.Click += new System.EventHandler(this.topADTankToolStripMenuItem_Click);
            // 
            // topAPToolStripMenuItem
            // 
            this.topAPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("topAPToolStripMenuItem.Image")));
            this.topAPToolStripMenuItem.Name = "topAPToolStripMenuItem";
            this.topAPToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.topAPToolStripMenuItem.Text = "Top AP";
            this.topAPToolStripMenuItem.Click += new System.EventHandler(this.topAPToolStripMenuItem_Click);
            // 
            // topAPTankToolStripMenuItem
            // 
            this.topAPTankToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("topAPTankToolStripMenuItem.Image")));
            this.topAPTankToolStripMenuItem.Name = "topAPTankToolStripMenuItem";
            this.topAPTankToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.topAPTankToolStripMenuItem.Text = "Top AP Tank";
            this.topAPTankToolStripMenuItem.Click += new System.EventHandler(this.topAPTankToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // groupBoxHealthPotions
            // 
            this.groupBoxHealthPotions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHealthPotions.Controls.Add(this.maxHealthPotionsLevel);
            this.groupBoxHealthPotions.Controls.Add(this.label4);
            this.groupBoxHealthPotions.Controls.Add(this.label3);
            this.groupBoxHealthPotions.Controls.Add(this.maxHealthPotionCount);
            this.groupBoxHealthPotions.Controls.Add(this.checkBoxBuyHealthPotion);
            this.groupBoxHealthPotions.Location = new System.Drawing.Point(12, 27);
            this.groupBoxHealthPotions.Name = "groupBoxHealthPotions";
            this.groupBoxHealthPotions.Size = new System.Drawing.Size(751, 112);
            this.groupBoxHealthPotions.TabIndex = 2;
            this.groupBoxHealthPotions.TabStop = false;
            this.groupBoxHealthPotions.Text = "Health Potions";
            // 
            // maxHealthPotionsLevel
            // 
            this.maxHealthPotionsLevel.Enabled = false;
            this.maxHealthPotionsLevel.Location = new System.Drawing.Point(317, 73);
            this.maxHealthPotionsLevel.Maximum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.maxHealthPotionsLevel.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.maxHealthPotionsLevel.Name = "maxHealthPotionsLevel";
            this.maxHealthPotionsLevel.Size = new System.Drawing.Size(120, 20);
            this.maxHealthPotionsLevel.TabIndex = 4;
            this.maxHealthPotionsLevel.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.maxHealthPotionsLevel.ValueChanged += new System.EventHandler(this.maxHealthPotionsLevel_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(293, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stop Buying Health Potion and sell them once reached level:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Max Health Potion Count to Buy:";
            // 
            // maxHealthPotionCount
            // 
            this.maxHealthPotionCount.Enabled = false;
            this.maxHealthPotionCount.Location = new System.Drawing.Point(173, 43);
            this.maxHealthPotionCount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.maxHealthPotionCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxHealthPotionCount.Name = "maxHealthPotionCount";
            this.maxHealthPotionCount.Size = new System.Drawing.Size(120, 20);
            this.maxHealthPotionCount.TabIndex = 1;
            this.maxHealthPotionCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxHealthPotionCount.ValueChanged += new System.EventHandler(this.maxHealthPotionCount_ValueChanged);
            // 
            // checkBoxBuyHealthPotion
            // 
            this.checkBoxBuyHealthPotion.AutoSize = true;
            this.checkBoxBuyHealthPotion.Location = new System.Drawing.Point(9, 19);
            this.checkBoxBuyHealthPotion.Name = "checkBoxBuyHealthPotion";
            this.checkBoxBuyHealthPotion.Size = new System.Drawing.Size(111, 17);
            this.checkBoxBuyHealthPotion.TabIndex = 0;
            this.checkBoxBuyHealthPotion.Text = "Buy Health Potion";
            this.checkBoxBuyHealthPotion.UseVisualStyleBackColor = true;
            this.checkBoxBuyHealthPotion.CheckedChanged += new System.EventHandler(this.checkBoxBuyHealthPotion_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 145);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxBuild);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(751, 519);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 5;
            // 
            // groupBoxBuild
            // 
            this.groupBoxBuild.Controls.Add(this.buildTree);
            this.groupBoxBuild.Controls.Add(this.buttonBuildMoveUp);
            this.groupBoxBuild.Controls.Add(this.buttonBuildMoveDown);
            this.groupBoxBuild.Controls.Add(this.buttonBuildRemove);
            this.groupBoxBuild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBuild.Location = new System.Drawing.Point(0, 0);
            this.groupBoxBuild.Name = "groupBoxBuild";
            this.groupBoxBuild.Size = new System.Drawing.Size(751, 225);
            this.groupBoxBuild.TabIndex = 4;
            this.groupBoxBuild.TabStop = false;
            this.groupBoxBuild.Text = "Build";
            // 
            // buildTree
            // 
            this.buildTree.AllowDrop = true;
            this.buildTree.AllowExternalDrop = false;
            this.buildTree.AllowUserToResizeColumns = false;
            this.buildTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buildTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.buildTree.BackgroundStyle.Class = "TreeBorderKey";
            this.buildTree.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.buildTree.Columns.Add(this.columnHeader1);
            this.buildTree.Columns.Add(this.columnHeader2);
            this.buildTree.Columns.Add(this.columnHeader3);
            this.buildTree.Columns.Add(this.columnHeader7);
            this.buildTree.Columns.Add(this.columnHeader8);
            this.buildTree.Columns.Add(this.columnHeader4);
            this.buildTree.DragDropEnabled = false;
            this.buildTree.DragDropNodeCopyEnabled = false;
            this.buildTree.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle;
            this.buildTree.ExpandWidth = 14;
            this.buildTree.Location = new System.Drawing.Point(6, 44);
            this.buildTree.Name = "buildTree";
            this.buildTree.NodeStyle = this.elementStyle2;
            this.buildTree.PathSeparator = ";";
            this.buildTree.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.buildTree.Size = new System.Drawing.Size(739, 175);
            this.buildTree.Styles.Add(this.elementStyle2);
            this.buildTree.Styles.Add(this.unreadEmailStyle);
            this.buildTree.TabIndex = 11;
            this.buildTree.Text = "advTree2";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width.Absolute = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "Icon";
            this.columnHeader2.Width.Absolute = 64;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Name = "columnHeader3";
            this.columnHeader3.StretchToFill = true;
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width.Absolute = 150;
            this.columnHeader3.Width.AutoSize = true;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Name = "columnHeader7";
            this.columnHeader7.Text = "Total Cost";
            this.columnHeader7.Width.Absolute = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Name = "columnHeader8";
            this.columnHeader8.Text = "Upgrade Cost";
            this.columnHeader8.Width.Absolute = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Name = "columnHeader4";
            this.columnHeader4.Text = "Sell ?";
            this.columnHeader4.Width.Absolute = 100;
            // 
            // elementStyle2
            // 
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // unreadEmailStyle
            // 
            this.unreadEmailStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.unreadEmailStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unreadEmailStyle.Name = "unreadEmailStyle";
            // 
            // buttonBuildMoveUp
            // 
            this.buttonBuildMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBuildMoveUp.Location = new System.Drawing.Point(457, 15);
            this.buttonBuildMoveUp.Name = "buttonBuildMoveUp";
            this.buttonBuildMoveUp.Size = new System.Drawing.Size(92, 23);
            this.buttonBuildMoveUp.TabIndex = 6;
            this.buttonBuildMoveUp.Text = "Move Up";
            this.buttonBuildMoveUp.UseVisualStyleBackColor = true;
            this.buttonBuildMoveUp.Click += new System.EventHandler(this.buttonBuildMoveUp_Click);
            // 
            // buttonBuildMoveDown
            // 
            this.buttonBuildMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBuildMoveDown.Location = new System.Drawing.Point(555, 15);
            this.buttonBuildMoveDown.Name = "buttonBuildMoveDown";
            this.buttonBuildMoveDown.Size = new System.Drawing.Size(92, 23);
            this.buttonBuildMoveDown.TabIndex = 5;
            this.buttonBuildMoveDown.Text = "Move Down";
            this.buttonBuildMoveDown.UseVisualStyleBackColor = true;
            this.buttonBuildMoveDown.Click += new System.EventHandler(this.buttonBuildMoveDown_Click);
            // 
            // buttonBuildRemove
            // 
            this.buttonBuildRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBuildRemove.Location = new System.Drawing.Point(653, 15);
            this.buttonBuildRemove.Name = "buttonBuildRemove";
            this.buttonBuildRemove.Size = new System.Drawing.Size(92, 23);
            this.buttonBuildRemove.TabIndex = 4;
            this.buttonBuildRemove.Text = "Remove";
            this.buttonBuildRemove.UseVisualStyleBackColor = true;
            this.buttonBuildRemove.Click += new System.EventHandler(this.buttonBuildRemove_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSellItem);
            this.groupBox3.Controls.Add(this.buttonAddItem);
            this.groupBox3.Controls.Add(this.textBoxFilterItemsDatabase);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dataGridItemDatabase);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(751, 290);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Item Database";
            // 
            // buttonSellItem
            // 
            this.buttonSellItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSellItem.Location = new System.Drawing.Point(634, 14);
            this.buttonSellItem.Name = "buttonSellItem";
            this.buttonSellItem.Size = new System.Drawing.Size(111, 23);
            this.buttonSellItem.TabIndex = 4;
            this.buttonSellItem.Text = "Add to Build ( Sell )";
            this.buttonSellItem.UseVisualStyleBackColor = true;
            this.buttonSellItem.Click += new System.EventHandler(this.buttonSellItem_Click);
            // 
            // buttonAddItem
            // 
            this.buttonAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddItem.Location = new System.Drawing.Point(517, 14);
            this.buttonAddItem.Name = "buttonAddItem";
            this.buttonAddItem.Size = new System.Drawing.Size(111, 23);
            this.buttonAddItem.TabIndex = 3;
            this.buttonAddItem.Text = "Add to Build ( Buy )";
            this.buttonAddItem.UseVisualStyleBackColor = true;
            this.buttonAddItem.Click += new System.EventHandler(this.buttonAddItem_Click);
            // 
            // textBoxFilterItemsDatabase
            // 
            this.textBoxFilterItemsDatabase.Location = new System.Drawing.Point(56, 16);
            this.textBoxFilterItemsDatabase.Name = "textBoxFilterItemsDatabase";
            this.textBoxFilterItemsDatabase.Size = new System.Drawing.Size(173, 20);
            this.textBoxFilterItemsDatabase.TabIndex = 2;
            this.textBoxFilterItemsDatabase.TextChanged += new System.EventHandler(this.textBoxFilterItemsDatabase_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search:";
            // 
            // dataGridItemDatabase
            // 
            this.dataGridItemDatabase.AllowUserToAddRows = false;
            this.dataGridItemDatabase.AllowUserToDeleteRows = false;
            this.dataGridItemDatabase.AllowUserToResizeColumns = false;
            this.dataGridItemDatabase.AllowUserToResizeRows = false;
            this.dataGridItemDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridItemDatabase.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridItemDatabase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridItemDatabase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemId,
            this.ItemIcon,
            this.ItemName,
            this.ItemGold});
            this.dataGridItemDatabase.EnableHeadersVisualStyles = false;
            this.dataGridItemDatabase.Location = new System.Drawing.Point(6, 43);
            this.dataGridItemDatabase.MultiSelect = false;
            this.dataGridItemDatabase.Name = "dataGridItemDatabase";
            this.dataGridItemDatabase.ReadOnly = true;
            this.dataGridItemDatabase.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridItemDatabase.RowHeadersVisible = false;
            this.dataGridItemDatabase.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridItemDatabase.RowTemplate.Height = 64;
            this.dataGridItemDatabase.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridItemDatabase.ShowCellErrors = false;
            this.dataGridItemDatabase.ShowCellToolTips = false;
            this.dataGridItemDatabase.ShowEditingIcon = false;
            this.dataGridItemDatabase.ShowRowErrors = false;
            this.dataGridItemDatabase.Size = new System.Drawing.Size(739, 241);
            this.dataGridItemDatabase.TabIndex = 0;
            // 
            // ItemId
            // 
            this.ItemId.FillWeight = 23.56322F;
            this.ItemId.HeaderText = "Id";
            this.ItemId.Name = "ItemId";
            this.ItemId.ReadOnly = true;
            this.ItemId.Width = 41;
            // 
            // ItemIcon
            // 
            this.ItemIcon.FillWeight = 31.42319F;
            this.ItemIcon.HeaderText = "Icon";
            this.ItemIcon.Name = "ItemIcon";
            this.ItemIcon.ReadOnly = true;
            this.ItemIcon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemIcon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ItemIcon.Width = 53;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemName.FillWeight = 335.8012F;
            this.ItemName.HeaderText = "Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // ItemGold
            // 
            this.ItemGold.FillWeight = 9.21237F;
            this.ItemGold.HeaderText = "Gold";
            this.ItemGold.Name = "ItemGold";
            this.ItemGold.ReadOnly = true;
            this.ItemGold.Width = 54;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Text files|*.txt";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "Text files|*.txt";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 676);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBoxHealthPotions);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(600, 550);
            this.Name = "Form1";
            this.Text = "AutoShop - Profile Creator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxHealthPotions.ResumeLayout(false);
            this.groupBoxHealthPotions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxHealthPotionsLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxHealthPotionCount)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxBuild.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buildTree)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridItemDatabase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem genericToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxHealthPotions;
        private System.Windows.Forms.ToolStripMenuItem jungleADToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jungleADTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jungleAPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jungleAPTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem midADToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem midAPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportADTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportAPBurstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportAPHealToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportAPTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topADToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topADTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topAPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topAPTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxBuild;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonAddItem;
        private System.Windows.Forms.TextBox textBoxFilterItemsDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridItemDatabase;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemId;
        private System.Windows.Forms.DataGridViewImageColumn ItemIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemGold;
        private System.Windows.Forms.Button buttonBuildRemove;
        private System.Windows.Forms.Button buttonBuildMoveUp;
        private System.Windows.Forms.Button buttonBuildMoveDown;
        private System.Windows.Forms.CheckBox checkBoxBuyHealthPotion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown maxHealthPotionCount;
        private System.Windows.Forms.NumericUpDown maxHealthPotionsLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private DevComponents.AdvTree.AdvTree buildTree;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private DevComponents.DotNetBar.ElementStyle unreadEmailStyle;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.ColumnHeader columnHeader2;
        private DevComponents.AdvTree.ColumnHeader columnHeader3;
        private DevComponents.AdvTree.ColumnHeader columnHeader7;
        private DevComponents.AdvTree.ColumnHeader columnHeader8;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonSellItem;
        private DevComponents.AdvTree.ColumnHeader columnHeader4;
    }
}

