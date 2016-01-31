using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mundasia.Objects;

namespace Mundasia.Communication
{
    public class TileChange
    {
        private static string delimiter = "─";
        private static char[] delim = new char[] { '─' };
        private static string dictDelimiter = "│";
        private static char[] dictDelim = new char[] { '│' };

        
        public string AccountName;
        public int SessionId;
        public string CharacterName;
        public List<Tile> RemovedTiles = new List<Tile>();
        public List<Tile> AddedTiles = new List<Tile>();

        public TileChange() { }

        public TileChange(string fileLine)
        {
            string[] line = fileLine.Split(delim);
            if (line.Length < 5) return;
            CharacterName = line[0];
            AccountName = line[1];
            Int32.TryParse(line[2], out SessionId);
            string[] rem = line[3].Split(dictDelim);
            foreach (string t in rem)
            {
                if (!String.IsNullOrWhiteSpace(t))
                {
                    RemovedTiles.Add(new Tile(t));
                }
            }
            string[] add = line[4].Split(dictDelim);
            foreach (string t in add)
            {
                if (!String.IsNullOrWhiteSpace(t))
                {
                    AddedTiles.Add(new Tile(t));
                }
            }
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(CharacterName);
            ret.Append(delimiter);
            ret.Append(AccountName);
            ret.Append(delimiter);
            ret.Append(SessionId);
            ret.Append(delimiter);
            foreach(Tile t in RemovedTiles)
            {
                ret.Append(t.ToString());
                ret.Append(dictDelimiter);
            }
            ret.Append(delimiter);
            foreach(Tile t in AddedTiles)
            {
                ret.Append(t.ToString());
                ret.Append(dictDelimiter);
            }
            return ret.ToString();
        }
    }
}
