using System.Collections.Generic;

namespace CSharpRulesEngine_ozi
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
        public override string Name => "contains";
        public override bool Evaluate(List<string> left, string right)
        {
            return left.Contains(right);
        }
    }
}