using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace adventureWorks.Common
{
    public class ColumnMapper
    {
        public string ColumnName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}