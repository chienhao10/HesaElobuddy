using AutoShop;
using AutoShop.Controllers;
using AutoShopGUI.Controllers;
using DevComponents.AdvTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutoShopGUI
{
    public partial class Form1 : Form
    {
        private string ProfileSavePath = "";
        bool elementsEnabled = true;
        string AutoShopDirectoryPath = "";
        
        public Form1()
        {
            InitializeComponent();
            DisableEnableProfileElements();
            LoadItemDatabase();
            AutoShopDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EloBuddy\\AutoShop\\";
            if (!Directory.Exists(AutoShopDirectoryPath))
            {
                Directory.CreateDirectory(AutoShopDirectoryPath);
            }
            saveFileDialog.InitialDirectory = AutoShopDirectoryPath;
            openFileDialog.InitialDirectory = AutoShopDirectoryPath;
        }

        public void SetTitle(string title)
        {
            Text = "AutoShop - Profile Creator - " + title;
        }
        
        private void LoadItemDatabase()
        {
            ItemController.Initialize();
            foreach(var item in  ItemController.ItemDatabase)
            {
                dataGridItemDatabase.Rows.Add(item.id.ToString(), GetBitmapFromItemId(item.id), item.name, item.totalGold.ToString());
            }
        }

        public void DisableEnableProfileElements()
        {
            elementsEnabled = !elementsEnabled;
            buttonAddItem.Enabled = !buttonAddItem.Enabled;
            buttonSellItem.Enabled = !buttonSellItem.Enabled;
            groupBoxHealthPotions.Enabled = !groupBoxHealthPotions.Enabled;
            groupBoxBuild.Enabled = !groupBoxBuild.Enabled;
        }

        public Bitmap GetBitmapFromItemId(int itemId)
        {
            Bitmap icon = null;
            try
            {
                icon = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + itemId);
            }catch(Exception ex)
            {

            }
            if(icon == null)
            {
                //Download the image...

            }
            return icon;
        }

        private void textBoxFilterItemsDatabase_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBoxFilterItemsDatabase.Text))
            {
                foreach(DataGridViewRow row in dataGridItemDatabase.Rows)
                {
                    row.Visible = true;
                }
            }else
            {
                foreach (DataGridViewRow row in dataGridItemDatabase.Rows)
                {
                    if (row.Cells[2].Value.ToString().Replace("'", "").ToLower().Contains(textBoxFilterItemsDatabase.Text.Replace("'", "").ToLower()))
                    {
                        row.Visible = true;
                    }else
                    {
                        row.Visible = false;
                    }
                }
            }
        }
        #region File Menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildController.NewBuild();
            saveAsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = false;
            if(!elementsEnabled)
            DisableEnableProfileElements();

            buildTree.Nodes.Clear();

            checkBoxBuyHealthPotion.Checked = false;
            maxHealthPotionCount.Value = 3;
            maxHealthPotionsLevel.Value = 11;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void checkBoxBuyHealthPotion_CheckedChanged(object sender, EventArgs e)
        {
            BuildController.CurrentBuild.UseHPotion = checkBoxBuyHealthPotion.Checked;
            if(checkBoxBuyHealthPotion.Checked)
            {
                maxHealthPotionCount.Enabled = true;
                maxHealthPotionsLevel.Enabled = true;
            }
            else
            {
                maxHealthPotionCount.Enabled = false;
                maxHealthPotionsLevel.Enabled = false;
            }
        }

        private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string savePath = saveFileDialog.FileName;
            var result = BuildController.SaveBuildToFile(savePath);
            if(result)
            {
                SetTitle(saveFileDialog.FileName);
                ProfileSavePath = savePath;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            var selectedRow = dataGridItemDatabase.SelectedRows[0];
            if (selectedRow == null) return;
            var itemId = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
            var buildItem = BuildController.AddItem(itemId, false);
            if (buildItem == null) return;

            var node = new Node(buildItem.Id.ToString());
            node.Editable = false;
            node.DragDropEnabled = false;
            var iconCell = new Cell();

            iconCell.Images.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + buildItem.Id);
            node.Cells.Add(iconCell);


            node.Cells.Add(new Cell("" + buildItem.Name));
            node.Cells.Add(new Cell("" + buildItem.TotalCost));
            node.Cells.Add(new Cell("" + buildItem.UpgradeCost));
            node.Cells.Add(new Cell(buildItem.Sell ? "Yes" : "No"));

            node.Nodes.AddRange(GetSubNodes(buildItem));

            buildTree.Nodes.Add(node);
        }
        private void buttonSellItem_Click(object sender, EventArgs e)
        {
            var selectedRow = dataGridItemDatabase.SelectedRows[0];
            if (selectedRow == null) return;
            var itemId = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
            var buildItem = BuildController.AddItem(itemId, true);
            if (buildItem == null) return;

            var node = new Node(buildItem.Id.ToString());
            node.Editable = false;
            node.DragDropEnabled = false;
            var iconCell = new Cell();

            iconCell.Images.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + buildItem.Id);
            node.Cells.Add(iconCell);


            node.Cells.Add(new Cell("" + buildItem.Name));
            node.Cells.Add(new Cell("" + buildItem.TotalCost));
            node.Cells.Add(new Cell("" + buildItem.UpgradeCost));
            node.Cells.Add(new Cell(buildItem.Sell ? "Yes" : "No"));

            node.Nodes.AddRange(GetSubNodes(buildItem));

            buildTree.Nodes.Add(node);
        }

        Node[] GetSubNodes(BuildItem item)
        {
            var returnList = new List<Node>();

            foreach (var subItem in item.BuildFrom)
            {
                if (subItem != null)
                {
                    var node = new Node(subItem.Id.ToString());
                    node.Editable = false;
                    node.DragDropEnabled = false;
                    var iconCell = new Cell();
                    

                    iconCell.Images.Image = (Bitmap) Properties.Resources.ResourceManager.GetObject("_" + subItem.Id);
                    node.Cells.Add(iconCell);
                    
                    node.Cells.Add(new Cell("" + subItem.Name));
                    node.Cells.Add(new Cell("" + subItem.TotalCost));
                    node.Cells.Add(new Cell("" + subItem.UpgradeCost));
                    node.Cells.Add(new Cell(subItem.Sell ? "Yes" : "No"));

                    node.Nodes.AddRange(GetSubNodes(subItem));

                    returnList.Add(node);
                }
            }

            return returnList.ToArray();
        }
        
        private void buttonBuildRemove_Click(object sender, EventArgs e)
        {
            var node = buildTree.SelectedNode;
            if (node == null) return;

            if (node.Parent != null) return;
            var index = buildTree.Nodes.IndexOf(node);
            buildTree.Nodes.Remove(node);
            BuildController.RemoveItem(index);
        }
        
        private void buttonBuildMoveUp_Click(object sender, EventArgs e)
        {
            var node = buildTree.SelectedNode;
            if (node == null) return;
            
            var indexes = GetIndexesFromNode(node);

            node.MoveUp();
            buildTree.SelectedNode = node;

            BuildController.MoveUp(indexes);
        }
        
        public List<int> GetIndexesFromNode(Node node)
        {
            var indexes = new List<int>();
            
            while(node.Parent != null)
            {
                if(node.Parent != null)
                {
                    indexes.Add(node.Parent.Nodes.IndexOf(node));
                }
                node = node.Parent;
            }
            indexes.Add(buildTree.Nodes.IndexOf(node));

            indexes.Reverse();

            return indexes;
        }
        
        private void buttonBuildMoveDown_Click(object sender, EventArgs e)
        {
            var node = buildTree.SelectedNode;
            if (node == null) return;

            var indexes = GetIndexesFromNode(node);

            node.MoveDown();
            buildTree.SelectedNode = node;
            BuildController.MoveDown(indexes);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ProfileSavePath)) return;
            BuildController.SaveBuildToFile(ProfileSavePath);
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void LoadProfile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
            var result = BuildController.GetBuildFromFile(filePath);
            if(result != null)
            {
                SetTitle(filePath);
                ProfileSavePath = filePath;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                if(!elementsEnabled) DisableEnableProfileElements();
                BuildController.CurrentBuild = result;
                checkBoxBuyHealthPotion.Checked = result.UseHPotion;
                maxHealthPotionCount.Value = result.MaxHPotionCount;
                maxHealthPotionsLevel.Value = result.MaxHPotionLevel;
                buildTree.Nodes.Clear();

                foreach (var buildItem in result.Items)
                {
                    var node = new Node(buildItem.Id.ToString());
                    node.Editable = false;
                    node.DragDropEnabled = false;
                    var iconCell = new Cell();

                    iconCell.Images.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("_" + buildItem.Id);
                    node.Cells.Add(iconCell);


                    node.Cells.Add(new Cell("" + buildItem.Name));
                    node.Cells.Add(new Cell("" + buildItem.TotalCost));
                    node.Cells.Add(new Cell("" + buildItem.UpgradeCost));
                    node.Cells.Add(new Cell(buildItem.Sell ? "Yes" : "No"));

                    node.Nodes.AddRange(GetSubNodes(buildItem));

                    buildTree.Nodes.Add(node);
                }
            }
        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                SetTitle(openFileDialog.FileName);
                LoadProfile(openFileDialog.FileName);
            }
            openFileDialog.FileName = "";
        }
        
        private void maxHealthPotionCount_ValueChanged(object sender, EventArgs e)
        {
            if (BuildController.CurrentBuild == null) return;
            BuildController.CurrentBuild.MaxHPotionCount = (int) maxHealthPotionCount.Value;
        }

        private void maxHealthPotionsLevel_ValueChanged(object sender, EventArgs e)
        {

            if (BuildController.CurrentBuild == null) return;
            BuildController.CurrentBuild.MaxHPotionLevel = (int)maxHealthPotionsLevel.Value;
        }

        private void aDCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericADC.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericADC"));
                }
            }
            LoadProfile(profilePath);
        }

        private void midADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericMidAD.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericMidAD"));
                }
            }
            LoadProfile(profilePath);
        }

        private void midAPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericMidAP.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericMidAP"));
                }
            }
            LoadProfile(profilePath);
        }

        private void jungleADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericJungleAD.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericJungleAD"));
                }
            }
            LoadProfile(profilePath);
        }

        private void jungleADTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericJungleADTank.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericJungleADTank"));
                }
            }
            LoadProfile(profilePath);
        }

        private void jungleAPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericJungleAP.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericJungleAP"));
                }
            }
            LoadProfile(profilePath);
        }

        private void jungleAPTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericJungleAPTank.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericJungleAPTank"));
                }
            }
            LoadProfile(profilePath);
        }

        private void supportADTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericSupportADTank.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericSupportADTank"));
                }
            }
            LoadProfile(profilePath);
        }

        private void supportAPBurstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericSupportAPBurst.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericSupportAPBurst"));
                }
            }
            LoadProfile(profilePath);
        }

        private void supportAPHealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericSupportAPHeal.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericSupportAPHeal"));
                }
            }
            LoadProfile(profilePath);
        }

        private void supportAPTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericSupportAPTank.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericSupportAPTank"));
                }
            }
            LoadProfile(profilePath);
        }

        private void topADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericTopAD.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericTopAD"));
                }
            }
            LoadProfile(profilePath);
        }

        private void topADTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericTopADTank.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericTopADTank"));
                }
            }
            LoadProfile(profilePath);
        }

        private void topAPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericTopAP.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericTopAP"));
                }
            }
            LoadProfile(profilePath);
        }

        private void topAPTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var profilePath = AutoShopDirectoryPath + "GenericTopAPTank.txt";
            if (!File.Exists(profilePath))
            {
                using (StreamWriter writer = new StreamWriter(profilePath))
                {
                    writer.Write(Properties.Resources.ResourceManager.GetString("GenericTopAPTank"));
                }
            }
            LoadProfile(profilePath);
        }
    }
}
 