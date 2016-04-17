using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 管理图书分类
{
    class Catalog
    {
        public int tId { get; set; }
        public string tNames { get; set; }
        public override string ToString()
        {
            return tNames;
        }
    }
}
