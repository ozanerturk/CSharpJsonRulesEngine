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
    public class BooleanCondition_Should
    {


        [Theory()]
        [InlineData(BooleanOperator.ANY, true)]
        [InlineData(BooleanOperator.ANY, true, false)]
        [InlineData(BooleanOperator.ALL, true)]
        [InlineData(BooleanOperator.ALL, true, true, true)]
        public void BooleanCondition_ReturnTrue(BooleanOperator booleanOperator, params bool[] conditionResults)
        {
            //Arrange
            var contex = Mock.Of<IExecutionContext>();
            var conditions = MockHelper.MakeMoqConditions(contex, conditionResults);

            //Action
            var booleanCondition = new BooleanCondition(booleanOperator, conditions);
            var result = booleanCondition.Evaluate(contex);

            //Assert
            Assert.True(result, $"should true");
        }

        [Theory()]
        [InlineData(BooleanOperator.ANY, false)]
        [InlineData(BooleanOperator.ANY, false, false)]
        [InlineData(BooleanOperator.ALL, false)]
        [InlineData(BooleanOperator.ALL, true, false, true)]
        public void BooleanCondition_ReturnFalse(BooleanOperator booleanOperator, params bool[] conditionResults)
        {
            //Arrange
            var contex = Mock.Of<IExecutionContext>();
            var conditions = MockHelper.MakeMoqConditions(contex, conditionResults);

            //Action
            var booleanCondition = new BooleanCondition(booleanOperator, conditions);
            var result = booleanCondition.Evaluate(contex);

            //Assert
            Assert.False(result, $"should true");
        }

        [Theory()]
        [InlineData(BooleanOperator.ANY)]
        [InlineData(BooleanOperator.ALL)]
        public void BooleanCondition_AddCondition_IncreasesSubConditionsCount(BooleanOperator booleanOpeartor)
        {
            //Arrange
            var contex = Mock.Of<IExecutionContext>();
            var initialContidionList = new List<ICondition>() { Mock.Of<ICondition>(), Mock.Of<ICondition>() };
            var conditionToAdd= Mock.Of<ICondition>();

            //Action
            var booleanCondition = new BooleanCondition(booleanOpeartor, initialContidionList);
            int initialConditionsCount = booleanCondition.subConditions.Count;
            booleanCondition.AddCondition(conditionToAdd);

            //Assert
            bool isInitialConditionSizeEqualsToInsertedListSize =  initialConditionsCount == initialContidionList.Count;
            bool isIncreasedByOne =  booleanCondition.subConditions.Count == initialContidionList.Count + 1;
            Assert.True(isInitialConditionSizeEqualsToInsertedListSize, $"should true");
            Assert.True(isIncreasedByOne, $"should true");
        }
    }
}
