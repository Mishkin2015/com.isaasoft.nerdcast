using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isaasoft.Core.Pagination
{
    public class ExpressionOrderBy
    {
        public string PropertyName { get; set; }

        public OrderByDirection Direction { get; set; }

        public string BuildExpressionString()
        {
            switch (Direction)
            {
                case OrderByDirection.Asc:
                    return string.Format("{0}", PropertyName);
                case OrderByDirection.Desc:
                    return string.Format("{0} DESCENDING", PropertyName);
                default:
                    throw new NotImplementedException(Direction.ToString());
            }
        }
    }
}
