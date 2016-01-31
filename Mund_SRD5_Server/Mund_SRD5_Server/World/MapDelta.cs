using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    public class MapDelta
    {
        private static string delimiter = "─";
        private static char[] delim = new char[] { '─' };
        private static string dictDelimiter = "│";
        private static char[] dictDelim = new char[] { '│' };
        private static string keyDelimiter = "┌";
        private static char[] keyDelim = new char[] { '┌' };

        public List<Tile> AddedTiles = new List<Tile>();

        public List<Tile> RemovedTiles = new List<Tile>();

        public Dictionary<uint, DisplayCharacter> ChangedCharacters = new Dictionary<uint, DisplayCharacter>();

        public Dictionary<uint, DisplayCharacter> AddedCharacters = new Dictionary<uint, DisplayCharacter>();

        public Dictionary<uint, DisplayCharacter> RemovedCharacters = new Dictionary<uint, DisplayCharacter>();

        public MapDelta() { }

        public MapDelta(string fileLine)
        {
            string[] cols = fileLine.Split(delim);
            if (cols.Length < 5) return;
            string[] tiles = cols[0].Split(dictDelim);
            foreach(string t in tiles)
            {
                if(!String.IsNullOrWhiteSpace(t))
                {
                    AddedTiles.Add(new Tile(t));
                }
            }
            tiles = cols[1].Split(dictDelim);
            foreach (string t in tiles)
            {
                if (!String.IsNullOrWhiteSpace(t))
                {
                    RemovedTiles.Add(new Tile(t));
                }
            }
            string[] chs = cols[2].Split(dictDelim);
            foreach(string ch in chs)
            {
                if (String.IsNullOrWhiteSpace(ch)) continue;
                string[] chkv = ch.Split(keyDelim);
                if (chkv.Length != 2) continue;
                uint key = 0;
                if(uint.TryParse(chkv[0], out key))
                {
                    ChangedCharacters.Add(key, new DisplayCharacter(chkv[1]));
                }
            }
            chs = cols[3].Split(dictDelim);
            foreach (string ch in chs)
            {
                if (String.IsNullOrWhiteSpace(ch)) continue;
                string[] chkv = ch.Split(keyDelim);
                if (chkv.Length != 2) continue;
                uint key = 0;
                if (uint.TryParse(chkv[0], out key))
                {
                    AddedCharacters.Add(key, new DisplayCharacter(chkv[1]));
                }
            }
            chs = cols[4].Split(dictDelim);
            foreach (string ch in chs)
            {
                if (String.IsNullOrWhiteSpace(ch)) continue;
                string[] chkv = ch.Split(keyDelim);
                if (chkv.Length != 2) continue;
                uint key = 0;
                if (uint.TryParse(chkv[0], out key))
                {
                    RemovedCharacters.Add(key, new DisplayCharacter(chkv[1]));
                }
            }
        }


        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            
            foreach (Tile t in AddedTiles)
            {
                ret.Append(t.ToString());
                ret.Append(dictDelimiter);
            }
            ret.Append(delimiter);

            foreach (Tile t in RemovedTiles)
            {
                ret.Append(t.ToString());
                ret.Append(dictDelimiter);
            }
            ret.Append(delimiter);

            foreach (KeyValuePair<uint, DisplayCharacter> c in ChangedCharacters)
            {
                ret.Append(c.Key);
                ret.Append(keyDelimiter);
                ret.Append(c.Value);
                ret.Append(dictDelimiter);
            }
            ret.Append(delimiter);

            foreach (KeyValuePair<uint, DisplayCharacter> c in AddedCharacters)
            {
                ret.Append(c.Key);
                ret.Append(keyDelimiter);
                ret.Append(c.Value);
                ret.Append(dictDelimiter);
            }
            ret.Append(delimiter);

            foreach (KeyValuePair<uint, DisplayCharacter> c in RemovedCharacters)
            {
                ret.Append(c.Key);
                ret.Append(keyDelimiter);
                ret.Append(c.Value);
                ret.Append(dictDelimiter);
            }
            return ret.ToString();
        }
    }
}
