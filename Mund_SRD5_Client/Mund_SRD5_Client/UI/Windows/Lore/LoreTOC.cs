using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using Mundasia.Client;

namespace Mundasia.Interface
{
    public class LoreTOC: Panel
    {
        public static int padding = 5;
        public static int width = 185;

        private LoreViewer _parent;
        public ListView TOC = new ListView();
        
        public LoreTOC(LoreViewer parent)
        {
            _parent = parent;
            this.Width = LoreTOC.width;
            this.Height = parent.ClientRectangle.Height - (padding * 3) - parent.CurrentComboBox.Height;
            this.Location = new Point(padding, (padding * 2) + parent.CurrentComboBox.Height);

            this.BorderStyle = BorderStyle.Fixed3D;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;

            this.Resize += LoreTOC_Resize;
            TOC.ItemSelectionChanged += TOC_ItemSelectionChanged;

            parent.Controls.Add(this);

            TOC.BackColor = Color.Black;
            TOC.ForeColor = Color.White;
            TOC.BorderStyle = BorderStyle.None;
            TOC.Height = this.ClientRectangle.Height;
            TOC.Width = this.ClientRectangle.Width;
            TOC.View = View.Details;
            TOC.Columns.Add("Id");
            TOC.Columns.Add("Name");
            TOC.HeaderStyle = ColumnHeaderStyle.None;
            TOC.Font = labelFont;
            TOC.FullRowSelect = true;
            TOC.Columns[0].Width = 0;
            TOC.Columns[1].Width = TOC.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - padding;
            this.Controls.Add(TOC);

            PopulateList();
        }

        void TOC_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                uint lore;
                if(uint.TryParse(e.Item.SubItems[0].Text, out lore))
                {
                    Lore foundLore = Lore.GetLore(lore);
                    if(foundLore != null)
                    {
                        _parent.CurrentLoreText.SetActiveLore(foundLore);
                    }
                }
            }
        }

        void LoreTOC_Resize(object sender, EventArgs e)
        {
            TOC.Height = this.ClientRectangle.Height;
            TOC.Width = this.ClientRectangle.Width;
        }

        public void PopulateList()
        {
            TOC.Items.Clear();
            foreach (Lore l in Lore.GetLores())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { l.Id.ToString(), l.Name });
                StyleListViewItem(toAdd);
                TOC.Items.Add(toAdd);
            }
        }

        public void PopulateList(List<Lore> nList)
        {
            TOC.Items.Clear();
            foreach(Lore l in nList)
            {
                ListViewItem toAdd = new ListViewItem(new string[] { l.Id.ToString(), l.Name });
                StyleListViewItem(toAdd);
                TOC.Items.Add(toAdd);
            }
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }
    }
}
