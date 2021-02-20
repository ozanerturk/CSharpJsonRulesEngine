using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine
{
    public class ExecutionContext
    {
        private Engine engine;
        private List<Rule> rules;
        private JObject value;
        public Dictionary<string, JToken> FactValueCache { get; set; }
        public List<RuleEvent> @events { get; set; }
        public bool isRunning = false;
        protected ExecutionContext(Engine engine, List<Rule> rules, JObject value)
        {
            this.engine = engine;
            this.rules = rules;
            this.value = value;
            FactValueCache = new Dictionary<string, JToken>();
            @events = new List<RuleEvent>();

        }
        public List<RuleEvent> Execute()
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
        public static ExecutionContext GetContext(Engine engine, JObject value)
        {
            return new ExecutionContext(engine, engine.Rules, value);
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

                if (this.engine.DynamicFacts.ContainsKey(fact))
                {
                    factValueToken = this.engine.DynamicFacts[fact].CalculateJToken(this, @params);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}