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
    public partial class FormAddSaving : Form
    {
        DataManager dm = DataManager.GetSingleton();
        public FormAddSaving()
        {
            InitializeComponent();
            
        }

        private void buttonSaveSaving_Click(object sender, EventArgs e)
        {
           
            Saving s = new Saving(Int32.Parse(this.textBoxAmountAddSaving.Text), this.dateTimePickerAddSaving.Value, 1);
            dm.SqlInsertion(s, LoginManager.GetSingleton());
            Income t = new Income("saving","",Int32.Parse(this.textBoxAmountAddSaving.Text)*-1, this.dateTimePickerAddSaving.Value, 1);
            dm.SqlInsertion(t, LoginManager.GetSingleton());
            this.Close();
        }
    }
}
