using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine_ozi
{

    public class Engine
    {
        public Dictionary<string, DynamicFact> DynamicFacts;
        public Dictionary<string, Operator> Operators;
        public Dictionary<string, RuleEvent> Events;
        public List<Rule> Rules;
        public Engine()
        {
            DynamicFacts = new Dictionary<string, DynamicFact>();
            Operators = new Dictionary<string, Operator>();
            Rules = new List<Rule>();
            Events = new Dictionary<string, RuleEvent>();
            ResolveOperators();
            ResolveEvents();
            ResolveDynamicFacts();
        }

        private void ResolveDynamicFacts()
        {
            AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(type => typeof(DynamicFact).IsAssignableFrom(type) && typeof(DynamicFact) != type && !type.ContainsGenericParameters)
                    .ToList()
                    .ForEach(type =>
                    {
                        DynamicFact instance = (DynamicFact)Activator.CreateInstance(type);
                        if (!DynamicFacts.ContainsKey(instance.Name))
                        {
                            DynamicFacts.Add(instance.Name, instance);
                        }
                    });
        }
        private void ResolveOperators()
        {

            AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(type => typeof(Operator).IsAssignableFrom(type) && typeof(Operator) != type && !type.ContainsGenericParameters)
                    .ToList()
                    .ForEach(type =>
                    {
                        Operator instance = (Operator)Activator.CreateInstance(type);
                        if (!Operators.ContainsKey(instance.Name))
                        {
                            Operators.Add(instance.Name, instance);
                        }
                    });
        }

        private void ResolveEvents()
        {
            AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(type => typeof(RuleEvent).IsAssignableFrom(type) && typeof(RuleEvent) != type && !type.ContainsGenericParameters)
                    .ToList()
                    .ForEach(type =>
                    {
                        RuleEvent instance = (RuleEvent)Activator.CreateInstance(type);
                        if (!Events.ContainsKey(instance.Name))
                        {
                            Events.Add(instance.Name, instance);
                        }
                    });
        }

        public void AddOperator(Operator @operator)
        {
            Operators.Add(@operator.Name, @operator);
        }
        private void RemoveRule(Rule r)
        {
            this.Rules.Remove(r);
        }
        public Rule AddRule(JObject jObject)
        {
            Rule r = RuleParser.ParseRule(this, jObject);
            this.Rules.Add(r);
            return r;
        }

        internal void AddDynamicFact(DynamicFact dynamicFact)
        {
            this.DynamicFacts.Add(dynamicFact.Name, dynamicFact);
        }

        public List<RuleEvent> Execute(JObject jObject)
        {
            ExecutionContext context = ExecutionContext.GetContext(this, jObject);
            return context.Execute();
        }
    }
}