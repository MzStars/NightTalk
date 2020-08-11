using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class FilterMessageHelp
    {
        public static bool ISFilter(string InText, string FilterContent)
        {
            string word = FilterContent;
            if (string.IsNullOrEmpty(word))
            {

                return false;
            }

            if (InText == null)
            {
                return false;
            }

            foreach (string key in word.Split('|'))
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (InText.ToLower().IndexOf(key) > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
