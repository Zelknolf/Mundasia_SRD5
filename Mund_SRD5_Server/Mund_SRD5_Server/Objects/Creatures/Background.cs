using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Mundasia.Objects
{
    public class Background
    {
        /// <summary>
        /// Deserializes a string into a background or accepts a background from a file line and
        /// and converts it to an object.
        /// </summary>
        /// <param name="fileLine">A pipe-delimited string containing the details of the background</param>
        public Background(string fileLine)
        {
            IsValid = false;
            string[] pieces = fileLine.Split('|');
            if (pieces.Length < 6) return;

            if (!uint.TryParse(pieces[0], out Id)) return;
            Name = pieces[1];
            if (!uint.TryParse(pieces[2], out SkillProf1)) return;
            if (!uint.TryParse(pieces[3], out SkillProf2)) return;
            if (!uint.TryParse(pieces[4], out ToolProf)) return;
            if (!uint.TryParse(pieces[5], out Description)) return;
            _iconFileName = pieces[6];
            IsValid = true;
        }

        /// <summary>
        /// Whether or not deserialization was successful.
        /// </summary>
        public bool IsValid = true;

        /// <summary>
        /// The background's Id, which can be used to refer to it easily.
        /// </summary>
        public uint Id;

        /// <summary>
        /// The background's name, which is used to display the background to humans.
        /// </summary>
        public string Name;

        /// <summary>
        /// The id of the first skill that this background provides proficiency in.
        /// </summary>
        public uint SkillProf1;

        /// <summary>
        /// The id of the second skill that this background provides proficiency in.
        /// </summary>
        public uint SkillProf2;

        /// <summary>
        /// The id of the tools that this background provides proficiency in.
        /// </summary>
        public uint ToolProf;

        /// <summary>
        /// The id of the description of this background.
        /// </summary>
        public uint Description;

        private string _iconFileName;

        private Image _icon;

        private static Image _nullIcon;

        public Image Icon
        {
            get
            {
                if (_icon != null) return _icon;
                string filePath = System.IO.Directory.GetCurrentDirectory() + "\\Images\\Backgrounds\\" + _iconFileName + ".png";
                if (File.Exists(filePath)) _icon = Image.FromFile(filePath);
                else _icon = NullBackgroundImage;
                return _icon;
            }
        }

        public static Image NullBackgroundImage
        {
            get
            {
                if (_nullIcon != null) return _nullIcon;
                _nullIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Backgrounds\\NoBackground.png");
                return _nullIcon;
            }
        }

        /// <summary>
        /// Static storage for backgrounds loaded by the static load function.
        /// </summary>
        private static Dictionary<uint, Background> _library = new Dictionary<uint, Background>();

        /// <summary>
        /// Open the default backgrounds.txt and load/cache all of the backgrounds from it.
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Backgrounds.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Background toAdd = new Background(read.ReadLine());
                    if(toAdd.IsValid) _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        /// <summary>
        /// Fetch a background by its Id.
        /// </summary>
        /// <param name="id">The ID to retrieve.</param>
        /// <returns>The background, or null if there is none.</returns>
        public static Background GetBackground(uint id)
        {
            if (_library.ContainsKey(id))
            {
                return _library[id];
            }
            return null;
        }

        /// <summary>
        /// Fetch a list of all loaded backgrounds.
        /// </summary>
        /// <returns>A collection of loaded backgrounds.</returns>
        public static IEnumerable<Background> GetBackgrounds()
        {
            return _library.Values;
        }
    }
}
