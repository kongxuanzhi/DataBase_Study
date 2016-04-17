using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 省份选择配置文件封装
{
    public class AreaInfo
    {
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public override string ToString()
        {
            return AreaName;
        }
    }
}
