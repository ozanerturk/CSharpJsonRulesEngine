using System.Collections.Generic;

namespace CSharpRulesEngine
{
    public abstract class Condition
    {
        public IList<Condition> subConditions { get; set; }
        public Condition()
        {
            subConditions = new List<Condition>();
        }
        public void AddCondition(Condition condition)
        {
            this.subConditions.Add(condition);
        }

        public abstract  bool Evaluate(ExecutionContext contex);

    }
}