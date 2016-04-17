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
using System.IO;

namespace Sqlhelp封装类
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public Loadc LoadCat;
        private void Form2_Load(object sender, EventArgs e)
        {

            listBox1.ContextMenuStrip = contextMenuStrip3;
            textBox1.ContextMenuStrip = contextMenuStrip4;
        }

        private void 在当前目录下增加子目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(treeView1.SelectedNode.Tag.ToString());
            Insert insertWindow = new Insert(treeView1.SelectedNode.Tag);
            insertWindow.LoadCat = LoadCat;
            insertWindow.Show();
        }

        private void 删除该目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "delete from Catalog where tid = @ID";
            SqlParameter p = new SqlParameter("@ID",treeView1.SelectedNode.Tag);
            int r = SqlDataHelp.ExecuteNonQuery(sql,p);
            if(r>0)
            {
                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK);
            }
            if(LoadCat != null)
            {
                LoadCat();
            }
        }

        private void 添加文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt|*.txt|*|*.*";
            DialogResult result =  openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                string AllfileName = openFileDialog1.FileName;
                //StreamReader reader = new StreamReader(AllfileName,);
                //选择正确的编码Gb2312能显示， 通过File静态方法读取文件所有内容
                string fileText = File.ReadAllText(AllfileName,Encoding.GetEncoding("Gb2312"));
                //通过Path类获得文件名，不包括完整路径和后缀名
                string filename = Path.GetFileNameWithoutExtension(AllfileName);
                //MessageBox.Show(filename);
                //MessageBox.Show(fileText);
                string sql = "Insert into ContentInfo(dtid, dname,dcontent) values(@parentId,@name, @Content)";
                SqlParameter[] pms = new SqlParameter[]
                {
                    new SqlParameter("@parentId",treeView1.SelectedNode.Tag),
                    new SqlParameter("@name",filename),
                    new SqlParameter("@Content",fileText)
                };
                SqlDataHelp.ExecuteNonQuery(sql,pms);
            }
        }

        private void 删除文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "delete from Catalog where tid =@Id";
           // MessageBox.Show(treeView1.SelectedNode.Tag.ToString());
            SqlParameter p = new SqlParameter("@Id",treeView1.SelectedNode.Tag);
            SqlDataHelp.ExecuteNonQuery(sql,p);
            if(LoadCat != null)
            {
                LoadCat();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Clear();
            listBox1.Items.Clear();
            string sql = "select * from ContentInfo where Dtid = @ParantId";
            SqlParameter p = new SqlParameter("@ParantId", treeView1.SelectedNode.Tag);
            List<Content> Contents = new List<Content>();
            using (SqlDataReader reader = SqlDataHelp.ExecuteReader(sql, p))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        #region 获取Content信息
                        Content cont = new Content();
                        cont.Id = reader.GetInt32(0);
                        cont.ParantId = reader.GetInt32(1);
                        cont.ContentName = reader.GetString(2);
                        if(reader.IsDBNull(3) != true)
                            cont.ContentText = reader.GetString(3);
                        //不能对datetime为null的项进行取值
                        //cont.StartTime = reader.GetDateTime(4);
                        //cont.EndTime = reader.GetDateTime(5);
                        //cont.IsDelete = reader.GetByte(6);
                        cont.IsSaved = reader.GetInt32(7);
                        #endregion
                        Contents.Add(cont);
                    }
                }
            }
            foreach (Content Contentitem in Contents)
            {
                listBox1.Items.Add(Contentitem);
            }
            //设置listBox1默认选中第一项
            if(listBox1.Items.Count>0)
                 this.listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            Content cont = (Content)(listBox1.SelectedItem);
            //IsHaveSaved();
            string sql = "select dcontent from ContentInfo where did = @ID";
            SqlParameter p = new SqlParameter("@ID",cont.Id);
            using(SqlDataReader reader = SqlDataHelp.ExecuteReader(sql, p))
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        if(!reader.IsDBNull(0))
                            textBox1.Text = reader.GetString(0);
                    }
                }
            } 
        }
        private void 删除该文章ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "delete from ContentInfo where did = @ID";
            if (listBox1.SelectedIndex >= 0)
            {
                Content cont = (Content)(listBox1.SelectedItem);
                SqlParameter p = new SqlParameter("@ID", cont.Id);
                SqlDataHelp.ExecuteNonQuery(sql, p);
                MessageBox.Show("删除成功");
                listBox1.Items.Clear();
                textBox1.Clear();
                //treeView1_AfterSelect(null,null);
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "update contentInfo set dContent=@Content,IsSaved = 1 where did = @Id";
            if (listBox1.SelectedIndex >= 0)
            {
                Content con = (Content)(this.listBox1.SelectedItem);
                SqlParameter[] pms = new SqlParameter[]
                {
                       new SqlParameter("@Content", textBox1.Text),
                       new SqlParameter("@Id", con.Id)
                };
                int r = SqlDataHelp.ExecuteNonQuery(sql, pms);
                if (r > 0)
                {
                    MessageBox.Show("文档已更新");
                }
            }
        }
        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            AddArticle add = new AddArticle();
            add.ShowDialog();
            //MessageBox.Show(add.NAME);
            if(add.NAME!=null)
            {
                 string sql = "Insert Into ContentInfo(dTid,dName) values(@Id,@Name)";
                 SqlParameter[] pms = new SqlParameter[]
                 {
                       new SqlParameter("@Id",treeView1.SelectedNode.Tag),
                       new SqlParameter("@Name",add.NAME),
                 };
                 SqlDataHelp.ExecuteNonQuery(sql, pms);
            }
            textBox1.Clear();
            listBox1.Items.Clear();
            if(LoadCat != null)
            {
                LoadCat();
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                AddArticle add = new AddArticle();
                add.ShowDialog();
                if (add.NAME != null)
                {

                    Content cont = (Content)(listBox1.SelectedItem);
                    string sql = "update ContentInfo set dName=@Name where did = @ID";
                    SqlParameter[] pms = new SqlParameter[]
                    {
                            new SqlParameter("@Name",add.NAME),
                            new SqlParameter("@ID",cont.Id)
                    };
                    SqlDataHelp.ExecuteNonQuery(sql, pms);
                }
            }
        }

        private void 导出该文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开文件夹路径
            FolderBrowserDialog OFD = new FolderBrowserDialog();
            if(OFD.ShowDialog() == DialogResult.OK)
            {
                string filePath = OFD.SelectedPath;
                if(listBox1.SelectedIndex>=0)
                {
                    Content cont = (Content)(listBox1.SelectedItem);
                    string FileName = cont.ContentName;
                    if (FileName != null)
                    {
                        //设置当前路径
                        Directory.SetCurrentDirectory(filePath);
                        //在当前文件夹下面创建文件
                        using (StreamWriter sw2file = new StreamWriter(FileName + ".txt"))
                        {
                            //文件内容就是TextBox1内容，用write写进去。
                            sw2file.Write(textBox1.Text);
                        }
                        MessageBox.Show("导出成功");
                    }
                }
            }
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //int issaved = IsHaveSaved();
            //if (issaved == 0)
            //{
            //   // DialogResult result = MessageBox.Show("保存文档", "保存", MessageBoxButtons.YesNoCancel);
            //    if (result == DialogResult.OK)
            //    {
            //        保存ToolStripMenuItem_Click(null, null);
            //    }
            //}
            //else if (issaved == 1 && 没有改变listbox)
            //{
                 
            //}
        }

        private int IsHaveSaved()
        {
            Content con = (Content)(this.listBox1.SelectedItem);
            string sql = "select IsSaved from ContentInfo where did = @ID";
            SqlParameter p = new SqlParameter("@ID", con.IsSaved);
            int r = Convert.ToInt32(SqlDataHelp.ExecuteScalar(sql, p));
            return r;
        }
    }
}
