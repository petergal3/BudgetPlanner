using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainFinance
{
    class Income : Transaction
    {

        public Income(string category, string description, int amount, DateTime date, int currencyId)
        {
            base.category = category;
            base.description = description;
            base.amount = amount;
            base.date = date;
            base.currencyId = currencyId;
        }
    }
}
