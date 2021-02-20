using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using static CSharpRulesEngine_ozi.ExecutionContext;

namespace CSharpRulesEngine_ozi
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