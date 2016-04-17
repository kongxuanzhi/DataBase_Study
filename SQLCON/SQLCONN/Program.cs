using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Windows.Forms;
namespace constrCONN
{
    class Program
    {
        static void Main(string[] args)
        {

            #region 连接数据库类
            string constr = "Data Source=PC-20150311ANYX;Initial Catalog=Test;Integrated Security=True";
            //连接字符串其实是 SqlConnectionStringBuilder的属性的添加
            //SqlConnectionStringBuilder C = new SqlConnectionStringBuilder();
            //C.DataSource = "PC-20150311ANYX";
            //C.InitialCatalog = "Test";
            //C.IntegratedSecurity = true;
            //C.UserID = "sa";
            //C.Password = "";
            //C.ConnectTimeout = 30; // 连接数据库最久的时间。

            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = "select Count(*) from CallRecods_bp";
                using (SqlCommand cmd = new SqlCommand())
                {
                    #region 数据库连接事件，判断是否连接上
                    con.StateChange += con_StateChange;
                    #endregion
                    #region 通过属性创建连接
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    #endregion
                    #region 在连接打开时检测异常
                    try
                    {
                        con.Open();
                        Console.WriteLine("open");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    con.Close();
                    #endregion
                    #region 由于连接不能重复打开，所以在打开之前，先判断是否打开了，用State属性
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    #endregion
                    #region 由于这些连接都继承了这个销毁的类所以用Dispose销毁连接。
                    con.Dispose();
	                #endregion
                }

            }
            Console.Read();
            #endregion
            
        }

        private static void con_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            Console.WriteLine(e.CurrentState.ToString());
        }

    }
}
