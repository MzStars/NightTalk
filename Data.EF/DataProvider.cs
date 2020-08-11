using Data;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public partial class DataProvider : IDataProvider
    {
        private readonly NightTalkContext context;

        public DataProvider(NightTalkContext context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
