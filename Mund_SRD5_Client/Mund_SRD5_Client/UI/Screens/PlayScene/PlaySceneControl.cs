using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Mundasia.Objects;
using System.Windows.Forms;

namespace Mundasia.Interface
{
    public class PlaySceneControl : IPlaySceneDrawable
    {
        // Controls always have a massive draw index to make certain that they draw on top of anything in the scene.
        public int DrawIndex = 10000;

        public bool Selected;
        public bool MouseOver;

        public Point ImageLocation;
        public Image TemplateImage;
        public Image MouseOverImage;
        public Image SelectedImage;

        public int PositionX;
        public int PositionY;
        public int PositionZ;

        public event OnSelectedHandler ControlSelected;

        public PlaySceneControlType ControlType;

        public PlaySceneControl(int ViewCenterX, int ViewCenterY, int ViewCenterZ, int ControlTargetX, int ControlTargetY, int ControlTargetZ, Panel targetPanel, Direction topDirection, PlaySceneControlType type, int position) 
        {
            PositionX = ControlTargetX;
            PositionY = ControlTargetY;
            PositionZ = ControlTargetZ;
            ControlType = type;

            Bitmap Day;
            switch(type)
            {
                case PlaySceneControlType.Move:
                    Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Controls\\move.png"));
                    break;
                case PlaySceneControlType.FaceBottomLeft:
                    Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Controls\\arrow_bl.png"));
                    break;
                case PlaySceneControlType.FaceBottomRight:
                    Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Controls\\arrow_br.png"));
                    break;
                case PlaySceneControlType.FaceTopLeft:
                    Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Controls\\arrow_tl.png"));
                    break;
                case PlaySceneControlType.FaceTopRight:
                    Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Controls\\arrow_tr.png"));
                    break;
                default:
                    Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Controls\\move.png"));
                    break;
            }
            TemplateImage = new Bitmap(Day);

            Bitmap MouseOverBmp = new Bitmap(Day);
            Bitmap SelectionBmp = Day;
            for (int c = 0; c < MouseOverBmp.Width; c++)
            {
                for (int cc = 0; cc < MouseOverBmp.Height; cc++)
                {
                    Color px = MouseOverBmp.GetPixel(c, cc);
                    if (px.A == 0) continue;
                    MouseOverBmp.SetPixel(c, cc, Color.FromArgb(50, 0, 255, 255));
                    SelectionBmp.SetPixel(c, cc, Color.FromArgb(100, 0, 150, 255));
                }
            }
            MouseOverImage = MouseOverBmp;
            SelectedImage = SelectionBmp;

            // The center of the panel is (roughly) half of the height and width of the panel.
            int centerY = targetPanel.Height / 2 - 10;
            int centerX = targetPanel.Width / 2 - 20;

            // Find offsets for this tile on the in-world coordinates
            int easternOffset = ControlTargetX - ViewCenterX;
            int northernOffset = ControlTargetY - ViewCenterY;
            int verticalOffset = ControlTargetZ - ViewCenterZ;

            int imagePosX = centerX;
            int imagePosY = centerY;
            switch (topDirection)
            {
                case Direction.NorthWest:
                    // North is top right. East is bottom right.
                    imagePosX += northernOffset * 20;
                    imagePosY -= northernOffset * 10;
                    imagePosX += easternOffset * 20;
                    imagePosY += easternOffset * 10;
                    break;
                case Direction.SouthWest:
                    // North is bottom right. East is bottom left.
                    imagePosX += northernOffset * 20;
                    imagePosY += northernOffset * 10;
                    imagePosX -= easternOffset * 20;
                    imagePosY += easternOffset * 10;
                    break;
                case Direction.SouthEast:
                    // North is bottom left. East is top left.
                    imagePosX -= northernOffset * 20;
                    imagePosY += northernOffset * 10;
                    imagePosX -= easternOffset * 20;
                    imagePosY -= easternOffset * 10;
                    break;
                default: // "default" is northeast.
                    // North is top left. East is top right.
                    imagePosX -= northernOffset * 20;
                    imagePosY -= northernOffset * 10;
                    imagePosX += easternOffset * 20;
                    imagePosY -= easternOffset * 10;
                    break;
            }
            int sortOrder = (imagePosY / 10) * 100; // Force the tiles into rows initially, with wide spaces in sort order
            imagePosY -= verticalOffset * 10;

            switch(position)
            {
                case 1: 
                    imagePosY -= 40;
                    break;
                case 2:
                    imagePosX += 40;
                    break;
                case 3:
                    imagePosY += 40;
                    break;
                case 4:
                    imagePosX -= 40;
                    break;
                case 5:
                    imagePosX -= 30;
                    imagePosY += 15;
                    break;
                case 6:
                    imagePosX += 30;
                    imagePosY += 15;
                    break;
                case 7:
                    imagePosX -= 30;
                    imagePosY -= 15;
                    break;
                case 8:
                    imagePosX += 30;
                    imagePosY -= 15;
                    break;
            }

            ImageLocation = new Point(imagePosX, imagePosY);
        }

        /// <summary>
        /// Used to sort a collection of TileImages such that the first objects
        /// in the collection are the first who need to be drawn.
        /// </summary>
        /// <param name="other">The other tile to be sorted against</param>
        /// <returns>The result of draw index sorting</returns>
        public int CompareTo(IPlaySceneDrawable other)
        {
            return DrawIndex.CompareTo(other.GetDrawIndex());
        }

        public int GetDrawIndex()
        {
            return DrawIndex;
        }

        public Point GetImageLocation()
        {
            return ImageLocation;
        }

        public bool GetMousedOver()
        {
            return MouseOver;
        }

        public void SetMousedOver(bool State)
        {
            MouseOver = State;
        }

        public bool GetSelected()
        {
            return Selected;
        }

        public void SetSelected(bool State)
        {
            Selected = State;
            if(State && ControlSelected != null)
            {
                ControlSelected(this, new EventArgs());
            }
        }

        public Image GetTemplateImage() { return TemplateImage; }
        public Image GetDayImage() { return TemplateImage; }
        public Image GetNightImage() { return TemplateImage; }
        public Image GetTwilightImage() { return TemplateImage; }
        public Image GetSelectedImage() { return SelectedImage; }
        public Image GetMouseOverImage() { return MouseOverImage; }

        public int GetObjectPositionX()
        {
            return PositionX;
        }

        public int GetObjectPositionY()
        {
            return PositionY;
        }

        public int GetObjectPositionZ()
        {
            return PositionZ;
        }

        public IPlaySceneDrawable GetNewDrawable() 
        {
            return this;
        }

        public DrawableType GetDrawableType()
        {
            return DrawableType.MoveControl;
        }
    }

    public enum PlaySceneControlType
    {
        Move,
        FaceBottomLeft,
        FaceBottomRight,
        FaceTopLeft,
        FaceTopRight,        
    }
}
