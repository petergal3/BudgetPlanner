using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainFinance
{
    interface IDatabaseOperator
    {

        //Handler Method for get
        public string GetMethodSqlHandler();

        //GetData
        public DataTable SqlGetData(DateTime timeFrom, DateTime timeTo, string table);

        public DataTable SqlGetData(DateTime timeFrom, string table);

        public DataTable SqlGetData(string timeFrom, string timeTo, int typeId);


        public DataTable SqlGetData(string timeFrom, int typeId);

        public string SqlGetDataSum(string timeFrom, string table);

        public string SqlGetDataSum(string timeFrom, string timeTo, string table);

        public string SqlGetSavingGoal();

        public List<int> ChartDataintFlow(string sqlCmd);

        public List<int> ChartDataintStack(string sqlCmd);
        public List<string> ChartDatastring(string sqlCmd);


        //Delete
        public void SqlDelete(int id, string table);



        //Insertion

        public void SqlInsertionSavingGoal(int goal, LoginManager user);

        public void SqlInsertionSavingGoal(int goal, DateTime date, LoginManager user);


        public void SqlInsertion(Transaction t, LoginManager user);


    }
}
