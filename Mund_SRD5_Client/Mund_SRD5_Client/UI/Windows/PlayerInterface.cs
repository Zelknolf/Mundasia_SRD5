using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mundasia.Objects;
using Mundasia.Communication;
using System.ComponentModel;
using System.Threading;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    public class PlayerInterface: Panel
    {
        private static int padding = 5;

        public static Creature DrivingCharacter;

        private static Form host;
        private static PlayScene playScene;

        private static BackgroundWorker Updater;
        private volatile static MapDelta UpdateResult;
        
        private static bool _eventsInitialized = false;

        private static Tile _currentTile;

        public static InventoryForm InventoryWindow = null;

        public PlayerInterface() { }

        public static void Set(Form primaryForm, CharacterSelection initialScene)
        {
            host = primaryForm;
            DrivingCharacter = initialScene.CentralCharacter;
            playScene = new PlayScene();
            playScene.Location = new Point(padding, padding);
            playScene.Size = new Size(host.ClientRectangle.Width - padding * 2, host.ClientRectangle.Height - padding * 2);
            host.Resize += host_Resize;
            if(!_eventsInitialized)
            {
                _eventsInitialized = true;
                if (DrivingCharacter.IsGM)
                {
                    host.KeyDown += playScene_DM_KeyDown;
                    playScene.TileSelected += playScene_DM_TileSelected;
                }
                else
                {
                    host.KeyDown += playScene_KeyDown;
                    playScene.TileSelected += playScene_TileSelected;
                }
            }
            playScene.ViewCenterX = initialScene.CentralCharacter.LocationX;
            playScene.ViewCenterY = initialScene.CentralCharacter.LocationY;
            playScene.ViewCenterZ = initialScene.CentralCharacter.LocationZ;

            host.Controls.Add(playScene);
            playScene.Add(initialScene.visibleTiles);
            playScene.Add(initialScene.visibleCharacters);

            Updater = new BackgroundWorker();
            Updater.DoWork += Updater_DoWork;
            Updater.RunWorkerCompleted += Updater_RunWorkerCompleted;
            Updater.RunWorkerAsync();
        }

        private static MapDelta _handleDelta(string deltaString)
        {
            MapDelta changes = new MapDelta(deltaString);
            playScene.ClearControls();
            playScene.Remove(changes.RemovedTiles);
            playScene.Remove(changes.RemovedCharacters);
            playScene.Add(changes.AddedTiles);
            playScene.Add(changes.AddedCharacters);
            playScene.ManageChanges(changes.ChangedCharacters);
            return changes;
        }

        static void playScene_DM_KeyDown(object sender, KeyEventArgs e)
        {
            #region Add a Tile
            if (e.KeyCode == Keys.NumPad0)
            {
                // Num pad 0 used to add a tile
                if (_currentTile == null) return;
                if (playScene.CollidesWithTile(_currentTile.PosX, _currentTile.PosY, _currentTile.PosZ + _currentTile.TileHeight, _currentTile.TileHeight, true, _currentTile)) return;

                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ + _currentTile.TileHeight);

                request.AddedTiles.Add(nTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile BL
            else if(e.KeyCode == Keys.NumPad1)
            {
                // Num pad 1 moves tile to bottom left
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch(playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targY--;
                        break;
                    case Direction.NorthEast:
                        targX--;
                        break;
                    case Direction.SouthWest:
                        targX++;
                        break;
                    case Direction.SouthEast:
                        targY++;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile B
            else if(e.KeyCode == Keys.NumPad2)
            {
                // Num pad 2 moves tile to bottom
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targY--;
                        targX++;
                        break;
                    case Direction.NorthEast:
                        targY--;
                        targX--;
                        break;
                    case Direction.SouthWest:
                        targY++;
                        targX++;
                        break;
                    case Direction.SouthEast:
                        targY++;
                        targX--;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile BR
            else if(e.KeyCode == Keys.NumPad3)
            {
                // Num pad 3 moves tile to bottom right
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targX++;
                        break;
                    case Direction.NorthEast:
                        targY--;
                        break;
                    case Direction.SouthWest:
                        targY++;
                        break;
                    case Direction.SouthEast:
                        targX--;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile L
            else if(e.KeyCode == Keys.NumPad4)
            {
                // Num pad 4 moves tile to left
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targX--;
                        targY--;
                        break;
                    case Direction.NorthEast:
                        targX--;
                        targY++;
                        break;
                    case Direction.SouthWest:
                        targX++;
                        targY--;
                        break;
                    case Direction.SouthEast:
                        targX--;
                        targY++;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Rotate a Tile
            else if(e.KeyCode == Keys.NumPad5)
            {
                // Num pad 5 rotates the tile
                if (_currentTile == null) return;

                Direction slopeDirection = _currentTile.Slope;

                switch(_currentTile.Slope)
                {
                    case Direction.DirectionLess:
                        slopeDirection = Direction.North;
                        break;
                    case Direction.North:
                        slopeDirection = Direction.NorthEast;
                        break;
                    case Direction.NorthEast:
                        slopeDirection = Direction.East;
                        break;
                    case Direction.East:
                        slopeDirection = Direction.SouthEast;
                        break;
                    case Direction.SouthEast:
                        slopeDirection = Direction.South;
                        break;
                    case Direction.South:
                        slopeDirection = Direction.SouthWest;
                        break;
                    case Direction.SouthWest:
                        slopeDirection = Direction.West;
                        break;
                    case Direction.West:
                        slopeDirection = Direction.NorthWest;
                        break;
                    case Direction.NorthWest:
                        slopeDirection = Direction.DirectionLess;
                        break;
                }

                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, slopeDirection, _currentTile.TileHeight, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile R
            else if(e.KeyCode == Keys.NumPad6)
            {
                // Num pad 6 moves tile to right
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targX++;
                        targY++;
                        break;
                    case Direction.NorthEast:
                        targX++;
                        targY--;
                        break;
                    case Direction.SouthWest:
                        targX--;
                        targY++;
                        break;
                    case Direction.SouthEast:
                        targX--;
                        targY--;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if(delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile TL
            else if(e.KeyCode == Keys.NumPad7)
            {
                // Num pad 7 moves tile to top left
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targX--;
                        break;
                    case Direction.NorthEast:
                        targY++;
                        break;
                    case Direction.SouthWest:
                        targY--;
                        break;
                    case Direction.SouthEast:
                        targX++;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile T
            else if(e.KeyCode == Keys.NumPad8)
            {
                // Num pad 8 moves tile to top
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targX--;
                        targY++;
                        break;
                    case Direction.NorthEast:
                        targX++;
                        targY++;
                        break;
                    case Direction.SouthWest:
                        targX--;
                        targY--;
                        break;
                    case Direction.SouthEast:
                        targX++;
                        targY--;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Move a Tile TR
            else if(e.KeyCode == Keys.NumPad9)
            {
                // Num pad 9 moves tile to top right
                if (_currentTile == null) return;

                int targX = _currentTile.PosX;
                int targY = _currentTile.PosY;
                int targZ = _currentTile.PosZ;

                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest:
                        targY++;
                        break;
                    case Direction.NorthEast:
                        targX++;
                        break;
                    case Direction.SouthWest:
                        targX--;
                        break;
                    case Direction.SouthEast:
                        targY--;
                        break;
                }
                if (playScene.CollidesWithTile(targX, targY, targZ, _currentTile.TileHeight, true, _currentTile)) return;
                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, targX, targY, targZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Change a Tileset
            else if(e.KeyCode == Keys.Enter)
            {
                // Enter key changes the tileset
                if (_currentTile == null) return;

                uint tileSet = _currentTile.CurrentTileSet;

                if (tileSet >= TileSet._library.Count - 1) tileSet = 0;
                else tileSet++;

                TileChange request = new TileChange();
                Tile nTile = new Tile(tileSet, _currentTile.Slope, _currentTile.TileHeight, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Remove a Tile
            else if(e.KeyCode == Keys.Decimal)
            {
                // Num pad . deletes the tile
                if (_currentTile == null) return;

                TileChange request = new TileChange();

                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Shorten a Tile
            else if(e.KeyCode == Keys.Divide)
            {
                // Divide key shortens the tile
                if (_currentTile == null) return;

                if (_currentTile.TileHeight <= 1) return;

                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight - 1, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ - 1);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Heighten a Tile
            else if(e.KeyCode == Keys.Multiply)
            {
                // Multiply key heightens the tile
                if (_currentTile == null) return;
                if (playScene.CollidesWithTile(_currentTile.PosX, _currentTile.PosY, _currentTile.PosZ + 1, _currentTile.TileHeight + 1, true, _currentTile)) return;
                if (_currentTile.TileHeight >= 4) return;

                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight + 1, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ + 1);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Lower a Tile
            else if(e.KeyCode == Keys.Subtract)
            {
                // Subtract key decreases the tile's elevation
                if (_currentTile == null) return;
                if (playScene.CollidesWithTile(_currentTile.PosX, _currentTile.PosY, _currentTile.PosZ - 1, _currentTile.TileHeight, true, _currentTile)) return;

                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ - 1);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
            #region Raise a Tile
            else if(e.KeyCode == Keys.Add)
            {
                // Add key increases the tile's elevation
                if (_currentTile == null) return;
                if (playScene.CollidesWithTile(_currentTile.PosX, _currentTile.PosY, _currentTile.PosZ + 1, _currentTile.TileHeight, true, _currentTile)) return;

                TileChange request = new TileChange();
                Tile nTile = new Tile(_currentTile.CurrentTileSet, _currentTile.Slope, _currentTile.TileHeight, _currentTile.PosX, _currentTile.PosY, _currentTile.PosZ + 1);

                request.AddedTiles.Add(nTile);
                request.RemovedTiles.Add(_currentTile);
                request.CharacterName = DrivingCharacter.CharacterName;
                request.AccountName = DrivingCharacter.AccountName;
                string resp = ServiceConsumer.ChangeTiles(request);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta delta = _handleDelta(resp);
                    if (delta.RemovedTiles.Contains(_currentTile))
                    {
                        _currentTile = null;
                    }
                    if (delta.AddedTiles.Count > 0)
                    {
                        _currentTile = delta.AddedTiles[0];
                    }
                }
                return;
            }
            #endregion
        }

        static void playScene_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.I)
            {
                if(InventoryWindow != null)
                {
                    InventoryWindow.Focus();
                }
                else
                {
                    InventoryWindow = new InventoryForm(DrivingCharacter);
                    InventoryWindow.Show();
                }
            }
        }

        static void playScene_DM_TileSelected(object Sender, TileSelectEventArgs e)
        {
            _currentTile = e.tile;
        }

        static void playScene_TileSelected(object Sender, TileSelectEventArgs e)
        {
            playScene.ClearControls();
            PlaySceneControl ctl = new PlaySceneControl(playScene.ViewCenterX, playScene.ViewCenterY, playScene.ViewCenterZ, e.tile.PosX, e.tile.PosY, e.tile.PosZ, playScene, playScene.TopDirection, Mundasia.Interface.PlaySceneControlType.Move, 1);
            ctl.ControlSelected += playScene_ControlSelected;
            playScene.Add(ctl);
        }

        static void playScene_ControlSelected(object Sender, EventArgs e)
        {
            PlaySceneControl ctl = Sender as PlaySceneControl;
            if(ctl.ControlType == PlaySceneControlType.Move)
            {
                playScene.ClearControls();
                PlaySceneControl bl = new PlaySceneControl(playScene.ViewCenterX, playScene.ViewCenterY, playScene.ViewCenterZ, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), playScene, playScene.TopDirection, PlaySceneControlType.FaceBottomLeft, 5);
                PlaySceneControl br = new PlaySceneControl(playScene.ViewCenterX, playScene.ViewCenterY, playScene.ViewCenterZ, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), playScene, playScene.TopDirection, PlaySceneControlType.FaceBottomRight, 6);
                PlaySceneControl tl = new PlaySceneControl(playScene.ViewCenterX, playScene.ViewCenterY, playScene.ViewCenterZ, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), playScene, playScene.TopDirection, PlaySceneControlType.FaceTopLeft, 7);
                PlaySceneControl tr = new PlaySceneControl(playScene.ViewCenterX, playScene.ViewCenterY, playScene.ViewCenterZ, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), playScene, playScene.TopDirection, PlaySceneControlType.FaceTopRight, 8);
                bl.ControlSelected += playScene_ControlSelected;
                br.ControlSelected += playScene_ControlSelected;
                tl.ControlSelected += playScene_ControlSelected;
                tr.ControlSelected += playScene_ControlSelected;
                List<PlaySceneControl> ctls = new List<PlaySceneControl>() { bl, br, tl, tr };
                playScene.Add(ctls);
            }
            else if(ctl.ControlType == PlaySceneControlType.FaceBottomLeft)
            {
                Direction facing = Direction.DirectionLess;
                switch(playScene.TopDirection)
                {
                    case Direction.NorthWest: facing = Direction.South; break;
                    case Direction.SouthWest: facing = Direction.East;  break;
                    case Direction.NorthEast: facing = Direction.West;  break;
                    case Direction.SouthEast: facing = Direction.North; break;
                    default:                  facing = Direction.North; break;
                }
                string resp = ServiceConsumer.MoveCharacter(DrivingCharacter.AccountName, DrivingCharacter.CharacterName, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), facing);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta changes = new MapDelta(resp);
                    playScene.ClearControls();
                    playScene.Remove(changes.RemovedTiles);
                    playScene.Remove(changes.RemovedCharacters);
                    playScene.Add(changes.AddedTiles);
                    playScene.Add(changes.AddedCharacters);
                    playScene.ManageChanges(changes.ChangedCharacters);
                }
            }
            else if (ctl.ControlType == PlaySceneControlType.FaceBottomRight)
            {
                Direction facing = Direction.DirectionLess;
                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest: facing = Direction.East;  break;
                    case Direction.SouthWest: facing = Direction.North; break;
                    case Direction.NorthEast: facing = Direction.South; break;
                    case Direction.SouthEast: facing = Direction.West;  break;
                    default: facing = Direction.North; break;
                }
                string resp = ServiceConsumer.MoveCharacter(DrivingCharacter.AccountName, DrivingCharacter.CharacterName, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), facing);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta changes = new MapDelta(resp);
                    playScene.ClearControls();
                    playScene.Remove(changes.RemovedTiles);
                    playScene.Remove(changes.RemovedCharacters);
                    playScene.Add(changes.AddedTiles);
                    playScene.Add(changes.AddedCharacters);
                    playScene.ManageChanges(changes.ChangedCharacters);
                }
            }
            else if (ctl.ControlType == PlaySceneControlType.FaceTopLeft)
            {
                Direction facing = Direction.DirectionLess;
                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest: facing = Direction.West;  break;
                    case Direction.SouthWest: facing = Direction.South; break;
                    case Direction.NorthEast: facing = Direction.North; break;
                    case Direction.SouthEast: facing = Direction.East;  break;
                    default: facing = Direction.North; break;
                }
                string resp = ServiceConsumer.MoveCharacter(DrivingCharacter.AccountName, DrivingCharacter.CharacterName, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), facing);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta changes = new MapDelta(resp);
                    playScene.ClearControls();
                    playScene.Remove(changes.RemovedTiles);
                    playScene.Remove(changes.RemovedCharacters);
                    playScene.Add(changes.AddedTiles);
                    playScene.Add(changes.AddedCharacters);
                    playScene.ManageChanges(changes.ChangedCharacters);
                }
            }
            else if (ctl.ControlType == PlaySceneControlType.FaceTopRight)
            {
                Direction facing = Direction.DirectionLess;
                switch (playScene.TopDirection)
                {
                    case Direction.NorthWest: facing = Direction.North; break;
                    case Direction.SouthWest: facing = Direction.West; break;
                    case Direction.NorthEast: facing = Direction.East; break;
                    case Direction.SouthEast: facing = Direction.South; break;
                    default: facing = Direction.North; break;
                }
                string resp = ServiceConsumer.MoveCharacter(DrivingCharacter.AccountName, DrivingCharacter.CharacterName, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ(), facing);
                if (!String.IsNullOrEmpty(resp))
                {
                    MapDelta changes = new MapDelta(resp);
                    playScene.ClearControls();
                    playScene.Remove(changes.RemovedTiles);
                    playScene.Remove(changes.RemovedCharacters);
                    playScene.Add(changes.AddedTiles);
                    playScene.Add(changes.AddedCharacters);
                    playScene.ManageChanges(changes.ChangedCharacters);
                }
            }
        }

        public static void Clear(Form primaryForm)
        {
            host.Controls.Clear();
        }

        static void host_Resize(object sender, EventArgs e)
        {
            playScene.Size = new Size(host.ClientRectangle.Width - padding * 2, host.ClientRectangle.Height - padding * 2);
        }

        #region Background Thread and Passive Updates
        static void Updater_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(100);
            string result = ServiceConsumer.UpdatePlayScene(DrivingCharacter.AccountName, DrivingCharacter.CharacterName);
            if (String.IsNullOrWhiteSpace(result))
            {
                UpdateResult = null;
            }
            else
            {
                UpdateResult = new MapDelta(result);
            }
        }

        static void Updater_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (UpdateResult != null)
            {
                if(UpdateResult.RemovedTiles.Count > 0) playScene.Remove(UpdateResult.RemovedTiles);
                if(UpdateResult.RemovedCharacters.Count > 0) playScene.Remove(UpdateResult.RemovedCharacters);
                if(UpdateResult.AddedCharacters.Count > 0) playScene.Add(UpdateResult.AddedCharacters);
                if(UpdateResult.AddedTiles.Count > 0) playScene.Add(UpdateResult.AddedTiles);
                if(UpdateResult.ChangedCharacters.Count > 0) playScene.ManageChanges(UpdateResult.ChangedCharacters);
            }
            Updater.RunWorkerAsync();
        }
        #endregion
    }
}
