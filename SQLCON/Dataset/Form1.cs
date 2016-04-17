using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dataset
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 创建Class数据库
             DataSet Ds = new DataSet("Class");
            #endregion

            #region 新建Login表
             DataTable dt = new DataTable("Login");
             Ds.Tables.Add(dt);
            #endregion
      
            #region 创建三列
             DataColumn dc = new DataColumn("Id");
             #region 对第0列进行约束
             dc.AutoIncrement = true;
             dc.AutoIncrementSeed = 1;
             dc.AutoIncrementStep = 1;
             dc.Unique = true;
             #endregion
             #region 创建了三列
             dt.Columns.Add(dc);
             dt.Columns.Add("IdName", typeof(string));
             //dt.Columns[1].Unique = true;
             dt.Columns.Add("LoginPwd", typeof(string));
            #endregion       
            
            #region 创建行
             for (int i = 0; i < 10; i++)
             {
                 //这里用表来NewRow出一行
                 DataRow dr = dt.NewRow();
                 //设置这行的各个列的值
                 dr[1] = "孔轩志";
                 dr[2] = "xuanzhi";
                 //加到表中
                 dt.Rows.Add(dr);
             }
            #endregion
            #endregion
            //将DataGridView的数据源绑定到dataset第一个表
            dataGridView1.DataSource = Ds.Tables[0];

            #region 控制台挨个表，挨个行，挨个列输出
            foreach (DataTable Dt in Ds.Tables) //在DataSet中Ds.Tables集合中遍历
            {
                //遍历行
                foreach (DataRow Dr in Dt.Rows)
                {
                    //遍历列
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        //Dr[i]重载的索引器，返回值是object类型，缺点就是增加了拆箱的负担。
                        Console.Write(Dr[i].ToString()+'\t');
                    }
                    Console.WriteLine();
                }
            }            
            #endregion

            //comboBox1.DisplayMember = dt.Columns[2].ToString();
            //comboBox1.ValueMember = dt.Columns[1].ToString();
            //comboBox1.DataSource = Ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Data source=.;Initial Catalog=Test; Integrated Security = true";
            string sql = "select * from 信息131学生";
            #region 使用适配器SqlDataAdapter读取执行链接和操作数据库
            SqlDataAdapter Adapter = new SqlDataAdapter(sql, constr);
            #endregion
            #region 将SqlDataAdapter适配器读到的内容填充到数据库中
            //DataSet ds = new DataSet();
            //Adapter.Fill(ds);
            //dataGridView2.DataSource = ds.Tables[0];
            #endregion
            #region 直接将SqlDataAdapter适配器读到的内容填充到数据库中的表上，并分页，每次取30
            DataTable dt = new DataTable();
            Adapter.Fill(0, 30, dt); 
            #endregion
            #region 将数据绑定到comboBox上 
		    comboBox1.DisplayMember = dt.Columns[1].ToString();
            comboBox1.ValueMember = dt.Columns[0].ToString();
            comboBox1.DataSource = dt;
	        #endregion
            dataGridView2.DataSource = dt;
        }
    }
}
