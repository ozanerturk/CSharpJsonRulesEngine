using System;

namespace CSharpRulesEngine
{

    public class Rule : IRule
    {
        public BooleanCondition rootCondition { get; private set; }
        public IRuleEvent @event { get; private set; }
        public Guid Id { get; }
        public Rule(BooleanCondition rootCondition, RuleEvent @event)
        {
            this.rootCondition = rootCondition;
            this.@event = @event;
            this.Id = Guid.NewGuid();
        }
        public bool Evaluate(ExecutionContext contex)
        {
            return rootCondition.Evaluate(contex);
        }

    }
}