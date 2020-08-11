using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class YesReturn
    {
        public int Status { get; set; }
        public string StatusMsg { get; set; }
        public string MsgID { get; set; }
        public Resultlist[] ResultList { get; set; }
    }

    public class Resultlist
    {
        public string Mobile { get; set; }
        public int RespCode { get; set; }
        public string RespMsg { get; set; }
    }

}
