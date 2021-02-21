using System.Collections.Generic;

namespace CSharpRulesEngine
{

    public abstract class Condition : ICondition
    {
        public IList<ICondition> subConditions { get; set; }
        public Condition()
        {
            subConditions = new List<ICondition>();
        }
        public void AddCondition(ICondition condition)
        {
            this.subConditions.Add(condition);
        }

        public abstract bool Evaluate(IExecutionContext contex);

    }
}