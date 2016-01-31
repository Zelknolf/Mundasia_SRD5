using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    public class ClothColor
    {
        public static Dictionary<int, Color> Light = new Dictionary<int, Color>();
        public static Dictionary<int, Color> Med = new Dictionary<int, Color>();
        public static Dictionary<int, Color> Dark = new Dictionary<int, Color>();

        public static void LoadColors()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Cloth_Color.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    string[] colorLine = read.ReadLine().Split(new char[] { '|' });
                    int index = 0;
                    int.TryParse(colorLine[0], out index);
                    foreach (string color in colorLine)
                    {
                        if (!color.Contains(',')) continue;
                        string[] splitColor = colorLine[1].Split(new char[] { ',' });

                        Light.Add(index, Color.FromArgb(Convert.ToInt32("FF" + splitColor[2], 16)));
                        Med.Add(index, Color.FromArgb(Convert.ToInt32("FF" + splitColor[1], 16)));
                        Dark.Add(index, Color.FromArgb(Convert.ToInt32("FF" + splitColor[0], 16)));
                    }
                }
            }
        }
    }
}
