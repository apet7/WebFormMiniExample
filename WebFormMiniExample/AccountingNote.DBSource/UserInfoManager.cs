using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBSource
{

    public class UserInfoManager
    {
        private static string GetConnectionString()
        {
            string val = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return val;
        }
        public static DataRow GetUserInfoByAccount(string account)
        {
            string connectionString = GetConnectionString();
            string dbCommandStirng =
                @"SELECT [ID], [Account], [PWD], [Name], [Email]
                    FROM UserInfo    
                    WHERE [Account] = @account
                ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand comand = new SqlCommand(dbCommandStirng, connection))
                {
                    comand.Parameters.AddWithValue("@account", account);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = comand.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        reader.Close();

                        // 假設是0筆資料，回傳null                     
                        if (dt.Rows.Count == 0)
                            return null;

                        // 假設是不是0筆，回傳資料
                        DataRow dr = dt.Rows[0];
                        return dr;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(ex);
                        return null;
                    }
                }
            }
        }

        
    }
}
