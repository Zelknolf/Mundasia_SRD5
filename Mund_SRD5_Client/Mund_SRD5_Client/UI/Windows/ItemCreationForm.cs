using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mundasia.Communication;
using Mundasia.Objects;
using System.Drawing;
using System.IO;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    public class ItemCreationForm: Form
    {
        int InventoryPadding = 20;
        Size IconSize = new Size(64, 64);
        ListView iconSelect = new ListView();
        Label itemNameLabel = new Label();
        TextBox itemName = new TextBox();
        Label appearanceLabel = new Label();
        TextBox appearance = new TextBox();
        Label primaryColorLabel = new Label();
        TextBox primaryColor = new TextBox();
        Label secondaryColorLabel = new Label();
        TextBox secondaryColor = new TextBox();

        public ItemCreationForm()
        {
            int boxLoc = 0;
            int boxWidth = 0;
            itemNameLabel.Text = "Name";
            itemNameLabel.Size = itemNameLabel.PreferredSize;
            itemNameLabel.Location = new Point(InventoryPadding, InventoryPadding);

            appearanceLabel.Text = "Appearance";
            appearanceLabel.Size = appearanceLabel.PreferredSize;
            appearanceLabel.Location = new Point(InventoryPadding, InventoryPadding + itemName.PreferredHeight);

            primaryColorLabel.Text = "Primary Color";
            primaryColorLabel.Size = primaryColorLabel.PreferredSize;
            primaryColorLabel.Location = new Point(InventoryPadding, InventoryPadding + itemName.PreferredHeight * 2);

            secondaryColorLabel.Text = "Secondary Color";
            secondaryColorLabel.Size = secondaryColorLabel.PreferredSize;
            secondaryColorLabel.Location = new Point(InventoryPadding, InventoryPadding + itemName.PreferredHeight * 3);

            boxLoc = Math.Max(Math.Max(itemNameLabel.Width, appearanceLabel.Width), Math.Max(primaryColorLabel.Width, secondaryColorLabel.Width)) + InventoryPadding;
            boxWidth = InventoryPadding * 3 + IconSize.Width * 3 - boxLoc;

            itemName.Size = new Size(boxWidth, itemName.PreferredHeight);
            itemName.Location = new Point(boxLoc, InventoryPadding);

            appearance.Size = new Size(boxWidth, appearance.PreferredHeight);
            appearance.Location = new Point(boxLoc, InventoryPadding + itemName.Height);

            primaryColor.Size = new Size(boxWidth, primaryColor.PreferredHeight);
            primaryColor.Location = new Point(boxLoc, InventoryPadding + itemName.Height * 2);

            secondaryColor.Size = new Size(boxWidth, secondaryColor.PreferredHeight);
            secondaryColor.Location = new Point(boxLoc, InventoryPadding + itemName.Height * 3);

            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            iconSelect.Location = new Point(InventoryPadding * 4 + IconSize.Width * 3, InventoryPadding);
            iconSelect.Width = 300;
            iconSelect.Height = InventoryPadding * 2 + IconSize.Height * 3;
            StyleListView(iconSelect);
            int imageIndex = 0;

            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar + "Icons" + Path.DirectorySeparatorChar + "Items"))
            {
                string tag = Path.GetFileName(file).Split('.')[0];
                Image icon = Image.FromFile(file);
                ListViewItem toAdd = new ListViewItem(new string[] { "", tag });
                toAdd.Tag = tag;
                toAdd.ImageIndex = imageIndex;
                StyleListViewItem(toAdd);
                imgs.Images.Add(icon);
                imageIndex++;
                iconSelect.Items.Add(toAdd);
            }

            this.Controls.Add(appearanceLabel);
            this.Controls.Add(appearance);
            this.Controls.Add(primaryColor);
            this.Controls.Add(primaryColorLabel);
            this.Controls.Add(secondaryColor);
            this.Controls.Add(secondaryColorLabel);
            this.Controls.Add(itemNameLabel);
            this.Controls.Add(itemName);
            this.Controls.Add(iconSelect);
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 14.0f);

        private static void StyleListView(ListView listView)
        {
            listView.Clear();
            listView.Columns.Add("");
            listView.Columns.Add("");
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.BackColor = Color.Black;
            listView.ForeColor = Color.White;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.Font = labelFont;
            listView.Columns[0].Width = 64;
            listView.Columns[1].Width = listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - 64;
        }

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }
    }
}
