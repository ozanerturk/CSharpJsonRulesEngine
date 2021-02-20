using System.Collections.Generic;
using System.Linq;

namespace CSharpRulesEngine
{
    public class BooleanCondition : Condition
    {

        BooleanOperator @operator = BooleanOperator.ALL;
        public BooleanCondition(BooleanOperator @operator, List<Condition> conditions)
        {
            this.@operator = @operator;
            this.subConditions = conditions;
        }
        public override bool Evaluate(ExecutionContext context)
        {
            if (this.@operator == BooleanOperator.ALL)
            {
                return this.subConditions.All(x => x.Evaluate(context));
            }
            else
            {
                return this.subConditions.Any(x => x.Evaluate(context));
            }
        }
    }
}