using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpRulesEngine;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace RuleEngine.Test
{
    public class FactCondition_Should
    {


        [Theory()]
        [InlineData("someFact", true)]
        public void FactCondition_ReturnTrue(string fact, bool opeartorResult)
        {
            //Arrange
            object @params = It.IsAny<object>();
            string @path = It.IsAny<String>();
            var fakeLeftValue = new JObject();
            var contexMoq = new Mock<IExecutionContext>();
            contexMoq.Setup(x => x.FactValue(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>())).Returns(fakeLeftValue);
            var context = contexMoq.Object;
            var @operator = MockHelper.MakeMoqOpeartor(opeartorResult);
            //Action
            var factCondition = new FactCondition(fact, @operator, It.IsAny<JObject>(), It.IsAny<object>(), It.IsAny<string>());
            var result = factCondition.Evaluate(context);

            //Assert
            Assert.True(result, $"should true");
        }


        [Theory()]
        [InlineData("someFact", false, true)]
        [InlineData("someFact", true, false)]
        [InlineData("someFact", false, false)]
        public void FactCondition_ReturnFalse(string fact, bool opeartorResult, bool factExists)
        {
            //Arrange
            object @params = It.IsAny<object>();
            string @path = It.IsAny<String>();
            var fakeLeftValue = new JObject();
            var contexMoq = new Mock<IExecutionContext>();
            contexMoq.Setup(x => x.FactValue(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>())).Returns(factExists ? fakeLeftValue : null);
            var context = contexMoq.Object;
            var @operator = MockHelper.MakeMoqOpeartor(opeartorResult);
            //Action
            var factCondition = new FactCondition(fact, @operator, It.IsAny<JObject>(), It.IsAny<object>(), It.IsAny<string>());
            var result = factCondition.Evaluate(context);

            //Assert
            Assert.False(result, $"should false");
        }
    }
}
