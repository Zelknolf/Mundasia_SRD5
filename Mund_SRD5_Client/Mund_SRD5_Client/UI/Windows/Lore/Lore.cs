using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Client
{
    public class Lore : IComparable
    {
        public Lore(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;
                Name = split[1];
                Image = split[2];
                Category = split[3];

                if (!uint.TryParse(split[4], out Description))
                {
                    Description = uint.MaxValue;
                }
            }
        }

        public int CompareTo(object other)
        {
            Lore otherLore = other as Lore;
            if(otherLore == null)
            {
                return other.ToString().CompareTo(this.ToString());
            }
            return this.Name.CompareTo(otherLore.Name);
        }

        public string Name;
        public string Image;
        public string Category;
        public uint Id;
        public uint Description;

        public static List<string> Categories = new List<string>();
        public static Dictionary<string, List<Lore>> CategoryLists = new Dictionary<string, List<Lore>>();
        private static Dictionary<uint, Lore> _library = new Dictionary<uint, Lore>();
        private static List<Lore> _sortedLores = new List<Lore>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Lore.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Lore toAdd = new Lore(read.ReadLine());
                    if (!Categories.Contains(toAdd.Category))
                    {
                        Categories.Add(toAdd.Category);
                    }
                    if(!CategoryLists.ContainsKey(toAdd.Category))
                    {
                        CategoryLists.Add(toAdd.Category, new List<Lore>());
                    }
                    CategoryLists[toAdd.Category].Add(toAdd);
                    _library.Add(toAdd.Id, toAdd);
                    _sortedLores.Add(toAdd);
                }
            }
            Categories.Sort();
            _sortedLores.Sort();
        }

        public static Lore GetLore(uint index)
        {
            if (_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Lore> GetLores()
        {
            return _sortedLores;
        }
    }
}
