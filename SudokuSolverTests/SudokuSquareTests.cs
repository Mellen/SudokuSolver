using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;

namespace SudokuSolverTests
{
    /// <summary>
    /// Summary description for SudokuSquareTests
    /// </summary>
    [TestClass]
    public class SudokuSquareTests
    {

        [TestMethod]
        public void UnsetEventTest()
        {
            SudokuSquare square1 = new SudokuSquare(0, 0, 0);
            SudokuSquare square2 = new SudokuSquare(0, 1, 0);
            SudokuSquare square3 = new SudokuSquare(0, 2, 0);

            square1.ValueSet += square2.RemovePossibleValue;
            square1.ValueSet += square3.RemovePossibleValue;
            square1.ValueUnset += square2.AddPossibleValue;
            square1.ValueUnset += square3.AddPossibleValue;

            square2.ValueSet += square1.RemovePossibleValue;
            square2.ValueSet += square3.RemovePossibleValue;
            square2.ValueUnset += square1.AddPossibleValue;
            square2.ValueUnset += square3.AddPossibleValue;

            square3.ValueSet += square2.RemovePossibleValue;
            square3.ValueSet += square1.RemovePossibleValue;
            square3.ValueUnset += square2.AddPossibleValue;
            square3.ValueUnset += square1.AddPossibleValue;

            square1.Value = 3;
            Assert.IsFalse(square2.PossibleValues.Contains(3));
            Assert.IsFalse(square3.PossibleValues.Contains(3));
            square2.Value = 4;
            Assert.IsFalse(square3.PossibleValues.Contains(4));
            square1.Value = null;
            Assert.IsTrue(square3.PossibleValues.Contains(3));
            Assert.IsFalse(square1.PossibleValues.Contains(4));
            square2.Value = null;
            Assert.IsTrue(square3.PossibleValues.Contains(4));
            Assert.IsTrue(square1.PossibleValues.Contains(4));
            Assert.IsTrue(square2.PossibleValues.Contains(3));
        }
    }
}
