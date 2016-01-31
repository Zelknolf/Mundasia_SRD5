using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mundasia.Objects
{
    public class CharacterPanel : Form
    {
        public Creature ShownCharacter;
        Label LabelName = new Label();
        Label LabelSexRace = new Label();

        Label LabelAbilityHead = new Label();
        Label LabelPhysicalHead = new Label();
        Label LabelMentalHead = new Label();
        Label LabelSocialHead = new Label();
        Label LabelSupernaturalHead = new Label();

        Label LabelSkillHead = new Label();
        Label LabelCombatSkillHead = new Label();
        Label LabelArtisanSkillHead = new Label();
        Label LabelScholarshipSkillHead = new Label();
        Label LabelSocialSkillHead = new Label();
        Label LabelSupernaturalSkillHead = new Label();
        Label LabelSurvivalSkillHead = new Label();

        public int padding = 5;
        public bool ReadOnly;
        public CharacterPanel(Creature toDisplay) 
        {
            Init(toDisplay, true);
        }

        public CharacterPanel(Creature toDisplay, bool readOnly)
        {
            Init(toDisplay, readOnly);
        }

        private void Init(Creature toDisplay, bool readOnly)
        {
            ShownCharacter = toDisplay;
            this.Text = ShownCharacter.CharacterName;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Width = 600;
            this.Height = 700;
            ReadOnly = readOnly;

            int currentY= 0;
            if (readOnly)
            {
                LabelName.Text = ShownCharacter.CharacterName;
                StyleLabel(LabelName);
                LabelName.Location = new Point(Math.Max(0, (this.Width - LabelName.Width) / 2), padding);
                Controls.Add(LabelName);
                currentY = LabelName.Location.Y + LabelName.Height;
            }
            else
            {
                TextBox ChangeName = new TextBox();
                ChangeName.Text = ShownCharacter.CharacterName;
                StyleLabel(ChangeName);
                ChangeName.BackColor = Color.DarkGray;
                ChangeName.Width = 500;
                ChangeName.Location = new Point(Math.Max(0, (this.Width - ChangeName.Width) / 2), padding);
                ChangeName.TextChanged += ChangeName_TextChanged;
                Controls.Add(ChangeName);
                currentY = ChangeName.Location.Y + ChangeName.Height;
            }

            if (readOnly)
            {
                LabelSexRace.Text = Race.GetRace(ShownCharacter.CharacterRace).Name;
                if (ShownCharacter.Sex == 1)
                {
                    LabelSexRace.Text += " Male ";
                }
                else
                {
                    LabelSexRace.Text += " Female ";
                }
                StyleLabel(LabelSexRace);
                LabelSexRace.Location = new Point(Math.Max(0, (this.Width - LabelSexRace.Width) / 2), currentY);
                currentY = LabelSexRace.Location.Y + LabelSexRace.Height;
            }
            else
            {
                ComboBox ComboRace = new ComboBox();
                ComboRace.ForeColor = Color.White;
                ComboRace.BackColor = Color.Black;
                ComboRace.Height = ComboRace.PreferredHeight;
                ComboRace.Width = 90;
                foreach(Race race in Race.GetRaces())
                {
                    ComboRace.Items.Add(race.Name);
                }
                ComboRace.AutoCompleteMode = AutoCompleteMode.Suggest;
                ComboRace.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ComboRace.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                ComboRace.Text = Race.GetRace(ShownCharacter.CharacterRace).Name;
                ComboRace.SelectedIndexChanged += ComboRace_SelectedIndexChanged;
                ComboRace.Location = new Point(100, currentY);
                this.Controls.Add(ComboRace);

                ComboBox ComboSex = new ComboBox();
                ComboSex.ForeColor = Color.White;
                ComboSex.BackColor = Color.Black;
                ComboSex.Height = ComboSex.PreferredHeight;
                ComboSex.Width = 90;
                ComboSex.Items.Add("Female");
                ComboSex.Items.Add("Male");
                ComboSex.AutoCompleteMode = AutoCompleteMode.Suggest;
                ComboSex.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ComboSex.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                if(ShownCharacter.Sex == 1)
                {
                    ComboSex.Text = "Male";
                }
                else
                {
                    ComboSex.Text = "Female";
                }
                ComboSex.SelectedIndexChanged += ComboSex_SelectedIndexChanged;
                ComboSex.Location = new Point(200, currentY);
                this.Controls.Add(ComboSex);
            }

            LabelAbilityHead.Text = "Abilities";
            StyleLabel(LabelAbilityHead);
            LabelAbilityHead.Location = new Point(Math.Max(0, (this.Width - LabelAbilityHead.Width) / 2), currentY + (padding * 2));

            int colWidth = (this.ClientRectangle.Width - (padding * 5)) / 4;

            LabelPhysicalHead.Text = "Physical";
            StyleLabel(LabelPhysicalHead);
            LabelPhysicalHead.Location = new Point(padding, LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            LabelMentalHead.Text = "Mental";
            StyleLabel(LabelMentalHead);
            LabelMentalHead.Location = new Point((padding * 2) + colWidth, LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            LabelSocialHead.Text = "Social";
            StyleLabel(LabelSocialHead);
            LabelSocialHead.Location = new Point((padding * 3) + (colWidth * 2), LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            LabelSupernaturalHead.Text = "Supernatural";
            StyleLabel(LabelSupernaturalHead);
            LabelSupernaturalHead.Location = new Point((padding * 4) + (colWidth * 3), LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            int currentPosY = padding;

            Controls.Add(LabelSexRace);
        }

        private void Redraw()
        {
            this.Controls.Clear();
            Init(ShownCharacter, ReadOnly);
        }

        void ComboSex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboSex = (ComboBox)sender;
            string index = ComboSex.SelectedItem.ToString();
            if(index == "Male")
            {
                ShownCharacter.Sex = 1;
            }
            else
            {
                ShownCharacter.Sex = 0;
            }
        }

        void ComboRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboRace = (ComboBox)sender;
            string index = ComboRace.SelectedItem.ToString();
            foreach(Race race in Race.GetRaces())
            {
                if(index == race.Name)
                {
                    ShownCharacter.CharacterRace = race.Id;
                }
            }
        }

        void ChangeName_TextChanged(object sender, EventArgs e)
        {
            TextBox text = sender as TextBox;
            if(!String.IsNullOrWhiteSpace(text.Text))
            {
                ShownCharacter.CharacterName = text.Text;
            }
        }

        /// <summary>
        /// Font used by the controls on this form.
        /// </summary>
        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f, FontStyle.Bold);

        private static Font labelFontLesser = new Font(FontFamily.GenericSansSerif, 10.0f);

        /// <summary>
        /// Used to provide the style information for all of the labels and
        /// buttons on the form.
        /// </summary>
        /// <param name="toStyle">the control to style</param>
        private static void StyleLabel(Control toStyle)
        {
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }

        private static void StyleLabelLesser(Control toStyle)
        {
            toStyle.Font = labelFontLesser;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }
    }

    public class StatBox: NumericUpDown
    {
        public uint StatId;
        public StatBox(uint statId)
        {
            StatId = statId;
            this.Font = new Font(FontFamily.GenericSansSerif, 10.0f);
            ForeColor = Color.White;
            BackColor = Color.Black;
            Size = PreferredSize;
        }
    }

    public class SkillBox: NumericUpDown
    {
        public uint  SkillId;
        public SkillBox(uint skillId)
        {
            SkillId = skillId;
            this.Font = new Font(FontFamily.GenericSansSerif, 10.0f);
            ForeColor = Color.White;
            BackColor = Color.Black;
            Size = PreferredSize;
        }
    }
}
