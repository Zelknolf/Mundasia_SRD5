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

        private static int selectionHeight = 250;
        private static int padding = 5;
        private static int indent = 20;

        private static CharacterCreationScreen _panel = new CharacterCreationScreen();
        private static Form _form;

        private static Panel _characterSheet = new Panel();
        private static Panel _currentEntry = new Panel();
        private static Panel _description = new Panel();

        private static Label _characterName = new Label();
        private static Label _characterSexRace = new Label();

        private static Label _traitsHead = new Label();
        private static Label _characterVirtue = new Label();
        private static Label _characterVice = new Label();

        private static Label _moralsHead = new Label();
        private static Label _characterMoralsAuthority = new Label();
        private static Label _characterMoralsCare = new Label();
        private static Label _characterMoralsFairness = new Label();
        private static Label _characterMoralsLoyalty = new Label();
        private static Label _characterMoralsTradition = new Label();

        private static Label _abilityHead = new Label();
        private static Label _characterProfession = new Label();
        private static Label _characterTalent = new Label();
        private static Label _characterHobby = new Label();
        private static Label _characterAspiration = new Label();
        private static Label _characterAppearance = new Label();

        private static Label _next = new Label();
        private static Label _cancel = new Label();

        private static Label _nameEntryLabel = new Label();
        private static TextBox _nameEntry = new TextBox();

        private static Label _female = new Label();
        private static Label _male = new Label();

        private static Label _hairStyle = new Label();
        private static Label _hairColor = new Label();
        private static Label _skinColor = new Label();

        private static ListView _raceList = new ListView();

        private static ListView _virtueList = new ListView();
        private static ListView _viceList = new ListView();

        private static ListView _authorityList = new ListView();
        private static ListView _careList = new ListView();
        private static ListView _fairnessList = new ListView();
        private static ListView _loyaltyList = new ListView();
        private static ListView _traditionList = new ListView();

        private static ListView _professionList = new ListView();
        private static ListView _talentList = new ListView();
        private static ListView _hobbyList = new ListView();
        private static ListView _aspirationList = new ListView();

        private static RichTextBox _descriptionText = new RichTextBox();

        private static PlayScene _appearanceScene = new PlayScene();

        private static DisplayCharacter _displayCharacter = new DisplayCharacter();

        private static string _name;
        private static int _sex;
        private static int _race;
        private static int _traitVirtue;
        private static int _traitVice;
        private static int _moralsAuthority;
        private static int _moralsCare;
        private static int _moralsFairness;
        private static int _moralsLoyalty;
        private static int _moralsTradition;
        private static int _abilityProfession;
        private static int _abilityTalent;
        private static int _abilityHobby;
        private static int _abilityAspiration;
        private static bool _appearanceSeen = false;

        private static bool _eventsInitialized = false;

        public static void Set(Form primaryForm)
        {
            _name = "";
            _sex = -1;
            _race = -1;
            _traitVirtue = -1;
            _traitVice = -1;
            _moralsAuthority = -1;
            _moralsCare = -1;
            _moralsFairness = -1;
            _moralsLoyalty = -1;
            _moralsTradition = -1;
            _abilityProfession = -1;
            _abilityTalent = -1;
            _abilityHobby = -1;
            _abilityAspiration = -1;
            
            _form = primaryForm;
            _form.Resize += _form_Resize;

            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);

            Label derp = new Label();
            derp.ForeColor = Color.White;
            derp.BackColor = Color.Black;
            derp.Text = "*";
            derp.Size = derp.PreferredSize;

            _characterSheet.Height = height;
            _characterSheet.Width = width;
            _characterSheet.Location = new Point(padding, padding);
            _characterSheet.BorderStyle = BorderStyle.FixedSingle;
            _characterSheet.BackColor = Color.Black;
            _characterSheet.ForeColor = Color.White;

            _currentEntry.Height = selectionHeight;
            _currentEntry.Width = width;
            _currentEntry.Location = new Point(width + (3 * padding), padding);
            _currentEntry.BorderStyle = BorderStyle.FixedSingle;
            _currentEntry.BackColor = Color.Black;
            _currentEntry.ForeColor = Color.White;

            _description.Height = Math.Max(height - (2 * padding) - selectionHeight, 0);
            _description.Width = width;
            _description.Location = new Point(width + (3 * padding), selectionHeight + (3 * padding));
            _description.BorderStyle = BorderStyle.FixedSingle;
            _description.BackColor = Color.Black;
            _description.ForeColor = Color.White;

            _form.Controls.Add(_characterSheet);
            _form.Controls.Add(_currentEntry);
            _form.Controls.Add(_description);
            _description.Controls.Add(_descriptionText);

            if (!_eventsInitialized)
            {
                _nameEntry.TextChanged += _nameEntry_TextChanged;
                _characterName.Click += _characterName_Click;
                _characterSexRace.Click += _characterSexRace_Click;
                _characterAppearance.Click += _characterAppearance_Click;
                _hairStyle.Click += _hairStyle_Click;
                _hairColor.Click += _hairColor_Click;
                _skinColor.Click += _skinColor_Click;
            }

            _descriptionText.Text = " "; // RichTextBox wants to be 0x0 unless it has contents
            _descriptionText.Location = new Point(padding, padding);
            _descriptionText.ReadOnly = true;
            _descriptionText.BorderStyle = BorderStyle.None;
            _descriptionText.Multiline = true;
            _descriptionText.Height = Math.Max(height - (4 * padding) - selectionHeight, 0);
            _descriptionText.Width = width - (padding * 2);
            _descriptionText.WordWrap = true;

            _displayCharacter.Facing = Direction.South;
            
            StyleLabel(_descriptionText);

            UpdateCharacterSheet();
            SetUnusedToPanel();
        }

        static void _characterAppearance_Click(object sender, EventArgs e)
        {
            SetAppearanceToPanel();
        }

        static void _characterSexRace_Click(object sender, EventArgs e)
        {
            SetRaceSexToPanel();
        }

        static void _characterName_Click(object sender, EventArgs e)
        {
            SetNameToPanel();
        }

        static void _nameEntry_TextChanged(object sender, EventArgs e)
        {
            _name = _nameEntry.Text;
            if (!String.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                _characterName.Text = _nameEntry.Text;
                _characterName.Size = _characterName.PreferredSize;
            }
            else
            {
                _characterName.Text = "No name";
                _characterName.Size = _characterName.PreferredSize;
            }
        }

        public static void Clear(Form primaryForm)
        {
            _characterSheet.Controls.Clear();
            _currentEntry.Controls.Clear();
            _description.Controls.Clear();
            _form.Controls.Clear();
        }

        private static void UpdateCharacterSheet()
        {
            #region Define Text Contents of Labels
            if (_name == "") _characterName.Text = "No Name";
            else _characterName.Text = _name;

            UpdateRaceSexText();

            if (!_appearanceSeen) _characterAppearance.Text = "Appearance not set";
            else _characterAppearance.Text = "Appearance set";

            _next.Text = StringLibrary.GetString(13);
            _cancel.Text = StringLibrary.GetString(7);

            _traitsHead.Text = "Traits";
            _moralsHead.Text = "Morals";
            _abilityHead.Text = "Abilities";

            _hairColor.Text = "Hair color";
            StyleLabel(_hairColor);
            _hairColor.Size = _hairColor.PreferredSize;
            _hairStyle.Text = "Hair style";
            StyleLabel(_hairStyle);
            _hairStyle.Size = _hairStyle.PreferredSize;
            _skinColor.Text = "Skin color";
            StyleLabel(_skinColor);
            _skinColor.Size = _skinColor.PreferredSize;
            #endregion

            #region Formatting and Positioning
            StyleLabel(_characterName);
            _characterName.Location = new Point(padding, padding);
            StyleLabel(_characterSexRace);
            _characterSexRace.Location = new Point(padding, _characterName.Location.Y + _characterName.Height);

            StyleLabel(_traitsHead);
            _traitsHead.Location = new Point(padding, _characterSexRace.Location.Y + (_characterSexRace.Height * 2));
            StyleLabel(_characterVirtue);
            _characterVirtue.Location = new Point(padding + indent, _traitsHead.Location.Y + _traitsHead.Height);
            StyleLabel(_characterVice);
            _characterVice.Location = new Point(padding + indent, _characterVirtue.Location.Y + _characterVirtue.Height);

            StyleLabel(_moralsHead);
            _moralsHead.Location = new Point(padding, _characterVice.Location.Y + (_characterVice.Height * 2));
            StyleLabel(_characterMoralsAuthority);
            _characterMoralsAuthority.Location = new Point(padding + indent, _moralsHead.Location.Y + _moralsHead.Height);
            StyleLabel(_characterMoralsCare);
            _characterMoralsCare.Location = new Point(padding + indent, _characterMoralsAuthority.Location.Y + _characterMoralsAuthority.Height);
            StyleLabel(_characterMoralsFairness);
            _characterMoralsFairness.Location = new Point(padding + indent, _characterMoralsCare.Location.Y + _characterMoralsCare.Height);
            StyleLabel(_characterMoralsLoyalty);
            _characterMoralsLoyalty.Location = new Point(padding + indent, _characterMoralsFairness.Location.Y + _characterMoralsFairness.Height);
            StyleLabel(_characterMoralsTradition);
            _characterMoralsTradition.Location = new Point(padding + indent, _characterMoralsLoyalty.Location.Y + _characterMoralsLoyalty.Height);

            StyleLabel(_abilityHead);
            _abilityHead.Location = new Point(padding, _characterMoralsTradition.Location.Y + (_characterMoralsTradition.Height * 2));
            StyleLabel(_characterProfession);
            _characterProfession.Location = new Point(padding + indent, _abilityHead.Location.Y + _abilityHead.Height);
            StyleLabel(_characterTalent);
            _characterTalent.Location = new Point(padding + indent, _characterProfession.Location.Y + _characterProfession.Height);
            StyleLabel(_characterHobby);
            _characterHobby.Location = new Point(padding + indent, _characterTalent.Location.Y + _characterTalent.Height);
            StyleLabel(_characterAspiration);
            _characterAspiration.Location = new Point(padding + indent, _characterHobby.Location.Y + _characterHobby.Height);
            StyleLabel(_characterAppearance);
            _characterAppearance.Location = new Point(padding, _characterAspiration.Location.Y + _characterAspiration.Height + padding);
            StyleLabel(_next);
            _next.Location = new Point(_characterSheet.Width - _next.Width - padding, _characterSheet.Height - _next.Height - padding);
            StyleLabel(_cancel);
            _cancel.Location = new Point(_next.Location.X - _cancel.Width - padding, _next.Location.Y);
            #endregion

            if (!_eventsInitialized)
            {
                _characterName.MouseEnter += _clickableMouseOver;
                _characterSexRace.MouseEnter += _clickableMouseOver;
                _characterVirtue.MouseEnter += _clickableMouseOver;
                _characterVice.MouseEnter += _clickableMouseOver;
                _characterMoralsAuthority.MouseEnter += _clickableMouseOver;
                _characterMoralsCare.MouseEnter += _clickableMouseOver;
                _characterMoralsFairness.MouseEnter += _clickableMouseOver;
                _characterMoralsLoyalty.MouseEnter += _clickableMouseOver;
                _characterMoralsTradition.MouseEnter += _clickableMouseOver;
                _characterProfession.MouseEnter += _clickableMouseOver;
                _characterTalent.MouseEnter += _clickableMouseOver;
                _characterHobby.MouseEnter += _clickableMouseOver;
                _characterAspiration.MouseEnter += _clickableMouseOver;
                _characterAppearance.MouseEnter += _clickableMouseOver;
                _female.MouseEnter += _clickableMouseOver;
                _male.MouseEnter += _clickableMouseOver;
                _next.MouseEnter += _clickableMouseOver;
                _cancel.MouseEnter += _clickableMouseOver;
                _hairColor.MouseEnter += _clickableMouseOver;
                _hairStyle.MouseEnter += _clickableMouseOver;
                _skinColor.MouseEnter += _clickableMouseOver;

                _characterName.MouseLeave += _clickableMouseLeave;
                _characterSexRace.MouseLeave += _clickableMouseLeave;
                _characterVirtue.MouseLeave += _clickableMouseLeave;
                _characterVice.MouseLeave += _clickableMouseLeave;
                _characterMoralsAuthority.MouseLeave += _clickableMouseLeave;
                _characterMoralsCare.MouseLeave += _clickableMouseLeave;
                _characterMoralsFairness.MouseLeave += _clickableMouseLeave;
                _characterMoralsLoyalty.MouseLeave += _clickableMouseLeave;
                _characterMoralsTradition.MouseLeave += _clickableMouseLeave;
                _characterProfession.MouseLeave += _clickableMouseLeave;
                _characterTalent.MouseLeave += _clickableMouseLeave;
                _characterHobby.MouseLeave += _clickableMouseLeave;
                _characterAspiration.MouseLeave += _clickableMouseLeave;
                _characterAppearance.MouseLeave += _clickableMouseLeave;
                _female.MouseLeave += _clickableMouseLeave;
                _male.MouseLeave += _clickableMouseLeave;
                _next.MouseLeave += _clickableMouseLeave;
                _cancel.MouseLeave += _clickableMouseLeave;
                _hairColor.MouseLeave += _clickableMouseLeave;
                _hairStyle.MouseLeave += _clickableMouseLeave;
                _skinColor.MouseLeave += _clickableMouseLeave;

                _female.Click += _female_Click;
                _male.Click += _male_Click;
                _next.Click += _next_Click;
                _cancel.Click += _cancel_Click;

                _raceList.ItemSelectionChanged += _raceList_ItemSelectionChanged;
            }

            _characterSheet.Controls.Add(_characterName);
            _characterSheet.Controls.Add(_characterSexRace);
            _characterSheet.Controls.Add(_traitsHead);
            _characterSheet.Controls.Add(_characterVirtue);
            _characterSheet.Controls.Add(_characterVice);
            _characterSheet.Controls.Add(_moralsHead);
            _characterSheet.Controls.Add(_characterMoralsAuthority);
            _characterSheet.Controls.Add(_characterMoralsCare);
            _characterSheet.Controls.Add(_characterMoralsFairness);
            _characterSheet.Controls.Add(_characterMoralsLoyalty);
            _characterSheet.Controls.Add(_characterMoralsTradition);
            _characterSheet.Controls.Add(_abilityHead);
            _characterSheet.Controls.Add(_characterProfession);
            _characterSheet.Controls.Add(_characterTalent);
            _characterSheet.Controls.Add(_characterHobby);
            _characterSheet.Controls.Add(_characterAspiration);
            _characterSheet.Controls.Add(_characterAppearance);
            _characterSheet.Controls.Add(_next);
            _characterSheet.Controls.Add(_cancel);
            _eventsInitialized = true;
        }

        static void _cancel_Click(object sender, EventArgs e)
        {
            _name = "";
            _sex = -1;
            _race = -1;
            _traitVirtue = -1;
            _traitVice = -1;
            _moralsAuthority = -1;
            _moralsCare = -1;
            _moralsFairness = -1;
            _moralsLoyalty = -1;
            _moralsTradition = -1;
            _abilityProfession = -1;
            _abilityTalent = -1;
            _abilityHobby = -1;
            _abilityAspiration = -1;
            _displayCharacter.Hair = 0;
            _displayCharacter.HairColor = 0;
            _displayCharacter.SkinColor = 0;
            _appearanceSeen = false;
            Form host = _form;
            Clear(host);
            CharacterSelectScreen.Set(host);
        }

        static void _next_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(_name) ||
                _sex == -1 ||
                _race == -1 ||
                _traitVirtue == -1 ||
                _traitVice == -1 ||
                _moralsAuthority == -1 ||
                _moralsCare == -1 ||
                _moralsFairness == -1 ||
                _moralsLoyalty == -1 ||
                _moralsTradition == -1 ||
                _abilityProfession == -1 ||
                _abilityTalent == -1 ||
                _abilityHobby == -1 ||
                _abilityAspiration == -1 ||
                !_appearanceSeen)
            {
                SetUnusedToPanel();
                return;
            }

            string creat = Mundasia.Communication.ServiceConsumer.CreateCharacter(_name, 
                                                                _moralsAuthority, 
                                                                _moralsCare, 
                                                                _moralsFairness, 
                                                                _abilityHobby, 
                                                                _moralsLoyalty, 
                                                                _abilityProfession, 
                                                                _race, 
                                                                _sex, 
                                                                _abilityTalent, 
                                                                _moralsTradition,
                                                                _traitVice, 
                                                                _traitVirtue,
                                                                _abilityAspiration,
                                                                _displayCharacter.Hair,
                                                                _displayCharacter.HairColor,
                                                                _displayCharacter.SkinColor);
            if(!creat.Contains("Success"))
            {
                MessageBox.Show(creat);
            }
            else
            {
                Form host = _form;
                Clear(host);
                CharacterSelectScreen.Set(host);
            }
        }

        static void _skinColor_Click(object sender, EventArgs e)
        {
            _appearanceScene.Remove(_displayCharacter);
            _displayCharacter.SkinColor++;
            if (_displayCharacter.SkinColor >= Race.GetRace((uint)_race).SkinColors.Count)
            {
                _displayCharacter.SkinColor = 0;
            }
            _displayCharacter.CachedImage = null;
            _appearanceScene.Add(_displayCharacter);
        }

        static void _hairStyle_Click(object sender, EventArgs e)
        {
            _appearanceScene.Remove(_displayCharacter);
            _displayCharacter.Hair++;
            if (_displayCharacter.Hair > Race.GetRace((uint)_race).PlayableHairStyles[_sex])
            {
                _displayCharacter.Hair = 0;
            }
            _displayCharacter.CachedImage = null;
            _appearanceScene.Add(_displayCharacter);
        }

        static void _hairColor_Click(object sender, EventArgs e)
        {
            _appearanceScene.Remove(_displayCharacter);
            _displayCharacter.HairColor++;
            if (_displayCharacter.HairColor >= Race.GetRace((uint)_race).HairColors.Count)
            {
                _displayCharacter.HairColor = 0;
            }
            _displayCharacter.CachedImage = null;
            _appearanceScene.Add(_displayCharacter);
        }

        static void _raceList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                int nRace;
                if(int.TryParse(e.Item.SubItems[0].Text, out nRace))
                {
                    _race = nRace;
                    UpdateRaceSexText();
                    _descriptionText.Text = StringLibrary.GetString(Race.GetRace((uint)_race).Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                    _displayCharacter.Hair = 0;
                    _displayCharacter.HairColor = 0;
                    _displayCharacter.SkinColor = 0;
                    _displayCharacter.Height = Race.GetRace((uint)_race).Height;
                    ShowCharacterPreview();
                }
            }
        }

        static void UpdateRaceSexText()
        {
            if (_sex == 0) _characterSexRace.Text = StringLibrary.GetString(10);
            else if (_sex == 1) _characterSexRace.Text = StringLibrary.GetString(11);
            else _characterSexRace.Text = "No Sex";

            if (_race == -1) _characterSexRace.Text += " No Race";
            else _characterSexRace.Text += " " + Race.GetRace((uint)_race).Name;

            _characterSexRace.Size = _characterSexRace.PreferredSize;
            ShowCharacterPreview();
        }

        static void _male_Click(object sender, EventArgs e)
        {
            _sex = 1;
            _male.BorderStyle = BorderStyle.FixedSingle;
            _male.BackColor = Color.DarkGray;
            _female.BorderStyle = BorderStyle.None;
            _female.BackColor = Color.Black;
            _male.Size = _male.PreferredSize;
            _displayCharacter.Hair = 0;
            _displayCharacter.HairColor = 0;
            _displayCharacter.SkinColor = 0;
            UpdateRaceSexText();
            ShowCharacterPreview();
        }

        static void _female_Click(object sender, EventArgs e)
        {
            _sex = 0;
            _male.BorderStyle = BorderStyle.None;
            _male.BackColor = Color.Black;
            _female.BorderStyle = BorderStyle.FixedSingle;
            _female.BackColor = Color.DarkGray;
            _female.Size = _female.PreferredSize;
            _displayCharacter.Hair = 0;
            _displayCharacter.HairColor = 0;
            _displayCharacter.SkinColor = 0;
            UpdateRaceSexText();
            ShowCharacterPreview();
        }

        
        private static void SetUnusedToPanel()
        {
            if (_name == "") SetNameToPanel();
            else if (_sex == -1 || _race == -1) SetRaceSexToPanel();
            else if (!_appearanceSeen) SetAppearanceToPanel();
            ShowCharacterPreview();
        }

        private static void ClearOld()
        {
            List<Control> list = new List<Control>();
            foreach (Control c in _currentEntry.Controls)
            {
                list.Add(c);
            }
            // Can't modify a collection while looping through it.
            foreach (Control c in list)
            {
                _currentEntry.Controls.Remove(c);
            }
        }

        private static void SetNameToPanel()
        {
            ClearOld();

            StyleLabel(_nameEntryLabel);
            _nameEntryLabel.Text = StringLibrary.GetString(12);
            _nameEntryLabel.Size = _nameEntryLabel.PreferredSize;
            _nameEntryLabel.Location = new Point(padding, padding);

            _nameEntry.Font = labelFont;
            _nameEntry.Height = _currentEntry.PreferredSize.Height;
            _nameEntry.Width = _currentEntry.Width - (padding * 2);
            _nameEntry.Location = new Point(padding, _nameEntryLabel.Location.Y + _nameEntryLabel.Height + padding);
            _nameEntry.BackColor = Color.DarkGray;
            _nameEntry.ForeColor = Color.White;

            _currentEntry.Controls.Add(_nameEntryLabel);
            _currentEntry.Controls.Add(_nameEntry);
            _nameEntry.Focus();
            _descriptionText.Text = StringLibrary.GetString(14);
            _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
            _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
        }

        private static void SetRaceSexToPanel()
        {
            ClearOld();
            StyleLabel(_female);
            _female.Text = StringLibrary.GetString(10);
            _female.Size = _female.PreferredSize;
            _female.Location = new Point(padding, padding);
            if (_sex == 0)
            {
                _female.BorderStyle = BorderStyle.FixedSingle;
                _female.BackColor = Color.DarkGray;
            }
            else
            {
                _female.BorderStyle = BorderStyle.None;
                _female.BackColor = Color.Black;
            }

            StyleLabel(_male);
            _male.Text = StringLibrary.GetString(11);
            _male.Size = _male.PreferredSize;
            _male.Location = new Point((padding * 2) + _female.Location.X + _female.Width, padding);
            if (_sex == 1)
            {
                _male.BorderStyle = BorderStyle.FixedSingle;
                _male.BackColor = Color.DarkGray;
            }
            else
            {
                _male.BorderStyle = BorderStyle.None;
                _male.BackColor = Color.Black;
            }

            _raceList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 3) - _female.Height);
            StyleListView(_raceList);


            foreach(Race race in Race.GetRaces())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { race.Id.ToString(), race.Name });
                StyleListViewItem(toAdd);
                _raceList.Items.Add(toAdd);
            }
            _raceList.Location = new Point(padding, _female.Height + _female.Location.Y + padding);

            _currentEntry.Controls.Add(_male);
            _currentEntry.Controls.Add(_female);
            _currentEntry.Controls.Add(_raceList);

            _descriptionText.Text = StringLibrary.GetString(15);
        }

        private static void SetAppearanceToPanel()
        {
            ClearOld();

            if(_race < 0 || _sex < 0)
            {
                SetRaceSexToPanel();
                return;
            }
            _appearanceSeen = true;
            _characterAppearance.Text = "Appearance set";
            _characterAppearance.Size = _characterAppearance.PreferredSize;

            _appearanceScene.Size = new Size(_currentEntry.Width / 2 - (padding * 2), _currentEntry.Height - (padding * 2));
            _appearanceScene.Location = new Point(padding, padding);

            _skinColor.Location = new Point(_currentEntry.Width / 2 + (padding * 2), padding);
            _hairStyle.Location = new Point(_currentEntry.Width / 2 + (padding * 2), _skinColor.Location.Y + _skinColor.Height);
            _hairColor.Location = new Point(_currentEntry.Width / 2 + (padding * 2), _hairStyle.Location.Y + _hairStyle.Height);

            _appearanceScene.ViewCenterX = 0;
            _appearanceScene.ViewCenterY = 0;
            _appearanceScene.ViewCenterZ = 2;

            _appearanceScene.Remove(_displayCharacter);
            _displayCharacter.CharacterId = 1;
            _displayCharacter.CharacterRace = (uint)_race;
            _displayCharacter.Height = Race.GetRace((uint)_race).Height;
            _displayCharacter.Sex = _sex;
            _appearanceScene.Add(_displayCharacter);

            _currentEntry.Controls.Add(_appearanceScene);
            _currentEntry.Controls.Add(_skinColor);
            _currentEntry.Controls.Add(_hairStyle);
            _currentEntry.Controls.Add(_hairColor);
        }

        static void _clickableMouseOver(object sender, EventArgs e)
        {
            Control over = sender as Control;
            if (over == null) return;

            over.ForeColor = Color.Yellow;
        }

        static void _clickableMouseLeave(object sender, EventArgs e)
        {
            Control over = sender as Control;
            if (over == null) return;

            over.ForeColor = Color.White;
        }

        static void _form_Resize(object sender, EventArgs e)
        {
            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);

            _characterSheet.Height = height;
            _characterSheet.Width = width;
            _characterSheet.Location = new Point(padding, padding);

            _currentEntry.Height = selectionHeight;
            _currentEntry.Width = width;
            _currentEntry.Location = new Point(width + (3 * padding), padding);

            _description.Height = Math.Max(height - (2 * padding) - selectionHeight, 0);
            _description.Width = width;
            _description.Location = new Point(width + (3 * padding), selectionHeight + (3 * padding));

            _nameEntry.Width = _currentEntry.Width - (padding * 2);
            _raceList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 3) - _female.Height);

            _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
            _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        private static void StyleLabel(Control toStyle)
        {
            toStyle.Size = toStyle.PreferredSize;
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
            listView.Columns[0].Width = 0;
            listView.Columns[1].Width = listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth;
        }

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }

        private static CharacterPanel chPanel = null;

        private static void ShowCharacterPreview()
        {
            if(_race == -1 ||
                _abilityProfession == -1 ||
                _abilityHobby == -1 ||
                _abilityTalent == -1 ||
                _abilityAspiration == -1)
            {
                return;
            }
            Creature ch = new Creature();
            ch.Skills = new List<uint>();
            ch.CharacterName = _name;
            ch.CharacterRace = (uint)_race;
            ch.Sex = _sex;
            ch.HairColor = (uint)_displayCharacter.HairColor;
            ch.HairStyle = (uint)_displayCharacter.Hair;
            ch.SkinColor = (uint)_displayCharacter.SkinColor;

            Race r = Race.GetRace(ch.CharacterRace);

            if(chPanel != null)
            {
                chPanel.Close();
                chPanel.Dispose();
            }
            chPanel = new CharacterPanel(ch, true);
            chPanel.Show();
        }
    }
}
