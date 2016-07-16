using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Mund_SRD5_Client.UI.Controls
{
    public class AbilityScoreBox: Panel
    {
        private static Image _abilityScoreIcon = null;
        private static Image IconAbilityScore
        {
            get
            {
                if (_abilityScoreIcon != null) return _abilityScoreIcon;
                _abilityScoreIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Chrome\\abilityScore.png");
                return _abilityScoreIcon;
            }
        }

        private int _score;
        private int _mod;

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                scoreLabel.Text = _score.ToString();
                if (_score >= 10)
                {
                    _mod = (_score - 10) / 2;
                    modLabel.Text = "+" + _mod.ToString();
                }
                else
                {
                    _mod = (_score - 11) / 2;
                    modLabel.Text = _mod.ToString();
                }
            }
        }

        public int Mod
        {
            get
            {
                return _mod;
            }
        }

        private static Font abilityModFont = new Font(FontFamily.GenericSansSerif, 18.0f);
        public Label modLabel = new Label();

        private static Font abilityScoreFont = new Font(FontFamily.GenericSansSerif, 8.0f);
        public Label scoreLabel = new Label();

        public AbilityScoreBox()
        {
            Size = new Size(64, 64);
            BackgroundImage = IconAbilityScore;

            modLabel.BackColor = Color.Black;
            modLabel.ForeColor = Color.White;
            modLabel.Font = abilityModFont;
            modLabel.Size = modLabel.PreferredSize;
            modLabel.Width = Width - 20;
            modLabel.Location = new Point(10, 10);
            modLabel.TextAlign = ContentAlignment.MiddleCenter;

            scoreLabel.BackColor = Color.Black;
            scoreLabel.ForeColor = Color.White;
            scoreLabel.Font = abilityScoreFont;
            scoreLabel.Size = scoreLabel.PreferredSize;
            scoreLabel.Width = Width - 40;
            scoreLabel.TextAlign = ContentAlignment.MiddleCenter;
            scoreLabel.Location = new Point(21, 62 - scoreLabel.Height);

            modLabel.Click += raiseClick;
            scoreLabel.Click += raiseClick;

            Controls.Add(modLabel);
            Controls.Add(scoreLabel);
        }

        private void raiseClick(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}
