using System;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class LessThan_Should
    {
        private readonly LessThan LessThan;
        public LessThan_Should()
        {
            LessThan = new LessThan();
        }

        [Theory]
        [InlineData("lessThan")]
        public void LessThan_Name_EqualsToLessThan(string name)
        {
            bool isEqual = LessThan.Name.Equals(name);
            Assert.True(isEqual, $"{LessThan.Name} should equals to {name}");
        }

        [Theory]
        [InlineData(-99.0f, "1.0")]
        [InlineData("-99.0", 1.0f)]
        [InlineData("2", "3")]
        [InlineData(float.MinValue, float.MaxValue)]
        public void LessThan_LeftIsLessThanRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = LessThan.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left}<{right} should true");
        }
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, "1")]
        [InlineData("1", "1")]
        [InlineData("1", 1)]
        [InlineData(1.0f, -99.0f)]
        [InlineData(1.0f, "-99.0")]
        [InlineData("1.0", -99.0f)]
        [InlineData("1.0", "-99.0")]
        [InlineData(float.MinValue, float.MinValue)]
        public void LessThan_LeftEqualsRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = LessThan.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.False(result, $"{left}<{right} = {result} should false");

        }
    }
}
