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
using Sqlhelp封装类;

namespace 分页
{
    public partial class Form1 : Form
    {
        
        string sql = "select * from (select *, row_number() over(order by studentid) as id from 理学院基本情况) as temp  where id between (@curPage*@front+1) and (@curPage*@page)";
        private int total;
        private int curPage;
        private int eachpage;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            eachpage = 18;
            curPage = 1;
            string sql2 = "select count(*) from 理学院基本情况";
            total = (Convert.ToInt32(SqlDataHelp.ExecuteScalar(sql2)) + eachpage-1) / eachpage;
            //MessageBox.Show(total.ToString());
            //go(sql);
        }
        private void go(string sql)
        {
            SqlParameter[] pms = new SqlParameter[] 
            {
                new SqlParameter("@front",curPage-1),
                new SqlParameter("@page", curPage),
                new SqlParameter("@curPage",eachpage)
            };
            DataTable dt = SqlDataHelp.ExecuteDataTable(sql, pms);
            this.dataGridView1.DataSource = dt;
        }
        private void back_Click(object sender, EventArgs e)
        {
            if (curPage > 1)
                curPage--;
        }
        private void next_Click(object sender, EventArgs e)
        {
            if (curPage<total)
            curPage++;
        }
        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            go(sql);
            label2.Text = curPage.ToString() + "/" + total.ToString();
        }

        private void go2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text !="")
            {
                int t = Convert.ToInt32(textBox1.Text);
                if (t >= 1 && t <= total)
                {
                    curPage = t;
                }
            }
            
        }
    }
}
