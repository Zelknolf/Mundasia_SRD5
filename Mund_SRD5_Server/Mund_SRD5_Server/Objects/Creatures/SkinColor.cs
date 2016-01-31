using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Mundasia.Objects
{
    public class SkinColor
    {
        public Color LineColor;
        public Color DarkColor;
        public Color MedColor;
        public Color LightColor;

        public SkinColor(string fileLine)
        {
            string[] color = fileLine.Split(new char[] { ',' });

            LineColor = Color.FromArgb(Convert.ToInt32("FF" + color[0], 16));
            DarkColor = Color.FromArgb(Convert.ToInt32("FF" + color[1], 16));
            MedColor = Color.FromArgb(Convert.ToInt32("FF" + color[2], 16));
            LightColor = Color.FromArgb(Convert.ToInt32("FF" + color[3], 16));
        }
    }
}
