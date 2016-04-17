using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 电话本管理系统
{
    public class Phone
    {
        public string FriendName { get; set; }
        public string FriendPhone { get; set; }
        public int friendGroup { get; set; }
        public override string ToString()
        {
            return FriendName;
        }
    }
}
