using System;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class GreaterThan_Should
    {
        private readonly GreaterThan GreaterThan;
        public GreaterThan_Should()
        {
            GreaterThan = new GreaterThan();
        }

        [Theory]
        [InlineData("greaterThan")]
        public void GreaterThan_Name_EqualsToGreaterThan(string name)
        {
            bool isEqual = GreaterThan.Name.Equals(name);
            Assert.True(isEqual, $"{GreaterThan.Name} should equals to {name}");
        }

        [Theory]
        [InlineData(1.0f, -99.0f)]
        [InlineData(1.0f, "-99.0")]
        [InlineData("1.0", -99.0f)]
        [InlineData("1.0", "-99.0")]
        [InlineData(float.MaxValue, float.MinValue)]
        public void GreaterThan_LeftIsGreaterThanRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = GreaterThan.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left}>{right} should true");
        }
        [Theory]
        [InlineData(1, 1)]
        [InlineData("1", "1")]
        [InlineData(1, "1")]
        [InlineData("1", 1)]
        [InlineData(1, 2)]
        [InlineData("1", "2")]
        [InlineData(1, "2")]
        [InlineData("1", 2)]
        [InlineData(float.MinValue, float.MinValue)]
        public void GreaterThan_LeftEqualOrGreaterThanRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = GreaterThan.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.False(result, $"{left}>{right} = {result} should false");
        }
    }
}
