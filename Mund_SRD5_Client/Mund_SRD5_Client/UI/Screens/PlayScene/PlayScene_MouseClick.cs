using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public partial class PlayScene : Panel
    {
        private IPlaySceneDrawable lastSelected = null;

        /// <summary>
        /// Manages selection of tiles and the use of standard buttons.
        /// </summary>
        private void PlayScene_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // TODO: Don't hard code these values.
                if (e.Location.X > this.Width - 63 &&
                    e.Location.Y < 50)
                {
                    switch (topDirection)
                    {
                        case Direction.NorthWest:
                            topDirection = Direction.NorthEast;
                            break;
                        case Direction.NorthEast:
                            topDirection = Direction.SouthEast;
                            break;
                        case Direction.SouthEast:
                            topDirection = Direction.SouthWest;
                            break;
                        case Direction.SouthWest:
                            topDirection = Direction.NorthWest;
                            break;
                    }
                    drawableImages.Clear();
                    foreach (Tile tile in drawableTiles)
                    {
                        TileImage img = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                        img.TileSelected += image_TileSelected;
                        drawableImages.Add(img);
                    }
                    foreach (DisplayCharacter ch in drawableCharacters)
                    {
                        drawableImages.Add(ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this));
                    }
                    drawableImages.Sort();
                    if (lastSelected != null)
                    {
                        lastSelected = lastSelected.GetNewDrawable();
                        lastSelected.SetSelected(true);
                    }
                    this.Refresh();
                }
                // TODO: Don't hard code these values.
                else if (e.Location.X < 50 &&
                        e.Location.Y < 50)
                {
                    switch (topDirection)
                    {
                        case Direction.NorthWest:
                            topDirection = Direction.SouthWest;
                            break;
                        case Direction.NorthEast:
                            topDirection = Direction.NorthWest;
                            break;
                        case Direction.SouthEast:
                            topDirection = Direction.NorthEast;
                            break;
                        case Direction.SouthWest:
                            topDirection = Direction.SouthEast;
                            break;
                    }
                    drawableImages.Clear();
                    foreach (Tile tile in drawableTiles)
                    {
                        TileImage img = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                        img.TileSelected += image_TileSelected;
                        drawableImages.Add(img);
                    }
                    foreach(DisplayCharacter ch in drawableCharacters)
                    {
                        drawableImages.Add(ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this));
                    }
                    drawableImages.Sort();
                    if (lastSelected != null)
                    {
                        lastSelected = lastSelected.GetNewDrawable();
                        lastSelected.SetSelected(true);
                    }
                    this.Refresh();
                }
                else if (currentMouseOver != null)
                {
                    currentMouseOver.SetSelected(!currentMouseOver.GetSelected());
                    if (lastSelected != null && lastSelected != currentMouseOver) lastSelected.SetSelected(false);
                    if(lastSelected != null) this.Invalidate(new Rectangle(lastSelected.GetImageLocation(), lastSelected.GetTemplateImage().Size));
                    this.Invalidate(new Rectangle(currentMouseOver.GetImageLocation(), currentMouseOver.GetTemplateImage().Size));
                    lastSelected = currentMouseOver;

                }
            }

            // TODO: Remove the temporary measure which allows adjustment of lighting.
            else
            {
                timeOfDay++;
                if (timeOfDay > 2) timeOfDay = 0;
                this.Refresh();
            }
        }

        private void PlayScene_DoubleClick(object Sender, MouseEventArgs e)
        {
            if(currentMouseOver != null)
            {
                ViewCenterX = currentMouseOver.GetObjectPositionX();
                ViewCenterY = currentMouseOver.GetObjectPositionY();
                ViewCenterZ = currentMouseOver.GetObjectPositionZ();
                drawableImages.Clear();
                foreach (Tile tile in drawableTiles)
                {
                    TileImage img = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                    img.TileSelected += image_TileSelected;
                    drawableImages.Add(img);
                }
                foreach (DisplayCharacter ch in drawableCharacters)
                {
                    drawableImages.Add(ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this));
                }
                drawableImages.Sort();
                if (lastSelected != null)
                {
                    lastSelected = lastSelected.GetNewDrawable();
                    lastSelected.SetSelected(true);
                }
                this.Refresh();
            }
        }
    }
}
