using System;
using System.Collections;
using System.Collections.Generic;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class Contains_Should
    {
        private readonly Contains Contains;
        public Contains_Should()
        {
            Contains = new Contains();
        }

        [Theory]
        [InlineData("contain")]
        public void Contains_Name_EqualsToIn(string name)
        {
            bool isEqual = Contains.Name.Equals(name);
            Assert.True(isEqual, $"{Contains.Name} should equals to {name}");
        }

        [Theory]
        [InlineData(new[] { "apple", "banana" }, "apple")]
        [InlineData(new[] { "apple" }, "apple")]
        [InlineData(new[] { 5, 6, 7 }, "5")]
        [InlineData(new[] { "5" }, 5)]
        public void Contain_LeftContainsRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = Contains.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left} contains  {right} should true");
        }
        [Theory()]
        [InlineData(new[] { "6", "7" }, 5)]
        [InlineData(new[] { 6, 7 }, "5")]
        [InlineData(new[] { 6, 7 }, 5)]
        [InlineData(new[] { "6", "7" }, "5")]
        public void Contain_LeftNotContainsRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = Contains.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.False(result, $"{left} contains {right} should false");
        }


    }
}
