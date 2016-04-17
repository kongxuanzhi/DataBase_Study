using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sqlhelp封装类
{
    public partial class AddArticle : Form
    {
        public AddArticle()
        {
            InitializeComponent();
        }
        public string NAME { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            NAME = textBox1.Text;
            MessageBox.Show(NAME);
            if(Name!=null)
            {
                this.Close();
            }
        }
    }
}
