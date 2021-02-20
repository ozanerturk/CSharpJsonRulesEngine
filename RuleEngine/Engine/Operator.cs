using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CSharpRulesEngine_ozi
{
    public abstract class Operator
    {
        public abstract string Name { get; }
        public abstract Type Left { get; }
        public abstract Type Right { get; }
        public abstract bool BaseEvaluate(JToken left, JToken right);
    }

    public abstract class Operator<L, R> : Operator
    {
        public override Type Left { get { return typeof(L); } }
        public override Type Right { get { return typeof(R); } }

        public abstract bool Evaluate(L left, R right);

        public override bool BaseEvaluate(JToken left, JToken right)
        {
            var res = this.Evaluate(left.ToObject<L>(), right.ToObject<R>());
            return res;
        }
    }
}