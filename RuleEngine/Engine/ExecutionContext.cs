using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{

    public class ExecutionContext : IExecutionContext
    {
        private List<IRule> rules;
        private JObject value;
        public Dictionary<string, JToken> FactValueCache { get; private set; }
        public Dictionary<string, IDynamicFact> DynamicFacts { get; private set; }
        public List<IRuleEvent> @events { get; private set; }
        private bool isRunning = false;
        public ExecutionContext(Dictionary<string, IDynamicFact> dynamicFacts, List<IRule> rules, JObject value)
        {
            this.DynamicFacts = dynamicFacts;
            this.rules = rules;
            this.value = value;
            FactValueCache = new Dictionary<string, JToken>();
            @events = new List<IRuleEvent>();
        }
        public List<IRuleEvent> Execute()
        {
            if (isRunning)
            {
                return @events;
            }
            isRunning = true;
            foreach (var rule in rules)
            {
                if (rule.Evaluate(this))
                {
                    @events.Add(rule.@event);
                }
            }
            return @events;
        }

        public JToken FactValue(string fact, dynamic @params = null, string path = "")
        {
            var comparer = new JTokenEqualityComparer();
            int hash1 = fact.GetHashCode();
            int hash2 = path.GetHashCode();
            int hash3 = comparer.GetHashCode(@params);
            string long_hash = string.Join(hash1.ToString(), hash2.ToString(), hash3.ToString());
            if (this.FactValueCache.ContainsKey(long_hash))
            {
                return this.FactValueCache[long_hash];
            }
            else
            {
                JToken factValueToken = "";

                if (this.DynamicFacts.ContainsKey(fact))
                {
                    factValueToken = this.DynamicFacts[fact].CalculateJToken(this, @params);
                }
                else
                {
                    if (!value.TryGetValue(fact, out factValueToken))
                    {
                        return null;
                    }
                }
                if (!string.IsNullOrEmpty(path))
                {
                    factValueToken = factValueToken.SelectToken(path);
                    if (factValueToken == null)
                    {
                        return null;
                    }
                }
                this.FactValueCache[long_hash] = factValueToken;
                return factValueToken;
            }
        }

    }
}