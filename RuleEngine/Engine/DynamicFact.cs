using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{

    public abstract class DynamicFact : IDynamicFact
    {
        public abstract string Name { get; }
        public abstract object Calculate(ExecutionContext context, dynamic @params);
        public JToken CalculateJToken(ExecutionContext context, dynamic @params)
        {
            return Calculate(context, @params);
        }
    }
}