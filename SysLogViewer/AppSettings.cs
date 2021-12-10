using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysLogViewer
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public int CommandTimeout { get; set; }
    }
}
