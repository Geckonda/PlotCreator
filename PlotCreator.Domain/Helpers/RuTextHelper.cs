using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers
{
    public static class RuTextHelper
    {
        public static string MakeToNvarChar(string text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/n");
            sb.Append(text);
            return sb.ToString();
        }

    }
}
