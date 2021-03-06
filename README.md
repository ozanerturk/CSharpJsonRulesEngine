# CSharp - Json Rules Engine
[![.NET Core CI/CD](https://github.com/ozanerturk/CSharpJsonRulesEngine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ozanerturk/CSharpJsonRulesEngine/actions/workflows/dotnet.yml)
[![codecov](https://codecov.io/gh/ozanerturk/CSharpJsonRulesEngine/branch/main/graph/badge.svg?token=XvcMUSDeUV)](https://codecov.io/gh/ozanerturk/CSharpJsonRulesEngine)

This project inspired originally from  [json-rules-engine](https://github.com/CacheControl/json-rules-engine) project. 
Even there are many similarities, This is not a complete PORT of 'json-rules-engine'
Still has lots of works to do. Feel free to make contribitions.


## Installation
Install using the [JsonRulesEngine NuGet package](https://www.nuget.org/packages/JsonRulesEngine/):

```
dotnet add package JsonRulesEngine --version 0.0.1
```


## Basic Usage

``` Csharp

var incomingJson= `{
    "age": 19,
    "line-item-names": [
        "banana",
        "apple"
    ],
    "amount":300
}`

var engine = new Engine();

engine.AddRule(ruleJson);// checkout Rules section

var events = e.Execute(incomingJson);

foreach (var @event in events)
    {
        if (@event is DiscountEvent)
        {
            var discountEvent = (DiscountEvent)@event;
            Console.WriteLine("DiscountEvent with amount : {0}", discountEvent.amount);
        }
        
    }
``` 

## Rules
Rule is a json object it contains two main properties. `conditions` and `event`
```json 
//If age is above 18 and total transaction amount is less than 1000 and ('line-item-names' list contains 'banana' or 'amount' is in range (50,300])
{
    "conditions": {
        "all": [
            {
                "fact": "age",
                "operator": "greaterThan",
                "value": 18
            },
            {
                "fact":"total-transaction-amount",//DynamicFact
                "params":{
                    "startDate":"2020-12-12T10:00:00",
                    "endDate":"2021-12-12T10:00:00",
                },
                "operator":"lessThan",
                "value":1000
            },
            {
                "any": [
                    {
                        "fact": "line-item-names",
                        "operator": "contain",
                        "value": "banana"
                    },
                    {
                        "fact": "amount",
                        "operator": "in-range",//CustomOperator
                        "value": [50,300]
                    }
                ]
            }
        ]
    },
    "event": {
        "type": "discount",
        "amount": 100
    }
}
```
## Operators
Engine has couple of default operators:
- `equal` *makes string comparison* 
- `greaterThan` *float comparison* 
- `greaterThanInclusive` *float comparison* 
- `lessThan` *float comparison* 
- `lessThanThanInclusive` *float comparison* 
- `in` *string array comparison* 
- `contain` *string array comparison* 

### Custom Operators
You could also define your custom operators like following. Declare a type inherited from `Operator<L,R>`, engine will resolve it automatically. 
    
```CSharp
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
```

## DynamicFact
If your `incomingJson` unable to contain certain information. You can inherit a class from `DynamicFact` to achive this. declare it anywhere you want. engine will resolve it for you. 

Example `total-transcation-amount`:
```CSharp
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
```

## RuleEvent
Engine should know the type of an RuleEvent in order to cast it to an instane. 
Just inherit a class from `RuleEvent`, engine will resolve types from assembly. 
```CSharp
 public class DiscountEvent : RuleEvent
    {
        public override string Name => "discount";
        public float amount;
    }
```

## TODOS
- Fact priorities
- Rule priorities
- Unit Tests
- Documentation
- Nuget Package
- CD/CI
- Async Execution and Event Handles
- 

