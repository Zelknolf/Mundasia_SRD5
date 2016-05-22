using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Mundasia.Objects
{
    public class Alignment
    {
        static char[] delim = new char[] { '|' };

        public Alignment(string fileLine)
        {
            string[] split = fileLine.Split(delim);

            Id = uint.Parse(split[0]);
            Name = split[1];
            Lawful = split[2] == "1";
            Chaotic = split[3] == "1";
            Good = split[4] == "1";
            Evil = split[5] == "1";
            Description = uint.Parse(split[6]);
            _iconFileName = split[7];
        }

        /// <summary>
        /// The Id of the alignment
        /// </summary>
        public uint Id;
        
        /// <summary>
        /// The plain-text name of the alignment
        /// </summary>
        public string Name;

        /// <summary>
        /// Boolean indicates whether or not this alignment is lawful.
        /// </summary>
        public bool Lawful;

        /// <summary>
        /// Boolean indicates whether or not this alignment is chaotic.
        /// </summary>
        public bool Chaotic;

        /// <summary>
        /// Boolean indicates whether or not this alignment is good.
        /// </summary>
        public bool Good;

        /// <summary>
        /// Boolean indicates whether or not this alignment is evil.
        /// </summary>
        public bool Evil;

        /// <summary>
        /// String reference indicates the description of the alignment.
        /// </summary>
        public uint Description;

        private string _iconFileName;

        private Image _icon;

        /// <summary>
        /// An icon to visually represent this alignment.
        /// </summary>
        public Image Icon
        {
            get
            {
                if (_icon != null) return _icon;
                _icon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Alignments\\" + _iconFileName + ".png");
                return _icon;
            }
        }

        private static Image _nullIcon;

        /// <summary>
        /// An icon ti visually represent the absence of an alignment.
        /// </summary>
        public static Image NullAlignmentImage
        {
            get
            {
                if (_nullIcon != null) return _nullIcon;
                _nullIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Alignments\\NoAlignment.png");
                return _nullIcon;
            }
        }


        /// <summary>
        /// Static storage for alignments loaded by the static load function.
        /// </summary>
        private static Dictionary<uint, Alignment> _library = new Dictionary<uint, Alignment>();

        /// <summary>
        /// Open the default alignments.txt and load/cache all of the alignments from it.
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Alignments.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Alignment toAdd = new Alignment(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        /// <summary>
        /// Fetch an alignment by its Id.
        /// </summary>
        /// <param name="id">The ID to retrieve.</param>
        /// <returns>The alignment, or null if there is none.</returns>
        public static Alignment GetAlignment(uint id)
        {
            if (_library.ContainsKey(id))
            {
                return _library[id];
            }
            return null;
        }

        /// <summary>
        /// Fetch a list of all loaded alignments.
        /// </summary>
        /// <returns>A collection of loaded alignments.</returns>
        public static IEnumerable<Alignment> GetAlignments()
        {
            return _library.Values;
        }
    }
}
