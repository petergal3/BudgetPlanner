using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MountainFinance
{
    class DataManager : IDatabaseOperator
    {
        static string cnnStrBase = Environment.CurrentDirectory;
       
        static string cnnString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={cnnStrBase};Integrated Security=True";
        //singleton típusú megoldás
        static DataManager d;
        public string _sqlTxt;
        private static int userId = -1;
        public string path ="";
        

        public string sqlTxt
        {
            get { return _sqlTxt; }
            set
            {
                _sqlTxt = value;
                //Raise event if change to be able to change userid in srting also
                OnValueChanged(null);
            }
        }

       
        private DataManager() { }

        public static DataManager GetSingleton()
        {
            LoginManager lm = LoginManager.GetSingleton();
            if (lm.userId != -1)
            {
                userId = lm.userId;
                if (d == null)
                {
                    d = new DataManager();
                    int n = cnnStrBase.IndexOf("bin");
                    cnnStrBase = cnnStrBase.Substring(0,n-1) + "\\MfDb.mdf";
                    cnnString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={cnnStrBase};Integrated Security=True";
                }
            }
            return d;
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            _sqlTxt = GetMethodSqlHandler();
        }


        //Handler Method for get
        public string GetMethodSqlHandler() {

            //1. Conn stringet beállítja
            //2. Hozzáadja a uderId szűrést
            string baseSql = sqlTxt;
            int test = baseSql.IndexOf("where");
            string modified = baseSql;
            List<int> indexes = new List<int> { };
            indexes.Add(test);

            int n = indexes.Count();


            for (int i = 0; i < n; i++)
            {
                int startl = indexes[i];
                int endl = startl + 5;
                modified = modified.Insert(endl, $" userId = {userId} and ");

                if (test > 0)
                {
                    test = modified.IndexOf("where", test + 1);
                    if (test != -1)
                    {
                        indexes.Add(test);
                        n++;
                    }
                }
            }

            //3.befuttatja az adatbázisba
            return modified;
        }

        //GetData
        public DataTable SqlGetData(DateTime timeFrom, DateTime timeTo, string table)
        {
            DataTable ds = new DataTable();
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                sqlTxt = $"SELECT * FROM dbo.{table} where dbo.{table}.date between {timeFrom} and {timeTo}";
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader);
                }
            }
            return ds;
        }

        public DataTable SqlGetData(string timeFrom, string timeTo, string table)
        {
            DataTable ds = new DataTable();
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                sqlTxt = $"SELECT * FROM dbo.{table} where dbo.{table}.date between '{timeFrom}' and '{timeTo}'";
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader);
                }
            }
            return ds;
        }

        public DataTable SqlGetData(DateTime timeFrom, string table)
        {
            DataTable ds = new DataTable();
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                sqlTxt = $"SELECT * FROM dbo.{table} where dbo.{table}.date  > {timeFrom}";
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader);
                }
            }
            return ds;
        }

        public DataTable SqlGetData(string timeFrom, string timeTo, int typeId)
        {
            string type;
            if (typeId == 1)
            {
                type = "expense";
            }
            else
            {
                type = "income";
            }
            DataTable ds = new DataTable();
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                sqlTxt = $"SELECT * FROM dbo.transactions where dbo.transactions.date between '{timeFrom}' and '{timeTo}' and type ='{type}' order by date";
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader);
                }
            }
            return ds;
        }


        public DataTable SqlGetData(string timeFrom, int typeId)
        {
            string type;
            if (typeId == 1)
            {
                type = "expense";
            }
            else
            {
                type = "income";
            }
            DataTable ds = new DataTable();
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                sqlTxt = $"SELECT transactionId, category, description, amount,date, currencyId FROM dbo.transactions where dbo.transactions.date > '{timeFrom}' and dbo.transactions.type ='{type}'";
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ds.Load(reader);
                }
            }
            return ds;
        }

        public string SqlGetDataSum(string timeFrom, string table)
        {
            string value = "";
            sqlTxt = $"SELECT sum(amount) FROM dbo.{table} where dbo.{table}.date  > '{timeFrom}'";
            switch (table)
            {
                case "income":
                    sqlTxt = $"SELECT sum(amount) FROM dbo.transactions where dbo.transactions.date  > '{timeFrom}' and type = 'income'";
                    break;
                case "expense":
                    sqlTxt = $"SELECT sum(amount) FROM dbo.transactions where dbo.transactions.date  > '{timeFrom} and type = 'expense'";
                    break;
                default:
                    break;
            }

            using (SqlConnection con = new SqlConnection(cnnString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    value = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            return value;
        }

        public string SqlGetDataSum(string timeFrom, string timeTo, string table)
        {
            string value = "";
            sqlTxt = $"SELECT sum(amount) FROM dbo.{table} where dbo.{table}.date  between '{timeFrom}' and '{timeTo}'";
            switch (table)
            {
                case "income":
                    sqlTxt = $"SELECT sum(amount) FROM dbo.transactions where dbo.transactions.date  between '{timeFrom}' and '{timeTo}' and type = 'income'";
                    break;
                case "expense":
                    sqlTxt = $"SELECT sum(amount) FROM dbo.transactions where dbo.transactions.date between '{timeFrom}' and '{timeTo}' and type = 'expense'";
                    break;
                case "savings":
                    sqlTxt = $"SELECT sum(amount) FROM dbo.saving where dbo.saving.date between '{timeFrom}' and '{timeTo}'";
                    break;
                default:
                    break;
            }

            using (SqlConnection con = new SqlConnection(cnnString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    value = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            return value;
        }

        public string SqlGetSavingGoal()
        {
            string goal;
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                sqlTxt = $"select goal from dbo.saving_goal where date =" +
                    $"(select max(date) from dbo.saving_goal where 1=1)";
                using (SqlCommand cmd = new SqlCommand(sqlTxt, con))
                {
                    con.Open();
                    goal = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            return goal;
        }

        public List<int> ChartDataintFlow(string sqlCmd)
        {
            sqlTxt = sqlCmd;
            List<int> integer = new List<int>();
            SqlConnection con = new SqlConnection(cnnString);
            DataSet ds = new DataSet();
            con.Open();

            SqlCommand cmd = new SqlCommand(sqlTxt, con);
            int lastValue = 0;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                integer.Add(lastValue + Convert.ToInt32(dr.GetValue(0)));
                lastValue = lastValue + Convert.ToInt32(dr.GetValue(0));
            }

            return integer;
        }

        public List<int> ChartDataintStack(string sqlCmd)
        {
            sqlTxt = sqlCmd;
            List<int> integer = new List<int>();
            SqlConnection con = new SqlConnection(cnnString);
            DataSet ds = new DataSet();
            con.Open();

            SqlCommand cmd = new SqlCommand(sqlTxt, con);
            int lastValue = 0;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                integer.Add(Convert.ToInt32(dr.GetValue(0)));
                lastValue = Convert.ToInt32(dr.GetValue(0));
            }

            return integer;
        }

        public List<string> ChartDatastring(string sqlCmd)
        {
            sqlTxt = sqlCmd;
            List<string> str = new List<string>();
            SqlConnection con = new SqlConnection(cnnString);
            DataSet ds = new DataSet();
            con.Open();

            SqlCommand cmd = new SqlCommand(sqlCmd, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                str.Add(Convert.ToString(dr.GetValue(0)));
            }

            return str;
        }


        //Delete
        public void SqlDelete(int id, string table)
        {
            string key = "transactionId";
            switch (table)
            {
                case "investment":
                    key = "investmentId";
                    break;
                case "saving":
                    key = "savingId";
                    break;
                case "transactions":
                    key = "transactionId";
                    break;
                case "userlogin":
                    key = "userId";
                    break;
                default:
                    break;
            }
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                string sqlCmd = $"DELETE FROM dbo.{table} WHERE {key} = '{id}'";
                using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }



        //Insertion

        public void SqlInsertionSavingGoal(int goal, LoginManager user)
        {
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                string sqlCmd = $"Insert into dbo.saving_goal(goal, date, userId) values({goal}, '{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")}', '{user.userId}')";
                using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SqlInsertionSavingGoal(int goal, DateTime date, LoginManager user)
        {
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                string sqlCmd = $"Insert into dbo.saving_goal(goal, date, userId) values({goal}, '{date.ToString("yyyy.MM.dd HH:mm:ss")}', '{user.userId}')";
                using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SqlInsertion(Transaction t, LoginManager user)
        {
            string transactionType = t.GetType().Name;
            string sqlCmd = "";
            switch (transactionType)
            {
                case "Expense":
                    sqlCmd = $"INSERT INTO dbo.transactions (userId, type, category, currencyId, date, description, amount)" +
                                   $" VALUES({user.userId}, 'expense', '{t.category}', {t.currencyId}, '{t.date.ToString("yyyy.MM.dd")}', '{t.description}', {t.amount})";
                    break;

                case "Income":
                    sqlCmd = $"INSERT INTO dbo.transactions (userId, type, category, currencyId, date, description, amount)" +
                                  $" VALUES({user.userId}, 'income', '{t.category}', {t.currencyId}, '{t.date.ToString("yyyy.MM.dd")}', '{t.description}', {t.amount})";
                    break;

                case "Saving":
                    sqlCmd = $"INSERT INTO dbo.saving (userId, currencyId, date, amount, investment)" +
                                 $" VALUES({user.userId}, {t.currencyId}, '{t.date.ToString("yyyy.MM.dd")}', {t.amount}, 0)";
                    break;

                case "Investment":
                    break;
                default:
                    break;
            }
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
