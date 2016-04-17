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

namespace 电话本管理系统
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //禁用自动生成列，手动生成
            dataGridView1.AutoGenerateColumns = false;

            string sql = "select pn.pName,pn.pCellPhone,pt.ptname, pn.pid from phoneType as pt  join phonenum as pn on pt.ptid = pn.ptypeid";
            DataTable dt = SqlDataHelp.ExecuteDataTable(sql);
            dataGridView1.DataSource = dt;
           
            comboBox2.ValueMember = dt.Columns[0].ColumnName;
            comboBox2.DataSource = dt;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            string name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string phonenum = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string group = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() ;
            string sql = "select ptname from phoneType where ptname=@group";
            SqlParameter p = new SqlParameter("@group", group);
            object Group = SqlDataHelp.ExeCuteScalar(sql, p);
            tbName.Text = name;
            tbphonenum.Text = phonenum;
            tbgroup.Text = Group.ToString();
            label5.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
           // MessageBox.Show(phonenum);
        }
    }
}
