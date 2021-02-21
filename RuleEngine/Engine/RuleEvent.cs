namespace CSharpRulesEngine
{

    public abstract class RuleEvent : IRuleEvent
    {
        public abstract string Name { get; }
    }
}