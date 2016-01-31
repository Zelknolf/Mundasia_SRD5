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
using Mundasia.Communication;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    public class LoginForm : Form
    {
        TextBox userName = new TextBox();
        TextBox password = new TextBox();

        
        public LoginForm()
        {
            this.Height = 150;
            this.Width = 400;
            this.BackColor = Color.Black;
            this.Text = StringLibrary.GetString(8);
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            Label userNameLabel = new Label();
            userNameLabel.Text = StringLibrary.GetString(2);
            userNameLabel.Location = new Point(10, 10);
            StyleLabel(userNameLabel);

            Label passwordLabel = new Label();
            passwordLabel.Text = StringLibrary.GetString(3);
            passwordLabel.Location = new Point(10, 40);
            StyleLabel(passwordLabel);

            userName.Height = userName.PreferredHeight;
            userName.Width = 200;
            userName.ShortcutsEnabled = true;
            userName.Location = new Point(175, 10);

            password.Height = password.PreferredHeight;
            password.Width = 200;
            password.PasswordChar = '●';
            password.ShortcutsEnabled = true;
            password.Location = new Point(175, 40);

            Button accept = new Button();
            StyleLabel(accept);
            accept.Text = StringLibrary.GetString(8);
            accept.Size = accept.PreferredSize;
            accept.Location = new Point(this.ClientRectangle.Width - accept.Width, this.ClientRectangle.Height - accept.Height);
            accept.Click += accept_Click;
            this.AcceptButton = accept;

            Button cancel = new Button();
            StyleLabel(cancel);
            cancel.Text = StringLibrary.GetString(7);
            cancel.Size = cancel.PreferredSize;
            cancel.Location = new Point(this.ClientRectangle.Width - accept.Width - cancel.Width, this.ClientRectangle.Height - cancel.Height);
            cancel.Click += cancel_Click;
            this.CancelButton = cancel;

            this.Controls.Add(userNameLabel);
            this.Controls.Add(passwordLabel);
            this.Controls.Add(userName);
            this.Controls.Add(password);
            this.Controls.Add(accept);
            this.Controls.Add(cancel);
        }

        void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        void accept_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(userName.Text))
            {
                MessageBox.Show("Please provide a user name.");
                return;
            }
            if (String.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Please select a password.");
                return;
            }
            int sessionID = ServiceConsumer.ClientLogin(userName.Text,password.Text);
            if(sessionID != -1)
            {
                LoginScreen.PassToCharacterSelect();
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("Login failed.");
            }
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        private static void StyleLabel(Control toStyle)
        {
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }
    }
}
