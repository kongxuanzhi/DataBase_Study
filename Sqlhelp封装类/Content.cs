using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlhelp封装类
{
    public class Content
    {
        public int Id { get; set; }
        public int ParantId { get; set; }
        public string ContentName { get; set; }
        public string ContentText { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Byte IsDelete { get; set; }
        public int IsSaved { get; set; }
        public override string ToString()
        {
            return ContentName;
        }
    }
}
