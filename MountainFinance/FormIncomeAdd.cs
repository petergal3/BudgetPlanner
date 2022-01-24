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
    public partial class FormIncomeAdd : Form
    {
        public FormIncomeAdd()
        {
            InitializeComponent();
        }

        private void buttonSaveIncome_Click(object sender, EventArgs e)
        {
            DataManager dm = DataManager.GetSingleton();
            Income t = new Income(this.textBoxCategoryAddIncome.Text, this.textBoxDescriprtionAddIncome.Text, Int32.Parse(this.textBoxAmountAddIncome.Text), this.dateTimePickerAddIncome.Value, 1);
            dm.SqlInsertion(t, LoginManager.GetSingleton());
            this.Close();
        }
    }
}
