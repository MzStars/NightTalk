using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public abstract class ServiceBase
    {

        public ServiceBase()
        {
        }

        public ServiceBase(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        protected virtual IDataProvider DataProvider
        {
            get;
            set;
        }

        protected void Require(Guid arg, string argName)
        {
            if (arg == Guid.Empty)
            {
                throw new ArgumentNullException(argName);
            }
        }

        protected void Require(string arg, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentNullException(argName);
            }
        }

        protected void ValidPager(int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentException("pageIndex");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("pageSize");
            }
        }

        protected void ValidModel(object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
        }
    }
}
