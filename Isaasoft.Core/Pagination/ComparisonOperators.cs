using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isaasoft.Core.Pagination
{
    public enum ComparisonOperators : short
    {
        Equals = 0,
        IsDifferentFrom = 1,
        IsGreaterThanOrEquals = 2,
        IsLessThanOrEquals = 3,
        Contains = 4,
        NotContains = 5,
        StartsWith = 6,
        EndsWith = 7,
        NotStartsWith = 8,
        NotEndsWith = 9,
        IsGreaterThan = 10,
        IsLessThan = 11
    }
}
