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

        Panel BroochPanel = new Panel();
        Panel HelmetPanel = new Panel();
        Panel LensPanel = new Panel();
        Panel CloakPanel = new Panel();
        Panel NeckSlotPanel = new Panel();
        Panel StonePanel = new Panel();
        Panel LeftRingPanel = new Panel();
        Panel RightRingPanel = new Panel();
        Panel LeftHandPanel = new Panel();
        Panel RightHandPanel = new Panel();
        Panel ChestPanel = new Panel();
        Panel BeltPanel = new Panel();
        Panel BracerPanel = new Panel();
        Panel GlovePanel = new Panel();
        Panel BootPanel = new Panel();

        Creature ChInv;

        bool eventsInitialized = false;

        public InventoryForm(Creature ch)
        {
            ChInv = ch;

            BroochPanel.Size = IconSize;
            HelmetPanel.Size = IconSize;
            LensPanel.Size = IconSize;

            CloakPanel.Size = IconSize;
            NeckSlotPanel.Size = IconSize;
            StonePanel.Size = IconSize;

            LeftHandPanel.Size = IconSize;
            ChestPanel.Size = IconSize;
            RightHandPanel.Size = IconSize;

            BracerPanel.Size = IconSize;
            BeltPanel.Size = IconSize;
            LeftRingPanel.Size = IconSize;

            GlovePanel.Size = IconSize;
            BootPanel.Size = IconSize;
            RightRingPanel.Size = IconSize;

            BroochPanel.Location = new Point(InventoryPadding, InventoryPadding);
            HelmetPanel.Location = new Point(BroochPanel.Location.X + BroochPanel.Width + InventoryPadding, InventoryPadding);
            LensPanel.Location = new Point(HelmetPanel.Location.X + HelmetPanel.Width + InventoryPadding, InventoryPadding);

            CloakPanel.Location = new Point(BroochPanel.Location.X, BroochPanel.Location.Y + BroochPanel.Height + InventoryPadding);
            NeckSlotPanel.Location = new Point(HelmetPanel.Location.X, CloakPanel.Location.Y);
            StonePanel.Location = new Point(LensPanel.Location.X, CloakPanel.Location.Y);

            LeftHandPanel.Location = new Point(BroochPanel.Location.X, CloakPanel.Location.Y + CloakPanel.Height + InventoryPadding);
            ChestPanel.Location = new Point(HelmetPanel.Location.X, LeftHandPanel.Location.Y);
            RightHandPanel.Location = new Point(LensPanel.Location.X, LeftHandPanel.Location.Y);

            BracerPanel.Location = new Point(BroochPanel.Location.X, LeftHandPanel.Location.Y + LeftHandPanel.Height + InventoryPadding);
            BeltPanel.Location = new Point(HelmetPanel.Location.X, BracerPanel.Location.Y);
            LeftRingPanel.Location = new Point(LensPanel.Location.X, BracerPanel.Location.Y);

            GlovePanel.Location = new Point(BroochPanel.Location.X, BracerPanel.Location.Y + BracerPanel.Height + InventoryPadding);
            BootPanel.Location = new Point(HelmetPanel.Location.X, GlovePanel.Location.Y);
            RightRingPanel.Location = new Point(LensPanel.Location.X, GlovePanel.Location.Y);

            unequippedItems.Location = new Point(InventoryPadding * 4 + IconSize.Width * 3, InventoryPadding);
            unequippedItems.Width = 300;
            unequippedItems.Height = InventoryPadding * 4 + IconSize.Height * 5;
            StyleListView(unequippedItems);

            this.BackColor = Color.Black;
            this.Size = new Size(unequippedItems.Location.X + unequippedItems.Width + InventoryPadding + this.Size.Width - this.ClientRectangle.Size.Width, unequippedItems.Location.Y + unequippedItems.Height + InventoryPadding + this.Size.Height - this.ClientRectangle.Size.Height);

            RefreshInventory();

            Controls.Add(NeckSlotPanel);
            Controls.Add(LeftRingPanel);
            Controls.Add(RightRingPanel);
            Controls.Add(LeftHandPanel);
            Controls.Add(RightHandPanel);
            Controls.Add(ChestPanel);
            Controls.Add(BeltPanel);
            Controls.Add(BroochPanel);
            Controls.Add(HelmetPanel);
            Controls.Add(LensPanel);
            Controls.Add(CloakPanel);
            Controls.Add(StonePanel);
            Controls.Add(BracerPanel);
            Controls.Add(GlovePanel);
            Controls.Add(BootPanel);
            Controls.Add(unequippedItems);

            this.FormClosed += InventoryForm_OnClosed;
            this.KeyDown += InventoryForm_KeyPress;
        }

        void RefreshInventory()
        {
            if (!eventsInitialized)
            {
                ChestPanel.DoubleClick += ChestPanel_DoubleClick;
                NeckSlotPanel.DoubleClick += NeckSlotPanel_DoubleClick;
                BeltPanel.DoubleClick += BeltPanel_DoubleClick;
                LeftRingPanel.DoubleClick += LeftRingPanel_DoubleClick;
                RightRingPanel.DoubleClick += RightRingPanel_DoubleClick;
                LeftHandPanel.DoubleClick += LeftHandPanel_DoubleClick;
                RightHandPanel.DoubleClick += RightHandPanel_DoubleClick;
                unequippedItems.DoubleClick += unequippedItems_DoubleClick;
                eventsInitialized = true;
            }

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

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Brooch))
            {
                BroochPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Brooch].Icon);
            }
            else
            {
                BroochPanel.BackgroundImage = GetInventoryIconByTag("EmptyBrooch");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Helm))
            {
                HelmetPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Helm].Icon);
            }
            else
            {
                HelmetPanel.BackgroundImage = GetInventoryIconByTag("EmptyHelmet");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Lens))
            {
                LensPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Lens].Icon);
            }
            else
            {
                LensPanel.BackgroundImage = GetInventoryIconByTag("EmptyLenses");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Cloak))
            {
                CloakPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Cloak].Icon);
            }
            else
            {
                CloakPanel.BackgroundImage = GetInventoryIconByTag("EmptyCloak");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Stone))
            {
                StonePanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Stone].Icon);
            }
            else
            {
                StonePanel.BackgroundImage = GetInventoryIconByTag("EmptyStone");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Bracers))
            {
                BracerPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Bracers].Icon);
            }
            else
            {
                BracerPanel.BackgroundImage = GetInventoryIconByTag("EmptyBracers");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Gloves))
            {
                GlovePanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Gloves].Icon);
            }
            else
            {
                GlovePanel.BackgroundImage = GetInventoryIconByTag("EmptyGloves");
            }

            if (ChInv.Equipment.ContainsKey((int)InventorySlot.Boots))
            {
                BootPanel.BackgroundImage = GetInventoryIconByTag(ChInv.Equipment[(int)InventorySlot.Boots].Icon);
            }
            else
            {
                BootPanel.BackgroundImage = GetInventoryIconByTag("EmptyBoots");
            }

            ImageList imgs = new ImageList();
            int imageIndex = 0;
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            unequippedItems.Items.Clear();
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
            string ret = ServiceConsumer.EquipItem(PlayerInterface.DrivingCharacter.AccountName, PlayerInterface.DrivingCharacter.CharacterName, ChInv.AccountName, ChInv.CharacterName, identifier, (int)slot);
            if (ret.StartsWith("Error")) return;
            else
            {
                ChInv = new Creature(ret);
                RefreshInventory();
                return;
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
                        else if(item.ItType == ItemType.Weapon)
                        {
                            equipItem(InventorySlot.RightHand, item.Identifier);
                        }
                        else if(item.ItType == ItemType.LightWeapon)
                        {
                            equipItem(InventorySlot.LeftHand, item.Identifier);
                        }
                        else if(item.ItType == ItemType.Ring)
                        {
                            equipItem(InventorySlot.LeftRing, item.Identifier);
                        }
                        else if(item.ItType == ItemType.HeavyOrAmmoWeapon)
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
