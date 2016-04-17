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
using System.Configuration;

namespace 省份选择配置文件封装
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProvince();
        }

        private void LoadProvince()
        {
            #region 方法2，数据绑定，初始化List集合
            List<ProvinceInfo> LP = new List<ProvinceInfo>();
            #region 通过配置文件App.config来添加链接字符串
            string constr = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            #endregion
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = "select provinceID, province from province";
                using(SqlCommand cmd = new SqlCommand(sql,con))
                {
                    con.Open();
                    using(SqlDataReader DR = cmd.ExecuteReader())
                    {
                        if(DR.HasRows)
                        {
                            while(DR.Read())
                            {
                                #region 由于上面的sql语句挑选的是后两行，所以这里索引是0,1，GetSring直接转化为字符串
                                //GetValue 返回object类型，需要强制转化
                                //GetString返回string类型。
                                //other operator return more Type of sql, such as Int,ect
                                string ProvinceId = DR.GetString(0);
                                string ProvinceName = DR.GetString(1);
                                #endregion
                                #region 控制台测试代码
                                //Console.Write(ProvinceId + " ");
                                //Console.WriteLine(ProvinceName);
                                #endregion
                                #region 创建ProvinceInfo类来存储两个字段，来适应combox只能添加string字段特性
                                //这一句必须写在里面，否则每次改变的是同一个引用的指向相同内存的值。
                                ProvinceInfo PI = new ProvinceInfo() { ProvinceId = ProvinceId, ProvinceName = ProvinceName };
                                #region 方法1、直接输出到Combox的下拉菜单
                                //cmb1.Items.Add(PI);
                                #endregion
                                #region 存放进List集合
                                LP.Add(PI);
                                #endregion
                                #endregion
                            }
                        }
                    }
                    #region 在combox中增加默认选项“请选择”,并置为默认选项
                    ProvinceInfo select = new ProvinceInfo() { ProvinceId = "0", ProvinceName = "请选择" };
                    cmb1.Items.Insert(0, select);
                    cmb1.SelectedIndex = 0;
                    #endregion
                }
            }
            #endregion
            #region 方法2、将List集合绑定发哦Combox控件中
            cmb1.DisplayMember = "ProvinceName";//List 中provinceInfo属性
            cmb1.ValueMember = "ProvinceId";
            cmb1.DataSource = LP;
            #endregion

            #region 将List集合和DataGridView绑定在一起，在窗体上显示
            dataGridView1.DataSource = LP;
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 将Combox1中的选中的内容装箱
            ProvinceInfo PI = (ProvinceInfo)(cmb1.SelectedItem);
            MessageBox.Show(PI.ProvinceId+" "+PI.ProvinceName,"省份信息");
            #endregion
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AreaInfo AI = (AreaInfo)cmb2.SelectedItem;
            MessageBox.Show(AI.AreaID+" "+ AI.AreaName);

        }
        private void cmb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 使用配置文件来读取链接字符串，用ConfigurationManager类读取数据
            string constr = ConfigurationManager.ConnectionStrings["ChinaData"].ConnectionString;
            #endregion
            using (SqlConnection con = new SqlConnection(constr))
            {
                string sql = "select a.cityID,a.city from area as a, province as p where a.father = p.provinceID and p.provinceID = @ID";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    ProvinceInfo province = (ProvinceInfo)cmb1.SelectedItem;
                    SqlParameter p1 = new SqlParameter("@ID",province.ProvinceId);
                    cmd.Parameters.Add(p1);
                    using(SqlDataReader DR = cmd.ExecuteReader())
                    {
                         cmb2.Items.Clear();
                         if(DR.HasRows)
                         {
                             while(DR.Read())
                             {
                                 //string cb = DR.GetString(1);
                                 #region 必须在这里定义
                                 AreaInfo AI = new AreaInfo();
                                 AI.AreaID = DR.GetString(0);
                                 AI.AreaName = DR.GetString(1);
                                 cmb2.Items.Add(AI);
	                             #endregion
                             }
                         }
                    }
                    //AreaInfo A2 = new AreaInfo() { AreaID = "-1", AreaName = "请选择" };
                    //cmb2.Items.Insert(0, A2);
                    //cmb2.Items.Insert(0, "请选择");
                    if(cmb2.Items.Count>0)
                        cmb2.SelectedIndex = 0;
                }
            }
        }

        #region 窗体中不让使用者改写，导致出错
        //在combox的属性中DropDownStyle 改为DropDownList就行了，这样就只能选择，不能修改了。
        #endregion
    }
}
