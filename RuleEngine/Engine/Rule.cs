namespace CSharpRulesEngine_ozi
{
    public class Rule
    {
        private BooleanCondition rootCondition;
        public RuleEvent @event;
        public Rule(BooleanCondition rootCondition, RuleEvent @event)
        {
            this.rootCondition = rootCondition;
            this.@event = @event;
        }

        public bool Evaluate(ExecutionContext contex)
        {
            return rootCondition.Evaluate(contex);
        }

    }
}