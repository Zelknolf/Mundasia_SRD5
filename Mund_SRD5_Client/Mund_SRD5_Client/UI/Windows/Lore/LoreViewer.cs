using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mundasia.Client;

namespace Mundasia.Interface
{
    public class LoreViewer: Form
    {
        private static int padding = 5;
        private static int offset = 200;
        
        public LoreTOC CurrentLoreTOC;
        public LoreText CurrentLoreText;

        public ComboBox CurrentComboBox = new ComboBox();

        public LoreViewer()
        {
            this.Height = 600;
            this.Width = 800;
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };

            CurrentComboBox.Height = CurrentComboBox.PreferredHeight;
            CurrentComboBox.Width = LoreTOC.width;
            CurrentComboBox.Location = new Point(LoreTOC.padding, LoreTOC.padding);
            CurrentComboBox.Items.Add("<All entries>");
            CurrentComboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            CurrentComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            CurrentComboBox.AutoCompleteCustomSource = new AutoCompleteStringCollection();

            foreach(string category in Lore.Categories)
            {
                CurrentComboBox.Items.Add(category);
                CurrentComboBox.AutoCompleteCustomSource.Add(category);
            }

            CurrentComboBox.Text = "<All entries>";
            CurrentComboBox.SelectedIndexChanged += CurrentComboBox_SelectionChange;

            this.Controls.Add(CurrentComboBox);


            CurrentLoreText = new LoreText(this);
            CurrentLoreTOC = new LoreTOC(this);

            this.BackgroundImage = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\BackPattern.png");
            this.BackgroundImageLayout = ImageLayout.Tile;

            this.Text = "Mundasia Lore";

            this.Resize += LoreViewer_Resize;
        }

        void CurrentComboBox_SelectionChange(object sender, EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox) sender;

            string index = senderComboBox.SelectedItem.ToString();
            if (index == "<All entries>" || String.IsNullOrEmpty(index))
            {
                CurrentLoreTOC.PopulateList();
            }
            else
            {
                CurrentLoreTOC.PopulateList(Lore.CategoryLists[index]);
            }
        }

        void LoreViewer_Resize(object sender, EventArgs e)
        {
            CurrentLoreTOC.Height = this.ClientRectangle.Height - (padding * 3) - CurrentComboBox.Height;
            CurrentLoreText.Width = this.ClientRectangle.Width - offset - padding;
            CurrentLoreText.Height = this.ClientRectangle.Height - (padding * 3) - CurrentComboBox.Height;
        }
    }
}
