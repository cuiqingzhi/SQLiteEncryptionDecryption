using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace SQLiteEncryptionDecryption
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            {
                //使用Microsoft.Data.Sqlite创建加密连接字符串
                string dbName = Path.Combine(Environment.CurrentDirectory, "SampleDB.db");
                dbName = @"D:\DB\SampleDB111111111.db";
                string connStr = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder()
                {
                    DataSource = dbName,
                    Mode = Microsoft.Data.Sqlite.SqliteOpenMode.ReadWriteCreate,
                    Password = "admin"
                }.ToString();

                ConnectionConfig config = new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.Sqlite,
                    ConnectionString = connStr,
                    IsAutoCloseConnection = false,
                };
                 
                using (SqlSugarClient client = new SqlSugarClient(config))
                {
                    client.Open();
                    client.CodeFirst.InitTables(typeof(User));
                    for (int i = 0; i < 5; i++)
                    {
                        User user = new User();
                        user.Name = "测试" + (i + 1).ToString();
                        client.Insertable<User>(user).ExecuteCommand();
                    }
                    var data = client.Queryable<User>().ToList();

                    client.Close();
                }
            }
            Application.Run(new Form1());
        }
    }
}
