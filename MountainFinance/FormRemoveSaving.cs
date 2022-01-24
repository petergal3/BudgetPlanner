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
    public partial class FormRemoveSaving : Form
    {
        DataManager dm = DataManager.GetSingleton();
        public FormRemoveSaving()
        {
            InitializeComponent();
        }

        private void buttonSaveRemoveSaving_Click(object sender, EventArgs e)
        {
            Saving s = new Saving(Int32.Parse(this.textBoxRemoveSaving.Text)*-1, this.dateTimePickerRemoveSaving.Value, 1);
            dm.SqlInsertion(s, LoginManager.GetSingleton());
            Income t = new Income("saving withdrawal", "", Int32.Parse(this.textBoxRemoveSaving.Text), this.dateTimePickerRemoveSaving.Value, 1);
            dm.SqlInsertion(t, LoginManager.GetSingleton());
            this.Close();
        }
    }
}
