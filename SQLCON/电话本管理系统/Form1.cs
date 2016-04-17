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
            #region 数据绑定
            //禁用自动生成列，手动生成
           // dataGridView1.AutoGenerateColumns = false;
            string sql = "select pn.pName,pn.pCellPhone,pt.ptname, pn.pid from phoneType as pt  join phonenum as pn on pt.ptid = pn.ptypeid";
            DataTable dt = SqlDataHelp.ExecuteDataTable(sql);
            dataGridView1.DataSource = dt;

            comboBox2.ValueMember = dt.Columns[0].ColumnName;
            comboBox2.DataSource = dt;
            #endregion
            comboBox1.SelectedIndex = 0;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            #region dataGridView1 被选中发生的事件，填充下面的内容
            string name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string phonenum = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string group = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

            string sql = "select ptname from phoneType where ptname=@group";
            SqlParameter p = new SqlParameter("@group", group);

            object Group = SqlDataHelp.ExeCuteScalar(sql, p);
            tbName.Text = name;
            tbphonenum.Text = phonenum;
            tbgroup.Text = Group.ToString();
            label5.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            #endregion
            //MessageBox.Show(phonenum);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region  拼接字符串，达到多条件搜索的效果,太弱
           // StringBuilder sql = new StringBuilder("select * from Phonenum where 1=1");
           // if(comboBox1.SelectedIndex != 0)
           // {
           //     sql.Append(" and ptypeid ="+comboBox1.Text.Split(',')[0]);
           // }
           // if(textBox1.Text.Trim().Length > 0)
           // {
           //     sql.Append(" and where pName like '%"+textBox1.Text.Trim()+"%'");
           // }
           // if(textBox2.Text.Trim().Length > 0)
           // {
           //     sql.Append(" and where pcellphone like '%"+textBox2.Text.Trim()+"%'");
           // }
           ////sql
           // MessageBox.Show(sql.ToString());
            //SqlDataHelp.ExeCuteReader();
            #endregion

            #region 使用List集合在搜索条件间添加and，
            List<string> wheres = new List<string>();
            List<SqlParameter> pms = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder("select pn.pName,pn.pCellPhone,pt.ptname, pn.pid from phoneType as pt  join phonenum as pn on pt.ptid = pn.ptypeid");
            if (comboBox1.SelectedIndex != 0)
            {
                //sql.Append(" and ptypeid =" + comboBox1.Text.Split(',')[0]);
                wheres.Add(" ptypeid like @Id");
                pms.Add(new SqlParameter("@Id", "%" + comboBox1.Text.Split(',')[0] + "%"));
            }
            if (textBox1.Text.Trim().Length > 0)
            {
                //sql.Append(" and where pName like '%" + textBox1.Text.Trim() + "%'");
                wheres.Add(" pName like @name");
                pms.Add(new SqlParameter("@name", "%" +textBox1.Text.Trim()+"%"));
            }
            if (textBox2.Text.Trim().Length > 0)
            {
                //sql.Append(" and where pcellphone like '%" + textBox2.Text.Trim() + "%'");
                wheres.Add(" pcellphone like @phone");
                pms.Add(new SqlParameter("@phone", "%" + textBox2.Text.Trim() + "%"));
            }
            if(wheres.Count > 0)
            {
                //在指定集合内容之间添加指定字符串。有一个就不加，有两个就在中间加and，有三个就在两个之间加
                string wh = string.Join(" and ",wheres.ToArray());
                sql.Append(" where "+wh); //只有有条件才会加上where
            }
            #endregion
            #region 对得到的sql语句和参数进行操作，这里把他绑定到了datagridview1上了
            DataTable dt = SqlDataHelp.ExecuteDataTable(sql.ToString(), pms.ToArray());
            dataGridView1.DataSource = dt;
            comboBox2.ValueMember = dt.Columns[0].ColumnName;
            comboBox2.DataSource = dt;
            #endregion
            // MessageBox.Show(sql.ToString());
        }
    }
}
