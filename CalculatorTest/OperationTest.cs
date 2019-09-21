using System;
using Calculator.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTest
{
    [TestClass]
    public class OperationTest
    {
        [TestMethod]
        public void Addition()
        {
            Assert.AreEqual(5.0, Operation.Addition.Calculate(2.0, 3.0));
        }

        [TestMethod]
        public void Subtraction()
        {
            Assert.AreEqual(-1.0, Operation.Subtraction.Calculate(2.0, 3.0));
        }

        [TestMethod]
        public void Multiplication()
        {
            Assert.AreEqual(6.0, Operation.Multiplication.Calculate(2.0, 3.0));
        }

        [TestMethod]
        public void Division()
        {
            Assert.AreEqual(1.5, Operation.Division.Calculate(3.0, 2.0));
        }

        [TestMethod]
        public void Precedence()
        {
            Assert.AreEqual(Operation.Addition.Precedence(), Operation.Subtraction.Precedence());
            Assert.AreEqual(Operation.Multiplication.Precedence(), Operation.Division.Precedence());
            Assert.IsTrue(Operation.Addition.Precedence() < Operation.Multiplication.Precedence());
        }
    }
}
