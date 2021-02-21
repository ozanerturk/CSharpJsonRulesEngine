using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{
    public interface IExecutionContext
    {
        Dictionary<string, JToken> FactValueCache { get; }
        Dictionary<string, IDynamicFact> DynamicFacts { get; }
        List<IRuleEvent> events { get; }

        List<IRuleEvent> Execute();
        JToken FactValue(string fact, dynamic @params = null, string path = "");
    }
}