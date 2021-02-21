using System;

namespace CSharpRulesEngine
{
    public interface IRule
    {
        public BooleanCondition rootCondition { get; }
        public IRuleEvent @event { get; }
        Guid Id { get; }
        bool Evaluate(ExecutionContext contex);
    }
}