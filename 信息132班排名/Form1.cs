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

namespace 信息132班排名
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

        string sql=null;
        string constr = "data source=PC-20150311ANYX; initial catalog=Test; integrated security = true";
        private void add_Click(object sender, EventArgs e)
        {
            sql = string.Format("insert into Grades values('{0}','{1}','{2}',{3})",
                    textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            Change();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            sql = string.Format("update Grades set 学号='{0}',姓名='{1}',平均绩点 = '{2}',专业排名='{3}' where 序号={4}",textBox1.Text,textBox2.Text,textBox3.Text,textBox4.Text,textBox5.Text);
             Change();
        }
        private void Change()
        {
            int r = 0;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    r = cmd.ExecuteNonQuery();
                }
            }
            //MessageBox.Show("更新了" + r + "行");
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO:  这行代码将数据加载到表“testDataSet.Grades”中。您可以根据需要移动或删除它。
            this.gradesTableAdapter.Fill(this.testDataSet.Grades);
        }

        private void Print_Click(object sender, EventArgs e)
        {
            sql = "select * from Grades";
            Read();
        }
        private void Read()
        {
            int r = 0;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    //r = cmd.ExecuteNonQuery();
                    SqlDataReader sqlread = cmd.ExecuteReader();
                    if(sqlread.HasRows)
                    {
                        while(sqlread.Read())
                        {
                            for (int i=0;i<sqlread.FieldCount; i++)
                            {
                                Console.Write(sqlread[i].ToString()+' ');
                            }
                            Console.WriteLine( );
                        }
                        
                    }
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            sql = string.Format("delete from Grades where 序号={0}",textBox5.Text);
            Change();
        }
    }
}
