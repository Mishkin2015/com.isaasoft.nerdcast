using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isaasoft.Core.Pagination
{
    public class ExpressionProperty
    {
        public Type TargetType { get; set; }

        public string Navigation { get; set; }

        public ComparisonOperators Comparison { get; set; }

        public object WithTheValue { get; set; }

        public string PropertyName
        {
            get
            {
                if (Navigation != null)
                {
                    return Navigation.Split('.').Last();
                }

                return null;
            }
        }

        public LogicalOperators LogicalWithNext { get; set; }

        public string BuildExpressionString()
        {
            return BuildExpressionString("@0");
        }

        public string BuildExpressionString(string paramName)
        {
            switch (Comparison)
            {
                case ComparisonOperators.Contains:
                    return string.Format("{0}.Contains({1})", Navigation, paramName);
                case ComparisonOperators.Equals:
                    return string.Format("{0} == {1}", Navigation, paramName);
                case ComparisonOperators.IsDifferentFrom:
                    return string.Format("{0} != {1}", Navigation, paramName);
                case ComparisonOperators.IsGreaterThanOrEquals:
                    return string.Format("{0} >= {1}", Navigation, paramName);
                case ComparisonOperators.IsLessThanOrEquals:
                    return string.Format("{0} <= {1}", Navigation, paramName);
                case ComparisonOperators.IsGreaterThan:
                    return string.Format("{0} > {1}", Navigation, paramName);
                case ComparisonOperators.IsLessThan:
                    return string.Format("{0} < {1}", Navigation, paramName);
                case ComparisonOperators.NotContains:
                    return string.Format("!{0}.Contains({1})", Navigation, paramName);
                case ComparisonOperators.StartsWith:
                    return string.Format("{0}.StartsWith({1})", Navigation, paramName);
                case ComparisonOperators.EndsWith:
                    return string.Format("{0}.EndsWith({1})", Navigation, paramName);
                case ComparisonOperators.NotStartsWith:
                    return string.Format("!{0}.StartsWith({1})", Navigation, paramName);
                case ComparisonOperators.NotEndsWith:
                    return string.Format("!{0}.EndsWith({1})", Navigation, paramName);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
