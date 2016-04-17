using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using 管理图书分类;

namespace 管理图书升级
{
    public partial class Form1 : Form
    {
        InsertOk IO;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int rootParent = -1;
            //RecursionCatalog Rc = new RecursionCatalog(this,rootParent);
            LoadCatalog(rootParent);
        }
        private void LoadCatalog(int p)
        {
            List<Catalog> CLog = new List<Catalog>();
            #region 初始化List<Catalog>集合
            GetList(p, CLog);
            #endregion
            #region 讲根节点加入到treeView控件上
            for (int i = 0; i < CLog.Count; i++)
            {

                TreeNode tnode = this.treeView1.Nodes.Add(CLog[i].tNames);
                tnode.Tag = CLog[i].tId;
                tnode.ContextMenu = this.treeView1.ContextMenu;
                tnode.ContextMenuStrip = this.contextMenuStrip1;
                LoadSubCatalog(CLog[i].tId, tnode);
            }
            #endregion
            #region 测试代码
            //string sqlt = "select tId, tNames from Catalog where tParentId=1";
            //LoadCatalog(sqlt);
            #endregion
        }
        private void LoadSubCatalog(int p, TreeNode root)
        {
            List<Catalog> SubCat = new List<Catalog>();
            #region 初始化子数组的List
            GetList(p, SubCat);
            #endregion
            #region 将下拉菜单加到桌面上
            for (int i = 0; i < SubCat.Count; i++)
            {
                TreeNode subroot = root.Nodes.Add(SubCat[i].tNames);
                subroot.Tag = SubCat[i].tId;
                LoadSubCatalog(SubCat[i].tId, subroot);
                if (i == SubCat.Count-1)
                  subroot.ContextMenuStrip = contextMenuStrip2;
                else
                {
                    subroot.ContextMenuStrip = contextMenuStrip1;
                }
            }
            #endregion
        }
        private void GetList(int p, List<Catalog> CLog)
        {
            string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = "select tId,tNames from Catalog where tParentId=@ID";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlParameter p1 = new SqlParameter("@ID", p);
                    cmd.Parameters.Add(p1);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int Id = reader.GetInt32(0);
                                string Name = reader.GetString(1);
                                Catalog CL = new Catalog() { tId = Id, tNames = Name };
                                CLog.Add(CL);
                            }
                        }
                    }
                }
            }
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
            
            List<ContentInfo> LCF = new List<ContentInfo>();
            LCF.Clear();
            //双击之后会重复输入，有问题.下面这句已解决。
            listBox1.Items.Clear();
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
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
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
                for (int i = 0; i < LCF.Count; i++)
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
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CNTID", cf.DID);
                    con.Open();
                    string article = Convert.ToString(cmd.ExecuteScalar());
                    textBox1.Text = article;
                }
            }
        }

        private void 增加子目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //TreeNode catalog = (TreeNode)sender;
            MessageBox.Show(this.treeView1.SelectedNode.Tag.ToString());
            #region Insert对话框
            //string InsertItemName = 
            IO = new InsertOk();
            //IO.Show();
            IO.ShowDialog();
            if(IO.name!=" ")//
            {
                #region 链接数据库,将内容添加到数据库
                string sql = "insert into Catalog values(@name, @tag, @note)";
                InsertInto(sql, IO.name,IO.ARTICLE, this.treeView1.SelectedNode.Tag);
                this.Refresh();
                #endregion
            }
            #endregion

        }

        

        private void InsertInto(string sql, string p1, string article, object p2)
        {
            string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            using(SqlConnection con=  new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql,con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@name",p1);
                    cmd.Parameters.AddWithValue("@tag",p2);
                    string p3 = article; 
                    cmd.Parameters.AddWithValue("@note", p3);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void 删除子目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(O.ID.ToString());
            string sql = "delete Catalog where tid = @tag";
            DeleteRow(sql, this.treeView1.SelectedNode.Nodes);
            
        }
        private void DeleteRow(string sql, TreeNodeCollection tree)
        {
            string constr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    foreach(TreeNode tn in tree)
                    {
                        int p = Convert.ToInt32(tn.Tag);
                        cmd.Parameters.AddWithValue("@tag", p);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
    }
}
