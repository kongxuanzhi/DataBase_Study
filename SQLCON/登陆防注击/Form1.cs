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

namespace 登陆防注击
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string constr = "Data source = PC-20150311ANYX;Initial catalog=Test; Integrated security =true";

            using (SqlConnection con = new SqlConnection(constr))
            {
                #region 这样的sql语句会遭受注入攻击。 如用户名中输入"abc' or 1=1 --"这样的语句，就会出现永真的情况，就会登陆成功。相当于"  'abc' or 1==1 --注销掉后面的', 永真的效果
                //string sql = string.Format("select count(*) from Login where PWD='{0}' and Id='{1}'", ID.Text.Trim(), pwd.Text);
                #endregion
                #region 用带参数的command语句来避免这种攻击 方法1
                string sql = "select count(*) from Login where PWD=@password and Id = @Id";
                //SqlParameter p1 = new SqlParameter("@password", SqlDbType.VarChar);
                SqlParameter p2 = new SqlParameter("@Id", SqlDbType.VarChar);
                #endregion
                using (SqlCommand cmd = new SqlCommand(sql,con))
                {
                    con.Open();
                    #region 直接加参数，并设置值
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = ID.Text.Trim();
                    #endregion
                    #region 在前面设置参数，间接赋值
                    cmd.Parameters.Add(p2);
                    p2.Value = pwd.Text;
                    #endregion
                    #region 用带参数的command语句来避免这种攻击 方法2
                    //cmd.Parameters.AddWithValue("@password", ID.Text.Trim());
                    //cmd.Parameters.AddWithValue("@Id", pwd.Text);
                    #endregion
                    int r = Convert.ToInt32(cmd.ExecuteScalar());
                    //cmd.Parameters.Add()
                    if(r>0)
                    {
                        MessageBox.Show("登陆成功!");
                    }
                    else
                    {
                        MessageBox.Show("登录失败!");
                    }
                }
            }
        }
    }
}
