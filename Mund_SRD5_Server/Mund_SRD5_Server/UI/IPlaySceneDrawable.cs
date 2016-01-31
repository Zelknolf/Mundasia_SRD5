using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mundasia.Objects
{
    public interface IPlaySceneDrawable: IComparable<IPlaySceneDrawable>
    {
        int GetDrawIndex();

        Point GetImageLocation();

        Image GetTemplateImage();

        new int CompareTo(IPlaySceneDrawable other);

        bool GetMousedOver();

        void SetMousedOver(bool State);

        bool GetSelected();

        void SetSelected(bool State);

        Image GetDayImage();
        Image GetNightImage();
        Image GetTwilightImage();
        Image GetSelectedImage();
        Image GetMouseOverImage();

        int GetObjectPositionX();
        int GetObjectPositionY();
        int GetObjectPositionZ();

        IPlaySceneDrawable GetNewDrawable();


        DrawableType GetDrawableType();
    }

    public static class PlaySceneDrawable
    {
        /// <summary>
        /// Returns the TileImage which represents the tile which is on top of the drawing
        /// at the point
        /// </summary>
        /// <param name="clientRectLocation">The location of the point to be evaluated</param>
        /// <param name="presentImages">The TileImages used to construct the view</param>
        /// <returns>The TileImage which is receiving the mouseover</returns>
        public static IPlaySceneDrawable GetTarget(Point clientRectLocation, List<IPlaySceneDrawable> presentImages)
        {
            List<IPlaySceneDrawable> mousedOver = new List<IPlaySceneDrawable>();
            foreach (IPlaySceneDrawable image in presentImages)
            {
                if (image.GetImageLocation().X < clientRectLocation.X &&
                    image.GetImageLocation().X + image.GetTemplateImage().Width > clientRectLocation.X &&
                    image.GetImageLocation().Y < clientRectLocation.Y &&
                    image.GetImageLocation().Y + image.GetTemplateImage().Height > clientRectLocation.Y)
                {
                    mousedOver.Add(image);
                }
            }
            if (mousedOver.Count > 0)
            {
                mousedOver.Sort();
                while (mousedOver.Count > 0)
                {
                    int imgPosX = clientRectLocation.X - mousedOver[mousedOver.Count - 1].GetImageLocation().X;
                    int imgPosY = clientRectLocation.Y - mousedOver[mousedOver.Count - 1].GetImageLocation().Y;
                    Color px = (mousedOver[mousedOver.Count - 1].GetTemplateImage() as Bitmap).GetPixel(imgPosX, imgPosY);
                    if (px.A != 0)
                    {
                        return mousedOver[mousedOver.Count - 1];
                    }
                    else
                    {
                        mousedOver.Remove(mousedOver[mousedOver.Count - 1]);
                    }
                }
            }
            return null;
        }
    }

    public enum DrawableType
    {
        Tile,
        Character,
        MoveControl
    }
}
