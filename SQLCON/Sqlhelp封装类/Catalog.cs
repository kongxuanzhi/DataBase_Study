using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlhelp封装类
{
    public class Catalog
    {
        public Catalog(int tid, string name)
        {
            this.TID = tid;
            this.TName = name;
        }
        public int TID { get; set; }
        public string  TName { get; set; }
    }
}
