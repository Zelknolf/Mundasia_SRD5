using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public class TileSelectEventArgs : EventArgs
    {
        public Tile tile;
    }
    
    public delegate void OnTileSelectedHandler(object Sender, TileSelectEventArgs e);

    public partial class PlayScene : Panel
    {
        public event OnTileSelectedHandler TileSelected;

        public PlayScene()
        {
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(PlayScene_Paint);
            this.MouseClick += new MouseEventHandler(PlayScene_MouseClick);
            this.MouseDoubleClick += new MouseEventHandler(PlayScene_DoubleClick);
            this.MouseMove += new MouseEventHandler(PlayScene_MouseMove);
            this.Resize += new EventHandler(PlayScene_Resize);
        }

        /// <summary>
        /// Adds tiles to those which are to be drawn in the scene.
        /// </summary>
        /// <param name="tiles">A list of tiles to add</param>
        public void Add(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                drawableTiles.Add(tile);
                TileImage image = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                image.SourceTile = tile;
                image.TileSelected += image_TileSelected;
                drawableImages.Add(image);
            }
            drawableImages.Sort();
            this.Refresh();
        }

        public void Add(List<DisplayCharacter> chs)
        {
            foreach(DisplayCharacter ch in chs)
            {
                drawableCharacters.Add(ch);
                CreatureImage image = ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                image.SourceCharacter = ch;
                drawableImages.Add(image);
            }
            drawableImages.Sort();
            this.Refresh();
        }

        public void Add(Dictionary<uint, DisplayCharacter> chs)
        {
            foreach (DisplayCharacter ch in chs.Values)
            {
                drawableCharacters.Add(ch);
                CreatureImage image = ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                image.SourceCharacter = ch;
                drawableImages.Add(image);
            }
            drawableImages.Sort();
            this.Refresh();
        }  

        public void Add(List<PlaySceneControl> ctls)
        {
            foreach(PlaySceneControl ctl in ctls)
            {
                drawableImages.Add(ctl);
            }
            drawableImages.Sort();
            this.Refresh();
        }

        /// <summary>
        /// Adds tiles to those which are to be drawn in the scene.
        /// 
        /// If many tiles are to be added, they should be added as a list to reduce redrawing.
        /// </summary>
        /// <param name="tile">A tile to add</param>
        public void Add(Tile tile)
        {
            drawableTiles.Add(tile);
            TileImage image = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
            image.TileSelected += image_TileSelected;
            image.SourceTile = tile;
            drawableImages.Add(image);
            drawableImages.Sort();
            this.Refresh();
        }

        public void Add(DisplayCharacter ch)
        {
            drawableCharacters.Add(ch);
            CreatureImage image = ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
            drawableImages.Add(image);
            drawableImages.Sort();
            this.Refresh();
        }

        public void Add(PlaySceneControl ctl)
        {
            drawableImages.Add(ctl);
            drawableImages.Sort();
            this.Refresh();
        }

        public void ClearControls()
        {
            List<IPlaySceneDrawable> removedControls = new List<IPlaySceneDrawable>();
            foreach(IPlaySceneDrawable drw in drawableImages)
            {
                if(drw.GetType() == typeof(PlaySceneControl))
                {
                    removedControls.Add(drw);
                }
            }
            foreach(IPlaySceneDrawable rem in removedControls)
            {
                drawableImages.Remove(rem);
            }
        }

        /// <summary>
        /// Removes tiles from those which are to be drawn in the scene.
        /// </summary>
        /// <param name="tiles">A list of tiles to remove.</param>
        public void Remove(List<Tile> tiles)
        {
            foreach(Tile toRemove in tiles)
            {
                List<Tile> removing = new List<Tile>();
                foreach(Tile currentTile in drawableTiles)
                {
                    if(currentTile.PosX == toRemove.PosX &&
                       currentTile.PosY == toRemove.PosY &&
                       currentTile.PosZ == toRemove.PosZ)
                    {
                        removing.Add(currentTile);
                    }
                }
                foreach(Tile t in removing)
                {
                    drawableTiles.Remove(t);
                    if (drawableImages.Contains(t.CachedImage))
                    {
                        drawableImages.Remove(t.CachedImage);
                    }
                }
            }
            this.Refresh();
        }

        public void Remove(Dictionary<uint, DisplayCharacter> chs)
        {
            foreach (DisplayCharacter ch in chs.Values)
            {
                if (drawableCharacters.Contains(ch))
                {
                    drawableCharacters.Remove(ch);
                }
                if (drawableImages.Contains(ch.CachedImage))
                {
                    drawableImages.Remove(ch.CachedImage);
                }
            }
            this.Refresh();
        }

        public void Remove(List<DisplayCharacter> chs)
        {
            foreach (DisplayCharacter ch in chs)
            {
                if (drawableCharacters.Contains(ch))
                {
                    drawableCharacters.Remove(ch);
                }
                if(drawableImages.Contains(ch.CachedImage))
                {
                    drawableImages.Remove(ch.CachedImage);
                }
            }
            this.Refresh();
        }

        /// <summary>
        /// Removes tiles from those which are to be drawn in the scene.
        /// 
        /// If many tiles are to be removed, they should be removed as a list to reduce redrawing.
        /// </summary>
        /// <param name="tile">A tile to remove</param>
        public void Remove(Tile toRemove)
        {
            List<Tile> removing = new List<Tile>();
            foreach (Tile currentTile in drawableTiles)
            {
                if (currentTile.PosX == toRemove.PosX &&
                   currentTile.PosY == toRemove.PosY &&
                   currentTile.PosZ == toRemove.PosZ)
                {
                    removing.Add(currentTile);
                }
            }
            foreach (Tile t in removing)
            {
                drawableTiles.Remove(t);
                if (drawableImages.Contains(t.CachedImage))
                {
                    drawableImages.Remove(t.CachedImage);
                }
            }
            this.Refresh();
        }

        public void Remove(DisplayCharacter ch)
        {
            if (drawableCharacters.Contains(ch))
            {
                drawableCharacters.Remove(ch);
            }
            if (drawableImages.Contains(ch.CachedImage))
            {
                drawableImages.Remove(ch.CachedImage);
            }
            this.Refresh();
        }

        void image_TileSelected(object Sender, EventArgs e)
        {
            Type sndType = Sender.GetType();
            TileImage image = Sender as TileImage;
            if(image != null)
            {
                TileSelected(this, new TileSelectEventArgs() { tile = image.SourceTile });
            }
        }

        public void ManageChanges(Dictionary<uint, DisplayCharacter> chs)
        {
            foreach(KeyValuePair<uint, DisplayCharacter> ch in chs)
            {
                DisplayCharacter current = null;
                foreach(DisplayCharacter old in drawableCharacters)
                {
                    if(old.CharacterId == ch.Key)
                    {
                        current = old;
                        break;
                    }
                }
                if (current != null)
                {
                    drawableCharacters.Remove(current);
                    if(drawableImages.Contains(current.CachedImage))
                    {
                        drawableImages.Remove(current.CachedImage);
                    }
                }
            }
            foreach (DisplayCharacter ch in chs.Values)
            {
                drawableCharacters.Add(ch);
                CreatureImage image = ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                image.SourceCharacter = ch;
                drawableImages.Add(image);
            }
            drawableImages.Sort();
            this.Refresh();
        }

        public bool CollidesWithTile(int x, int y, int z, int height, bool isTile, Tile exceptionTile)
        {
            if (isTile)
            {
                foreach (Tile t in drawableTiles)
                {
                    if (t != exceptionTile &&
                        t.PosX == x &&
                        t.PosY == y &&
                        t.PosZ > z - height &&
                        t.PosZ < z + t.TileHeight)
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach(Tile t in drawableTiles)
                {
                    if (t != exceptionTile &&
                        t.PosX == x &&
                       t.PosY == y &&
                       t.PosZ <= z &&
                       t.PosZ > z + height + t.TileHeight)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// TopDirection stores the PlayScene's current idea of which in-game direction translates to
        /// the top of the panel. It is of no interest to the server.
        /// </summary>
        private Direction topDirection = Direction.NorthWest;

        public Direction TopDirection
        {
            get { return topDirection; }
        }

        /// <summary>
        /// A sortable collection of images which contains all that should be visible in the current scene.
        /// </summary>
        private List<IPlaySceneDrawable> drawableImages = new List<IPlaySceneDrawable>();

        /// <summary>
        /// A collection of tiles which should be visible in the current scene.
        /// </summary>
        private List<Tile> drawableTiles = new List<Tile>();

        private List<DisplayCharacter> drawableCharacters = new List<DisplayCharacter>();

        /// <summary>
        /// stores which object is currently displaying as moused over to the user,
        /// and thus what the user would expect to receive a click.
        /// </summary>
        private IPlaySceneDrawable currentMouseOver = null;

        /// <summary>
        /// TODO: Turn this into a real system for time of day.
        /// </summary>
        int timeOfDay = 0;

        /// <summary>
        /// Contains the X coordinate of the center of the view.
        /// </summary>
        public int ViewCenterX;

        /// <summary>
        /// Contains the Y coordinate of the center of the view.
        /// </summary>
        public int ViewCenterY;

        /// <summary>
        /// Contains the Z coordinate of the center of the view.
        /// </summary>
        public int ViewCenterZ;
    }
}
