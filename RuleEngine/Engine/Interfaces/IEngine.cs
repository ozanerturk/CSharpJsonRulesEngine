using System;
using System.Collections.Generic;

namespace CSharpRulesEngine
{
    public interface IEngine
    {
        Dictionary<string, IDynamicFact> DynamicFacts { get; }
        Dictionary<string, IOperator> Operators { get; }
        Dictionary<string, IRuleEvent> Events { get; }
        List<IRule> Rules { get; }

        void AddOperator(IOperator @operator);
        IRule AddRule(string ruleJson);
        List<IRuleEvent> Execute(string incoming);
        IRule GetRule(Guid id);
        bool RemoveRule(IRule r);
    }
}