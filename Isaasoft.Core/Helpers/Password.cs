using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isaasoft.Core.Helpers
{
    public struct Password
    {
        public string Hash { get; set; }

        public string Salt { get; set; }

        public int Iterations { get; set; }
    }
}
