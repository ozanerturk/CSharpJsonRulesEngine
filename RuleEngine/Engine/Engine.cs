using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{

    public class Engine : IEngine
    {
        public Dictionary<string, IDynamicFact> DynamicFacts { get; private set; }
        public Dictionary<string, IOperator> Operators { get; private set; }
        public Dictionary<string, IRuleEvent> Events { get; private set; }
        public List<IRule> Rules { get; private set; }
        public static object lockObjc = new object();
        public Engine()
        {
            DynamicFacts = new Dictionary<string, IDynamicFact>();
            Operators = new Dictionary<string, IOperator>();
            Rules = new List<IRule>();
            Events = new Dictionary<string, IRuleEvent>();
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
                        var instance = (DynamicFact)Activator.CreateInstance(type);
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
                        var instance = (Operator)Activator.CreateInstance(type);
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
                        var instance = (RuleEvent)Activator.CreateInstance(type);
                        if (!Events.ContainsKey(instance.Name))
                        {
                            Events.Add(instance.Name, instance);
                        }
                    });
        }
        public void AddOperator(IOperator @operator)
        {
            Operators.Add(@operator.Name, @operator);
        }
        public IRule GetRule(Guid id)
        {
            return this.Rules.FirstOrDefault(x => x.Id.Equals(id));
        }

        public bool RemoveRule(IRule r)
        {
            lock (lockObjc)
            {
                return this.Rules.Remove(r);
            }
        }
        public IRule AddRule(string ruleJson)
        {
            var jObject = JObject.Parse(ruleJson);
            var r = RuleParser.ParseRule(this, jObject);
            lock (lockObjc)
            {
                this.Rules.Add(r);
            }
            return r;
        }

        public static ExecutionContext GetContext(IEngine engine, JObject value)
        {
            lock (lockObjc)
            {
                var ruleCopy = new List<IRule>();
                ruleCopy.AddRange(engine.Rules);
                var valueClone = value.DeepClone();
                return new ExecutionContext(engine.DynamicFacts, ruleCopy, value);
            }

        }
        public List<IRuleEvent> Execute(string incoming)
        {
            var jObject = JObject.Parse(incoming);
            var context = GetContext(this, jObject);
            return context.Execute();
        }
    }
}