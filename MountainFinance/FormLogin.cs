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
    public partial class FormLogin : Form
    {

        public bool usernameUntouched = true;
        public bool passwordUntouched = true;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            pictureLogo.BackColor = Color.FromArgb(218,230,242);
            labelWelcome.BackColor = Color.FromArgb(218, 230, 242);
            labeldesc.BackColor = Color.FromArgb(218, 230, 242);
            labeldesc1.BackColor = Color.FromArgb(218, 230, 242);
            labeldesc2.BackColor = Color.FromArgb(218, 230, 242);
            labeldesc3.BackColor = Color.FromArgb(218, 230, 242);
            labelAppName.BackColor = Color.FromArgb(218, 230, 242);
            buttonLogin.BackColor = Color.FromArgb(119, 167, 213);
            buttonRegister.ForeColor = Color.FromArgb(119, 167, 213);

            

            if (textBoxUsername.Text == "")
            {
                textBoxUsername.Text = "Username";
                textBoxUsername.ForeColor = Color.Silver;
            }
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.Text = "Password";
                textBoxPassword.ForeColor = Color.Silver;
                textBoxPassword.UseSystemPasswordChar = false;
            }
            
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

        private void textBoxUsername_Leave(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "")
            {
                textBoxUsername.Text = "Username";
                textBoxUsername.ForeColor = Color.Silver;

                usernameUntouched = true;
            }
        }

        private void textBoxPassword_Click(object sender, EventArgs e)
        {
            if (passwordUntouched)
            {
                textBoxPassword.Text = "";
                textBoxPassword.ForeColor = Color.Black;
                textBoxPassword.UseSystemPasswordChar = true;

            }
            passwordUntouched = false;
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.Text = "Password";
                textBoxPassword.ForeColor = Color.Silver;

                passwordUntouched = true;
                textBoxPassword.UseSystemPasswordChar = false;
            }

        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            FormRegistration fr1 = new FormRegistration();
            this.Hide();
            fr1.ShowDialog();

            textBoxPassword.Text = "Password";
            textBoxPassword.ForeColor = Color.Silver;
            passwordUntouched = true;
            textBoxPassword.UseSystemPasswordChar = false;

            textBoxUsername.Text = "Username";
            textBoxUsername.ForeColor = Color.Silver;
            usernameUntouched = true;

            this.Show();

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginManager lm =  LoginManager.GetSingleton(textBoxUsername.Text, textBoxPassword.Text);
            
            if (LoginManager.GetSingleton().userId != -1)
            {
                this.Hide();
                FormMain fm = new FormMain();
                fm.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
           
        }

       
    }
}
