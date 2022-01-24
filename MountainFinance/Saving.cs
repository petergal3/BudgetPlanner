using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainFinance
{
    class Saving : Transaction
    {
        public Saving(int amount, DateTime date, int currencyId)
        {
            base.amount = amount;
            base.date = date;
            base.currencyId = currencyId;
        }
    }
}
