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
    public partial class FormExpenseAdd : Form
    {
        public FormExpenseAdd()
        {
            InitializeComponent();
        }

        private void buttonSaveExpense_Click(object sender, EventArgs e)
        {
            
            DataManager dm = DataManager.GetSingleton();
            Expense t = new Expense(this.textBoxCategoryAddExpense.Text, this.textBoxDescriptionAddExpense.Text, Int32.Parse(this.textBoxAmountAddExpense.Text), this.dateTimePickerAddExpense.Value, 1);
            dm.SqlInsertion(t,LoginManager.GetSingleton() );
            this.Close();

        }
    }
}
