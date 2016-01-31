using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    class PrimaryForm : Form
    {
        public static PrimaryForm cachedForm;
        public static string ServerClock;

        public PrimaryForm()
        {
            this.Height = 768;
            this.Width = 1024;
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };

            this.BackgroundImage = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\BackPattern.png");
            this.BackgroundImageLayout = ImageLayout.Tile;

            this.Text = "Mundasia";
        }
    }
}
