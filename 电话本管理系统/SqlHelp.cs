using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace 电话本管理系统
{
    public static class SqlDataHelp
    {
       private readonly static string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
       
       public static int ExecuteNonQuery(string sql, params SqlParameter[] pms)
       {
           using (SqlConnection con = new SqlConnection(constr))
           {
               using(SqlCommand cmd = new SqlCommand(sql, con))
               {
                   con.Open();
                   if(pms != null)
                   {
                       cmd.Parameters.AddRange(pms);
                   }
                   return cmd.ExecuteNonQuery();
               }
           }
       }

       public static object ExeCuteScalar(string sql, params SqlParameter[] pms)
       {
           using (SqlConnection con = new SqlConnection(constr))
           {
               using (SqlCommand cmd = new SqlCommand(sql, con))
               {
                   con.Open();
                   if(pms != null)
                   {
                       cmd.Parameters.AddRange(pms);
                   }
                   return cmd.ExecuteScalar();
               }
           }
       }
       public static SqlDataReader ExeCuteReader(string sql, params SqlParameter[] pms)
       {
           SqlConnection con = new SqlConnection(constr);
           try
           {
               using (SqlCommand cmd = new SqlCommand(sql, con))
               {
                   con.Open();
                   if (pms != null)
                   {
                       cmd.Parameters.AddRange(pms);
                   }
                   return cmd.ExecuteReader(CommandBehavior.CloseConnection);
               }
           }
           catch
           {
               con.Dispose();
               throw;
           }
       }

       public static DataTable ExecuteDataTable(string sql, params SqlParameter[] pms)
       {
           using(SqlDataAdapter adapter = new SqlDataAdapter(sql, constr))
           {
               if(pms != null)
               {
                   adapter.SelectCommand.Parameters.AddRange(pms);
               }
               DataTable dt = new DataTable();
               adapter.Fill(dt);
               return dt;
           }
           
       }
    }
}
