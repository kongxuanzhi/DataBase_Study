using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace readDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string conStr = "data source=PC-20150311ANYX; Initial Catalog = Test; Integrated security = true;";
            int r = 0;
            using(SqlConnection con = new SqlConnection(conStr))
            {
                //string sql = string.Format("insert into Orders values('{0}','{1}')",textBox1.Text, textBox2.Text);
                string sq1 = string.Format("select * from Person");
                using(SqlCommand cmd = new SqlCommand(sq1,con))
                {
                    con.Open();
                    #region SqlCommand类中三个方法：ExecuteNonQuery，ExecuteScalar，ExecuteReader
                    #region 使用ExecuteNonQuery方法insert， delete，update元素到表中，返回改变的行数
                    r = cmd.ExecuteNonQuery();
                    #endregion
                    #region 使用ExecuteScalar()方法读取表的开头0,0位置的元素。返回object
                    object o = cmd.ExecuteScalar();  //如果是空表的话返回null就会报 异常
                    //当表中没有数据时，返回null，当表中有数据，但是查询的该数据就是NULL时返回{}。其他正常返回
                    if(o != null)//DBNull.Value != null
                    {
                        ;// MessageBox.Show(o.ToString());
                    }
                   
                    #endregion
                    #region 使用SqlDataReader类中的Read方法来一行一行读取表
                    SqlDataReader dateread = cmd.ExecuteReader();//异常在如果是空表的话，就会出错
                    //if(dateread.HasRows) //如果有一行就读，没有行就不读
                    //{
                    //    while(dateread.Read())// 一行一行的读
                    //    {
                    //        Console.Write(dateread[0]+" "); //用索引和GetValue(第i列)是一样的。
                    //        Console.Write(dateread.GetValue(1) + " ");
                    //        Console.Write(dateread.GetValue(2)+"\n");
                    //    }
                    //}
                    //索引是按列增加
                    while (dateread.Read())
                    {
                        string str = "";
                        for (int i = 0; i < dateread.FieldCount; i++)
                        {
                            str += dateread[i].ToString() + '\t';
                        }
                        //MessageBox.Show(str);
                        Console.WriteLine(str);
                    }
                    Console.WriteLine();
                    #endregion
                    #endregion
                }
            }
            // MessageBox.Show("更新了"+r+"行");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region 执行插入语句，返回插入行的自动编号
            string constr = "data source=PC-20150311ANYX; initial catalog=Test; integrated security = true";
            int r=0;
            using (SqlConnection con = new SqlConnection(constr))
            {
                //string sql = string.Format("insert into Person output inserted.Id_p values('{0}','{1}','{2}','{3}')");
                string sql = string.Format("insert into Person values('{0}','{1}','{2}','{3}')",
                    textBox1.Text,textBox2.Text,textBox3.Text,textBox4.Text);
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                   // if (con.State == ConnectionState.Open)
                     //  ;//Console.WriteLine("open");
                    con.Open();
                    r = cmd.ExecuteNonQuery();
                    #region 由于上面的sql语句中output inserted.Id_p输出了插入行的自动编号，所以这里ExecuteScalar返回更新行号
                    ////object rr = cmd.ExecuteScalar();
                    //object rr = Convert.ToInt32(cmd.ExecuteScalar());
                    //Console.WriteLine(rr.ToString());
                    #endregion

                }
            }
            MessageBox.Show("更新了"+r+"行");
            
            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int r = 0;
            string constr = "data source=PC-20150311ANYX; initial catalog=Test; integrated security = true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = string.Format(@"update Person set LastName='{0}',FirstName='{1}',
                Address='{2}', City='{3}' where Id_P = {4}", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text,textBox5.Text);
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    try
                    { con.Open(); 
                    }
                    catch(Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                    r = cmd.ExecuteNonQuery();
                    
                }
            }
            button1_Click(null, null);
            MessageBox.Show("update"+r+"row");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string constr = "data source=PC-20150311ANYX; initial catalog=Test; Integrated security=true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = null;
                if(textBox5.Text!="")
               {    
                    //问题：如果这里textBox5没赋值，会产生异常，怎么办
                    sql = string.Format("delete from Person where Id_P = {0}", textBox5.Text);
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //con.Close();
                        //MessageBox.Show("delete successfully");
                    }
               }
            }
            button1_Click(null,null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string constr = "data source=PC-20150311ANYX;initial catalog=Test; integrated security = true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = string.Format("truncate table Person");
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
