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
    public partial class Form1 : Form
    {
        private Form2 CataLog; //依赖关系，所以是私有字段
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "select  * from 理学院基本情况";
           // string sql2 = "Insert into 理学院基本情况(专业班级,学号,姓名,性别,政治面貌) values(@projClass, @Id, @Name,@Gender,@Polity)";
            #region 测试SqlDataHelp中的ExecuteReader方法，成功
            //using (SqlDataReader reader = SqlDataHelp.ExecuteReader(sql))
            //{
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            for (int i = 0; i < reader.FieldCount - 8; i++)
            //            {
            //                //这里可以新建一个类，用List集合封装，以后如果频繁使用时，就不用经常拆箱了。
            //                //用索引引用列的名称牵扯到拆箱和封箱的问题
            //                Console.Write(reader.GetValue(i).ToString() + '\t');
            //            }
            //            Console.WriteLine();
            //        }
            //    }
            //}
            #endregion
            #region 测试ExecuteNonQuery方法，成功
            //SqlParameter[] pms = new SqlParameter[]
            //{
            //    new SqlParameter("@projClass", "信息132"),
            //    new SqlParameter("@Id", "1920135159"),
            //    new SqlParameter("@Name", "孔龙飞"),
            //    new SqlParameter("@Gender", "男"),
            //    new SqlParameter("@Polity", "党员")
            //};
            //SqlDataHelp.ExecuteNonQuery(sql2, pms);
            #endregion
            #region 测试SqlDataHelp中的ExecuteDataTable方法，成功 
            //DataTable dt = SqlDataHelp.ExecuteDataTable(sql);
            //dataGridView1.DataSource = dt;
            #endregion
            #region 测试SqlDataHelp中的ExecuteScalar方法，成功 
            //Console.WriteLine(SqlDataHelp.ExecuteScalar(sql).ToString());
            #endregion
            #region 在第二个窗体上操作
            CataLog = new Form2();
            
            //使用委托，是的在另一个线程的窗体能调用这个类的方法，间接地能使用主线程的窗体的控件treeview1
            CataLog.LoadCat = LoadCata;
            LoadCata();
            CataLog.ShowDialog();
            #endregion
        }
        private void LoadCata()
        {
            CataLog.treeView1.Nodes.Clear();
            string sql4 = "select * from Catalog where tparentid  = @Id";
            SqlParameter p = new SqlParameter("@Id", -1);
            List<Catalog> LCata = new List<Catalog>();
            //将根目录中添加到List集合LCata中
             LoadCatalog(LCata,sql4, p);
            foreach (Catalog Cataitem in LCata)
            {
                TreeNode tnode = CataLog.treeView1.Nodes.Add(Cataitem.TName);
                tnode.Tag = Cataitem.TID;
                if(tnode.IsExpanded == false)
                    tnode.Expand();
                //绑定Form2中的contextMenuStrip1控件，前提是将contextMenuStrip1属性设置为public
                tnode.ContextMenuStrip = CataLog.contextMenuStrip1;

                SubRecursive(tnode, CataLog);
            }
        }
        /// <summary>
        /// 递归生成子目录
        /// </summary>
        /// <param name="tnode">选中的当前节点，在该节点下创建子目录</param>
        private void SubRecursive(TreeNode tnode, Form2 CataLog)
        {
            List<Catalog> sub = new List<Catalog>();
           
            string sql4 = "select * from Catalog where tparentid  = @Id";
            SqlParameter p = new SqlParameter("@Id",tnode.Tag);
            //p代表父亲节点的序号
            LoadCatalog(sub,sql4, p);
            //该节点下已没有子目录
            if (sub.Count > 0)
                tnode.ContextMenuStrip = CataLog.contextMenuStrip1;
            else
                tnode.ContextMenuStrip = CataLog.contextMenuStrip2;

            foreach (Catalog Cataitem in sub)
            {
                TreeNode Sub = tnode.Nodes.Add(Cataitem.TName);
                Sub.Tag = Cataitem.TID;
                SubRecursive(Sub, CataLog);
            }
        }
        /// <summary>
        /// 根据父亲的下标找到当前分支下的所有子目录，放到subLCata，List集合中
        /// </summary>
        /// <param name="LCata">List集合</param>
        /// <param name="sql"></param>
        /// <param name="p"></param>
        private void LoadCatalog(List<Catalog> LCata, string sql,SqlParameter p)
        {
            #region 初始化List集合
            using (SqlDataReader reader = SqlDataHelp.ExecuteReader(sql, p))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Catalog cat = new Catalog(reader.GetInt32(0), reader.GetString(1));
                        LCata.Add(cat);
                    }
                }
            }
            #endregion
        }
    }
}
