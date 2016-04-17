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

namespace SQLCONN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder C = new SqlConnectionStringBuilder();
            C.DataSource = "PC-20150311ANYX";
            C.InitialCatalog = "Test";
            C.IntegratedSecurity = true;
            C.UserID = "sa";
            C.Password = "";
            C.ConnectTimeout = 30; // 连接数据库最久的时间。
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}
