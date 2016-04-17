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
    class RecursionCatalog
    {
        public RecursionCatalog(TreeView treeView, int rootparent)
        {
            LoadCatalog(treeView, rootparent);
        }
        private void LoadCatalog(TreeView treeView1, int p)
        {
            List<Catalog> CLog = new List<Catalog>();
            #region 初始化List<Catalog>集合
            GetList(p, CLog);
            #endregion
            #region 讲根节点加入到treeView控件上
            for (int i = 0; i < CLog.Count; i++)
            {
                TreeNode tnode = treeView1.Nodes.Add(CLog[i].tNames);
                tnode.Tag = CLog[i].tId;
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
    }
}
