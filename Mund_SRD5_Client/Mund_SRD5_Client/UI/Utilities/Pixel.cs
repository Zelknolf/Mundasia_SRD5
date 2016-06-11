using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Mund_SRD5_Client.UI.Utilities
{
    public class Pixel
    {
        public static Color FromHSL(int alpha, float hue, float saturation, float luminosity)
        {
            if(saturation < 0.00001)
            {
                int L = (int)luminosity;
                return Color.FromArgb(alpha, L, L, L);
            }

            double H = hue / 360d;

            double Max = luminosity < 0.5d ? luminosity * (1 + saturation) : (luminosity + saturation) - (luminosity * saturation);
            double Min = (luminosity * 2d) - Max;

            return Color.FromArgb(alpha, (int)(255 * _RGBChannelFromHue(Min, Max, H + 1 / 3d)),
                                            (int)(255 * _RGBChannelFromHue(Min, Max, H)),
                                            (int)(255 * _RGBChannelFromHue(Min, Max, H - 1 / 3d)));
        }

        private static double _RGBChannelFromHue(double m1, double m2, double h)
        {
            h = (h + 1d) % 1d;
            if (h < 0) h += 1;
            if (h * 6 < 1) return m1 + (m2 - m1) * 6 * h;
            else if (h * 2 < 1) return m2;
            else if (h * 3 < 2) return m1 + (m2 - m1) * 6 * (2d / 3d - h);
            else return m1;

        }

        public static float GetBrightness(Color color)
        {
            return (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f) / 256f;
        }
    }
}
