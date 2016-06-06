using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;


namespace Mundasia.Objects
{
    public class Power
    {
        /// <summary>
        /// Default delimiter used to split properties from config files.
        /// </summary>
        const char delim = '|';

        /// <summary>
        /// Turns a line on the powers resource files into a power object.
        /// </summary>
        /// <param name="fileLine">the string from a line in the config file</param>
        public Power(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { delim });
            uint index = 0;
            if(uint.TryParse(split[0], out index))
            {
                Id = index;

                Name = split[1];

                uint desc = 0;
                if(uint.TryParse(split[2], out desc))
                {
                    Description = desc;
                }
                else
                {
                    Description = uint.MaxValue;
                }

                _iconFileName = split[3];
            }
            else
            {
                Id = uint.MaxValue;
                Name = "Error: " + fileLine;
                Description = uint.MaxValue;
            }
        }

        /// <summary>
        /// Create a new power with given properties
        /// </summary>
        /// <param name="id">the ID of the power</param>
        /// <param name="name">the Name of the power</param>
        /// <param name="description">The string reference for the power's description</param>
        public Power(uint id, string name, uint description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// The power's Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// The power's name
        /// </summary>
        public string Name;

        /// <summary>
        /// A string reference for the description of the power.
        /// </summary>
        public uint Description;

        /// <summary>
        /// Caching for loaded powers.
        /// </summary>
        private static Dictionary<uint, Power> _library = new Dictionary<uint, Power>();

        private string _iconFileName;

        private Image _icon;

        private static Image _nullIcon;

        public Image Icon
        {
            get
            {
                if (_icon != null) return _icon;
                _icon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Powers\\" + _iconFileName + ".png");
                return _icon;
            }
        }

        public static Image NullIcon
        {
            get
            {
                if (_nullIcon != null) return _nullIcon;
                _nullIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Powers\\NoPower.png");
                return _nullIcon;
            }
        }

        /// <summary>
        /// Load powers from the DataArrays\Powers.txt folder and store them in _library
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Powers.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Power toAdd = new Power(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        /// <summary>
        /// Fetch a Power from _library
        /// </summary>
        /// <param name="skillId">The power's Id</param>
        /// <returns>The power on success; null on failure</returns>
        public static Power GetPower(uint powerId)
        {
            if (_library.ContainsKey(powerId))
            {
                return _library[powerId];
            }
            return null;
        }

        /// <summary>
        /// Get all currently-loaded powers as an enumerable.
        /// </summary>
        /// <returns>an IEnumerable containing all loaded powers</returns>
        public static IEnumerable<Power> GetPowers()
        {
            return _library.Values;
        }
    }
}
