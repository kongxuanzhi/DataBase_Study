using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 管理图书升级
{
    public partial class InsertOk : Form
    {
        public InsertOk()
        {
            InitializeComponent();
        }
        public string name { get; set; }
        public string ARTICLE { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            name = CatalogName.Text;
        }
        private void InsertOk_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }
        private void CatalogName_TextChanged(object sender, EventArgs e)
        {
            if (CatalogName.Text != "")
            {
                button1.Enabled = true;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ARTICLE = textBox1.Text;
        }
    }
}
