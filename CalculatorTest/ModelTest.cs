using System;
using System.Data;
using Calculator.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTest
{
    [TestClass]
    public class ModelTest
    {
        private Model _model;

        [TestInitialize]
        public void TestInitialize()
        {
            _model = new Model();
        }

        [TestMethod]
        public void StartingState()
        {
            Assert.IsTrue(_model.CanPushOperand());
            Assert.IsFalse(_model.CanPushOperation());
            Assert.AreEqual(0.0, _model.GetResult());

            Assert.ThrowsException<SyntaxErrorException>(() => _model.Push(Operation.Addition));
        }

        [TestMethod]
        public void PushOperand_WhenEmpty_ThenCheckResult()
        {
            _model.Push(2.0);
            Assert.AreEqual(2.0, _model.GetResult());
        }

        [TestMethod]
        public void PushOperand_WhenHasOperand_IsAnError()
        {
            _model.Push(3.0);

            Assert.IsTrue(_model.CanPushOperation());
            Assert.IsFalse(_model.CanPushOperand());
            Assert.ThrowsException<SyntaxErrorException>(() => _model.Push(5.0));
        }

        [TestMethod]
        public void GetResult_WhenOperationIsOnTop_IsAnError()
        {
            _model.Push(2.0);
            _model.Push(Operation.Addition);

            Assert.ThrowsException<EvaluateException>(() => _model.GetResult());
        }

        [TestMethod]
        public void GetResult_WhenModelHasCorrectExpression_ThenReturnsResult()
        {
            _model.Push(2.0);
            _model.Push(Operation.Multiplication);
            _model.Push(3.0);

            Assert.AreEqual(6.0, _model.GetResult());
        }

        [TestMethod]
        public void GetResult_WithMoreThanOneOperation()
        {
            _model.Push(2.0);
            _model.Push(Operation.Subtraction);
            _model.Push(3.0);
            _model.Push(Operation.Addition);
            _model.Push(4.0);

            Assert.AreEqual(3.0, _model.GetResult());
        }

        [TestMethod]
        public void GetResult_WithMultiplePrecedences()
        {
            _model.Push(2.0);
            _model.Push(Operation.Addition);
            _model.Push(3.0);
            _model.Push(Operation.Multiplication);
            _model.Push(15.0);

            Assert.AreEqual(47.0, _model.GetResult());
        }
    }
}
