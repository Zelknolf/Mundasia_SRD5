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
        /// <summary>
        /// Has simple handling to relocate and re-render the tiles in the scene with positions appropriate
        /// to the new scene size.
        /// </summary>
        void PlayScene_Resize(object sender, EventArgs e)
        {
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
