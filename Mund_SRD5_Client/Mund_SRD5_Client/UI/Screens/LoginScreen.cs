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

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    public class LoginScreen: Panel
    {
        private static LoginScreen _latestPanel = new LoginScreen();
        private static Form _hostingForm = new Form();

        private Panel _buttons = new Panel();

        public static void PassToCharacterSelect()
        {
            Form host = _hostingForm;
            Clear(host);
            CharacterSelectScreen.Set(host);
        }

        /// <summary>
        /// Set the provided windows Form as a login view, adding the special controls for the region.
        /// </summary>
        public static void Set(Form primaryForm)
        {
            primaryForm.Resize += primaryForm_Resize;
            Center(_latestPanel, primaryForm);
            primaryForm.Controls.Add(_latestPanel);

            _latestPanel._buttons.Height = 300;
            _latestPanel._buttons.Width = 300;
            _latestPanel._buttons.Location = new Point(_latestPanel._buttons.Location.X, _latestPanel.Height - _latestPanel._buttons.Height);
            CenterX(_latestPanel._buttons, _latestPanel);
            _latestPanel._buttons.BackColor = Color.Black;

            Label login = new Label();
            login.Text = StringLibrary.GetString(8);
            login.Location = new Point(0, 30);
            FormatButton(login);
            login.Size = login.PreferredSize;
            CenterX(login, _latestPanel._buttons);
            login.Click += login_Click;

            Label createAccount = new Label();
            createAccount.Text = StringLibrary.GetString(1);
            createAccount.Location = new Point(0, 60);
            FormatButton(createAccount);
            createAccount.Size = createAccount.PreferredSize;
            CenterX(createAccount, _latestPanel._buttons);
            createAccount.Click += createAccount_Click;

            Label lore = new Label();
            lore.Text = StringLibrary.GetString(35);
            lore.Location = new Point(0, 90);
            FormatButton(lore);
            lore.Size = lore.PreferredSize;
            CenterX(lore, _latestPanel._buttons);
            lore.Click += lore_Click;

            Label exit = new Label();
            exit.Text = StringLibrary.GetString(9);
            exit.Location = new Point(0, 120);
            FormatButton(exit);
            exit.Size = exit.PreferredSize;
            CenterX(exit, _latestPanel._buttons);
            exit.Click += exit_Click;

            _latestPanel._buttons.Controls.Add(login);
            _latestPanel._buttons.Controls.Add(createAccount);
            _latestPanel._buttons.Controls.Add(lore);
            _latestPanel._buttons.Controls.Add(exit);
            
            _latestPanel.Controls.Add(_latestPanel._buttons);
            _hostingForm = primaryForm;
        }

        /// <summary>
        /// Remove all of the vestiges of a login view from the provided windows Form.
        /// </summary>
        public static void Clear(Form primaryForm)
        {
            primaryForm.Resize -= primaryForm_Resize;
            primaryForm.Controls.Remove(_latestPanel);

            _hostingForm = null;
        }

        static void lore_Click(object sender, EventArgs e)
        {
            new LoreViewer().Show();
        }

        static void exit_Click(object sender, EventArgs e)
        {
            _hostingForm.Close();
            _hostingForm.Dispose();
        }

        static void createAccount_Click(object sender, EventArgs e)
        {
            NewAccountForm form = new NewAccountForm();
            form.ShowDialog();
        }

        static void login_Click(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();
            form.ShowDialog();
        }

        public LoginScreen()
        {
            this.Width = 1024;
            this.Height = 768;
            this.BackgroundImage = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\TitleScreen.png");
        }

        /// <summary>
        /// Reposition the panels contained in the form.
        /// </summary>
        static void primaryForm_Resize(object sender, EventArgs e)
        {
            _hostingForm.Invalidate(new Rectangle(_latestPanel.Location, _latestPanel.Size));
            Center(_latestPanel, _hostingForm);
            _hostingForm.Invalidate(new Rectangle(_latestPanel.Location, _latestPanel.Size));
        }

        /// <summary>
        /// Place a panel centered on a form.
        /// </summary>
        /// <param name="toCenter">The panel to be centered</param>
        /// <param name="centerOn">The form to be centered on</param>
        static void Center(Panel toCenter, Form centerOn)
        {
            toCenter.Location = new Point()
            {
                X = Math.Max(0, (centerOn.ClientRectangle.Width - toCenter.Width)/2),
                Y = Math.Max(0, (centerOn.ClientRectangle.Height - toCenter.Height)/2)
            };
        }

        /// <summary>
        /// Centers a control inside of another control on the X coordinate
        /// </summary>
        /// <param name="toCenter">The control to be centered</param>
        /// <param name="centerOn">The control it is to be centered on</param>
        static void CenterX(Control toCenter, Control centerOn)
        {
            toCenter.Location = new Point()
            {
                X = Math.Max(0, (centerOn.ClientRectangle.Width - toCenter.Width) / 2),
                Y = toCenter.Location.Y
            };
        }

        static Font buttonFont = new Font(FontFamily.GenericSansSerif, 14.0f);

        static void FormatButton(Label button)
        {
            button.Font = buttonFont;
            button.ForeColor = Color.White;
            button.BackColor = Color.Black;
            button.MouseEnter += button_MouseEnter;
            button.MouseLeave += button_MouseLeave;
        }

        static void button_MouseLeave(object sender, EventArgs e)
        {
            Control snd = sender as Control;
            snd.ForeColor = Color.White;
        }

        static void button_MouseEnter(object sender, EventArgs e)
        {
            Control snd = sender as Control;
            snd.ForeColor = Color.Yellow;
        }
    }
}
