using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{
    public interface IDynamicFact
    {
        string Name { get; }

        object Calculate(ExecutionContext context, dynamic @params);
        JToken CalculateJToken(ExecutionContext context, dynamic @params);
    }
}