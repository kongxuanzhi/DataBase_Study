using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using System.Data;
namespace 读文件到数据库_和从中读出数据
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Data source = PC-20150311ANYX; Initial catalog=Test;Integrated security =True;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = "select * from Grades";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if(dr.HasRows)
                        {
                            using(StreamWriter sw = new StreamWriter(@"132班排名.txt"))
                            {
                                while (dr.Read())
                                {
                                    string line = null;
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        line += dr[i].ToString() + "    ";
                                    }
                                    sw.WriteLine(line);
                                }
                                MessageBox.Show("写入成功");
                                sw.Close();
                            }  
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string constr = "Data source=PC-20150311ANYX; Initial catalog=Test; Integrated security = true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                //参数不用加' '
                string sql = "Insert into Grades values(@学号,@姓名,@平均绩点,@专业排名)";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    string line=null;
                    #region 方法3、循环外设置变量参数传值,使用System.Data.SqlDbTypeF的类型属性
                    //SqlParameter p1 = new SqlParameter("@学号", SqlDbType.VarChar); cmd.Parameters.Add(p1);
                    //SqlParameter p2 = new SqlParameter("@姓名", SqlDbType.VarChar); cmd.Parameters.Add(p2);
                    //SqlParameter p3 = new SqlParameter("@平均绩点", SqlDbType.VarChar); cmd.Parameters.Add(p3);
                    //SqlParameter p4 = new SqlParameter("@专业排名", SqlDbType.Int); cmd.Parameters.Add(p4);
                    #endregion

                    #region 方法4、使用参数数组来初始化，用AddRange来添加到cmd中
                    SqlParameter[] ps = new SqlParameter[]
                    {
                        new SqlParameter("@学号",SqlDbType.VarChar),
                        new SqlParameter("@姓名",SqlDbType.VarChar),
                        new SqlParameter("@平均绩点",SqlDbType.VarChar),
                        new SqlParameter("@专业排名",SqlDbType.Int)
                    };
                    cmd.Parameters.AddRange(ps);
                    #endregion
                    using (StreamReader SR = new StreamReader(@"信息132班排名.txt"))
                    {
                        while(!SR.EndOfStream)
                        {
                            line = SR.ReadLine();
                            string[] Line = line.Split(new char[]{ '\t',' ' });
                            #region 方法1、用AddWithValue为cmd增加参数，为sql语句中的全局变量赋值
                            //cmd.Parameters.AddWithValue("@学号", Line[0]);
                            //cmd.Parameters.AddWithValue("@姓名", Line[1]);
                            //cmd.Parameters.AddWithValue("@平均绩点", Line[2]);
                            //cmd.Parameters.AddWithValue("@专业排名", Line[3]);
                            #endregion

                            #region 方法2、再循环内创建参数对象，用Add为cmd增加参数，为sql语句中的全局变量赋值
                            //SqlParameter p1 = new SqlParameter("@学号", Line[0]); cmd.Parameters.Add(p1);
                            //SqlParameter p2 = new SqlParameter("@姓名", Line[1]); cmd.Parameters.Add(p2);
                            //SqlParameter p3 = new SqlParameter("@平均绩点", Line[2]); cmd.Parameters.Add(p3);
                            //SqlParameter p4 = new SqlParameter("@专业排名", Line[3]); cmd.Parameters.Add(p4);
                            //cmd.ExecuteNonQuery();
                            //cmd.Parameters.Clear();//变量值在循环中传值，会重复。所以清空后，在赋值。
                            #endregion

                            #region 方法3、//在循环外将参数添加到cmd的sql语句中，在循环内用参数属性Value赋值就行了。
                            //p1.Value = Line[0];
                            //p2.Value = Line[1];
                            //p3.Value = Line[2];
                            //p4.Value = Line[3];
                            //cmd.ExecuteNonQuery();
                            #endregion
                           
                            #region 方法4、用参数数组，循环来设置参数
                            for (int i = 0; i < Line.Length; i++)
                            {
                                ps[i].Value = Line[i];
                            }
                            cmd.ExecuteNonQuery();
                            #endregion                           
 
                            #region 控制台测试代码
                            //Console.WriteLine(Line.Length);
                            //for (int i = 0; i < Line.Length; i++)
                            //{
                            //    Console.Write(Line[i] + " ");
                            //}
                            //Console.WriteLine();
                            #endregion
                            
                        }
                        MessageBox.Show("写入数据库成功!");
                        SR.Close();
                    }
                }
               // 
            }
        }


    }
}
