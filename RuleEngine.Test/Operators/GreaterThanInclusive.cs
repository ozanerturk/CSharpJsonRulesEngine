using System;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class GreaterThanInclusive_Should
    {
        private readonly GreaterThanInclusive GreaterThanInclusive;
        public GreaterThanInclusive_Should()
        {
            GreaterThanInclusive = new GreaterThanInclusive();
        }

        [Theory]
        [InlineData("greaterThanInclusive")]
        public void GreaterThanInclusive_Name_EqualsToGreaterThanInclusive(string name)
        {
            bool isEqual = GreaterThanInclusive.Name.Equals(name);
            Assert.True(isEqual, $"{GreaterThanInclusive.Name} should equals to {name}");
        }

        [Theory]
        [InlineData(1.0f, -99.0f)]
        [InlineData(1.0f, "-99.0")]
        [InlineData("1.0", -99.0f)]
        [InlineData("1.0", "-99.0")]
        [InlineData("1", 1)]
        [InlineData(1, "1")]
        [InlineData("1", "1")]
        [InlineData(float.MaxValue, float.MinValue)]
        public void GreaterThanInclusive_LeftIsGreaterThanInclusiveRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = GreaterThanInclusive.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left}>{right} should true");
        }
        [Theory]
        [InlineData(-99.0f, "1.0")]
        [InlineData("-99.0", 1.0f)]
        [InlineData("2", "3")]
        [InlineData(float.MinValue, float.MaxValue)]
        public void GreaterThanInclusive_LeftLessThanRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = GreaterThanInclusive.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.False(result, $"{left}>{right} = {result} should false");
        }
    }
}
