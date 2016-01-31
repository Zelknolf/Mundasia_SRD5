using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mundasia.Client
{
    [System.ComponentModel.DesignerCategory("")]
    public partial class SplashScreen : Form
    {
        public ProgressBar progress = new ProgressBar();

        public SplashScreen()
        {
            InitializeComponent();

            this.Width = 500;
            this.Height = 500;
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point 
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };

            this.BackgroundImage = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\SplashScreen.png");

            progress.Size = new Size(400, 10);
            progress.Location = new Point(50, 395);
            progress.Step = 1;
            progress.Maximum = 17;
            progress.Style = ProgressBarStyle.Continuous;
            progress.ForeColor = Color.DarkViolet;
            progress.PerformStep();

            this.Controls.Add(progress);
        }
    }
}
