using System;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class LessThanInclusive_Should
    {
        private readonly LessThanInclusive LessThanInclusive;
        public LessThanInclusive_Should()
        {
            LessThanInclusive = new LessThanInclusive();
        }

        [Theory]
        [InlineData("lessThanInclusive")]
        public void LessThanInclusive_Name_EqualsToLessThanInclusive(string name)
        {
            bool isEqual = LessThanInclusive.Name.Equals(name);
            Assert.True(isEqual, $"{LessThanInclusive.Name} should equals to {name}");
        }

        [Theory]
        [InlineData(-99.0f, 1.0f)]
        [InlineData("-99.0", 1.0f)]
        [InlineData(-99.0f, "1.0")]
        [InlineData("-99.0", "1.0")]
        [InlineData("1", 1)]
        [InlineData(1, "1")]
        [InlineData("1", "1")]
        [InlineData(float.MinValue, float.MaxValue)]
        public void LessThanInclusive_LeftIsLessThanRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = LessThanInclusive.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left}<{right} should true");
        }

        [Theory]
        [InlineData(1.0f, -99.0f)]
        [InlineData(1.0f, "-99.0")]
        [InlineData("1.0", -99.0f)]
        [InlineData("1.0", "-99.0")]
        [InlineData(float.MaxValue, float.MinValue)]
        public void LessThanInclusive_LeftIsGreaterThanRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = LessThanInclusive.BaseEvaluate(leftToken, rightToken);


            //Assert
            Assert.False(result, $"{left}>{right} should false");
        }
    }
}
