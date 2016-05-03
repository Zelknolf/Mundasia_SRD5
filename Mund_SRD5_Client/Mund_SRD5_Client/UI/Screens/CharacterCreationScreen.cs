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

        static Label characterClassLabel = new Label();
        static Label characterClassSelection = new Label();

        static ListView characterClassBox = new ListView();

        private static bool _eventsInitialized = false;

        public static void Set(Form primaryForm)
        {
            _form = primaryForm;
            _form.Resize += _form_Resize;

            _characterSheet.Height = _form.ClientRectangle.Height - (padding * 2);
            _characterSheet.Width = (_form.ClientRectangle.Width / 2) - (padding * 3);
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

            characterClassLabel.Text = "Class:";
            characterClassLabel.Size = characterClassLabel.PreferredSize;
            characterClassLabel.Location = new Point(padding, padding);
            StyleLabel(characterClassLabel);

            characterClassSelection.Text = "<not selected>";
            characterClassSelection.Size = characterClassSelection.PreferredSize;
            characterClassSelection.Location = new Point(characterClassLabel.Location.X + characterClassLabel.Width + padding, padding);
            StyleLabel(characterClassSelection);


            if (!_eventsInitialized)
            {
                characterClassLabel.Click += EditCharacterClass;
                characterClassSelection.Click += EditCharacterClass;
                _eventsInitialized = true;
            }

            _characterSheet.Controls.Add(characterClassLabel);
            _characterSheet.Controls.Add(characterClassSelection);

            _panel.Controls.Add(_characterSheet);
            _panel.Controls.Add(_editPanel);
            _form.Controls.Add(_panel);
        }

        public static void Clear()
        {
            _form.Resize -= _form_Resize;
            _form.Controls.Remove(_panel);
        }

        
        public static void EditCharacterClass(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Class) return;

            characterClassBox.Height = _editPanel.Height - (padding * 2);
            characterClassBox.Width = _editPanel.Width - (padding * 2);
            characterClassBox.Location = new Point(padding, padding);
            StyleListView(characterClassBox);

            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            int imageIndex = 0;

            foreach (CharacterClass cl in CharacterClass.GetClasses())
            {
                if (cl.HitDie > 0) // Subclasses mark themselves as 0 HD.
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { "", cl.Name });
                    toAdd.ImageIndex = imageIndex;
                    imgs.Images.Add(cl.Icon);
                    imageIndex++;
                    StyleListViewItem(toAdd);
                    characterClassBox.Items.Add(toAdd);
                }
            }
            characterClassBox.SmallImageList = imgs;

            _editPanel.Controls.Add(characterClassBox);

            _currentEdit = CurrentEdit.Class;
        }

        private static void _form_Resize(object sender, EventArgs e)
        {
        }

        private static CurrentEdit _currentEdit;

        private enum CurrentEdit
        {
            None,
            Class
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
