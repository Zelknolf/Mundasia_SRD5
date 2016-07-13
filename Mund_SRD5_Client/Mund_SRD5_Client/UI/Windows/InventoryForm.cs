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
    public class InventoryForm: Form
    {
        int InventoryPadding = 20;
        Size IconSize = new Size(64, 64);
        ListView unequippedItems = new ListView();

        Panel NeckSlotPanel = new Panel();
        Panel LeftRingPanel = new Panel();
        Panel RightRingPanel = new Panel();
        Panel LeftHandPanel = new Panel();
        Panel RightHandPanel = new Panel();
        Panel ChestPanel = new Panel();
        Panel BeltPanel = new Panel();

        Creature ChInv;

        public InventoryForm(Creature ch)
        {
            ChInv = ch;
            if(ch.Equipment.ContainsKey((int)InventorySlot.Chest))
            {
                ChestPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.Chest].Icon);
            }
            else
            {
                ChestPanel.BackgroundImage = GetInventoryIconByTag("EmptyChest");
            }

            if(ch.Equipment.ContainsKey((int)InventorySlot.Neck))
            {
                NeckSlotPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.Neck].Icon);
            }
            else
            {
                NeckSlotPanel.BackgroundImage = GetInventoryIconByTag("EmptyNeck");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.Belt))
            {
                BeltPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.Belt].Icon);
            }
            else
            {
                BeltPanel.BackgroundImage = GetInventoryIconByTag("EmptyBelt");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.LeftRing))
            {
                LeftRingPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.LeftRing].Icon);
            }
            else
            {
                LeftRingPanel.BackgroundImage = GetInventoryIconByTag("EmptyRing");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.RightRing))
            {
                RightRingPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.RightRing].Icon);
            }
            else
            {
                RightRingPanel.BackgroundImage = GetInventoryIconByTag("EmptyRing");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.LeftHand))
            {
                LeftHandPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.LeftHand].Icon);
            }
            else
            {
                LeftHandPanel.BackgroundImage = GetInventoryIconByTag("EmptyHand");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.RightHand))
            {
                RightHandPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.RightHand].Icon);
            }
            else
            {
                RightHandPanel.BackgroundImage = GetInventoryIconByTag("EmptyHand");
            }

            NeckSlotPanel.Size = IconSize;
            LeftRingPanel.Size = IconSize;
            RightRingPanel.Size = IconSize;
            LeftHandPanel.Size = IconSize;
            RightHandPanel.Size = IconSize;
            ChestPanel.Size = IconSize;
            BeltPanel.Size = IconSize;

            LeftHandPanel.Location = new Point(InventoryPadding, InventoryPadding * 2 + IconSize.Height);
            LeftRingPanel.Location = new Point(InventoryPadding, InventoryPadding * 3 + IconSize.Height * 2);

            NeckSlotPanel.Location = new Point(InventoryPadding * 2 + IconSize.Width, InventoryPadding);
            ChestPanel.Location = new Point(InventoryPadding * 2 + IconSize.Width, InventoryPadding * 2 + IconSize.Height);
            BeltPanel.Location = new Point(InventoryPadding * 2 + IconSize.Width, InventoryPadding * 3 + IconSize.Height * 2);

            RightHandPanel.Location = new Point(InventoryPadding * 3 + IconSize.Width * 2, InventoryPadding * 2 + IconSize.Height);
            RightRingPanel.Location = new Point(InventoryPadding * 3 + IconSize.Width * 2, InventoryPadding * 3 + IconSize.Height * 2);

            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            unequippedItems.Location = new Point(InventoryPadding * 4 + IconSize.Width * 3, InventoryPadding);
            unequippedItems.Width = 300;
            unequippedItems.Height = InventoryPadding * 2 + IconSize.Height * 3;
            StyleListView(unequippedItems);
            unequippedItems.DoubleClick += unequippedItems_DoubleClick;
            int imageIndex = 0;

            foreach(InventoryItem item in ch.Inventory)
            {
                ListViewItem toAdd = new ListViewItem(new string[] { "", item.Name });
                toAdd.Tag = item.Identifier;
                toAdd.ImageIndex = imageIndex;
                StyleListViewItem(toAdd);
                imgs.Images.Add(GetInventoryIconByTag(item.Icon));
                imageIndex++;
                unequippedItems.Items.Add(toAdd);
            }
            unequippedItems.SmallImageList = imgs;


            this.BackColor = Color.Black;
            this.Size = new Size(InventoryPadding * 5 + IconSize.Width * 3 + 300 + this.Size.Width - this.ClientRectangle.Size.Width, InventoryPadding * 4 + IconSize.Height * 3 + this.Size.Height - this.ClientRectangle.Size.Height);

            Controls.Add(NeckSlotPanel);
            Controls.Add(LeftRingPanel);
            Controls.Add(RightRingPanel);
            Controls.Add(LeftHandPanel);
            Controls.Add(RightHandPanel);
            Controls.Add(ChestPanel);
            Controls.Add(BeltPanel);
            Controls.Add(unequippedItems);

            this.FormClosed += InventoryForm_OnClosed;
            this.KeyDown += InventoryForm_KeyPress;
        }

        void RefreshInventory()
        {
            ChestPanel.DoubleClick += ChestPanel_DoubleClick;
            NeckSlotPanel.DoubleClick += NeckSlotPanel_DoubleClick;
            BeltPanel.DoubleClick += BeltPanel_DoubleClick;
            LeftRingPanel.DoubleClick += LeftRingPanel_DoubleClick;
            RightRingPanel.DoubleClick += RightRingPanel_DoubleClick;
            LeftHandPanel.DoubleClick += LeftHandPanel_DoubleClick;
            RightHandPanel.DoubleClick += RightHandPanel_DoubleClick;
            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Chest))
            {
                ChestPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Chest].Icon);
            }
            else
            {
                ChestPanel.BackgroundImage = GetInventoryIconByTag("EmptyChest");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Neck))
            {
                NeckSlotPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Neck].Icon);
            }
            else
            {
                NeckSlotPanel.BackgroundImage = GetInventoryIconByTag("EmptyNeck");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Belt))
            {
                BeltPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Belt].Icon);
            }
            else
            {
                BeltPanel.BackgroundImage = GetInventoryIconByTag("EmptyBelt");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.LeftRing))
            {
                LeftRingPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.LeftRing].Icon);
            }
            else
            {
                LeftRingPanel.BackgroundImage = GetInventoryIconByTag("EmptyRing");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.RightRing))
            {
                RightRingPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.RightRing].Icon);
            }
            else
            {
                RightRingPanel.BackgroundImage = GetInventoryIconByTag("EmptyRing");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.LeftHand))
            {
                LeftHandPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.LeftHand].Icon);
            }
            else
            {
                LeftHandPanel.BackgroundImage = GetInventoryIconByTag("EmptyHand");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.RightHand))
            {
                RightHandPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.RightHand].Icon);
            }
            else
            {
                RightHandPanel.BackgroundImage = GetInventoryIconByTag("EmptyHand");
            }

            ImageList imgs = new ImageList();
            int imageIndex = 0;

            unequippedItems.Clear();
            foreach (InventoryItem item in ChInv.Inventory)
            {
                ListViewItem toAdd = new ListViewItem(new string[] { "", item.Name });
                toAdd.Tag = item.Identifier;
                toAdd.ImageIndex = imageIndex;
                StyleListViewItem(toAdd);
                imgs.Images.Add(GetInventoryIconByTag(item.Icon));
                imageIndex++;
                unequippedItems.Items.Add(toAdd);
            }
            unequippedItems.SmallImageList = imgs;
        }

        private void unequipItem(InventorySlot slot)
        {
            equipItem(slot, String.Empty);
        }

        private void equipItem(InventorySlot slot, string identifier)
        {
            if (ChInv.Equipment.ContainsKey((int)slot))
            {
                string ret = ServiceConsumer.EquipItem(PlayerInterface.DrivingCharacter.AccountName, PlayerInterface.DrivingCharacter.CharacterName, ChInv.AccountName, ChInv.CharacterName, identifier, (int)slot);
                if (ret.StartsWith("Error")) return;
                else
                {
                    ChInv = new Creature(ret);
                    RefreshInventory();
                    return;
                }
            }
        }

        void ChestPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.Chest);
        }

        void NeckSlotPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.Neck);
        }

        void BeltPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.Belt);
        }

        void LeftRingPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.LeftRing);
        }

        void RightRingPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.RightRing);
        }

        void LeftHandPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.LeftHand);
        }

        void RightHandPanel_DoubleClick(object sender, EventArgs e)
        {
            unequipItem(InventorySlot.RightHand);
        }

        void unequippedItems_DoubleClick(object sender, EventArgs e)
        {
            foreach(ListViewItem it in unequippedItems.SelectedItems)
            {
                foreach(InventoryItem item in ChInv.Inventory)
                {
                    if(it.Tag.ToString() == item.Identifier)
                    {
                        if(item.ItType == ItemType.Clothing)
                        {
                            equipItem(InventorySlot.Chest, item.Identifier);
                        }
                        else if (item.ItType == ItemType.Belt)
                        {
                            equipItem(InventorySlot.Belt, item.Identifier);
                        }
                        else if(item.ItType == ItemType.Necklace)
                        {
                            equipItem(InventorySlot.Neck, item.Identifier);
                        }
                        else if(item.ItType == ItemType.OneHand)
                        {
                            equipItem(InventorySlot.RightHand, item.Identifier);
                        }
                        else if(item.ItType == ItemType.OffHand)
                        {
                            equipItem(InventorySlot.LeftHand, item.Identifier);
                        }
                        else if(item.ItType == ItemType.Ring)
                        {
                            equipItem(InventorySlot.LeftRing, item.Identifier);
                        }
                        else if(item.ItType == ItemType.TwoHand)
                        {
                            equipItem(InventorySlot.RightHand, item.Identifier);
                        }
                    }
                }
            }
        }
        
        public static Image GetInventoryIconByTag(string Tag)
        {
            string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar + "Icons" + Path.DirectorySeparatorChar + "Items" + Path.DirectorySeparatorChar + Tag + ".png";
            if(IconCache.ContainsKey(Tag))
            {
                return IconCache[Tag];
            }
            if(!File.Exists(path))
            {
                if(!IconCache.ContainsKey("Unknown"))
                {
                    IconCache.Add("Unknown", Image.FromFile(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar + "Icons" + Path.DirectorySeparatorChar + "Items" + Path.DirectorySeparatorChar + "Unknown.png"));
                }
                return IconCache["Unknown"];
            }
            IconCache.Add(Tag, Image.FromFile(path));
            return IconCache[Tag];
        }

        public static Dictionary<string, Image> IconCache = new Dictionary<string, Image>();

        private void InventoryForm_OnClosed(object Sender, EventArgs e)
        {
            PlayerInterface.InventoryWindow = null;
        }

        private void InventoryForm_KeyPress(object Sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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
