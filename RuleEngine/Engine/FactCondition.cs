using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{
    public class FactCondition : Condition
    {
        public string Fact { get; }
        public IOperator @operator { get; }
        public JToken Value { get; }
        public dynamic @Params { get; }
        public string Path { get; }

        public FactCondition(string fact, IOperator @operator, JToken value, dynamic @params, string path)
        {
            Fact = fact;
            this.@operator = @operator;
            Value = value;
            Path = path;
            @Params = @params;
        }

        public override bool Evaluate(IExecutionContext context)
        {
            JToken value = context.FactValue(this.Fact, this.Params, this.Path);
            if (value == null)
            {
                return false;
            }
            return @operator.BaseEvaluate(value, this.Value);
        }
    }
}