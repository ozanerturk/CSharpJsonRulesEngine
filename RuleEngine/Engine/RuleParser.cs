using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine_ozi
{
    public static class RuleParser
    {
        private static BooleanCondition ParseBooleanCondition(Engine engine, BooleanOperator @operator, JToken jsonSubConditions)
        {
            List<Condition> conditions = new List<Condition>();
            foreach (JToken token in jsonSubConditions.Children())
            {
                if (token.Type != JTokenType.Object)
                {
                    throw new Exception("'condition' shoul be an object" + ((JProperty)token).Path);
                }
                BooleanOperator booleanOperator = BooleanOperator.NONE;
                string booleanOperator_key = "";
                if (((JObject)token).ContainsKey("all"))
                {
                    booleanOperator = BooleanOperator.ALL;
                    booleanOperator_key = "all";
                }
                else if (((JObject)token).ContainsKey("any"))
                {
                    booleanOperator = BooleanOperator.ANY;
                    booleanOperator_key = "any";
                }

                if (booleanOperator != BooleanOperator.NONE)
                {
                    var condition = ParseBooleanCondition(engine, booleanOperator, (JArray)((JObject)token).GetValue(booleanOperator_key));
                    conditions.Add(condition);
                }
                else
                {
                    JToken fact;
                    if (!((JObject)token).TryGetValue("fact", out fact))
                    {
                        throw new Exception("'facts conditions' shoul contain 'fact'" + ((JProperty)token).Path);
                    }
                    JToken fact_operator;
                    if (!((JObject)token).TryGetValue("operator", out fact_operator))
                    {
                        throw new Exception("'facts conditions' shoul contain 'operator'" + ((JProperty)token).Path);
                    }
                    JToken valueToken;
                    if (!((JObject)token).TryGetValue("value", out valueToken))
                    {
                        throw new Exception("'facts conditions' shoul contain 'value'" + ((JProperty)token).Path);
                    }
                    dynamic valueDynamicToken = null;
                    if (valueToken.Type == JTokenType.Array)
                    {
                        valueDynamicToken = valueToken.Children().Select(x => x.ToObject<dynamic>()).ToList();
                    }
                    else
                    {
                        valueDynamicToken = valueToken.ToObject<dynamic>();
                    }
                    string path = string.Empty;
                    JToken pathToken = token["path"];
                    if (pathToken != null)
                    {
                        path = pathToken.Value<string>();
                    }
                    dynamic @params = null;
                    var parmasToken = (JToken)token.SelectToken("$.params");
                    if (parmasToken != null)
                    {
                        @params = parmasToken.ToObject<dynamic>();
                    }
                    string @operatorName = fact_operator.Value<string>();
                    if (!engine.Operators.ContainsKey(@operatorName))
                    {
                        throw new Exception("Unknwon operator");
                    }
                    var @factOperator = engine.Operators[@operatorName];
                    conditions.Add(new FactCondition(fact.Value<string>(), @factOperator, valueToken.ToObject<dynamic>(), @params, path));
                }
            }
            return new BooleanCondition(@operator, conditions);
        }
        public static Rule ParseRule(Engine engine, JObject jsonRule)
        {
            JToken rootConditionToken;
            if (!jsonRule.TryGetValue("conditions", out rootConditionToken))
            {
                throw new Exception("json should contain 'conditions' in the root object");
            }
            if (rootConditionToken.Type != JTokenType.Object)
            {
                throw new Exception("'conditions' shoul be an object");
            }

            JToken rootBooleanCondition = rootConditionToken.First;
            if (rootBooleanCondition == null)
            {
                throw new Exception("'conditions' shoul have exactly one 'all' or 'any' condition");
            }

            string rootBooleanOperator = ((JProperty)rootBooleanCondition).Name;
            if (!rootBooleanOperator.Equals("any") && !rootBooleanOperator.Equals("all"))
            {
                throw new Exception("'conditions' shoul have exactly one 'all' or 'any' condition");
            }
            JToken eventToken;
            if (!jsonRule.TryGetValue("event", out eventToken))
            {
                throw new Exception("json should contain 'event' in the root object");
            }
            JToken typeToken;
            if (!((JObject)eventToken).TryGetValue("type", out typeToken))
            {
                throw new Exception("'event' should contain 'type'");
            }
            string type = typeToken.Value<string>();
            if (!engine.Events.ContainsKey(type))
            {
                throw new Exception($"unknwon event:{type}");
            }
            var @event = engine.Events[type];

            JArray j_subConditions = (JArray)(((JProperty)rootBooleanCondition).Value);
            var rootCondition = ParseBooleanCondition(engine, BooleanOperator.ALL, j_subConditions);

            return new Rule(rootCondition, @event);
        }
    }
}