using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
// ...


namespace CSharpRulesEngine_ozi
{


    public class InRange : Operator<float, List<float>>
    {
        public override string Name => "in-range";

        public override bool Evaluate(float left, List<float> right) // range(1,3]
        {
            float upperBound = right.Last();
            float lowerBound = right.Last();
            if (left > lowerBound && left <= upperBound)
            {
                return true;
            }
            return false;
        }
    }
    public class TotalTransactionAmount : DynamicFact
    {
        public override string Name => "total-transaction-amount";

        public override object Calculate(ExecutionContext context, dynamic @params)
        {
            var productToken = context.FactValue("products");//able to get fact value from context
                                                             //it will not compute if fact value if it has already computed beforehand
            var products = productToken.ToObject<List<string>>();

            DateTime startDate = @params["startDate"];
            DateTime endDate = @params["endDate"];
            // go anywhere you want
            return 100;
        }
    }

    public class DiscountEvent : RuleEvent
    {
        public override string Name => "discount";
    }
    class Program
    {

        static void Main(string[] args)
        {
            string text = File.ReadAllText("C:\\WORK\\github\\CSharpRulesEngine_ozi\\rule.json");
            string incoming = File.ReadAllText("C:\\WORK\\github\\CSharpRulesEngine_ozi\\incoming.json");


            JObject doc = JObject.Parse(text);
            JObject incomingObj = JObject.Parse(incoming);
            var e = new Engine();

            e.AddRule(doc);

            Console.WriteLine("Result : {0}", e.Execute(incomingObj));


        }
    }
}



