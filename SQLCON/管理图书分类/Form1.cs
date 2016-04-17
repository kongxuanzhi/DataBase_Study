using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace 管理图书分类
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int rootParent = -1;
            #region 通过RecursionCatalog在构造方法里，添加了目录。
            RecursionCatalog Catalog = new RecursionCatalog(this.treeView1, rootParent);
            #endregion
            //LoadCatalog(rootParent);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            #region 测试代码
            //listBox1.Items.Clear();
            //listBox1.Items.Add(e.Node.Text + " " + e.Node.Tag);
            //textBox1.Text = ;
            // MessageBox.Show();//
            // select ContentInfo.dName from ContentInfo,Catalog where ContentInfo.dTId = Catalog.tId
            //MessageBox.Show(e.Node.GetNodeCount(true).ToString());
            #endregion
            //双击之后会重复输入，有问题
            List<ContentInfo> LCF = new List<ContentInfo>();
            LCF.Clear();
            #region 初始化List中ContentInfo的值
            if (e.Node.GetNodeCount(true) == 0) //Level表示结点的深度 GetNodeCount(true)返回子节点个数
            {
                string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string sql = "select ContentInfo.dId, ContentInfo.dName from ContentInfo where ContentInfo.dTId =@PID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@PID", e.Node.Tag);
                        Console.WriteLine(e.Node.Tag);
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                while(reader.Read())
                                {
                                    int dId = reader.GetInt32(0);
                                    string dName = reader.GetString(1);
                                    ContentInfo cf = new ContentInfo();
                                    cf.DID = dId;
                                    cf.DName = dName;

                                    LCF.Add(cf);
                                }
                            }
                        }
                    }
                }
                for(int i=0;i<LCF.Count; i++)
                {
                    ContentInfo cf = new ContentInfo();
                    cf.DID = LCF[i].DID;
                    cf.DName = LCF[i].DName;
                    listBox1.Items.Add(LCF[i]);
                }
            }
            #endregion
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //文章储存在哪里？？？？？
            textBox1.Clear();
            ContentInfo cf = (ContentInfo)listBox1.SelectedItem;
            //MessageBox.Show(cf.DName+cf.DID);
            string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = " select dContent from ContentInfo where did = @CNTID";
                using (SqlCommand cmd = new SqlCommand(sql,con))
                {
                    cmd.Parameters.AddWithValue("@CNTID",cf.DID);
                    con.Open();
                    string article = Convert.ToString(cmd.ExecuteScalar());
                    textBox1.Text = article;
                }
            }
        }
    }
}
