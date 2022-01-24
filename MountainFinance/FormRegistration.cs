using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MountainFinance
{
    public partial class FormRegistration : Form
    {
        private bool usernameUntouched = true;
        private bool mailUntouched = true;
        private bool passwordUntouched = true;
        public FormRegistration()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            LoginManager lm = LoginManager.GetSingleton(textBoxUsername.Text,textBoxMail.Text, textBoxPassword.Text);
            this.Close();
        }

        private void FormRegistration_Load(object sender, EventArgs e)
        {
            pictureLogo.BackColor = Color.FromArgb(218, 230, 242);
            labelBigLine.BackColor = Color.FromArgb(218, 230, 242);
            labelAppName.BackColor = Color.FromArgb(218, 230, 242);

            
            textBoxUsername.Text = "Username";
            textBoxUsername.ForeColor = Color.Silver;

            textBoxMail.Text = "E-mail";
            textBoxMail.ForeColor = Color.Silver;

            textBoxPassword.Text = "Password";
            textBoxPassword.ForeColor = Color.Silver;

            this.ActiveControl = pictureLogo;
        }

        private void textBoxUsername_Click(object sender, EventArgs e)
        {
            if (usernameUntouched)
            {
                textBoxUsername.Text = "";
                textBoxUsername.ForeColor = Color.Black;
            }
            usernameUntouched = false;
        }

        private void textBoxMail_Click(object sender, EventArgs e)
        {
            if (mailUntouched)
            {
                textBoxMail.Text = "";
                textBoxMail.ForeColor = Color.Black;
            }
            mailUntouched = false;
        }

        private void textBoxPassword_Click(object sender, EventArgs e)
        {
            if (passwordUntouched)
            {
                textBoxPassword.Text = "";
                textBoxPassword.ForeColor = Color.Black;
            }
            passwordUntouched = false;
        }

        private void textBoxUsername_Leave(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "")
            {
                textBoxUsername.Text = "Username";
                textBoxUsername.ForeColor = Color.Silver;

                usernameUntouched = true;
            }
        }

        private void textBoxMail_Leave(object sender, EventArgs e)
        {
            if (textBoxMail.Text == "")
            {
                textBoxMail.Text = "E-mail";
                textBoxMail.ForeColor = Color.Silver;

                mailUntouched = true;
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.Text = "Password";
                textBoxPassword.ForeColor = Color.Silver;

                passwordUntouched = true;
            }
        }
    }
}
