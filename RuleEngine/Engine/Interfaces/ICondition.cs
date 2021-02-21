using System.Collections.Generic;

namespace CSharpRulesEngine
{
    public interface ICondition
    {
        IList<ICondition> subConditions { get; set; }

        void AddCondition(ICondition condition);
        bool Evaluate(IExecutionContext contex);
    }
}