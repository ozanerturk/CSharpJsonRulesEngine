using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpRulesEngine;

namespace RuleEngine.Test
{
    public class InRange : Operator<float, List<float>>
    {
        public override string Name => "in-range";

        public override bool Evaluate(float left, List<float> right) // range(1,3]
        {
            float upperBound = right.Last();
            float lowerBound = right.First();
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
        public float amount;
    }
    class Program
    {

        static void Main(string[] args)
        {
            string rule = File.ReadAllText("ExampleJson/rule.json");
            string incoming = File.ReadAllText("ExampleJson/incoming.json");

            var e = new Engine();
            e.AddRule(rule);

            Parallel.For(1, 100, (i) =>
            {
                e.AddRule(rule);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var events = e.Execute(incoming);
                sw.Stop();
                Console.WriteLine("Execution time : {0} ms", sw.ElapsedMilliseconds);
                foreach (var @event in events)
                {
                    if (@event is DiscountEvent)
                    {
                        var discountEvent = (DiscountEvent)@event;
                        Console.WriteLine("DiscountEvent with amount : {0}", discountEvent.amount);
                    }
                }
            });
            Console.WriteLine("Total rule:{0}", e.Rules.Count);

        }
    }
}



