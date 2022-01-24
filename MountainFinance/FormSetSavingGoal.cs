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
    public partial class FormSetSavingGoal : Form
    {
        DataManager dm = DataManager.GetSingleton();
        LoginManager lm = LoginManager.GetSingleton();

        public FormSetSavingGoal()
        {
            InitializeComponent();
        }

        private void buttonSaveGoal_Click(object sender, EventArgs e)
        {
            dm.SqlInsertionSavingGoal(Int32.Parse(this.textBoxSavingGoal.Text),lm );
            this.Close();
        }
    }
}
