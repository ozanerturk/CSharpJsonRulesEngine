using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{
    public abstract class DynamicFact
    {
        public abstract string Name { get; }
        public abstract object Calculate(ExecutionContext engine, dynamic @params);

        public JToken CalculateJToken(ExecutionContext context, dynamic @params)
        {
            return Calculate(context, @params);
        }
    }
}