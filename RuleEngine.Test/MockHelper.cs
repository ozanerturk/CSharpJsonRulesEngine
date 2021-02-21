using System.Collections.Generic;
using System.Linq;
using CSharpRulesEngine;
using Moq;
using Newtonsoft.Json.Linq;

namespace RuleEngine.Test
{
    public class MockHelper
    {

        public static IList<ICondition> MakeMoqConditions(IExecutionContext context, params bool[] x)
        {
            return x.ToList().Select(x =>
            {
                var condition = new Mock<ICondition>();
                condition.Setup(x => x.Evaluate(context)).Returns(x);
                return condition.Object;
            }).ToList();
        }

        public static IOperator MakeMoqOpeartor(bool result)
        {
            var @operator = new Mock<IOperator>();
            @operator.Setup(x => x.BaseEvaluate(It.IsAny<JToken>(), It.IsAny<JToken>())).Returns(result);
            return @operator.Object;
        }

    }
}