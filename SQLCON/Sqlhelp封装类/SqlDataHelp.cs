using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Sqlhelp封装类
{
    public class SqlDataHelp
    {
        private readonly static string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
       /// <summary>ExecuteNonQuery类用于执行insert delete update语句
       /// 
       /// </summary>
       /// <param name="sql">带参数的sql语句</param>
       /// <param name="pms">SQLCommand类中SqlParamester类型的可变参数数组</param>
       /// <returns>改变的行数</returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] pms)
        {
            using(SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql,con))
                {
                    con.Open();
                    if(pms != null)//如果传过来的参数是null的话，就会报错
                    {
                        cmd.Parameters.AddRange(pms);
                    }//返回insert delete update 所影响的行数
                    return cmd.ExecuteNonQuery();
                    //cmd.ExecuteScalar
                }
            }
        }
        /// <summary>ExecuteScalar静态方法用于返回表格0,0处的数据
        /// ，由于不知道具体什么类型，所以返回object类型。
        /// </summary>
        /// <param name="sql">带参数的sql语句</param>
        /// <param name="pms">可变参数数组</param>
        /// <returns>首行首列的元素</returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    if(pms!=null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        /// <summary>用于读取数据库中的内容
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns>返回读到的表的头指针</returns>
        public static  SqlDataReader ExecuteReader(string sql,params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(constr);
            try//try-catch语句为了避免执行语句发生异常后，不能关闭链接的情况
            {
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    if(pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

            }catch
            {
                con.Dispose();
                throw;
            }
        }
        /// <summary>用于临时存储从数据库中读取到的内容到DataTable中,这种事
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] pms)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter(sql, constr);
            if(pms != null)
            {
                Adapter.SelectCommand.Parameters.AddRange(pms);
            }
            DataTable datatable = new DataTable();
            Adapter.Fill(datatable);
            return datatable;
        }
    }
}
