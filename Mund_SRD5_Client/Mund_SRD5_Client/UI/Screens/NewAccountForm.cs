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
    /// <summary>
    /// This form is used to take the inputs that a user would provide to create a new account and then calls the
    /// correct methods in ClientConnector to request account creation on the server.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public class NewAccountForm: Form
    {
        TextBox userName = new TextBox();
        TextBox password = new TextBox();
        TextBox repeatPassword = new TextBox();
        TextBox emailAddy = new TextBox();

        public NewAccountForm()
        {
            this.Height = 200;
            this.Width = 400;
            this.BackColor = Color.Black;
            this.Text = StringLibrary.GetString(1);
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

            Label repeatPasswordLabel = new Label();
            repeatPasswordLabel.Text = StringLibrary.GetString(4);
            repeatPasswordLabel.Location = new Point(10, 70);
            StyleLabel(repeatPasswordLabel);

            Label emailAddyLabel = new Label();
            emailAddyLabel.Text = StringLibrary.GetString(5);
            emailAddyLabel.Location = new Point(10, 100);
            StyleLabel(emailAddyLabel);

            userName.Height = userName.PreferredHeight;
            userName.Width = 200;
            userName.ShortcutsEnabled = true;
            userName.Location = new Point(175, 10);

            password.Height = password.PreferredHeight;
            password.Width = 200;
            password.PasswordChar = '●';
            password.ShortcutsEnabled = true;
            password.Location = new Point(175, 40);

            repeatPassword.Height = repeatPassword.PreferredHeight;
            repeatPassword.Width = 200;
            repeatPassword.PasswordChar = '●';
            repeatPassword.ShortcutsEnabled = true;
            repeatPassword.Location = new Point(175, 70);

            emailAddy.Height = emailAddy.PreferredHeight;
            emailAddy.Width = 200;
            emailAddy.ShortcutsEnabled = true;
            emailAddy.Location = new Point(175, 100);

            Button accept = new Button();
            StyleLabel(accept);
            accept.Text = StringLibrary.GetString(6);
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
            this.Controls.Add(repeatPasswordLabel);
            this.Controls.Add(emailAddyLabel);
            this.Controls.Add(userName);
            this.Controls.Add(password);
            this.Controls.Add(repeatPassword);
            this.Controls.Add(emailAddy);
            this.Controls.Add(accept);
            this.Controls.Add(cancel);
        }

        void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// The accept event, making certain that the input provided is a sensible
        /// thing to send to the server before it packages it for sending.
        /// </summary>
        void accept_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(userName.Text))
            {
                MessageBox.Show("Please provide a user name.");
                return;
            }
            if(String.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Please select a password.");
                return;
            }
            if(String.IsNullOrEmpty(repeatPassword.Text))
            {
                MessageBox.Show("Please retype your password.");
                return;
            }
            if(String.IsNullOrEmpty(emailAddy.Text))
            {
                MessageBox.Show("Please provide an email address.");
                return;
            }
            if(password.Text.Length < 5)
            {
                MessageBox.Show("Your password must be at least five characters long.");
                return;
            }
            if(userName.Text.Length < 5)
            {
                MessageBox.Show("Your username must be at least five characters long.");
                return;
            }
            if(password.Text != repeatPassword.Text)
            {
                MessageBox.Show("Your password and its repetition do not match.");
                return;
            }
            Int64 a;
            if(password.Text == userName.Text ||
               password.Text.ToLower().Contains("password") || 
               Int64.TryParse(password.Text, out a))
            {
                MessageBox.Show("Your password may not be your username, may not contain \"password\", and may not be numeric");
                return;
            }
            if(userName.Text.Contains('|') ||
               password.Text.Contains('|'))
            {
                MessageBox.Show("Usernames and passwords may not contain the following characters: |");
            }
            if(ServiceConsumer.CreateAccount(userName.Text, password.Text))
            {
                MessageBox.Show("Success");
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("Failure");
            }
        }

        /// <summary>
        /// Font used by the controls on this form.
        /// </summary>
        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

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
    }
}