using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Mundasia.Objects
{
    public class TileSet
    {
        public TileSet(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;
                Name = split[1];

                uint.TryParse(split[2], out BlastsInto);
            }
        }

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Tilesets.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    TileSet toAdd = new TileSet(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public string Name;
        public uint Id;
        public uint BlastsInto;

        public static Dictionary<uint, TileSet> _library = new Dictionary<uint, TileSet>();

        public static TileSet GetSet(uint Id)
        {
            if(_library.ContainsKey(Id))
            {
                return _library[Id];
            }
            return null;
        }

        public static IEnumerable<TileSet> GetSets()
        {
            return _library.Values;
        }
    }
}
