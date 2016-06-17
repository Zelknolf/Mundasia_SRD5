using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using Mund_SRD5_Client.UI.Utilities;

namespace Mundasia.Objects
{
    public class DisplayCharacter
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };
        private static string groupDelimiter = "[";
        private static char[] groupDelim = new char[] { '[' };

        public static uint currentId = 0;

        public static Color LineHair = Color.FromArgb(255, 102, 93, 61);
        public static Color LineHair2 = Color.FromArgb(255, 102, 94, 61);
        public static Color DarkHair = Color.FromArgb(255, 127, 116, 76);
        public static Color MediumHair = Color.FromArgb(255, 191, 177, 114);
        public static Color MediumHair2 = Color.FromArgb(255, 191, 175, 114);
        public static Color LightHair = Color.FromArgb(255, 255, 234, 153);
        public static Color LightHair2 = Color.FromArgb(255, 255, 238, 153);

        public static Color LineSkin = Color.FromArgb(255, 102, 84, 65);
        public static Color DarkSkin = Color.FromArgb(255, 127, 108, 82);
        public static Color MediumSkin = Color.FromArgb(255, 191, 162, 124);
        public static Color MediumSkin2 = Color.FromArgb(255, 191, 163, 124);
        public static Color LightSkin = Color.FromArgb(255, 255, 217, 165);
        public static Color LightSkin2 = Color.FromArgb(255, 255, 216, 165);

        public static Color DarkClothA = Color.FromArgb(255, 127, 0, 0);
        public static Color MediumClothA = Color.FromArgb(255, 255, 0, 0);
        public static Color LightClothA = Color.FromArgb(255, 255, 100, 100);

        public static Color DarkClothB = Color.FromArgb(255, 127, 51, 0);
        public static Color MediumClothB = Color.FromArgb(255, 255, 106, 0);
        public static Color LightClothB = Color.FromArgb(255, 255, 200, 150);

        public static string DisplayCharacterCollectionToString(List<DisplayCharacter> list)
        {
            StringBuilder str = new StringBuilder();
            foreach(DisplayCharacter dc in list)
            {
                str.Append(dc.ToString());
                str.Append(groupDelimiter);
            }
            return str.ToString().Trim(groupDelim);
        }

        public static List<DisplayCharacter> DisplayCharacterCollectionFromString(string fileString)
        {
            string[] charBuilders = fileString.Split(groupDelim);
            List<DisplayCharacter> dcBuilders = new List<DisplayCharacter>();
            foreach (string dcBuilder in charBuilders)
            {
                DisplayCharacter dc = new DisplayCharacter(dcBuilder);
                if (dc.Hair >= 0) dcBuilders.Add(dc);
            }
            return dcBuilders;
        }

        public static DisplayCharacter GetDisplayCharacter(Creature ch)
        {
            if (ch.CachedDisplay != null &&
                ch.CachedDisplay.x == ch.LocationX &&
                ch.CachedDisplay.y == ch.LocationY &&
                ch.CachedDisplay.z == ch.LocationZ &&
                ch.CachedDisplay.Facing == ch.LocationFacing)
            {
                if(ch.Equipment != null && 
                   ch.Equipment.ContainsKey((int)InventorySlot.Chest) &&
                   ch.Equipment[(int)InventorySlot.Chest].Appearance == ch.CachedDisplay.Clothes &&
                   ch.Equipment[(int)InventorySlot.Chest].PrimaryColor == ch.CachedDisplay.ClothColorA &&
                   ch.Equipment[(int)InventorySlot.Chest].SecondaryColor == ch.CachedDisplay.ClothColorB)
                {
                    return ch.CachedDisplay;
                }
                else if((ch.Equipment == null || ch.Equipment.ContainsKey((int)InventorySlot.Chest)) && ch.CachedDisplay.Clothes == 0)
                {
                    return ch.CachedDisplay;
                }
            }
            return new DisplayCharacter(ch);
        }
        
        public DisplayCharacter()
        {
            CharacterId = 0;
            Height = 0;
            x = 0;
            y = 0;
            z = 0;
            Facing = Direction.North;
            CharacterRace = 0;
            SkinColor = 0;
            HairColor = 0;
            Sex = 0;
            Hair = 0;
            Clothes = 0;
            ClothColorA = 0;
            ClothColorB = 0;
        }

        public DisplayCharacter(string fileLine)
        {
            if (String.IsNullOrWhiteSpace(fileLine)) return;
            string[] input = fileLine.Split(delim);
            if (input.Length < 11) return;
            uint.TryParse(input[0], out CharacterId);
            int.TryParse(input[1], out Height);
            int.TryParse(input[2], out x);
            int.TryParse(input[3], out y);
            int.TryParse(input[4], out z);
            Direction.TryParse(input[5], out Facing);
            uint.TryParse(input[6], out CharacterRace);
            int.TryParse(input[7], out SkinColor);
            int.TryParse(input[8], out HairColor);
            int.TryParse(input[9], out Sex);
            int.TryParse(input[10], out Hair);
        }

        private DisplayCharacter(Creature ch)
        {
            if(ch.CachedDisplay == null)
            { 
                CharacterId = currentId++;
            }
            else
            {
                CharacterId = ch.CachedDisplay.CharacterId;
            }
            Height = ch.CharacterRace.Height;
            x = ch.LocationX;
            y = ch.LocationY;
            z = ch.LocationZ;
            Facing = ch.LocationFacing;
            CharacterRace = ch.CharacterRace.Id;
            SkinColor = (int)ch.SkinColor;
            HairColor = (int)ch.HairColor;
            Sex = ch.Gender;
            Hair = (int)ch.HairStyle;
            if(ch.Equipment != null && ch.Equipment.ContainsKey((int)InventorySlot.Chest))
            {
                InventoryItem clothes = ch.Equipment[(int)InventorySlot.Chest];
                Clothes = clothes.Appearance;
                ClothColorA = clothes.PrimaryColor;
                ClothColorB = clothes.SecondaryColor;
            }
            ch.CachedDisplay = this;
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(CharacterId);
            ret.Append(delimiter);
            ret.Append(Height);
            ret.Append(delimiter);
            ret.Append(x);
            ret.Append(delimiter);
            ret.Append(y);
            ret.Append(delimiter);
            ret.Append(z);
            ret.Append(delimiter);
            ret.Append(Facing);
            ret.Append(delimiter);
            ret.Append(CharacterRace);
            ret.Append(delimiter);
            ret.Append(SkinColor);
            ret.Append(delimiter);
            ret.Append(HairColor);
            ret.Append(delimiter);
            ret.Append(Sex);
            ret.Append(delimiter);
            ret.Append(Hair);
            return ret.ToString();
        }

        public uint CharacterId;

        public int Height;
        public int x;
        public int y;
        public int z;
        public Direction Facing;

        public uint CharacterRace;
        public int SkinColor;
        public int HairColor;
        public int Sex;
        public int Hair;
        public int Clothes;
        public int ClothColorA;
        public int ClothColorB;

        public CreatureImage CachedImage;

        public Color ConvertPixel(Color startingPixel)
        {
            Color ret = startingPixel;
            Race currentRace = Race.GetRace(CharacterRace);
            if (ret.A == 0) { }
            else if (startingPixel == LineHair || startingPixel == LineHair2)
            {
                ret = currentRace.HairColors[HairColor].LineColor;
            }
            else if (startingPixel == DarkHair)
            {
                ret = currentRace.HairColors[HairColor].DarkColor;
            }
            else if (startingPixel == MediumHair || startingPixel == MediumHair2)
            {
                ret = currentRace.HairColors[HairColor].MedColor;
            }
            else if (startingPixel == LightHair || startingPixel == LightHair2)
            {
                ret = currentRace.HairColors[HairColor].LightColor;
            }
            else if(startingPixel == LineSkin)
            {
                ret = currentRace.SkinColors[SkinColor].LineColor;
            }
            else if(startingPixel == DarkSkin)
            {
                ret = currentRace.SkinColors[SkinColor].DarkColor;
            }
            else if(startingPixel == MediumSkin || startingPixel == MediumSkin2)
            {
                ret = currentRace.SkinColors[SkinColor].MedColor;
            }
            else if(startingPixel == LightSkin || startingPixel == LightSkin2)
            {
                ret = currentRace.SkinColors[SkinColor].LightColor;
            }
            else if(startingPixel == DarkClothA)
            {
                ret = ClothColor.Dark[ClothColorA];
            }
            else if(startingPixel == MediumClothA)
            {
                ret = ClothColor.Med[ClothColorA];
            }
            else if(startingPixel == LightClothA)
            {
                ret = ClothColor.Light[ClothColorA];
            }
            else if(startingPixel == DarkClothB)
            {
                ret = ClothColor.Dark[ClothColorB];
            }
            else if(startingPixel == MediumClothB)
            {
                ret = ClothColor.Med[ClothColorB];
            }
            else if(startingPixel == LightClothB)
            {
                ret = ClothColor.Light[ClothColorB];
            }
            else if(startingPixel.B == 0 && startingPixel.R != 0 && startingPixel.G != 0)
            {
                // Channel-based coloring. This is a skin pixel.
                float brightness = Pixel.GetBrightness(currentRace.SkinColors[SkinColor].DarkColor) - 0.5f;
                brightness = Math.Max(0.0f, ((float)startingPixel.R / 255.0f) + brightness);
                brightness = Math.Min(1.0f, brightness);
                ret = Pixel.FromHSL(startingPixel.A, currentRace.SkinColors[SkinColor].DarkColor.GetHue(), currentRace.SkinColors[SkinColor].DarkColor.GetSaturation(), brightness);
            }
            else if(startingPixel.B == 0 && startingPixel.R != 0 && startingPixel.G == 0)
            {
                // Channel-based coloring. This is a hair pixel.
                float brightness = Pixel.GetBrightness(currentRace.HairColors[HairColor].DarkColor) - 0.5f;
                brightness = Math.Max(0.0f, ((float)startingPixel.R / 255.0f) + brightness);
                brightness = Math.Min(1.0f, brightness);
                ret = Pixel.FromHSL(startingPixel.A, currentRace.HairColors[HairColor].DarkColor.GetHue(), currentRace.HairColors[HairColor].DarkColor.GetSaturation(), brightness);
            }
            else if(startingPixel.B != 0 && startingPixel.R == 0 && startingPixel.G != 0)
            {
                // Channel-based coloring. This is the primary cloth color.
                float brightness = Pixel.GetBrightness(ClothColor.Med[ClothColorA]) - 0.5f;
                brightness = Math.Max(0.0f, ((float)startingPixel.B / 255.0f) + brightness);
                brightness = Math.Min(1.0f, brightness);
                ret = Pixel.FromHSL(startingPixel.A, ClothColor.Med[ClothColorA].GetHue(), ClothColor.Med[ClothColorA].GetSaturation(), brightness);
            }
            else if (startingPixel.B != 0 && startingPixel.R == 0 && startingPixel.G == 0)
            {
                // Channel-based coloring. This is the secondary cloth color.
                float brightness = Pixel.GetBrightness(ClothColor.Med[ClothColorB]) - 0.5f;
                brightness = Math.Max(0.0f, ((float)startingPixel.B / 255.0f) + brightness);
                brightness = Math.Min(1.0f, brightness);
                ret = Pixel.FromHSL(startingPixel.A, ClothColor.Med[ClothColorB].GetHue(), ClothColor.Med[ClothColorB].GetSaturation(), brightness);
            }
            return ret;
        }

        public Color CombinePixels(Color topPixel, Color bottomPixel)
        {
            if (topPixel.A == 255) return topPixel;
            if (topPixel.A == 0) return bottomPixel;
            if (bottomPixel.A == 0) return topPixel;

            if(topPixel.A + bottomPixel.A >= 255)
            {
                int bottomA = 255 - topPixel.A;

                int R = ((topPixel.R * topPixel.A) / 255) + ((bottomPixel.R * bottomA) / 255);
                int G = ((topPixel.G * topPixel.A) / 255) + ((bottomPixel.G * bottomA) / 255);
                int B = ((topPixel.B * topPixel.A) / 255) + ((bottomPixel.B * bottomA) / 255);

                return Color.FromArgb(255, R, G, B);
            }

            else
            {
                int totalA = topPixel.A + bottomPixel.A;

                int R = ((topPixel.R * topPixel.A) / totalA) + ((bottomPixel.R * bottomPixel.A) / totalA);
                int G = ((topPixel.G * topPixel.A) / totalA) + ((bottomPixel.G * bottomPixel.A) / totalA);
                int B = ((topPixel.B * topPixel.A) / totalA) + ((bottomPixel.B * bottomPixel.A) / totalA);

                return Color.FromArgb(totalA, R, G, B);
            }
        }

        /// <summary>
        /// Provides a TileImage containing images that might be drawn to represent this tile.
        /// 
        /// TileImage is recalculated per call, as the details may change based on position or angle.
        /// </summary>
        /// <param name="centerCoordX">the X coordinate of the center tile, where a larger X is east</param>
        /// <param name="centerCoordY">the Y coordinate of the center tile, where a larger Y is north</param>
        /// <param name="centerCoordZ">the Z coordinate of the top of the tallest part of the tile</param>
        /// <param name="topDirection">the Direction of the screen</param>
        /// <param name="targetPanel">the panel which is intended to receive the tiles</param>
        /// <returns></returns>
        public CreatureImage Image(int centerCoordX, int centerCoordY, int centerCoordZ, Direction topDirection, System.Windows.Forms.Panel targetPanel)
        {
            // -------------------------------------------
            // This region reviews the panel, gathering
            // information that will later be necessary
            // to determine the draw order of the tile
            // and what can fit in the view
            // -------------------------------------------
            #region Study the Panel
            // Find out how many rows of tiles we can fit on this panel. If we don't get an odd number, add one.
            int indexDepth = targetPanel.Height / 10;
            if (indexDepth % 2 != 1) indexDepth += 1;
            #endregion

            // -------------------------------------------
            // This section determines the X/Y coordinates
            // of the top left corner of the image to be
            // used to display the tile.
            // -------------------------------------------
            #region Position Calculation
            // The center of the panel is (roughly) half of the height and width of the panel.
            int centerY = targetPanel.Height / 2 - 10;
            int centerX = targetPanel.Width / 2 - 20;

            // Find offsets for this tile on the in-world coordinates
            int easternOffset = x - centerCoordX;
            int northernOffset = y - centerCoordY;
            int verticalOffset = z - centerCoordZ;

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
            int sortOrder = ((imagePosY / 10) * 100) + 5; // Force the tiles into rows initially, with wide spaces in sort order
            imagePosY -= (verticalOffset * 10) + (Height * 10);

            // vertical offset needs to be more than 50 for this to run into other rows-- which is 500 pixels tall. The thing
            // is already horrendously dominating the screen if it exists.
            sortOrder += verticalOffset;
            #endregion

            // -------------------------------------------
            // This region takes the orientation of the
            // tile compared to the top edge direction to
            // determine which image should be used to
            // draw the tile.
            // -------------------------------------------
            #region Get a File Name
            string facingSuffix = "";
            switch (topDirection)
            {
                case Direction.NorthWest:
                    switch (Facing)
                    {
                        // Direction.Directionless is unhandled, as it has no suffix after block_#
                        case Direction.North: facingSuffix = "_tr"; break;
                        case Direction.East: facingSuffix = "_br"; break;
                        case Direction.South: facingSuffix = "_bl"; break;
                        case Direction.West: facingSuffix = "_tl"; break;
                        default: facingSuffix = "_bl"; break;
                    }
                    break;
                case Direction.SouthEast:
                    switch (Facing)
                    {
                        case Direction.North: facingSuffix = "_bl"; break;
                        case Direction.East: facingSuffix = "_tl"; break;
                        case Direction.South: facingSuffix = "_tr"; break;
                        case Direction.West: facingSuffix = "_br"; break;
                        default: facingSuffix = "_bl"; break;
                    }
                    break;
                case Direction.SouthWest:
                    switch (Facing)
                    {
                        case Direction.North: facingSuffix = "_br"; break;
                        case Direction.East: facingSuffix = "_bl"; break;
                        case Direction.South: facingSuffix = "_tl"; break;
                        case Direction.West: facingSuffix = "_tr"; break;
                        default: facingSuffix = "_bl"; break;
                    }
                    break;
                case Direction.NorthEast:
                    switch (Facing)
                    {
                        case Direction.North: facingSuffix = "_tl"; break;
                        case Direction.East: facingSuffix = "_tr"; break;
                        case Direction.South: facingSuffix = "_br"; break;
                        case Direction.West: facingSuffix = "_bl"; break;
                        default: facingSuffix = "_bl"; break;
                    }
                    break;
            }
            #endregion

            // -------------------------------------------
            // This region checks for a cached version of
            // the images, and creates one if such an
            // image does not exist already.
            // -------------------------------------------
            #region Get an Image
            Bitmap Day = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\"+ CharacterRace +"\\" + Sex + "\\stand" + facingSuffix + ".png"));
            for (int c = 0; c < Day.Width; c++)
            {
                for (int cc = 0; cc < Day.Height; cc++)
                {
                    Color px = Day.GetPixel(c, cc);
                    if (px.A != 0)
                    {
                        Day.SetPixel(c, cc, ConvertPixel(px));
                    }
                }
            }
            if (Hair > 0)
            {
                Bitmap HairBottom = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\" + CharacterRace + "\\" + Sex + "\\Hair\\stand_" + Hair + facingSuffix + "_b.png"));
                for (int c = 0; c < Day.Width; c++)
                {
                    for (int cc = 0; cc < Day.Height; cc++)
                    {
                        Color hair = ConvertPixel(HairBottom.GetPixel(c, cc));
                        Color original = Day.GetPixel(c, cc);
                        Day.SetPixel(c, cc, CombinePixels(original, hair));
                    }
                }

                if (Clothes > 0)
                {
                    Bitmap ClothesImage = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\" + CharacterRace + "\\" + Sex + "\\Clothes\\stand_" + Clothes + facingSuffix + ".png"));
                    for (int c = 0; c < ClothesImage.Width; c++)
                    {
                        for (int cc = 0; cc < ClothesImage.Height; cc++)
                        {
                            Color clothes = ConvertPixel(ClothesImage.GetPixel(c, cc));
                            Color original = Day.GetPixel(c, cc);
                            Day.SetPixel(c, cc, CombinePixels(clothes, original));
                        }
                    }
                }

                Bitmap HairTop = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\" + CharacterRace + "\\" + Sex + "\\Hair\\stand_" + Hair + facingSuffix + "_t.png"));
                for (int c = 0; c < HairTop.Width; c++)
                {
                    for (int cc = 0; cc < HairTop.Height; cc++)
                    {
                        Color hair = ConvertPixel(HairTop.GetPixel(c, cc));
                        Color original = Day.GetPixel(c, cc);
                        Day.SetPixel(c, cc, CombinePixels(hair, original));
                    }
                }
            }
            else
            {
                if (Clothes > 0)
                {
                    Bitmap ClothesImage = new Bitmap(System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\" + CharacterRace + "\\" + Sex + "\\Clothes\\stand_" + Clothes + facingSuffix + ".png"));
                    for (int c = 0; c < ClothesImage.Width; c++)
                    {
                        for (int cc = 0; cc < ClothesImage.Height; cc++)
                        {
                            Color clothes = ConvertPixel(ClothesImage.GetPixel(c, cc));
                            Color original = Day.GetPixel(c, cc);
                            Day.SetPixel(c, cc, CombinePixels(clothes, original));
                        }
                    }
                }
            }

            Bitmap NightBmp = new Bitmap(Day);
            Bitmap TwilightBmp = new Bitmap(Day);
            Bitmap MouseOverBmp = new Bitmap(Day);
            Bitmap SelectionBmp = new Bitmap(Day);

            // Convert night to dark blue
            for (int c = 0; c < NightBmp.Width; c++)
            {
                for (int cc = 0; cc < NightBmp.Height; cc++)
                {
                    Color px = NightBmp.GetPixel(c, cc);
                    if (px.A == 0) continue;
                    int nR = px.R / 4;
                    int nG = px.G * 3 / 8;
                    int nB = px.B / 2;
                    NightBmp.SetPixel(c, cc, Color.FromArgb(px.A, nR, nG, nB));
                }
            }
            // Convert twilight to warm red
            for (int c = 0; c < TwilightBmp.Width; c++)
            {
                for (int cc = 0; cc < TwilightBmp.Height; cc++)
                {
                    Color px = TwilightBmp.GetPixel(c, cc);
                    if (px.A == 0) continue;
                    int nR = px.R;
                    int nG = px.G * 3 / 4;
                    int nB = px.B / 2;
                    TwilightBmp.SetPixel(c, cc, Color.FromArgb(px.A, nR, nG, nB));
                }
            }
            // Create a mouse over sheet
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

            DayCache[CharacterId] = Day;
            TwilightCache[CharacterId] = TwilightBmp;
            NightCache[CharacterId] = NightBmp;
            MouseOverCache[CharacterId] = MouseOverBmp;
            SelectedCache[CharacterId] = SelectionBmp;
            #endregion

            #region Return a new TileImage
            CreatureImage retVal = new CreatureImage()
            {
                DayImage = DayCache[CharacterId],
                NightImage = NightCache[CharacterId],
                TwilightImage = TwilightCache[CharacterId],
                MouseOverImage = MouseOverCache[CharacterId],
                SelectedImage = SelectedCache[CharacterId],
                Selected = false,
                MousedOver = false,
                DrawIndex = sortOrder,
                ImageLocation = new Point(imagePosX, imagePosY),
                SourceCharacter = this
            };
            this.CachedImage = retVal;
            return retVal;
            #endregion
        }

        #region Caching of Tiles
        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be DayCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> DayCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be TwilightCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> TwilightCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be NightCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> NightCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which indicate a mouse over effect for given tiles. Expected syntax would be MouseOverCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> MouseOverCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which indicate a selection effect for given tiles. Expected syntax would be MouseOverCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> SelectedCache = new Dictionary<uint, Image>();
        #endregion
    }
}
