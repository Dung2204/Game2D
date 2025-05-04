using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;


namespace Expand
{
    public static class ExpandFunction
    {
        public static string FormatDesc(this string s, string format, int obj1)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char item in s)
            {
                if (item != '【' || item != 'x' || item != '】')
                    sb.Append(item);
                else if (item == 'x')
                    sb.Append(obj1);
            }
            return sb.ToString();
        }
    }
}


