using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MountainFinance
{
    class LoginManager 
    {
        private static string cnnString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\peter\\Source\\Repos\\MountainFinance\\MountainFinance\\MfDb.mdf;Integrated Security=True";
        public int userId = -1;
        static LoginManager d;
        public string userName;
        public string password;

        private LoginManager(string userName, string password) 
        { 
            this.userName = userName; 
            this.password = password;

            SqlGetData(userName, password);
        }

        public static LoginManager GetSingleton()
        {
            if (d != null)
            {
                return d;
            }
            else
            {
                return null;
            }
            
        }

        public static LoginManager GetSingleton(string username, string password)
        {
            if (d == null)
            {

                d = new LoginManager(username, password);

            }
            else
            {
                while (d.password != password && d.userName != username)
                {
                    d = null;
                    d = new LoginManager(username, password);
                }
            }
            return d;
        }

        public static LoginManager GetSingleton(string username, string email, string password)
        {

            if (d == null)
            {
                string sqlCmd = $"insert into dbo.userlogin(username, email,password) values('{username}','{email}','{password}')";

                using (SqlConnection con = new SqlConnection(cnnString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                d = new LoginManager(username, password);

            }
            return d;
        }

        public void SqlNewUserRegistration(string username, string email, string password)
        {
            
        }

        public void SqlGetData(string userName, string password)
        {
            using (SqlConnection con = new SqlConnection(cnnString))
            {
                using (SqlCommand cmd = new SqlCommand($"Select dbo.userlogin.userId FROM dbo.userlogin WHERE dbo.userlogin.username = '{userName}' and dbo.userlogin.password = '{password}'", con))
                {
                    con.Open();
                    if (cmd.ExecuteScalar() != null )
                    {
                        this.userId = (int)cmd.ExecuteScalar();
                    }
                    else
                    {
                        this.userId = -1;
                    }
                   
                    
                }
            }
        }

        public string SqlGetUsername()
        {
            string reader = "";
            DataTable ds = new DataTable();
            if (userId != -1)
            {

                using (SqlConnection con = new SqlConnection(cnnString))
                {
                    using (SqlCommand cmd = new SqlCommand($"SELECT username FROM dbo.userlogin where dbo.userlogin.userId = {userId}", con))
                    {
                        con.Open();
                        reader = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            return reader;
        }
    }
}
