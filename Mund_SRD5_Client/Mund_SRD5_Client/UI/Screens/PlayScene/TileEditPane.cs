using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public class TileEditPane : Panel
    {
        public static int padding = 5;
        public TileEditPane(Tile tile)
        {
            this.BackColor = Color.Black;
            shownTile = tile;

            Label labX = new Label();
            labX.BackColor = Color.Black;
            labX.ForeColor = Color.White;
            labX.Text = "X: ";
            labX.Size = labX.PreferredSize;
            labX.Location = new Point(padding, padding);
            this.Controls.Add(labX);

            X = new NumericUpDown();
            X.Minimum = Int32.MinValue;
            X.Maximum = Int32.MaxValue;
            X.BackColor = Color.Black;
            X.ForeColor = Color.White;
            X.Height = X.PreferredHeight;
            X.Width = Math.Max(0, this.ClientRectangle.Width - labX.Width - (padding * 3));
            X.Location = new Point(labX.Location.X + labX.Width + padding, labX.Location.Y);
            this.Controls.Add(X);

            Label labY = new Label();
            labY.BackColor = Color.Black;
            labY.ForeColor = Color.White;
            labY.Text = "Y: ";
            labY.Size = labY.PreferredSize;
            labY.Location = new Point(padding, labX.Location.Y + labX.Height + padding);
            this.Controls.Add(labY);

            Y = new NumericUpDown();
            Y.Minimum = Int32.MinValue;
            Y.Maximum = Int32.MaxValue;
            Y.BackColor = Color.Black;
            Y.ForeColor = Color.White;
            Y.Height = Y.PreferredHeight;
            Y.Width = Math.Max(0, this.ClientRectangle.Width - labY.Width - (padding * 3));
            Y.Location = new Point(labY.Location.X + labY.Width + padding, labY.Location.Y);
            this.Controls.Add(Y);

            Label labZ = new Label();
            labZ.BackColor = Color.Black;
            labZ.ForeColor = Color.White;
            labZ.Text = "Z: ";
            labZ.Size = labZ.PreferredSize;
            labZ.Location = new Point(padding, labY.Location.Y + labY.Height + padding);
            this.Controls.Add(labZ);

            Z = new NumericUpDown();
            Z.Minimum = Int32.MinValue;
            Z.Maximum = Int32.MaxValue;
            Z.BackColor = Color.Black;
            Z.ForeColor = Color.White;
            Z.Height = Z.PreferredHeight;
            Z.Width = Math.Max(0, this.ClientRectangle.Width - labZ.Width - (padding * 3));
            Z.Location = new Point(labZ.Location.X + labZ.Width + padding, labZ.Location.Y);
            this.Controls.Add(Z);

            Label tileSetLabel = new Label();
            tileSetLabel.BackColor = Color.Black;
            tileSetLabel.ForeColor = Color.White;
            tileSetLabel.Text = "Tileset: ";
            tileSetLabel.Size = tileSetLabel.PreferredSize;
            tileSetLabel.Location = new Point(padding, labZ.Location.Y + labZ.Height + (padding * 2));
            this.Controls.Add(tileSetLabel);

            Set = new ComboBox();
            Set.BackColor = Color.Black;
            Set.ForeColor = Color.White;
            Set.AutoCompleteMode = AutoCompleteMode.Suggest;
            Set.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Set.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            foreach (TileSet ts in TileSet.GetSets())
            {
                Set.Items.Add(ts.Name);
            }
            Set.Height = Set.PreferredHeight;
            Set.Width = Math.Max(0, this.ClientRectangle.Width - tileSetLabel.Width - (padding * 3));
            Set.Location = new Point(tileSetLabel.Location.X + tileSetLabel.Width + padding, tileSetLabel.Location.Y);
            this.Controls.Add(Set);

            Label tileHeightLabel = new Label();
            tileHeightLabel.BackColor = Color.Black;
            tileHeightLabel.ForeColor = Color.White;
            tileHeightLabel.Text = "Height: ";
            tileHeightLabel.Size = tileHeightLabel.PreferredSize;
            tileHeightLabel.Location = new Point(padding, Set.Location.Y + Set.Height + (padding * 2));
            this.Controls.Add(tileHeightLabel);

            TileHeight = new ComboBox();
            TileHeight.BackColor = Color.Black;
            TileHeight.ForeColor = Color.White;
            TileHeight.AutoCompleteMode = AutoCompleteMode.Suggest;
            TileHeight.AutoCompleteSource = AutoCompleteSource.CustomSource;
            TileHeight.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            TileHeight.Items.Add("1");
            TileHeight.Items.Add("2");
            TileHeight.Items.Add("3");
            TileHeight.Items.Add("4");
            TileHeight.Height = TileHeight.PreferredHeight;
            TileHeight.Width = Math.Max(0, this.ClientRectangle.Width - tileHeightLabel.Width - (padding * 3));
            TileHeight.Location = new Point(tileHeightLabel.Location.X + tileHeightLabel.Width + padding, tileHeightLabel.Location.Y);
            this.Controls.Add(TileHeight);

            Label tileDirectionLabel = new Label();
            tileDirectionLabel.BackColor = Color.Black;
            tileDirectionLabel.ForeColor = Color.White;
            tileDirectionLabel.Text = "Height: ";
            tileDirectionLabel.Size = tileDirectionLabel.PreferredSize;
            tileDirectionLabel.Location = new Point(padding, TileHeight.Location.Y + TileHeight.Height + (padding * 2));
            this.Controls.Add(tileDirectionLabel);

            TileDirection = new ComboBox();
            TileDirection.BackColor = Color.Black;
            TileDirection.ForeColor = Color.White;
            TileDirection.AutoCompleteMode = AutoCompleteMode.Suggest;
            TileDirection.AutoCompleteSource = AutoCompleteSource.CustomSource;
            TileDirection.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                TileDirection.Items.Add(dir.ToString());
            }
            TileDirection.Height = TileDirection.PreferredHeight;
            TileDirection.Width = Math.Max(0, this.ClientRectangle.Width - tileDirectionLabel.Width - (padding * 3));
            TileDirection.Location = new Point(tileDirectionLabel.Location.X + tileDirectionLabel.Width + padding, tileDirectionLabel.Location.Y);
            this.Controls.Add(TileDirection);

            AddTile = new Button();
            AddTile.BackColor = Color.Black;
            AddTile.ForeColor = Color.White;
            AddTile.Text = "Add Tile";
            AddTile.Height = AddTile.PreferredSize.Height;
            AddTile.Width = this.ClientRectangle.Width - (padding * 2);
            AddTile.Location = new Point(padding, TileDirection.Location.Y + TileDirection.Height + padding);
            this.Controls.Add(AddTile);

            RemoveTile = new Button();
            RemoveTile.BackColor = Color.Black;
            RemoveTile.ForeColor = Color.White;
            RemoveTile.Text = "Remove Tile";
            RemoveTile.Height = RemoveTile.PreferredSize.Height;
            RemoveTile.Width = this.ClientRectangle.Width - (padding * 2);
            RemoveTile.Location = new Point(padding, AddTile.Location.Y + AddTile.Height + padding);
            this.Controls.Add(RemoveTile);

            RefreshDisplay();
        }

        public Tile shownTile;

        public NumericUpDown X;
        public NumericUpDown Y;
        public NumericUpDown Z;

        public ComboBox Set;
        public ComboBox TileHeight;
        public ComboBox TileDirection;

        public Button AddTile;
        public Button RemoveTile;

        public bool SettingNewTile = false;

        public void SetTile(Tile tile)
        {
            SettingNewTile = true;
            shownTile = tile;
            if (tile != null)
            {
                X.Value = tile.PosX;
                Y.Value = tile.PosY;
                Z.Value = tile.PosZ;
                Set.Text = TileSet.GetSet(shownTile.CurrentTileSet).Name;
                TileHeight.Text = shownTile.TileHeight.ToString();
                TileDirection.Text = shownTile.Slope.ToString();
            }
            else
            {
                Z.Value = 0;
                Y.Value = 0;
                X.Value = 0;
                Set.Text = "";
                TileHeight.Text = "";
                TileDirection.Text = "";
            }
            SettingNewTile = false;
        }

        public void RefreshDisplay()
        {
            SettingNewTile = true;
            if (shownTile != null)
            {
                X.Value = shownTile.PosX;
                Y.Value = shownTile.PosY;
                Z.Value = shownTile.PosZ;
                Set.Text = TileSet.GetSet(shownTile.CurrentTileSet).Name;
                TileHeight.Text = shownTile.TileHeight.ToString();
                TileDirection.Text = shownTile.Slope.ToString();
            }
            else
            {
                Z.Value = 0;
                Y.Value = 0;
                X.Value = 0;
                Set.Text = "";
                TileHeight.Text = "";
                TileDirection.Text = "";
            }
            SettingNewTile = false;
        }
    }
}
