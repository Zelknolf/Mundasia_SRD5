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

        #region Static Storage
        static Size MiniIconSize = new Size(10, 10);
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

        static Label pointsLeft = new Label();

        private static Image _iconNoGender;
        private static Image _iconMasculine;
        private static Image _iconFeminine;
        static Image IconNoGender
        {
            get
            {
                if (_iconNoGender != null) return _iconNoGender;
                _iconNoGender = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Gender\\no_gender.png");
                return _iconNoGender;
            }
        }
        static Image IconMasculine
        {
            get
            {
                if (_iconMasculine != null) return _iconMasculine;
                _iconMasculine = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Gender\\masculine.png");
                return _iconMasculine;
            }
        }
        static Image IconFeminine
        {
            get
            {
                if (_iconFeminine != null) return _iconFeminine;
                _iconFeminine = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Gender\\feminine.png");
                return _iconFeminine;
            }
        }

        private static Image _abilityScoreIcon;
        static Image IconAbilityScore
        {
            get
            {
                if (_abilityScoreIcon != null) return _abilityScoreIcon;
                _abilityScoreIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Chrome\\abilityScore.png");
                return _abilityScoreIcon;
            }
        }
        static Panel genderIcon = new Panel();
        static Label genderText = new Label();
        static Panel backgroundIcon = new Panel();
        static Label backgroundText = new Label();
        static Panel characterClassIcon = new Panel();
        static Label characterClassText = new Label();
        static Panel raceIcon = new Panel();
        static Label raceText = new Label();

        static Label labelStrength = new Label();
        static Label labelStrengthMod = new Label();
        static Label labelStrengthScore = new Label();
        static Panel abilityStrength = new Panel();

        static Label labelDexterity = new Label();
        static Label labelDexterityMod = new Label();
        static Label labelDexterityScore = new Label();
        static Panel abilityDexterity = new Panel();

        static Label labelConstitution = new Label();
        static Label labelConstitutionMod = new Label();
        static Label labelConstitutionScore = new Label();
        static Panel abilityConstitution = new Panel();

        static Label labelIntelligence = new Label();
        static Label labelIntelligenceMod = new Label();
        static Label labelIntelligenceScore = new Label();
        static Panel abilityIntelligence = new Panel();

        static Label labelWisdom = new Label();
        static Label labelWisdomMod = new Label();
        static Label labelWisdomScore = new Label();
        static Panel abilityWisdom = new Panel();

        static Label labelCharisma = new Label();
        static Label labelCharismaMod = new Label();
        static Label labelCharismaScore = new Label();
        static Panel abilityCharisma = new Panel();

        static Label strengthEditLabel = new Label();
        static NumericUpDown strengthEdit = new NumericUpDown();
        static Label dexterityEditLabel = new Label();
        static NumericUpDown dexterityEdit = new NumericUpDown();
        static Label constitutionEditLabel = new Label();
        static NumericUpDown constitutionEdit = new NumericUpDown();
        static Label intelligenceEditLabel = new Label();
        static NumericUpDown intelligenceEdit = new NumericUpDown();
        static Label wisdomEditLabel = new Label();
        static NumericUpDown wisdomEdit = new NumericUpDown();
        static Label charismaEditLabel = new Label();
        static NumericUpDown charismaEdit = new NumericUpDown();

        static ListView strengthSkills = new ListView();
        static ListView dexteritySkills = new ListView();
        static ListView constitutionSkills = new ListView();
        static ListView intelligenceSkills = new ListView();
        static ListView wisdomSkills = new ListView();
        static ListView charismaSkills = new ListView();

        static ListView genderBox = new ListView();
        static ListView backgroundBox = new ListView();
        static ListView characterClassBox = new ListView();
        static ListView raceBox = new ListView();

        static Label raceSkillsLabel = new Label();
        static ListView raceSkills = new ListView();
        static Label classSkillsLabel = new Label();
        static ListView classSkills = new ListView();
        static Label classToolsLabel = new Label();
        static ListView classTools = new ListView();

        private static bool _eventsInitialized = false;

        private static int _strengthScore = 6;
        private static int _dexterityScore = 6;
        private static int _constitutionScore = 6;
        private static int _intelligenceScore = 6;
        private static int _wisdomScore = 6;
        private static int _charismaScore = 6;

        private static Race _selectedRace;
        private static CharacterClass _selectedClass;
        private static CharacterClass _selectedSubClass;
        private static Background _selectedBackground;

        private static List<Skill> _raceSkills = new List<Skill>();
        private static List<Skill> _classSkills = new List<Skill>();
        private static List<Skill> _classTools = new List<Skill>();
        #endregion

        #region Initialization
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

            genderIcon.Size = new Size(64, 64);
            genderIcon.Location = new Point(padding, padding);
            genderIcon.BackgroundImage = IconNoGender;

            genderText.Text = "No sprite type selected";
            genderText.Location = new Point(genderIcon.Location.X + genderIcon.Width + padding, padding);
            genderText.TextAlign = ContentAlignment.MiddleCenter;
            StyleLabel(genderText);
            genderText.Size = new Size((_characterSheet.Width - (genderIcon.Width * 2) - (padding * 5)) / 2, 64);

            backgroundIcon.Size = new Size(64, 64);
            backgroundIcon.Location = new Point(genderText.Location.X + genderText.Size.Width + padding, padding);
            backgroundIcon.BackgroundImage = Background.NullBackgroundImage;

            backgroundText.Text = "No background selected";
            backgroundText.Location = new Point(backgroundIcon.Location.X + backgroundIcon.Size.Width + padding, padding);
            backgroundText.TextAlign = ContentAlignment.MiddleCenter;
            StyleLabel(backgroundText);
            backgroundText.Size = genderText.Size;

            raceIcon.Size = new Size(64, 64);
            raceIcon.Location = new Point(padding, genderIcon.Location.Y + genderIcon.Size.Height + padding);
            raceIcon.BackgroundImage = Race.NullRaceImage;

            raceText.Text = "No Race Selected";
            raceText.Location = new Point(raceIcon.Location.X + raceIcon.Width + padding, raceIcon.Location.Y);
            raceText.TextAlign = ContentAlignment.MiddleCenter;
            StyleLabel(raceText);
            raceText.Size = genderText.Size;

            characterClassIcon.Size = new Size(64, 64);
            characterClassIcon.Location = new Point(raceText.Location.X + raceText.Width + padding, raceIcon.Location.Y);
            characterClassIcon.BackgroundImage = CharacterClass.NullClassImage;

            characterClassText.Text = "No Class Selected";
            characterClassText.Location = new Point(characterClassIcon.Location.X + characterClassIcon.Width + padding, raceIcon.Location.Y);
            characterClassText.TextAlign = ContentAlignment.MiddleCenter;
            StyleLabel(characterClassText);
            characterClassText.Size = raceText.Size;

            // strength
            abilityStrength.Size = new Size(64, 64);
            abilityStrength.BackgroundImage = IconAbilityScore;
            labelStrength.Text = "STR";
            labelStrength.Location = new Point(padding, raceIcon.Location.Y + raceIcon.Size.Height + padding);
            StyleLabel(labelStrength);
            labelStrength.Width = abilityStrength.Width;
            labelStrength.TextAlign = ContentAlignment.MiddleCenter;
            abilityStrength.Location = new Point(padding, labelStrength.Location.Y + labelStrength.Size.Height);

            labelStrengthMod.Text = "-2";
            StyleAbilityMod(labelStrengthMod);
            abilityStrength.Controls.Add(labelStrengthMod);

            labelStrengthScore.Text = "6";
            StyleAbilityScore(labelStrengthScore);
            abilityStrength.Controls.Add(labelStrengthScore);

            strengthSkills.Location = new Point(abilityStrength.Width + padding * 2, labelStrength.Location.Y + padding);
            strengthSkills.Size = new Size(_characterSheet.ClientRectangle.Width - abilityStrength.Width - padding * 3, abilityStrength.Height + labelStrengthScore.Height);
            strengthSkills.ItemSelectionChanged += NoSelection;
            StyleListView(strengthSkills, true);

            // dexterity
            abilityDexterity.Size = new Size(64, 64);
            abilityDexterity.BackgroundImage = IconAbilityScore;
            labelDexterity.Text = "DEX";
            labelDexterity.Location = new Point(padding, abilityStrength.Location.Y + abilityStrength.Size.Height + padding);
            StyleLabel(labelDexterity);
            labelDexterity.Width = abilityDexterity.Width;
            labelDexterity.TextAlign = ContentAlignment.MiddleCenter;
            abilityDexterity.Location = new Point(padding, labelDexterity.Location.Y + labelDexterity.Size.Height);

            labelDexterityMod.Text = "-2";
            StyleAbilityMod(labelDexterityMod);
            abilityDexterity.Controls.Add(labelDexterityMod);

            labelDexterityScore.Text = "6";
            StyleAbilityScore(labelDexterityScore);
            abilityDexterity.Controls.Add(labelDexterityScore);

            dexteritySkills.Location = new Point(abilityDexterity.Width + padding * 2, labelDexterity.Location.Y + padding);
            dexteritySkills.Size = new Size(_characterSheet.ClientRectangle.Width - abilityDexterity.Width - padding * 3, abilityDexterity.Height + labelDexterityScore.Height);
            dexteritySkills.ItemSelectionChanged += NoSelection;
            StyleListView(dexteritySkills, true);

            // constitution
            abilityConstitution.Size = new Size(64, 64);
            abilityConstitution.BackgroundImage = IconAbilityScore;
            labelConstitution.Text = "CON";
            labelConstitution.Location = new Point(padding, abilityDexterity.Location.Y + abilityDexterity.Size.Height + padding);
            StyleLabel(labelConstitution);
            labelConstitution.Width = abilityConstitution.Width;
            labelConstitution.TextAlign = ContentAlignment.MiddleCenter;
            abilityConstitution.Location = new Point(padding, labelConstitution.Location.Y + labelConstitution.Size.Height);

            labelConstitutionMod.Text = "-2";
            StyleAbilityMod(labelConstitutionMod);
            abilityConstitution.Controls.Add(labelConstitutionMod);

            labelConstitutionScore.Text = "6";
            StyleAbilityScore(labelConstitutionScore);
            abilityConstitution.Controls.Add(labelConstitutionScore);

            constitutionSkills.Location = new Point(abilityConstitution.Width + padding * 2, labelConstitution.Location.Y + padding);
            constitutionSkills.Size = new Size(_characterSheet.ClientRectangle.Width - abilityConstitution.Width - padding * 3, abilityConstitution.Height + labelConstitutionScore.Height);
            constitutionSkills.ItemSelectionChanged += NoSelection;
            StyleListView(constitutionSkills, true);

            // intelligence
            abilityIntelligence.Size = new Size(64, 64);
            abilityIntelligence.BackgroundImage = IconAbilityScore;
            labelIntelligence.Text = "INT";
            labelIntelligence.Location = new Point(padding, abilityConstitution.Location.Y + abilityConstitution.Size.Height + padding);
            StyleLabel(labelIntelligence);
            labelIntelligence.Width = abilityIntelligence.Width;
            labelIntelligence.TextAlign = ContentAlignment.MiddleCenter;
            abilityIntelligence.Location = new Point(padding, labelIntelligence.Location.Y + labelIntelligence.Size.Height);

            labelIntelligenceMod.Text = "-2";
            StyleAbilityMod(labelIntelligenceMod);
            abilityIntelligence.Controls.Add(labelIntelligenceMod);

            labelIntelligenceScore.Text = "6";
            StyleAbilityScore(labelIntelligenceScore);
            abilityIntelligence.Controls.Add(labelIntelligenceScore);

            intelligenceSkills.Location = new Point(abilityIntelligence.Width + padding * 2, labelIntelligence.Location.Y + padding);
            intelligenceSkills.Size = new Size(_characterSheet.ClientRectangle.Width - abilityIntelligence.Width - padding * 3, abilityIntelligence.Height + labelIntelligenceScore.Height);
            intelligenceSkills.ItemSelectionChanged += NoSelection;
            StyleListView(intelligenceSkills, true);

            // wisdom
            abilityWisdom.Size = new Size(64, 64);
            abilityWisdom.BackgroundImage = IconAbilityScore;
            labelWisdom.Text = "WIS";
            labelWisdom.Location = new Point(padding, abilityIntelligence.Location.Y + abilityIntelligence.Size.Height + padding);
            StyleLabel(labelWisdom);
            labelWisdom.Width = abilityWisdom.Width;
            labelWisdom.TextAlign = ContentAlignment.MiddleCenter;
            abilityWisdom.Location = new Point(padding, labelWisdom.Location.Y + labelWisdom.Size.Height);

            labelWisdomMod.Text = "-2";
            StyleAbilityMod(labelWisdomMod);
            abilityWisdom.Controls.Add(labelWisdomMod);

            labelWisdomScore.Text = "6";
            StyleAbilityScore(labelWisdomScore);
            abilityWisdom.Controls.Add(labelWisdomScore);

            wisdomSkills.Location = new Point(abilityWisdom.Width + padding * 2, labelWisdom.Location.Y + padding);
            wisdomSkills.Size = new Size(_characterSheet.ClientRectangle.Width - abilityWisdom.Width - padding * 3, abilityWisdom.Height + labelWisdomScore.Height);
            wisdomSkills.ItemSelectionChanged += NoSelection;
            StyleListView(wisdomSkills, true);

            // charisma
            abilityCharisma.Size = new Size(64, 64);
            abilityCharisma.BackgroundImage = IconAbilityScore;
            labelCharisma.Text = "CHA";
            labelCharisma.Location = new Point(padding, abilityWisdom.Location.Y + abilityWisdom.Size.Height + padding);
            StyleLabel(labelCharisma);
            labelCharisma.Width = abilityCharisma.Width;
            labelCharisma.TextAlign = ContentAlignment.MiddleCenter;
            abilityCharisma.Location = new Point(padding, labelCharisma.Location.Y + labelCharisma.Size.Height);

            labelCharismaMod.Text = "-2";
            StyleAbilityMod(labelCharismaMod);
            abilityCharisma.Controls.Add(labelCharismaMod);

            labelCharismaScore.Text = "6";
            StyleAbilityScore(labelCharismaScore);
            abilityCharisma.Controls.Add(labelCharismaScore);

            charismaSkills.Location = new Point(abilityCharisma.Width + padding * 2, labelCharisma.Location.Y + padding);
            charismaSkills.Size = new Size(_characterSheet.ClientRectangle.Width - abilityCharisma.Width - padding * 3, abilityCharisma.Height + labelCharismaScore.Height);
            charismaSkills.ItemSelectionChanged += NoSelection;
            StyleListView(charismaSkills, true);

            if (!_eventsInitialized)
            {
                genderIcon.Click += EditGender;
                genderText.Click += EditGender;
                genderBox.ItemSelectionChanged += GenderBox_ItemSelectionChanged;
                backgroundIcon.Click += EditBackground;
                backgroundText.Click += EditBackground;
                backgroundBox.ItemSelectionChanged += BackgroundBox_ItemSelectionChanged;
                raceIcon.Click += EditRace;
                raceText.Click += EditRace;
                raceBox.ItemSelectionChanged += RaceBox_ItemSelectionChanged;
                characterClassIcon.Click += EditCharacterClass;
                characterClassText.Click += EditCharacterClass;
                characterClassBox.ItemSelectionChanged += CharacterClassBox_ItemSelectionChanged;
                labelStrength.Click += EditAbilityScores;
                labelStrengthMod.Click += EditAbilityScores;
                labelStrengthScore.Click += EditAbilityScores;
                abilityStrength.Click += EditAbilityScores;
                labelDexterity.Click += EditAbilityScores;
                labelDexterityMod.Click += EditAbilityScores;
                labelDexterityScore.Click += EditAbilityScores;
                abilityDexterity.Click += EditAbilityScores;
                labelConstitution.Click += EditAbilityScores;
                labelConstitutionMod.Click += EditAbilityScores;
                labelConstitutionScore.Click += EditAbilityScores;
                abilityConstitution.Click += EditAbilityScores;
                labelIntelligence.Click += EditAbilityScores;
                labelIntelligenceMod.Click += EditAbilityScores;
                labelIntelligenceScore.Click += EditAbilityScores;
                abilityIntelligence.Click += EditAbilityScores;
                labelWisdom.Click += EditAbilityScores;
                labelWisdomMod.Click += EditAbilityScores;
                labelWisdomScore.Click += EditAbilityScores;
                abilityWisdom.Click += EditAbilityScores;
                labelCharisma.Click += EditAbilityScores;
                labelCharismaMod.Click += EditAbilityScores;
                labelCharismaScore.Click += EditAbilityScores;
                abilityCharisma.Click += EditAbilityScores;

                strengthEdit.Value = _strengthScore;
                dexterityEdit.Value = _dexterityScore;
                constitutionEdit.Value = _constitutionScore;
                intelligenceEdit.Value = _intelligenceScore;
                wisdomEdit.Value = _wisdomScore;
                charismaEdit.Value = _charismaScore;

                strengthEdit.ValueChanged += AfterAbilityEdit;
                dexterityEdit.ValueChanged += AfterAbilityEdit;
                constitutionEdit.ValueChanged += AfterAbilityEdit;
                intelligenceEdit.ValueChanged += AfterAbilityEdit;
                wisdomEdit.ValueChanged += AfterAbilityEdit;
                charismaEdit.ValueChanged += AfterAbilityEdit;

                strengthSkills.MouseDown += EditSkills;
                dexteritySkills.MouseDown += EditSkills;
                constitutionSkills.MouseDown += EditSkills;
                intelligenceSkills.MouseDown += EditSkills;
                wisdomSkills.MouseDown += EditSkills;
                charismaSkills.MouseDown += EditSkills;

                raceSkills.ItemSelectionChanged += ToggleRaceSkill;
                classSkills.ItemSelectionChanged += ToggleClassSkill;
                classTools.ItemSelectionChanged += ToggleClassTool;
                _eventsInitialized = true;
            }

            _characterSheet.Controls.Add(genderIcon);
            _characterSheet.Controls.Add(genderText);
            _characterSheet.Controls.Add(backgroundIcon);
            _characterSheet.Controls.Add(backgroundText);
            _characterSheet.Controls.Add(raceIcon);
            _characterSheet.Controls.Add(raceText);
            _characterSheet.Controls.Add(characterClassIcon);
            _characterSheet.Controls.Add(characterClassText);
            _characterSheet.Controls.Add(labelStrength);
            _characterSheet.Controls.Add(abilityStrength);
            _characterSheet.Controls.Add(strengthSkills);
            _characterSheet.Controls.Add(labelDexterity);
            _characterSheet.Controls.Add(abilityDexterity);
            _characterSheet.Controls.Add(dexteritySkills);
            _characterSheet.Controls.Add(labelConstitution);
            _characterSheet.Controls.Add(abilityConstitution);
            _characterSheet.Controls.Add(constitutionSkills);
            _characterSheet.Controls.Add(labelIntelligence);
            _characterSheet.Controls.Add(abilityIntelligence);
            _characterSheet.Controls.Add(intelligenceSkills);
            _characterSheet.Controls.Add(labelWisdom);
            _characterSheet.Controls.Add(abilityWisdom);
            _characterSheet.Controls.Add(wisdomSkills);
            _characterSheet.Controls.Add(labelCharisma);
            _characterSheet.Controls.Add(abilityCharisma);
            _characterSheet.Controls.Add(charismaSkills);

            _panel.Controls.Add(_characterSheet);
            _panel.Controls.Add(_editPanel);
            _form.Controls.Add(_panel);

            UpdateSkills();
        }

        public static void Clear()
        {
            _form.Resize -= _form_Resize;
            _form.Controls.Remove(_panel);
        }

        private static void NoSelection(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }
        #endregion

        #region Gender Selection
        private static void GenderBox_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                genderText.Text = e.Item.Name;
                genderIcon.BackgroundImage = (int)e.Item.Tag == 0 ? IconFeminine : IconMasculine;

                _populateGenderList();
            }
        }

        public static void EditGender(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Gender) return;

            _editPanel.Controls.Clear();

            genderBox.Height = _editPanel.Height - (padding * 2);
            genderBox.Width = _editPanel.Width - (padding * 2);
            genderBox.Location = new Point(padding, padding);
            StyleListView(genderBox, false);

            _populateGenderList();

            _editPanel.Controls.Add(genderBox);

            _currentEdit = CurrentEdit.Gender;
        }

        private static void _populateGenderList()
        {
            genderBox.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            ListViewItem toAdd = new ListViewItem(new string[] { "", "Feminine Sprite" });
            toAdd.Name = "Feminine Sprite";
            toAdd.ImageIndex = 0;
            toAdd.Tag = 0;
            imgs.Images.Add(IconFeminine);
            StyleListViewItem(toAdd);
            genderBox.Items.Add(toAdd);

            toAdd = new ListViewItem(new string[] { "", "Masculine Sprite" });
            toAdd.Name = "Masculine Sprite";
            toAdd.ImageIndex = 1;
            toAdd.Tag = 1;
            imgs.Images.Add(IconMasculine);
            StyleListViewItem(toAdd);
            genderBox.Items.Add(toAdd);

            genderBox.SmallImageList = imgs;
        }
        #endregion

        #region Background Selection
        private static void BackgroundBox_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                Background background = Background.GetBackground((uint)e.Item.Tag);
                if(background != null)
                {
                    backgroundText.Text = e.Item.Name;
                    backgroundIcon.BackgroundImage = background.Icon;

                    _selectedBackground = background;

                    _populateBackgroundList();
                    UpdateSkills();
                }
            }
        }

        public static void EditBackground(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Background) return;

            _editPanel.Controls.Clear();

            backgroundBox.Height = _editPanel.Height - (padding * 2);
            backgroundBox.Width = _editPanel.Width - (padding * 2);
            backgroundBox.Location = new Point(padding, padding);
            StyleListView(backgroundBox, false);

            _populateBackgroundList();

            _editPanel.Controls.Add(backgroundBox);

            _currentEdit = CurrentEdit.Background;
        }

        private static void _populateBackgroundList()
        {
            uint topItemIndex = uint.MaxValue;
            if (backgroundBox.TopItem != null)
            {
                topItemIndex = (uint)backgroundBox.TopItem.Tag;
            }
            ListViewItem topItem = null;
            backgroundBox.BeginUpdate();
            backgroundBox.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = IconSize;
            imgs.ColorDepth = ColorDepth.Depth32Bit;

            int imageIndex = 0;

            foreach (Background bkgrd in Background.GetBackgrounds())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { "", bkgrd.Name });
                toAdd.Name = bkgrd.Name;
                toAdd.ImageIndex = imageIndex;
                toAdd.Tag = bkgrd.Id;
                toAdd.ToolTipText = StringLibrary.GetString(bkgrd.Description);
                imgs.Images.Add(bkgrd.Icon);
                imageIndex++;
                StyleListViewItem(toAdd);
                if (bkgrd == _selectedBackground) toAdd.BackColor = SelectedRowColor;
                backgroundBox.Items.Add(toAdd);
                if (bkgrd.Id == topItemIndex) topItem = toAdd;
            }
            backgroundBox.SmallImageList = imgs;
            backgroundBox.EndUpdate();
            if(topItem != null)
            {
                backgroundBox.TopItem = topItem;
            }
        }
        #endregion

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
                    _selectedRace = selectedRace;
                    _populateRaceList();
                }
                UpdateAbilityScores();
                UpdateSkills();
            }
        }

        public static void EditRace(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Race) return;

            _editPanel.Controls.Clear();

            raceBox.Height = _editPanel.Height - (padding * 2);
            raceBox.Width = _editPanel.Width - (padding * 2);
            raceBox.Location = new Point(padding, padding);
            StyleListView(raceBox, false);

            _populateRaceList();

            _editPanel.Controls.Add(raceBox);

            _currentEdit = CurrentEdit.Race;
        }

        private static void _populateRaceList()
        {
            uint topItemIndex = uint.MaxValue;
            if (raceBox.TopItem != null)
            {
                topItemIndex = (uint)raceBox.TopItem.Tag;
            }
            ListViewItem topItem = null;
            raceBox.BeginUpdate();
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
                if (race == _selectedRace) toAdd.BackColor = SelectedRowColor;
                raceBox.Items.Add(toAdd);
                if (topItemIndex == race.Id) topItem = toAdd;
            }
            raceBox.SmallImageList = imgs;
            raceBox.EndUpdate();
            if(topItem != null) raceBox.TopItem = topItem;
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

                    if (selectedClass.HitDie == 0)
                    {
                        _selectedSubClass = selectedClass;
                    }
                    else
                    {
                        _selectedClass = selectedClass;
                        UpdateSkills();
                    }
                    if (selectedClass.SubClassLevel == 1)
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
            StyleListView(characterClassBox, false);

            _populateCharacterClassList();

            _editPanel.Controls.Add(characterClassBox);

            _currentEdit = CurrentEdit.Class;
        }

        private static void _populateCharacterClassList()
        {
            uint topItemIndex = uint.MaxValue;
            if (characterClassBox.TopItem != null)
            {
                topItemIndex = (uint)characterClassBox.TopItem.Tag;
            }
            ListViewItem topItem = null;
            characterClassBox.BeginUpdate();
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
                    if (cl == _selectedClass) toAdd.BackColor = SelectedRowColor;
                    characterClassBox.Items.Add(toAdd);
                    if (topItemIndex == cl.Id) topItem = toAdd;
                }
            }
            characterClassBox.SmallImageList = imgs;
            characterClassBox.EndUpdate();
            if (topItem != null) characterClassBox.TopItem = topItem;
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

        #region Ability Scores
        private static void EditAbilityScores(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.AbilityScores) return;

            _editPanel.Controls.Clear();

            strengthEditLabel.Text = "Strength";
            dexterityEditLabel.Text = "Dexterity";
            constitutionEditLabel.Text = "Constitution";
            intelligenceEditLabel.Text = "Intelligence";
            wisdomEditLabel.Text = "Wisdom";
            charismaEditLabel.Text = "Charisma";

            StyleLabel(strengthEditLabel);         
            StyleLabel(dexterityEditLabel);
            StyleLabel(constitutionEditLabel);
            StyleLabel(intelligenceEditLabel);
            StyleLabel(wisdomEditLabel);
            StyleLabel(charismaEditLabel);

            strengthEditLabel.TextAlign = ContentAlignment.MiddleLeft;
            dexterityEditLabel.TextAlign = ContentAlignment.MiddleLeft;
            constitutionEditLabel.TextAlign = ContentAlignment.MiddleLeft;
            intelligenceEditLabel.TextAlign = ContentAlignment.MiddleLeft;
            wisdomEditLabel.TextAlign = ContentAlignment.MiddleLeft;
            charismaEditLabel.TextAlign = ContentAlignment.MiddleLeft;

            int width = Math.Max(Math.Max(Math.Max(strengthEditLabel.Width, dexterityEditLabel.Width), Math.Max(constitutionEditLabel.Width, intelligenceEditLabel.Width)), Math.Max(wisdomEditLabel.Width, charismaEditLabel.Width));
            strengthEditLabel.Width = width;
            dexterityEditLabel.Width = width;
            constitutionEditLabel.Width = width;
            intelligenceEditLabel.Width = width;
            wisdomEditLabel.Width = width;
            charismaEditLabel.Width = width;

            strengthEdit.Minimum = 3;
            dexterityEdit.Minimum = 3;
            constitutionEdit.Minimum = 3;
            intelligenceEdit.Minimum = 3;
            wisdomEdit.Minimum = 3;
            charismaEdit.Minimum = 3;

            strengthEdit.Maximum = 16;
            dexterityEdit.Maximum = 16;
            constitutionEdit.Maximum = 16;
            intelligenceEdit.Maximum = 16;
            wisdomEdit.Maximum = 16;
            charismaEdit.Maximum = 16;

            StyleLabel(strengthEdit);
            StyleLabel(dexterityEdit);
            StyleLabel(constitutionEdit);
            StyleLabel(intelligenceEdit);
            StyleLabel(wisdomEdit);
            StyleLabel(charismaEdit);

            strengthEditLabel.Height = strengthEdit.Height;
            dexterityEditLabel.Height = dexterityEdit.Height;
            constitutionEditLabel.Height = constitutionEdit.Height;
            intelligenceEditLabel.Height = intelligenceEdit.Height;
            wisdomEditLabel.Height = wisdomEdit.Height;
            charismaEditLabel.Height = charismaEdit.Height;

            strengthEditLabel.Location = new Point(padding, padding);
            dexterityEditLabel.Location = new Point(padding, strengthEditLabel.Location.Y + strengthEditLabel.Height + padding);
            constitutionEditLabel.Location = new Point(padding, dexterityEditLabel.Location.Y + dexterityEditLabel.Height + padding);
            intelligenceEditLabel.Location = new Point(padding, constitutionEditLabel.Location.Y + constitutionEditLabel.Height + padding);
            wisdomEditLabel.Location = new Point(padding, intelligenceEditLabel.Location.Y + intelligenceEditLabel.Height + padding);
            charismaEditLabel.Location = new Point(padding, wisdomEditLabel.Location.Y + wisdomEditLabel.Height + padding);

            strengthEdit.Location = new Point(strengthEditLabel.Width + padding * 2, strengthEditLabel.Location.Y);
            dexterityEdit.Location = new Point(dexterityEditLabel.Width + padding * 2, dexterityEditLabel.Location.Y);
            constitutionEdit.Location = new Point(constitutionEditLabel.Width + padding * 2, constitutionEditLabel.Location.Y);
            intelligenceEdit.Location = new Point(intelligenceEditLabel.Width + padding * 2, intelligenceEditLabel.Location.Y);
            wisdomEdit.Location = new Point(wisdomEditLabel.Width + padding * 2, wisdomEditLabel.Location.Y);
            charismaEdit.Location = new Point(charismaEditLabel.Width + padding * 2, charismaEditLabel.Location.Y);

            pointsLeft.Location = new Point(padding * 3, charismaEditLabel.Location.Y + charismaEditLabel.Height + padding);
            StyleLabel(pointsLeft);
            AfterAbilityEdit(sender, e);

            _editPanel.Controls.Add(strengthEditLabel);
            _editPanel.Controls.Add(dexterityEditLabel);
            _editPanel.Controls.Add(constitutionEditLabel);
            _editPanel.Controls.Add(intelligenceEditLabel);
            _editPanel.Controls.Add(wisdomEditLabel);
            _editPanel.Controls.Add(charismaEditLabel);

            _editPanel.Controls.Add(strengthEdit);
            _editPanel.Controls.Add(dexterityEdit);
            _editPanel.Controls.Add(constitutionEdit);
            _editPanel.Controls.Add(intelligenceEdit);
            _editPanel.Controls.Add(wisdomEdit);
            _editPanel.Controls.Add(charismaEdit);

            _editPanel.Controls.Add(pointsLeft);

            _currentEdit = CurrentEdit.AbilityScores;
        }

        private static void AfterAbilityEdit(object sender, EventArgs e)
        {
            _strengthScore = (int)strengthEdit.Value;
            _dexterityScore = (int)dexterityEdit.Value;
            _constitutionScore = (int)constitutionEdit.Value;
            _intelligenceScore = (int)intelligenceEdit.Value;
            _wisdomScore = (int)wisdomEdit.Value;
            _charismaScore = (int)charismaEdit.Value;

            int _strengthCostAdjust = 0;
            int _dexterityCostAdjust = 0;
            int _constitutionCostAdjust = 0;
            int _wisdomCostAdjust = 0;
            int _intelligenceCostAdjust = 0;
            int _charismaCostAdjust = 0;
            if (_selectedRace != null &&
                _selectedRace.OtherBonusStats > 0)
            {
                int bonuses = _selectedRace.OtherBonusStats;
                List<int> scores = new List<int>();
                if (_selectedRace.Strength == 0) scores.Add(_strengthScore);
                if (_selectedRace.Dexterity == 0) scores.Add(_dexterityScore);
                if (_selectedRace.Constitution == 0) scores.Add(_constitutionScore);
                if (_selectedRace.Intelligence == 0) scores.Add(_intelligenceScore);
                if (_selectedRace.Wisdom == 0) scores.Add(_wisdomScore);
                if (_selectedRace.Charisma == 0) scores.Add(_charismaScore);

                scores.Sort();
                while(bonuses > 0)
                {
                    if(_strengthScore == scores[scores.Count - bonuses] &&
                        _strengthCostAdjust == 0)
                    {
                        _strengthCostAdjust--;
                    }
                    else if(_dexterityScore == scores[scores.Count - bonuses] &&
                        _dexterityCostAdjust == 0)
                    {
                        _dexterityCostAdjust--;
                    }
                    else if(_constitutionScore == scores[scores.Count - bonuses] &&
                        _constitutionCostAdjust == 0)
                    {
                        _constitutionCostAdjust--;
                    }
                    else if(_intelligenceScore == scores[scores.Count - bonuses] &&
                        _intelligenceCostAdjust == 0)
                    {
                        _intelligenceCostAdjust--;
                    }
                    else if(_wisdomScore == scores[scores.Count - bonuses] &&
                        _wisdomCostAdjust == 0)
                    {
                        _wisdomCostAdjust--;
                    }
                    else if(_charismaScore == scores[scores.Count - bonuses] &&
                        _charismaCostAdjust == 0)
                    {
                        _charismaCostAdjust--;
                    }
                    bonuses--;
                }
            }


            int points = 40 - 
                        AbilityScoreCosts[_strengthScore + _strengthCostAdjust] - 
                        AbilityScoreCosts[_dexterityScore + _dexterityCostAdjust] - 
                        AbilityScoreCosts[_constitutionScore + _constitutionCostAdjust] - 
                        AbilityScoreCosts[_intelligenceScore + _intelligenceCostAdjust] - 
                        AbilityScoreCosts[_wisdomScore + _wisdomCostAdjust] - 
                        AbilityScoreCosts[_charismaScore + _charismaCostAdjust];

            pointsLeft.Text = points.ToString() + " points remain";
            pointsLeft.Size = pointsLeft.PreferredSize;

            UpdateAbilityScores();
            UpdateSkills();
        }

        private static void UpdateAbilityScores()
        {
            if (_selectedRace != null)
            {
                labelStrengthScore.Text = (_strengthScore + _selectedRace.Strength).ToString();
                if ((_strengthScore + _selectedRace.Strength) >= 10)
                {
                    labelStrengthMod.Text = "+" + ((_strengthScore + _selectedRace.Strength - 10) / 2).ToString();
                }
                else
                {
                    labelStrengthMod.Text = ((_strengthScore + _selectedRace.Strength - 11) / 2).ToString();
                }
                labelDexterityScore.Text = (_dexterityScore + _selectedRace.Dexterity).ToString();
                if ((_dexterityScore + _selectedRace.Dexterity) >= 10)
                {
                    labelDexterityMod.Text = "+" + ((_dexterityScore + _selectedRace.Dexterity - 10) / 2).ToString();
                }
                else
                {
                    labelDexterityMod.Text = ((_dexterityScore + _selectedRace.Dexterity - 11) / 2).ToString();
                }
                labelConstitutionScore.Text = (_constitutionScore + _selectedRace.Constitution).ToString();
                if ((_constitutionScore + _selectedRace.Constitution) >= 10)
                {
                    labelConstitutionMod.Text = "+" + ((_constitutionScore + _selectedRace.Constitution - 10) / 2).ToString();
                }
                else
                {
                    labelConstitutionMod.Text = ((_constitutionScore + _selectedRace.Constitution - 11) / 2).ToString();
                }
                labelIntelligenceScore.Text = (_intelligenceScore + _selectedRace.Intelligence).ToString();
                if ((_intelligenceScore + _selectedRace.Intelligence) >= 10)
                {
                    labelIntelligenceMod.Text = "+" + ((_intelligenceScore + _selectedRace.Intelligence - 10) / 2).ToString();
                }
                else
                {
                    labelIntelligenceMod.Text = ((_intelligenceScore + _selectedRace.Intelligence - 11) / 2).ToString();
                }
                labelWisdomScore.Text = (_wisdomScore + _selectedRace.Wisdom).ToString();
                if ((_wisdomScore + _selectedRace.Wisdom) >= 10)
                {
                    labelWisdomMod.Text = "+" + ((_wisdomScore + _selectedRace.Wisdom - 10) / 2).ToString();
                }
                else
                {
                    labelWisdomMod.Text = ((_wisdomScore + _selectedRace.Wisdom - 11) / 2).ToString();
                }
                labelCharismaScore.Text = (_charismaScore + _selectedRace.Charisma).ToString();
                if ((_charismaScore + _selectedRace.Charisma) >= 10)
                {
                    labelCharismaMod.Text = "+" + ((_charismaScore + _selectedRace.Charisma - 10) / 2).ToString();
                }
                else
                {
                    labelCharismaMod.Text = ((_charismaScore + _selectedRace.Charisma - 11) / 2).ToString();
                }
            }
            else
            {
                labelStrengthScore.Text = _strengthScore.ToString();
                if (_strengthScore >= 10)
                {
                    labelStrengthMod.Text = "+" + ((_strengthScore - 10) / 2).ToString();
                }
                else
                {
                    labelStrengthMod.Text = ((_strengthScore - 11) / 2).ToString();
                }
                labelDexterityScore.Text = _dexterityScore.ToString();
                if (_dexterityScore >= 10)
                {
                    labelDexterityMod.Text = "+" + ((_dexterityScore - 10) / 2).ToString();
                }
                else
                {
                    labelDexterityMod.Text = ((_dexterityScore - 11) / 2).ToString();
                }
                labelConstitutionScore.Text = _constitutionScore.ToString();
                if (_constitutionScore >= 10)
                {
                    labelConstitutionMod.Text = "+" + ((_constitutionScore - 10) / 2).ToString();
                }
                else
                {
                    labelConstitutionMod.Text = ((_constitutionScore - 11) / 2).ToString();
                }
                labelIntelligenceScore.Text = _intelligenceScore.ToString();
                if (_intelligenceScore >= 10)
                {
                    labelIntelligenceMod.Text = "+" + ((_intelligenceScore - 10) / 2).ToString();
                }
                else
                {
                    labelIntelligenceMod.Text = ((_intelligenceScore - 11) / 2).ToString();
                }
                labelWisdomScore.Text = _wisdomScore.ToString();
                if (_wisdomScore >= 10)
                {
                    labelWisdomMod.Text = "+" + ((_wisdomScore - 10) / 2).ToString();
                }
                else
                {
                    labelWisdomMod.Text = ((_wisdomScore - 11) / 2).ToString();
                }
                labelCharismaScore.Text = _charismaScore.ToString();
                if (_charismaScore >= 10)
                {
                    labelCharismaMod.Text = "+" + ((_charismaScore - 10) / 2).ToString();
                }
                else
                {
                    labelCharismaMod.Text = ((_charismaScore - 11) / 2).ToString();
                }
            }
        }

        private static Dictionary<int, int> AbilityScoreCosts = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 1 },
            { 8, 2 },
            { 9, 3 },
            { 10, 4 },
            { 11, 5 },
            { 12, 6 },
            { 13, 8 },
            { 14, 10 },
            { 15, 12 },
            { 16, 14 },
        };
        #endregion

        #region Skills
        private static void ToggleRaceSkill(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                Skill sk = Skill.GetSkill((uint)e.Item.Tag);
                if(sk != null)
                {
                    if (_raceSkills.Contains(sk))
                    {
                        _raceSkills.Remove(sk);
                    }
                    else
                    {
                        _raceSkills.Add(sk);
                    }
                }
                UpdateSkills();
                PopulateSkillEdits();
            }
        }

        private static void ToggleClassSkill(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                Skill sk = Skill.GetSkill((uint)e.Item.Tag);
                if(sk != null)
                {
                    if (_classSkills.Contains(sk))
                    {
                        _classSkills.Remove(sk);
                    }
                    else
                    {
                        _classSkills.Add(sk);
                    }
                }
                UpdateSkills();
                PopulateSkillEdits();
            }
        }

        private static void ToggleClassTool(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                Skill sk = Skill.GetSkill((uint)e.Item.Tag);
                if(sk != null)
                {
                    if (_classTools.Contains(sk))
                    {
                        _classTools.Remove(sk);
                    }
                    else
                    {
                        _classTools.Add(sk);
                    }
                    UpdateSkills();
                    PopulateSkillEdits();
                }
            }
        }

        private static void EditSkills(object sender, EventArgs e)
        {
            if (_currentEdit == CurrentEdit.Skill) return;

            _editPanel.Controls.Clear();

            if(_selectedRace == null)
            {
                raceSkillsLabel.Text = "Racial Skills (No Class Selected)";
            }
            else
            {
                raceSkillsLabel.Text = "Racial Skills (" + (_selectedRace.FreeSkills - _raceSkills.Count) + " Remaining)";
            }
            StyleLabel(raceSkillsLabel);
            raceSkillsLabel.Width = _editPanel.ClientRectangle.Width - padding * 2;

            if(_selectedClass == null)
            {
                classSkillsLabel.Text = "Class Skills (No Class Selected)";
                classToolsLabel.Text = "Class Tools (No Class Selected)";
            }
            else
            {
                classSkillsLabel.Text = "Class Skills (" + (_selectedClass.SkillChoices - _classSkills.Count) + " Remaining)";
                classToolsLabel.Text = "Class Tools (" + (_selectedClass.ToolChoices - _classTools.Count) + " Remaining)";
            }
            StyleLabel(classSkillsLabel);
            StyleLabel(classToolsLabel);
            classSkillsLabel.Width = _editPanel.ClientRectangle.Width - padding * 2;
            classToolsLabel.Width = _editPanel.ClientRectangle.Width - padding * 2;

            raceSkills.Width = _editPanel.ClientRectangle.Width - padding * 2;
            classSkills.Width = _editPanel.ClientRectangle.Width - padding * 2;
            classTools.Width = _editPanel.ClientRectangle.Width - padding * 2;
            raceSkills.Height = (_editPanel.ClientRectangle.Height - padding * 4 - raceSkillsLabel.Height * 3) / 3;
            classSkills.Height = (_editPanel.ClientRectangle.Height - padding * 4 - raceSkillsLabel.Height * 3) / 3;
            classTools.Height = (_editPanel.ClientRectangle.Height - padding * 4 - raceSkillsLabel.Height * 3) / 3;
            raceSkillsLabel.Location = new Point(padding, padding);
            raceSkills.Location = new Point(padding, raceSkillsLabel.Location.Y + raceSkillsLabel.Height);
            classSkillsLabel.Location = new Point(padding, raceSkills.Location.Y + raceSkills.Height + padding);
            classSkills.Location = new Point(padding, classSkillsLabel.Location.Y + classSkillsLabel.Height);
            classToolsLabel.Location = new Point(padding, classSkills.Location.Y + classSkills.Height + padding);
            classTools.Location = new Point(padding, classToolsLabel.Location.Y + classToolsLabel.Height);

            StyleListView(raceSkills, false);
            StyleListView(classSkills, false);
            StyleListView(classTools, false);

            PopulateSkillEdits();
            
            _editPanel.Controls.Add(raceSkillsLabel);
            _editPanel.Controls.Add(raceSkills);
            _editPanel.Controls.Add(classSkillsLabel);
            _editPanel.Controls.Add(classSkills);
            _editPanel.Controls.Add(classToolsLabel);
            _editPanel.Controls.Add(classTools);

            _currentEdit = CurrentEdit.Skill;
        }

        private static void PopulateSkillEdits()
        {
            if (_selectedRace != null)
            { 
                raceSkillsLabel.Text = "Racial Skills (" + (_selectedRace.FreeSkills - _raceSkills.Count) + " Remaining)";
            }
            if (_selectedClass != null)
            {
                classSkillsLabel.Text = "Class Skills (" + (_selectedClass.SkillChoices - _classSkills.Count) + " Remaining)";
                classToolsLabel.Text = "Class Tools (" + (_selectedClass.ToolChoices - _classTools.Count) + " Remaining)";
            }

            uint topItemIndex = uint.MaxValue;
            if(raceSkills.TopItem != null)
            {
                topItemIndex = (uint)raceSkills.TopItem.Tag;
            }
            ListViewItem topItem = null;
            raceSkills.BeginUpdate();
            raceSkills.Items.Clear();
            if (_selectedRace != null)
            {
                ImageList imgs = new ImageList();
                imgs.ImageSize = IconSize;
                imgs.ColorDepth = ColorDepth.Depth32Bit;
                int imageIndex = 0;

                foreach (Skill sk in _selectedRace.SelectedSkill)
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { String.Empty, sk.Name });
                    toAdd.Name = sk.Name;
                    toAdd.Tag = sk.Id;
                    toAdd.ToolTipText = StringLibrary.GetString(sk.Description);
                    toAdd.ImageIndex = imageIndex;
                    imgs.Images.Add(sk.Icon);
                    imageIndex++;
                    StyleListViewItem(toAdd);
                    if (_raceSkills.Contains(sk)) toAdd.BackColor = SelectedRowColor;
                    raceSkills.Items.Add(toAdd);
                    if(sk.Id == topItemIndex) topItem = toAdd;
                }
                raceSkills.SmallImageList = imgs;
            }
            raceSkills.EndUpdate();
            if (topItem != null) raceSkills.TopItem = topItem;

            topItemIndex = uint.MaxValue;
            if (classSkills.TopItem != null)
            {
                topItemIndex = (uint)classSkills.TopItem.Tag;
            }
            topItem = null;
            classSkills.BeginUpdate();
            classSkills.Items.Clear();
            if (_selectedClass != null)
            {
                ImageList imgs = new ImageList();
                imgs.ImageSize = IconSize;
                imgs.ColorDepth = ColorDepth.Depth32Bit;
                int imageIndex = 0;

                foreach (Skill sk in _selectedClass.AvailableSkills)
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { String.Empty, sk.Name });
                    toAdd.Name = sk.Name;
                    toAdd.Tag = sk.Id;
                    toAdd.ToolTipText = StringLibrary.GetString(sk.Description);
                    toAdd.ImageIndex = imageIndex;
                    imgs.Images.Add(sk.Icon);
                    imageIndex++;
                    StyleListViewItem(toAdd);
                    if (_classSkills.Contains(sk)) toAdd.BackColor = SelectedRowColor;
                    classSkills.Items.Add(toAdd);
                    if (sk.Id == topItemIndex) topItem = toAdd;
                }
                classSkills.SmallImageList = imgs;
            }
            classSkills.EndUpdate();
            if (topItem != null) classSkills.TopItem = topItem;

            topItemIndex = uint.MaxValue;
            if (classTools.TopItem != null)
            {
                topItemIndex = (uint)classTools.TopItem.Tag;
            }
            topItem = null;
            classTools.BeginUpdate();
            classTools.Items.Clear();
            if (_selectedClass != null)
            {
                ImageList imgs = new ImageList();
                imgs.ImageSize = IconSize;
                imgs.ColorDepth = ColorDepth.Depth32Bit;
                int imageIndex = 0;

                foreach (Skill sk in _selectedClass.AvailableTools)
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { String.Empty, sk.Name });
                    toAdd.Name = sk.Name;
                    toAdd.Tag = sk.Id;
                    toAdd.ToolTipText = StringLibrary.GetString(sk.Description);
                    toAdd.ImageIndex = imageIndex;
                    imgs.Images.Add(sk.Icon);
                    imageIndex++;
                    StyleListViewItem(toAdd);
                    if (_classTools.Contains(sk)) toAdd.BackColor = SelectedRowColor;
                    classTools.Items.Add(toAdd);
                    if (sk.Id == topItemIndex) topItem = toAdd;
                }

                classTools.SmallImageList = imgs;
            }
            classTools.EndUpdate();
            if (topItem != null) classTools.TopItem = topItem;
        }

        private static void UpdateSkills()
        {
            strengthSkills.Items.Clear();
            dexteritySkills.Items.Clear();
            constitutionSkills.Items.Clear();
            intelligenceSkills.Items.Clear();
            wisdomSkills.Items.Clear();
            charismaSkills.Items.Clear();

            int strengthSave = _strengthScore;
            int dexteritySave = _dexterityScore;
            int constitutionSave = _constitutionScore;
            int intelligenceSave = _intelligenceScore;
            int wisdomSave = _wisdomScore;
            int charismaSave = _charismaScore;

            if(_selectedRace != null)
            {
                strengthSave += _selectedRace.Strength;
                dexteritySave += _selectedRace.Dexterity;
                constitutionSave += _selectedRace.Constitution;
                intelligenceSave += _selectedRace.Intelligence;
                wisdomSave += _selectedRace.Wisdom;
                charismaSave += _selectedRace.Charisma;
            }
            if (strengthSave < 10)
            {
                strengthSave -= 1; 
            }
            if (dexteritySave < 10)
            {
                dexteritySave -= 1;
            }
            if (constitutionSave < 10)
            {
                constitutionSave -= 1;
            }
            if (intelligenceSave < 10)
            {
                intelligenceSave -= 1;
            }
            if (wisdomSave < 10)
            {
                wisdomSave -= 1;
            }
            if (charismaSave < 10)
            {
                charismaSave -= 1;
            }

            strengthSave = ((strengthSave - 10) / 2);
            dexteritySave = ((dexteritySave - 10) / 2);
            constitutionSave = ((constitutionSave - 10) / 2);
            intelligenceSave = ((intelligenceSave - 10) / 2);
            wisdomSave = ((wisdomSave - 10) / 2);
            charismaSave = ((charismaSave - 10) / 2);

            int strengthMod = strengthSave + 2;
            int dexterityMod = dexteritySave + 2;
            int constitutionMod = constitutionSave + 2;
            int intelligenceMod = intelligenceSave + 2;
            int wisdomMod = wisdomSave + 2;
            int charismaMod = charismaSave + 2;

            if(_selectedClass != null)
            {
                if (_selectedClass.ProficientSaves.Contains(0)) strengthSave += 2;
                if (_selectedClass.ProficientSaves.Contains(1)) dexteritySave += 2;
                if (_selectedClass.ProficientSaves.Contains(2)) constitutionSave += 2;
                if (_selectedClass.ProficientSaves.Contains(3)) intelligenceSave += 2;
                if (_selectedClass.ProficientSaves.Contains(4)) wisdomSave += 2;
                if (_selectedClass.ProficientSaves.Contains(5)) charismaSave += 2;
            }

            ListViewItem toAdd = new ListViewItem(new string[] { String.Empty, "Saving Throw", strengthSave > 0 ? "+ " + strengthSave.ToString() : strengthSave.ToString() });
            StyleListViewItem(toAdd);
            strengthSkills.Items.Add(toAdd);

            toAdd = new ListViewItem(new string[] { String.Empty, "Saving Throw", dexteritySave > 0 ? "+ " + dexteritySave.ToString() : dexteritySave.ToString() });
            StyleListViewItem(toAdd);
            dexteritySkills.Items.Add(toAdd);

            toAdd = new ListViewItem(new string[] { String.Empty, "Saving Throw", constitutionSave > 0 ? "+ " + constitutionSave.ToString() : constitutionSave.ToString() });
            StyleListViewItem(toAdd);
            constitutionSkills.Items.Add(toAdd);

            toAdd = new ListViewItem(new string[] { String.Empty, "Saving Throw", intelligenceSave > 0 ? "+ " + intelligenceSave.ToString() : intelligenceSave.ToString() });
            StyleListViewItem(toAdd);
            intelligenceSkills.Items.Add(toAdd);

            toAdd = new ListViewItem(new string[] { String.Empty, "Saving Throw", wisdomSave > 0 ? "+ " + wisdomSave.ToString() : wisdomSave.ToString() });
            StyleListViewItem(toAdd);
            wisdomSkills.Items.Add(toAdd);

            toAdd = new ListViewItem(new string[] { String.Empty, "Saving Throw", charismaSave > 0 ? "+ " + charismaSave.ToString() : charismaSave.ToString() });
            StyleListViewItem(toAdd);
            charismaSkills.Items.Add(toAdd);

            if(_selectedBackground != null)
            {
                AddSkillToDisplay(Skill.GetSkill(_selectedBackground.SkillProf1), strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
                AddSkillToDisplay(Skill.GetSkill(_selectedBackground.SkillProf2), strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
            }
            if (_selectedRace != null)
            {
                foreach (Skill sk in _selectedRace.AutomaticSkills)
                {
                    AddSkillToDisplay(sk, strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
                }
            }

            foreach(Skill sk in _classSkills)
            {
                AddSkillToDisplay(sk, strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
            }

            foreach(Skill sk in _raceSkills)
            {
                AddSkillToDisplay(sk, strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
            }

            if(_selectedBackground != null)
            {
                AddSkillToDisplay(Skill.GetSkill(_selectedBackground.ToolProf), strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
            }

            foreach(Skill sk in _classTools)
            {
                AddSkillToDisplay(sk, strengthMod, dexterityMod, constitutionMod, intelligenceMod, wisdomMod, charismaMod);
            }
        }

        private static void AddSkillToDisplay(Skill sk, int strengthMod, int dexterityMod, int constitutionMod, int intelligenceMod, int wisdomMod, int charismaMod)
        {
            if (sk != null)
            {
                ListViewItem toAdd = null;
                switch (sk.Ability)
                {
                    case 0:
                        toAdd = new ListViewItem(new string[] { String.Empty, sk.Name, strengthMod > 0 ? "+ " + strengthMod.ToString() : strengthMod.ToString() });
                        StyleListViewItem(toAdd);
                        strengthSkills.Items.Add(toAdd);
                        break;
                    case 1:
                        toAdd = new ListViewItem(new string[] { String.Empty, sk.Name, dexterityMod > 0 ? "+ " + dexterityMod.ToString() : dexterityMod.ToString() });
                        StyleListViewItem(toAdd);
                        dexteritySkills.Items.Add(toAdd);
                        break;
                    case 2:
                        toAdd = new ListViewItem(new string[] { String.Empty, sk.Name, constitutionMod > 0 ? "+ " + constitutionMod.ToString() : constitutionMod.ToString() });
                        StyleListViewItem(toAdd);
                        constitutionSkills.Items.Add(toAdd);
                        break;
                    case 3:
                        toAdd = new ListViewItem(new string[] { String.Empty, sk.Name, intelligenceMod > 0 ? "+ " + intelligenceMod.ToString() : intelligenceMod.ToString() });
                        StyleListViewItem(toAdd);
                        intelligenceSkills.Items.Add(toAdd);
                        break;
                    case 4:
                        toAdd = new ListViewItem(new string[] { String.Empty, sk.Name, wisdomMod > 0 ? "+ " + wisdomMod.ToString() : wisdomMod.ToString() });
                        StyleListViewItem(toAdd);
                        wisdomSkills.Items.Add(toAdd);
                        break;
                    case 5:
                        toAdd = new ListViewItem(new string[] { String.Empty, sk.Name, charismaMod > 0 ? "+ " + charismaMod.ToString() : charismaMod.ToString() });
                        StyleListViewItem(toAdd);
                        charismaSkills.Items.Add(toAdd);
                        break;
                }
            }
        }
        #endregion

        #region Styles, Sizing, and Color
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
            AbilityScores,
            Background,
            Class,
            Gender,
            Race,
            Skill
        }

        private static Font abilityModFont = new Font(FontFamily.GenericSansSerif, 18.0f);
        private static Font abilityScoreFont = new Font(FontFamily.GenericSansSerif, 8.0f);
        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        private static Color SelectedRowColor = Color.FromArgb(25, 25, 25);

        private static void StyleAbilityMod(Label toStyle)
        {
            toStyle.BackColor = Color.Black;
            toStyle.ForeColor = Color.White;
            toStyle.Font = abilityModFont;
            toStyle.Size = labelStrengthMod.PreferredSize;
            toStyle.Width = abilityStrength.Width - 20;
            toStyle.Location = new Point(10, 10);
            toStyle.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static void StyleAbilityScore(Label toStyle)
        {
            toStyle.BackColor = Color.Black;
            toStyle.ForeColor = Color.White;
            toStyle.Font = abilityScoreFont;
            toStyle.Size = labelStrengthScore.PreferredSize;
            toStyle.Width = abilityStrength.Width - 40;
            toStyle.TextAlign = ContentAlignment.MiddleCenter;
            toStyle.Location = new Point(21, 62 - labelStrengthScore.Height);
        }

        private static void StyleLabel(Control toStyle)
        {
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }

        private static void StyleListView(ListView listView, bool threeColumns)
        {
            listView.Clear();
            listView.Columns.Add("");
            listView.Columns.Add("");
            if (threeColumns) listView.Columns.Add("");
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.BackColor = Color.Black;
            listView.ForeColor = Color.White;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.ShowItemToolTips = true;
            listView.Font = labelFont;
            if (threeColumns)
            {
                listView.Columns[0].Width = MiniIconSize.Width + 2;
                listView.Columns[1].Width = ((listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - IconSize.Width - 2) * 8) / 10;
                listView.Columns[2].Width = ((listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - IconSize.Width - 2) * 2) / 10;
            }
            else
            {
                listView.Columns[0].Width = IconSize.Width + 2;
                listView.Columns[1].Width = listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - IconSize.Width - 2;
            }
        }

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }
        #endregion
    }
}
