using System;
using System.Collections;
using System.Collections.Generic;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class Equal_Should
    {
        private readonly Equals Equal;
        public Equal_Should()
        {
            Equal = new Equals();
        }

        [Theory]
        [InlineData("equal")]
        public void Equal_Name_EqualsToIn(string name)
        {
            bool isEqual = Equal.Name.Equals(name);
            Assert.True(isEqual, $"{Equal.Name} should equals to {name}");
        }

        [Theory]
        [InlineData("apple", "apple")]
        [InlineData(1, 1.0f)]
        [InlineData(5, 5)]
        [InlineData(-5, "-5")]
        public void Equal_LeftEqualsRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = Equal.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left} is in  {right} should true");
        }
        [Theory()]
        [InlineData("apple", "banana")]
        [InlineData(1, 1.1f)]
        [InlineData(5, 4)]
        [InlineData(-5, "-4")]
        public void Equal_LeftNotEqualsRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = Equal.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.False(result, $"{left} is in  {right} should false");
        }
    }
}
