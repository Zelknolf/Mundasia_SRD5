using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    public class Map
    {
        /// <summary>
        /// A collection of all maps which have been loaded.
        /// </summary>
        public static Dictionary<string, Map> LoadedMaps = new Dictionary<string, Map>();
        
        /// <summary>
        /// A collection of all of the tiles in the area. Tiles appear as Tiles[X][Y][Z].
        /// </summary>
        public Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> Tiles = new Dictionary<int, Dictionary<int, Dictionary<int, Tile>>>();

        /// <summary>
        /// Storage of whether or not a given X-Y-Z/1000 coordinate has had its stack loaded.
        /// (all Z coordinates from 0-999 are "0" in this dictionary, per Tile.LoadStack's behavior.)
        /// </summary>
        public Dictionary<int, Dictionary<int, Dictionary<int, bool>>> TilesLoaded = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

        /// <summary>
        /// This is a list of all characters who are currently logged in and present on this map.
        /// </summary>
        public List<Creature> PresentCharacters = new List<Creature>();

        /// <summary>
        /// This is a going list of all of the changes made to this map.
        /// </summary>
        public Dictionary<Creature, MapDelta> MapDeltas = new Dictionary<Creature, MapDelta>();

        /// <summary>
        /// The name of the map being built.
        /// </summary>
        public string Name;

        /// <summary>
        /// Add a tile to this map.
        /// </summary>
        /// <param name="ToAdd">The tile to add</param>
        /// <returns>true if the tile is added; false on error</returns>
        public bool Add(Tile ToAdd)
        {
            if(ToAdd.PosX > 40000000 || // 40,000 km. Roughly the circumference of the Earth.
               ToAdd.PosX < 0 ||        // -1 = 40000000
               ToAdd.PosY > 40000000 ||
               ToAdd.PosY < 0 ||
               ToAdd.PosZ < -50000 ||   // thickness of the crust
               ToAdd.PosZ > 8000)       // atmosphere too thin to support life
            {
                return false;
            }
            int tileHeight = ToAdd.TileHeight;
            while (tileHeight > 1)
            {
                tileHeight--;
                if (GetTileExact(ToAdd.PosX, ToAdd.PosY, ToAdd.PosZ - tileHeight) != null)
                {
                    return false;
                }
            }
            if(GetTileOverlap(ToAdd.PosX, ToAdd.PosY, ToAdd.PosZ) != null)
            {
                return false;
            }
            if(!Tiles.ContainsKey(ToAdd.PosX))
            {
                Tiles.Add(ToAdd.PosX, new Dictionary<int, Dictionary<int, Tile>>());
            }
            if(!Tiles[ToAdd.PosX].ContainsKey(ToAdd.PosY))
            {
                Tiles[ToAdd.PosX].Add(ToAdd.PosY, new Dictionary<int, Tile>());
            }
            if(!Tiles[ToAdd.PosX][ToAdd.PosY].ContainsKey(ToAdd.PosZ))
            {
                Tiles[ToAdd.PosX][ToAdd.PosY].Add(ToAdd.PosZ, ToAdd);
            }
            else
            {
                Tiles[ToAdd.PosX][ToAdd.PosY][ToAdd.PosZ] = null;
            }

            foreach (Creature observer in PresentCharacters)
            {
                if (!MapDeltas.ContainsKey(observer))
                {
                    MapDeltas.Add(observer, new MapDelta());
                }
                MapDeltas[observer].AddedTiles.Add(ToAdd);
            }

            return true;

        }

        /// <summary>
        /// Remove a tile from this map.
        /// </summary>
        /// <param name="ToRemove">The tile to remove</param>
        /// <returns>True on success; false if the tile is not found</returns>
        public bool Remove(Tile ToRemove)
        {
            if(!Tiles.ContainsKey(ToRemove.PosX))
            {
                return false;
            }
            if(!Tiles[ToRemove.PosX].ContainsKey(ToRemove.PosY))
            {
                return false;
            }
            if(!Tiles[ToRemove.PosX][ToRemove.PosY].ContainsKey(ToRemove.PosZ))
            {
                return false;
            }
            Tiles[ToRemove.PosX][ToRemove.PosY].Remove(ToRemove.PosZ);
            if(Tiles[ToRemove.PosX][ToRemove.PosY].Count == 0)
            {
                Tiles[ToRemove.PosX].Remove(ToRemove.PosY);
            }
            if(Tiles[ToRemove.PosX].Count == 0)
            {
                Tiles.Remove(ToRemove.PosX);
            }

            foreach (Creature observer in PresentCharacters)
            {
                if (!MapDeltas.ContainsKey(observer))
                {
                    MapDeltas.Add(observer, new MapDelta());
                }
                MapDeltas[observer].RemovedTiles.Add(ToRemove);
            }

            return true;
        }

        /// <summary>
        /// Finds a tile that is placed exactly at the X/Y/Z coordinates provided.
        /// </summary>
        /// <param name="X">The X coordinate</param>
        /// <param name="Y">The Y coordinate</param>
        /// <param name="Z">The Z coordinate</param>
        /// <returns>The tile, or null if one isn't found.</returns>
        public Tile GetTileExact(int X, int Y, int Z)
        {
            if (!Tiles.ContainsKey(X))
            {
                return null;
            }
            if (!Tiles[X].ContainsKey(Y))
            {
                return null;
            }
            if (Tiles[X][Y].ContainsKey(Z))
            {
                return Tiles[X][Y][Z];
            }
            return null;
        }

        /// <summary>
        /// Returns any tile that overlaps the given coordinates
        /// </summary>
        /// <param name="X">The X coordinate of the tile</param>
        /// <param name="Y">The Y coordinate of the tile</param>
        /// <param name="Z">The Z coordinate of the tile</param>
        /// <returns>The overlapping tile, or null if none.</returns>
        public Tile GetTileOverlap(int X, int Y, int Z)
        {
            Tile exact = GetTileExact(X, Y, Z);
            if (exact != null) return exact;
            if (!Tiles.ContainsKey(X)) return null;
            if (!Tiles[X].ContainsKey(Y)) return null;
            if (!Tiles[X][Y].ContainsKey(Z)) return null;
            if(Tiles[X][Y].ContainsKey(Z+1))
            {
                Tile toCheck = Tiles[X][Y][Z + 1];
                if(toCheck.TileHeight > 1)
                {
                    return toCheck;
                }
            }
            if (Tiles[X][Y].ContainsKey(Z + 2))
            {
                Tile toCheck = Tiles[X][Y][Z + 2];
                if (toCheck.TileHeight > 2)
                {
                    return toCheck;
                }
            }
            if (Tiles[X][Y].ContainsKey(Z + 3))
            {
                Tile toCheck = Tiles[X][Y][Z + 3];
                if (toCheck.TileHeight > 3)
                {
                    return toCheck;
                }
            }
            return null;
        }

        /// <summary>
        /// This method will load all of the tiles within 40 horizontal tiles of the center point, and within the 1000-tall block of tiles.
        /// </summary>
        /// <param name="X">The X coordinate at the center of the loading.</param>
        /// <param name="Y">The Y coordinate at the center of the loading.</param>
        /// <param name="Z">The Z coordinate at the center of the loading.</param>
        /// <returns></returns>
        public void LoadNearby(int X, int Y, int Z)
        {
            for (int c = X - 40; c < X + 40; c++)
            {
                for(int cc = Y - 40; cc < Y + 40; cc++)
                {
                    if (!TilesLoaded.ContainsKey(c) ||
                        !TilesLoaded[c].ContainsKey(cc) ||
                        !TilesLoaded[c][cc].ContainsKey(Z/1000) ||
                        !TilesLoaded[c][cc][Z/1000])
                    {
                        List<Tile> nearbyTiles = Tile.LoadStack(c, cc, Z, Name);
                        foreach (Tile nearby in nearbyTiles)
                        {
                            if (nearby != null)
                            {
                                if (Tiles.ContainsKey(c) &&
                                    Tiles[c].ContainsKey(cc) &&
                                    Tiles[c][cc].ContainsKey(nearby.PosZ))
                                {
                                    continue;
                                }
                                if (!Tiles.ContainsKey(c))
                                {
                                    Tiles.Add(c, new Dictionary<int, Dictionary<int, Tile>>());
                                }
                                if (!Tiles[c].ContainsKey(cc))
                                {
                                    Tiles[c].Add(cc, new Dictionary<int, Tile>());
                                }
                                Tiles[c][cc].Add(nearby.PosZ, nearby);
                            }
                        }
                        if (!TilesLoaded.ContainsKey(c))
                        {
                            TilesLoaded.Add(c, new Dictionary<int, Dictionary<int, bool>>());
                        }
                        if (!TilesLoaded[c].ContainsKey(cc))
                        {
                            TilesLoaded[c].Add(cc, new Dictionary<int, bool>());
                        }
                        if (!TilesLoaded[c][cc].ContainsKey(Z/1000))
                        {
                            TilesLoaded[c][cc].Add(Z/1000, true);
                        }
                        else
                        {
                            TilesLoaded[c][cc][Z/1000] = true;
                        }
                    }
                }
            }
        }

        public List<Tile> GetNearby(int X, int Y, int Z)
        {
            LoadNearby(X, Y, Z);
            List<Tile> ret = new List<Tile>();
            for (int c = X - 40; c < X + 40; c++)
            {
                if (Tiles.ContainsKey(c))
                {
                    for (int cc = Y - 40; cc < Y + 40; cc++)
                    {
                        if (Tiles[c].ContainsKey(cc))
                        {
                            foreach (Tile tile in Tiles[c][cc].Values)
                            {
                                if (Math.Abs(tile.PosZ - Z) < 1000)
                                {
                                    ret.Add(tile);
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public List<DisplayCharacter> GetNearbyCharacters(int X, int Y, int Z)
        {
            List<DisplayCharacter> ret = new List<DisplayCharacter>();
            foreach(Creature ch in PresentCharacters)
            {
                if(ch.LocationX > X - 40 && ch.LocationX < X + 40 &&
                   ch.LocationY > Y - 40 && ch.LocationY < Y + 40 &&
                   ch.LocationZ > Z - 40 && ch.LocationZ < Z + 40)
                {
                    ret.Add(DisplayCharacter.GetDisplayCharacter(ch));
                }
            }
            return ret;
        }

        public bool AddCharacter(Creature ch)
        {
            DisplayCharacter dch = DisplayCharacter.GetDisplayCharacter(ch);
            foreach (Creature observer in PresentCharacters)
            {
                if (!MapDeltas.ContainsKey(observer))
                {
                    MapDeltas.Add(observer, new MapDelta());
                }
                if (MapDeltas[observer].AddedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].AddedCharacters[dch.CharacterId] = dch;
                }
                else
                {
                    MapDeltas[observer].AddedCharacters.Add(dch.CharacterId, dch);
                }
                if (MapDeltas[observer].RemovedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].RemovedCharacters.Remove(dch.CharacterId);
                }
            }
            if(!PresentCharacters.Contains(ch))
            {
                PresentCharacters.Add(ch);
            }
            return true;
        }

        public bool RemoveCharacter(Creature ch)
        {
            DisplayCharacter dch = DisplayCharacter.GetDisplayCharacter(ch);
            foreach (Creature observer in PresentCharacters)
            {
                if (!MapDeltas.ContainsKey(observer))
                {
                    MapDeltas.Add(observer, new MapDelta());
                }
                if (MapDeltas[observer].RemovedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].RemovedCharacters[dch.CharacterId] = dch;
                }
                else
                {
                    MapDeltas[observer].RemovedCharacters.Add(dch.CharacterId, dch);
                }
                if(MapDeltas[observer].AddedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].AddedCharacters.Remove(dch.CharacterId);
                }
                if(MapDeltas[observer].ChangedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].ChangedCharacters.Remove(dch.CharacterId);
                }
            }
            if (PresentCharacters.Contains(ch))
            {
                PresentCharacters.Remove(ch);
            }
            return true;
        }

        public bool MoveCharacter(Creature ch, int X, int Y, int Z, Direction Facing)
        {
            Tile targetTile = GetTileExact(X, Y, Z);
            if(targetTile != null)
            {
                int characterHeight = ch.CharacterRace.Height;
                int testedHeight = characterHeight;
                while(testedHeight > 0)
                {
                    if(GetTileOverlap(X, Y, Z + testedHeight) != null)
                    {
                        return false;
                    }
                    testedHeight--;
                }
                ch.LocationX = X;
                ch.LocationY = Y;
                ch.LocationZ = Z;
                ch.LocationFacing = Facing;

                DisplayCharacter dch = DisplayCharacter.GetDisplayCharacter(ch);
                foreach(Creature observer in PresentCharacters)
                {
                    if(!MapDeltas.ContainsKey(observer))
                    {
                        MapDeltas.Add(observer, new MapDelta());
                    }
                    if (MapDeltas[observer].ChangedCharacters.ContainsKey(dch.CharacterId))
                    {
                        MapDeltas[observer].ChangedCharacters[dch.CharacterId] = dch;
                    }
                    else
                    {
                        MapDeltas[observer].ChangedCharacters.Add(dch.CharacterId, dch);
                    }
                    if (MapDeltas[observer].RemovedCharacters.ContainsKey(dch.CharacterId))
                    {
                        MapDeltas[observer].RemovedCharacters.Remove(dch.CharacterId);
                    }
                }
                return true;
            }
            return false;
        }

        public bool ChangeCharacterAppearance(Creature ch)
        {
            DisplayCharacter dch = DisplayCharacter.GetDisplayCharacter(ch);
            foreach(Creature observer in PresentCharacters)
            {
                if (!MapDeltas.ContainsKey(observer))
                {
                    MapDeltas.Add(observer, new MapDelta());
                }
                if (MapDeltas[observer].ChangedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].ChangedCharacters[dch.CharacterId] = dch;
                }
                else
                {
                    MapDeltas[observer].ChangedCharacters.Add(dch.CharacterId, dch);
                }
                if (MapDeltas[observer].RemovedCharacters.ContainsKey(dch.CharacterId))
                {
                    MapDeltas[observer].RemovedCharacters.Remove(dch.CharacterId);
                }
            }
            return true;
        }
    }
}
