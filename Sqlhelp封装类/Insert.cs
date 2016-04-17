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
namespace Sqlhelp封装类
{
    public partial class Insert : Form
    {
        public Insert()
        {
            InitializeComponent();
        }
        public Insert(object parentid)
        {
            InitializeComponent();
            ParentId = parentid;
        }
        public Loadc LoadCat;
        public object ParentId { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "Insert into Catalog(tNames,tparentid) values(@Name, @parentid)";
            SqlParameter[] pms = new SqlParameter[]
            {
                    new SqlParameter("@Name", textBox1.Text),
                    new SqlParameter("@parentid",ParentId)
            };
            SqlDataHelp.ExecuteNonQuery(sql, pms);
            if (LoadCat != null)
            {
                LoadCat();
            }
            MessageBox.Show("添加成功!");
        }
    }
}
