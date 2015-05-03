using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isaasoft.Core.Pagination
{
    public class ExpressionBlock
    {
        public IEnumerable<ExpressionProperty> Expressions { get; set; }

        public IEnumerable<ExpressionBlock> SubBlocks { get; set; }

        public LogicalOperators LogicalWithNext { get; set; }

        public static string BuildExpressionString(IEnumerable<ExpressionBlock> blocks, out IEnumerable<ExpressionProperty> localizedExpressions)
        {
            localizedExpressions = null;

            if (blocks == null || !blocks.Any())
            {
                return null;
            }

            var paramIndex = 0;
            var expressions = new List<ExpressionProperty>();

            var expressionString = ExpressionBlock.BuildExpressionString(blocks, null, ref paramIndex, ref expressions);

            localizedExpressions = expressions;
            return expressionString;
        }

        public static string BuildExpressionString(IEnumerable<ExpressionBlock> blocks, StringBuilder inBuilder, ref int paramIndex, ref List<ExpressionProperty> expressions)
        {
            var lastLogical = default(LogicalOperators);

            if (inBuilder == null)
            {
                inBuilder = new StringBuilder();
            }

            if (expressions == null)
            {
                expressions = new List<ExpressionProperty>();
            }

            foreach (var group in blocks)
            {
                if (inBuilder.Length > 0)
                {
                    inBuilder.Append(" " + Helpers.GetLogicalString(lastLogical) + " ");
                }

                lastLogical = group.LogicalWithNext;

                if (blocks.Count() > 0)
                {
                    inBuilder.Append("(");
                }

                expressions.AddRange(group.Expressions);
                inBuilder.Append(BuildExpressionString(group.Expressions, ref paramIndex));

                if (group.SubBlocks != null && group.SubBlocks.Any())
                {
                    BuildExpressionString(group.SubBlocks, inBuilder, ref paramIndex, ref expressions);
                }

                if (blocks.Count() > 0)
                {
                    inBuilder.Append(")");
                }
            }

            return inBuilder.ToString();
        }

        private static string BuildExpressionString(IEnumerable<ExpressionProperty> expressions, ref int paramIndex)
        {
            var lastLogical = default(LogicalOperators);
            var expressionString = new StringBuilder();

            foreach (var expression in expressions)
            {
                if (expressionString.Length > 0)
                {
                    expressionString.Append(" " + Helpers.GetLogicalString(lastLogical) + " ");
                }

                lastLogical = expression.LogicalWithNext;
                expressionString.Append(expression.BuildExpressionString("@" + paramIndex++));
            }

            return expressionString.ToString();
        }
    }
}
