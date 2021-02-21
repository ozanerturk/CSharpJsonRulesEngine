using System.Collections.Generic;

namespace CSharpRulesEngine
{
    public class EqualsOperator : Operator<string, string>
    {
        public override string Name => "equal";

        public override bool Evaluate(string left, string right)
        {
            return left.Equals(right);
        }
    }

    public class GreaterThan : Operator<float, float>
    {
        public override string Name => "greaterThan";
        public override bool Evaluate(float left, float right)
        {
            return left > right;
        }
    }
    public class LessThan : Operator<float, float>
    {
        public override string Name => "lessThan";
        public override bool Evaluate(float left, float right)
        {
            return left < right;
        }
    }
    public class LessThanInclusive : Operator<float, float>
    {
        public override string Name => "lessThanInclusive";
        public override bool Evaluate(float left, float right)
        {
            return left <= right;
        }
    }
    public class GreaterThanInclusive : Operator<float, float>
    {
        public override string Name => "greaterThanInclusive";
        public override bool Evaluate(float left, float right)
        {
            return left >= right;
        }
    }
    public class In : Operator<string, List<string>>
    {
        public override string Name => "in";
        public override bool Evaluate(string left, List<string> right)
        {
            return right.Contains(left);
        }
    }
    public class Contains : Operator<List<string>, string>
    {
        public override string Name => "contain";
        public override bool Evaluate(List<string> left, string right)
        {
            return left.Contains(right);
        }
    }
}