using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 管理图书分类
{
    class ContentInfo
    {
        public int  DID{ get; set; }
        public string  DName{ get; set; }
        public override string ToString()
        {
            return DName;
        }
    }
}
