using System;
using System.Collections;
using System.Collections.Generic;
using CSharpRulesEngine;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class In_Should
    {
        private readonly In In;
        public In_Should()
        {
            In = new In();
        }

        [Theory]
        [InlineData("in")]
        public void In_Name_EqualsToIn(string name)
        {
            bool isEqual = In.Name.Equals(name);
            Assert.True(isEqual, $"{In.Name} should equals to {name}");
        }

        [Theory]
        [InlineData("apple", new[] { "apple", "banana" })]
        [InlineData("apple", new[] { "apple" })]
        [InlineData(5, new[] { 5, 6, 7 })]
        [InlineData(5, new[] { 5 })]
        [InlineData(5, new[] { "5" })]
        [InlineData("5", new[] { 5 })]
        public void In_LeftInRight_ReturnTrue(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = In.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.True(result, $"{left} is in  {right} should true");
        }
        [Theory()]
        [InlineData("apple", new[] { "orange", "banana" })]
        [InlineData("apple", new[] { "banana" })]
        [InlineData(5, new[] { 6, 7 })]
        [InlineData(5, new[] { "6", "7" })]
        public void In_LeftNotInRight_ReturnFalse(object left, object right)
        {
            //Arrange
            var leftToken = JToken.FromObject(left);
            var rightToken = JToken.FromObject(right);

            //Action
            var result = In.BaseEvaluate(leftToken, rightToken);

            //Assert
            Assert.False(result, $"{left} is in  {right} should false");
        }
    }
}
