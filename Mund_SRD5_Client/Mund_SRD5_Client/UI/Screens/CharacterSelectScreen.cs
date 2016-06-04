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
using Mundasia.Communication;

namespace Mundasia.Interface
{
   [System.ComponentModel.DesignerCategory("")] 
    public class CharacterSelectScreen: Panel
    {
        public CharacterSelectScreen() { }
    
        private static int padding = 5;

        private static Panel _character = new Panel();
        private static Panel _message = new Panel();
        private static Panel _description = new Panel();

        private static Form _form;

        private static ListView _characterList = new ListView();
        private static ListView _messageList = new ListView();
        private static RichTextBox _descriptionBox = new RichTextBox();

        private static Label _createCharacter = new Label();

        private static Label _selectCharacter = new Label();

        private static bool _eventsInitialized = false;

        public static void Set(Form primaryForm)
        {
            _form = primaryForm;
            _form.Resize += _form_Resize;

            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);
            int halfHeight = (_form.ClientRectangle.Height / 2) - (padding * 2);

            if (_eventsInitialized == false)
            {
                StyleLabel(_character);
                StyleLabel(_message);
                StyleLabel(_description);
                StyleLabel(_createCharacter);
                StyleLabel(_selectCharacter);
                StyleListView(_messageList);

                _characterList.ItemSelectionChanged += _characterList_ItemSelectionChanged;
                _createCharacter.MouseEnter += _createCharacter_MouseEnter;
                _createCharacter.MouseLeave += _createCharacter_MouseLeave;
                _createCharacter.Click += _createCharacter_Click;

                _selectCharacter.MouseEnter += _selectCharacter_MouseEnter;
                _selectCharacter.MouseLeave += _selectCharacter_MouseLeave;
                _selectCharacter.Click += _selectCharacter_Click;

                _eventsInitialized = true;
            }

            _character.Size = new Size(width, halfHeight);
            _character.Location = new Point(padding, padding);
            _character.BorderStyle = BorderStyle.FixedSingle;

            _message.Size = new Size(width, halfHeight);
            _message.Location = new Point(padding, halfHeight + (padding * 3));
            _message.BorderStyle = BorderStyle.FixedSingle;

            _description.Size = new Size(width, height);
            _description.Location = new Point(width + (padding * 3), padding);
            _description.BorderStyle = BorderStyle.FixedSingle;

            _createCharacter.Text = StringLibrary.GetString(34);
            _createCharacter.Size = _createCharacter.PreferredSize;
            _createCharacter.Location = new Point(_character.ClientRectangle.Width - _createCharacter.Width, _character.ClientRectangle.Height - _createCharacter.Height);

            _selectCharacter.Text = StringLibrary.GetString(37);
            _selectCharacter.Size = _selectCharacter.PreferredSize;
            _selectCharacter.Location = new Point(_createCharacter.Location.X - _selectCharacter.Width - padding, _createCharacter.Location.Y);

            _character.Controls.Add(_createCharacter);
            _character.Controls.Add(_selectCharacter);

            _characterList.Size = new Size(_character.ClientRectangle.Size.Width, _character.ClientRectangle.Size.Height - _createCharacter.Height);
            StyleListView(_characterList);
            _character.Controls.Add(_characterList);

            _messageList.Size = _message.ClientRectangle.Size;
            _message.Controls.Add(_messageList);

            _form.Controls.Add(_character);
            _form.Controls.Add(_message);
            _form.Controls.Add(_description);
            _description.Controls.Add(_descriptionBox);
            StyleLabel(_descriptionBox);
            _descriptionBox.Size = _description.ClientRectangle.Size;

            PopulateCharacterList();
        }

        public static void Clear(Form primaryForm)
        {
            _character.Controls.Clear();
            _message.Controls.Clear();
            _description.Controls.Clear();
            _form.Controls.Clear();
            _form.Resize -= _form_Resize;
        }

        public static void PopulateCharacterList()
        {
            string ch = Mundasia.Communication.ServiceConsumer.ListCharacters();
            if(String.IsNullOrWhiteSpace(ch))
            {
                return;
            }
            string[] chs = ch.Split(new char[] { '|' });
            foreach(string c in chs)
            {
                if(!String.IsNullOrWhiteSpace(c))
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { c, c });
                    StyleListViewItem(toAdd);
                    _characterList.Items.Add(toAdd);
                }
            }
        }

        private static void SetCharacterDetails(Creature cha)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cha.CharacterName);
            sb.Append(Environment.NewLine);
            if(cha.Gender == 0)
            {
                sb.Append(StringLibrary.GetString(10));
            }
            else
            {
                sb.Append(StringLibrary.GetString(11));
            }
            sb.Append(" ");
            sb.Append(cha.CharacterRace.Name);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(26));
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(27));
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(28));
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(31) + ": ");
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(33) + ": ");
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(32) + ": ");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(29));
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(30));

            _descriptionBox.Text = sb.ToString();
        }

        static void _characterList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                string ret = Mundasia.Communication.ServiceConsumer.CharacterDetails(e.Item.SubItems[0].Text);
                Creature ch = new Creature(ret);
                SetCharacterDetails(ch);
            }
        }

        static void _createCharacter_Click(object sender, EventArgs e)
        {
            Form host = _form;
            Clear(_form);
            CharacterCreationScreen.Set(_form);
        }

        static void _createCharacter_MouseLeave(object sender, EventArgs e)
        {
            _createCharacter.ForeColor = Color.White;
        }

        static void _createCharacter_MouseEnter(object sender, EventArgs e)
        {
            _createCharacter.ForeColor = Color.Yellow;
        }

        static void _selectCharacter_MouseLeave(object sender, EventArgs e)
        {
            _selectCharacter.ForeColor = Color.White;
        }

        static void _selectCharacter_MouseEnter(object sender, EventArgs e)
        {
            _selectCharacter.ForeColor = Color.Yellow;
        }

        static void _selectCharacter_Click(object sender, EventArgs e)
        {
            if(_characterList.SelectedItems.Count < 1)
            {
                return;
            }
            string result = Mundasia.Communication.ServiceConsumer.SelectCharacter(_characterList.SelectedItems[0].SubItems[0].Text);
            CharacterSelection ch = new CharacterSelection(result);
            if(ch.CentralCharacter == null || ch.visibleCharacters == null || ch.visibleTiles == null)
            {
                MessageBox.Show("An error has occured. This character selection has resulted in no scene to draw.");
                return;
            }
            Clear(_form);
            PlayerInterface.Set(_form, ch);
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);
        
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
            listView.Columns[0].Width = 0;
            listView.Columns[1].Width = listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth;
        }
       
        private static void StyleLabel(Control toStyle)
        {
            toStyle.Size = toStyle.PreferredSize;
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }

        static void _form_Resize(object sender, EventArgs e)
        {
            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);
            int halfHeight = (_form.ClientRectangle.Height / 2) - (padding * 2);

            _character.Size = new Size(width, halfHeight);
            _character.Location = new Point(padding, padding);
            _characterList.Size = new Size(_character.ClientRectangle.Size.Width, _character.ClientRectangle.Size.Height - _createCharacter.Height);

            _message.Size = new Size(width, halfHeight);
            _message.Location = new Point(padding, halfHeight + (padding * 3));

            _description.Size = new Size(width, height);
            _description.Location = new Point(width + (padding * 3), padding);

            _characterList.Size = _character.ClientRectangle.Size;
            _messageList.Size = _message.ClientRectangle.Size;
        }
    }
}
