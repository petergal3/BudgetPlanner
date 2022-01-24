using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainFinance
{
    abstract class Transaction
    {
        public int transactionId;
        public int amount;
        public DateTime date;
        public string description;
        public string category;
        public int currencyId;
        public string type;


        public override string ToString()
        {
            return base.ToString();
        }

    }
}
