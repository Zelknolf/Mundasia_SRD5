using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mundasia.Client;
using Mundasia.Objects;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")] 
    public class CharacterCreationScreen: Panel
    {
        public CharacterCreationScreen() {}

        static Size IconSize = new Size(64, 64);

        private static int selectionHeight = 250;
        private static int padding = 5;
        private static int indent = 20;

        private static CharacterCreationScreen _panel = new CharacterCreationScreen();
        private static Panel _characterSheet = new Panel();
        private static Panel _editPanel = new Panel();
        private static Form _form;

        static Label strengthLabel = new Label();
        static Label dexterityLabel = new Label();
        static Label constitutionLabel = new Label();
        static Label intelligenceLabel = new Label();
        static Label wisdomLabel = new Label();
        static Label charismaLabel = new Label();

        static NumericUpDown strengthEntry = new NumericUpDown();
        static NumericUpDown dexterityEntry = new NumericUpDown();
        static NumericUpDown constitutionEntry = new NumericUpDown();
        static NumericUpDown intelligenceEntry = new NumericUpDown();
        static NumericUpDown wisdomEntry = new NumericUpDown();
        static NumericUpDown charismaEntry = new NumericUpDown();

        static Panel characterClassIcon = new Panel();
        static Label characterClassText = new Label();
        static Panel raceIcon = new Panel();
        static Label raceText = new Label();

        static ListView characterClassBox = new ListView();
        static ListView raceBox = new ListView();

        private static bool _eventsInitialized = false;

        public static void Set(Form primaryForm)
        {
            _form = primaryForm;
            _form.Resize += _form_Resize;

            _characterSheet.Height = _form.ClientRectangle.Height - (padding * 2);
            _characterSheet.Width = (_form.ClientRectangle.Width - (padding * 3)) / 2;
            _characterSheet.Location = new Point(padding, padding);
            _characterSheet.BackColor = Color.Black;
            _characterSheet.BorderStyle = BorderStyle.FixedSingle;

            _editPanel.Height = _characterSheet.Height;
            _editPanel.Width = _characterSheet.Width;
            _editPanel.Location = new Point(_characterSheet.Location.X + _characterSheet.Width + padding, padding);
            _editPanel.BackColor = Color.Black;
            _editPanel.BorderStyle = BorderStyle.FixedSingle;

            _panel.Size = _form.ClientRectangle.Size;
            _panel.BackColor = Color.Black;

            raceIcon.Size = new Size(64, 64);
            raceIcon.Location = new Point(padding, padding);
            raceIcon.BackgroundImage = Race.NullRaceImage;

            raceText.Text = "No Race Selected";
            raceText.Location = new Point(raceIcon.Location.X + raceIcon.Width + padding, padding);
            raceText.TextAlign = ContentAlignment.MiddleCenter;
            StyleLabel(raceText);
            raceText.Size = new Size((_characterSheet.Width - (raceIcon.Width * 2) - (padding * 5)) / 2, 64);

            characterClassIcon.Size = new Size(64, 64);
            characterClassIcon.Location = new Point(raceText.Location.X + raceText.Width + padding, padding);
            characterClassIcon.BackgroundImage = CharacterClass.NullClassImage;

            characterClassText.Text = "No Class Selected";
            characterClassText.Location = new Point(characterClassIcon.Location.X + characterClassIcon.Width + padding, padding);
            characterClassText.TextAlign = ContentAlignment.MiddleCenter;
            StyleLabel(characterClassText);
            characterClassText.Size = raceText.Size;


            if (!_eventsInitialized)
            {
                raceIcon.Click += EditRace;
                raceText.Click += EditRace;
                raceBox.ItemSelectionChanged += RaceBox_ItemSelectionChanged;
                characterClassIcon.Click += EditCharacterClass;
                characterClassText.Click += EditCharacterClass;
                characterClassBox.ItemSelectionChanged += CharacterClassBox_ItemSelectionChanged;
                _eventsInitialized = true;
            }

            _characterSheet.Controls.Add(raceIcon);
            _characterSheet.Controls.Add(raceText);
            _characterSheet.Controls.Add(characterClassIcon);
            _characterSheet.Controls.Add(characterClassText);

            _panel.Controls.Add(_characterSheet);
            _panel.Controls.Add(_editPanel);
            _form.Controls.Add(_panel);
        }

        public static void Clear()
        {
            _form.Resize -= _form_Resize;
            _form.Controls.Remove(_panel);
        }

        #region Race Selection
        private static void RaceBox_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                Race selectedRace = Race.GetRace((uint)e.Item.Tag);
                if (selectedRace != null)
                {
                    raceText.Text = e.Item.Name;
                    raceIcon.BackgroundImage = selectedRace.Icon;

                    _populateRaceList();
                }
            }
        }

        public static void EditRace(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Race) return;

            _editPanel.Controls.Clear();

            raceBox.Height = _editPanel.Height - (padding * 2);
            raceBox.Width = _editPanel.Width - (padding * 2);
            raceBox.Location = new Point(padding, padding);
            StyleListView(raceBox);

            _populateRaceList();

            _editPanel.Controls.Add(raceBox);

            _currentEdit = CurrentEdit.Race;
        }

        private static void _populateRaceList()
        {
            raceBox.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            int imageIndex = 0;

            foreach(Race race in Race.GetRaces())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { "", race.Name });
                toAdd.Name = race.Name;
                toAdd.ImageIndex = imageIndex;
                toAdd.Tag = race.Id;
                toAdd.ToolTipText = StringLibrary.GetString(race.Description);
                imgs.Images.Add(race.Icon);
                imageIndex++;
                StyleListViewItem(toAdd);
                raceBox.Items.Add(toAdd);
            }
            raceBox.SmallImageList = imgs;
        }
        #endregion

        #region Character Class Selection
        private static void CharacterClassBox_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                CharacterClass selectedClass = CharacterClass.GetClass((uint)e.Item.Tag);
                if(selectedClass != null)
                {
                    characterClassText.Text = e.Item.Name;
                    characterClassIcon.BackgroundImage = selectedClass.Icon;

                    if(selectedClass.SubClassLevel == 1)
                    {
                        EditCharacterSubClass(selectedClass);
                    }
                    else
                    {
                        _populateCharacterClassList();
                    }
                }
            }
        }

        public static void EditCharacterClass(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Class) return;

            _editPanel.Controls.Clear();

            characterClassBox.Height = _editPanel.Height - (padding * 2);
            characterClassBox.Width = _editPanel.Width - (padding * 2);
            characterClassBox.Location = new Point(padding, padding);
            StyleListView(characterClassBox);

            _populateCharacterClassList();

            _editPanel.Controls.Add(characterClassBox);

            _currentEdit = CurrentEdit.Class;
        }

        private static void _populateCharacterClassList()
        {
            characterClassBox.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            int imageIndex = 0;

            foreach (CharacterClass cl in CharacterClass.GetClasses())
            {
                if (cl.HitDie > 0) // Subclasses mark themselves as 0 HD.
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { "", cl.Name });
                    toAdd.Name = cl.Name;
                    toAdd.ImageIndex = imageIndex;
                    toAdd.Tag = cl.Id;
                    toAdd.ToolTipText = StringLibrary.GetString(cl.Description);
                    imgs.Images.Add(cl.Icon);
                    imageIndex++;
                    StyleListViewItem(toAdd);
                    characterClassBox.Items.Add(toAdd);
                }
            }
            characterClassBox.SmallImageList = imgs;
        }

        public static void EditCharacterSubClass(CharacterClass selClass)
        {
            characterClassBox.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            int imageIndex = 0;

            foreach (CharacterClass cl in selClass.SubClasses)
            {
                ListViewItem toAdd = new ListViewItem(new string[] { "", selClass.Name + ", " + cl.Name });
                toAdd.Name = selClass.Name + ", " + cl.Name;
                toAdd.ImageIndex = imageIndex;
                toAdd.Tag = cl.Id;
                toAdd.ToolTipText = StringLibrary.GetString(cl.Description);
                imgs.Images.Add(cl.Icon);
                imageIndex++;
                StyleListViewItem(toAdd);
                characterClassBox.Items.Add(toAdd);
            }
            characterClassBox.SmallImageList = imgs;
        }
        #endregion

        private static void _form_Resize(object sender, EventArgs e)
        {
            _panel.Size = _form.ClientRectangle.Size;

            _characterSheet.Height = _form.ClientRectangle.Height - (padding * 2);
            _characterSheet.Width = (_form.ClientRectangle.Width - (padding * 3)) / 2;
            _characterSheet.Location = new Point(padding, padding);

            _editPanel.Height = _characterSheet.Height;
            _editPanel.Width = _characterSheet.Width;
            _editPanel.Location = new Point(_characterSheet.Location.X + _characterSheet.Width + padding, padding);

            raceIcon.Location = new Point(padding, padding);
            raceText.Location = new Point(raceIcon.Location.X + raceIcon.Width + padding, padding);
            raceText.Size = new Size((_characterSheet.Width - (raceIcon.Width * 2) - (padding * 5)) / 2, 64);

            characterClassIcon.Location = new Point(raceText.Location.X + raceText.Width + padding, padding);
            characterClassText.Location = new Point(characterClassIcon.Location.X + characterClassIcon.Width + padding, padding);
            characterClassText.Size = raceText.Size;

            switch (_currentEdit)
            {
                case CurrentEdit.Class:
                    characterClassBox.Height = _editPanel.Height - (padding * 2);
                    characterClassBox.Width = _editPanel.Width - (padding * 2);
                    characterClassBox.Location = new Point(padding, padding);
                    break;
                case CurrentEdit.Race:
                    raceBox.Height = _editPanel.Height - (padding * 2);
                    raceBox.Width = _editPanel.Width - (padding * 2);
                    raceBox.Location = new Point(padding, padding);
                    break;
            }
        }

        private static CurrentEdit _currentEdit;

        private enum CurrentEdit
        {
            None,
            Class,
            Race
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        private static void StyleLabel(Control toStyle)
        {
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }

        private static void StyleListView(ListView listView)
        {
            listView.Clear();
            listView.Columns.Add("");
            listView.Columns.Add("");
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.BackColor = Color.Black;
            listView.ForeColor = Color.White;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.ShowItemToolTips = true;
            listView.Font = labelFont;
            listView.Columns[0].Width = IconSize.Width + 2;
            listView.Columns[1].Width = listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - IconSize.Width - 2;
        }

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }
    }
}
