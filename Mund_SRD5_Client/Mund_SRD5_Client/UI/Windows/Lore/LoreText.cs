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
    public class LoreText: Panel
    {
        private LoreViewer _parent;

        public static int padding = 5;
        public static int offset = 200;

        private static int imageDimensions = 150;

        private Panel ImagePanel = new Panel();
        private RichTextBox Title = new RichTextBox();
        private RichTextBox DescriptionText = new RichTextBox();

        private Font titleFont = new Font(FontFamily.GenericSansSerif, 25.0f);
        private Font textFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        public LoreText(LoreViewer parent)
        {
            _parent = parent;
            this.Resize += LoreText_Resize;
            this.Width = _parent.ClientRectangle.Width - offset - padding;
            this.Height = _parent.ClientRectangle.Height - (padding * 3) - parent.CurrentComboBox.Height;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Location = new Point(offset, (padding * 2) + parent.CurrentComboBox.Height);
            _parent.Controls.Add(this);

            ImagePanel.Size = new Size(imageDimensions, imageDimensions);
            this.Controls.Add(ImagePanel);

            Title.Size = new Size(this.ClientRectangle.Width - imageDimensions, imageDimensions);
            Title.Location = new Point(imageDimensions, 0);
            Title.BackColor = Color.Black;
            Title.ForeColor = Color.White;
            Title.Font = titleFont;
            Title.BorderStyle = BorderStyle.None;
            Title.ReadOnly = true;
            Title.SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(Title);

            DescriptionText.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height - imageDimensions);
            DescriptionText.Location = new Point(0, imageDimensions);
            DescriptionText.BackColor = Color.Black;
            DescriptionText.ForeColor = Color.White;
            DescriptionText.Font = textFont;
            DescriptionText.BorderStyle = BorderStyle.None;
            DescriptionText.ReadOnly = true;
            this.Controls.Add(DescriptionText);
        }

        public void SetActiveLore(Lore lore)
        {
            Image loadedImage = new Bitmap(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Lore\\" + lore.Image);
            ImagePanel.BackgroundImage = loadedImage;

            Title.Text = Environment.NewLine + lore.Name;

            DescriptionText.Text = StringLibrary.GetString(lore.Description);
        }

        void LoreText_Resize(object sender, EventArgs e)
        {
            Title.Size = new Size(this.ClientRectangle.Width - imageDimensions, imageDimensions);
            DescriptionText.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height - imageDimensions);
        }
    }
}
