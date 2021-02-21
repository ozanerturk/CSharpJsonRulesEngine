using System;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{
    public interface IOperator
    {
        string Name { get; }
        Type Left { get; }
        Type Right { get; }

        bool BaseEvaluate(JToken left, JToken right);
    }
}