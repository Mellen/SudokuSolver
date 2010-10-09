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

        [TestMethod]
        public void SwitchValueTest()
        {
            SudokuSquare square1 = new SudokuSquare(0, 0, 0);
            SudokuSquare square2 = new SudokuSquare(0, 1, 0);

            square1.ValueSet += square2.RemovePossibleValue;
            square1.ValueUnset += square2.AddPossibleValue;
            
            square2.ValueSet += square1.RemovePossibleValue;
            square2.ValueUnset += square1.AddPossibleValue;

            square1.Value = 3;
            Assert.IsFalse(square2.PossibleValues.Contains(3));
            square1.Value = 4;
            Assert.IsTrue(square2.PossibleValues.Contains(3));
            Assert.IsFalse(square2.PossibleValues.Contains(4));
        }

        [TestMethod]
        public void MultipleValueSwitchTest()
        {
            SudokuSquare[] squares = new SudokuSquare[9];
            for(int i = 0; i < 9; i++)
            {
                squares[i] = new SudokuSquare(0, 0, 0);
            }
            foreach (var square in squares)
            {
                var otherSquares = from s in squares
                                   where s != square
                                   select s;
                foreach (var otherSquare in otherSquares)
                {
                    square.ValueSet += otherSquare.RemovePossibleValue;
                    square.ValueUnset += otherSquare.AddPossibleValue;
                }
            }

            squares[0].Value = 4;
            squares[1].Value = 9;
            squares[2].Value = 8;
            squares[3].Value = 6;
            squares[4].Value = 1;
            squares[5].Value = 2;
            squares[6].Value = 3;
            squares[7].Value = 7;
            squares[8].Value = 5;

            squares[8].Value = 6;
            squares[5].Value = 9;
            squares[3].Value = null;
            squares[1].Value = null;

            Assert.IsTrue(squares[3].PossibleValues.Contains(2));
            Assert.IsTrue(squares[3].PossibleValues.Contains(5));
            Assert.IsTrue(squares[1].PossibleValues.Contains(2));
            Assert.IsTrue(squares[1].PossibleValues.Contains(5));

            squares[3].Value = 5;

            Assert.AreEqual(2, squares[1].Value);
        }
    }
}
