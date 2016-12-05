using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotComMonitor
{
    public class PlatformResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Package[] Packages { get; set; }
        public bool Available { get; set; }
    }

    public class Package
    {
        public int Package_Id { get; set; }
        public string Package_Name { get; set; }
        public int Platform_Id { get; set; }
    }

}
