using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuGridTests
    {
        SudokuGrid grid = new SudokuGrid();

        public SudokuGridTests()
        {
            grid.Squares.ElementAt(0).Value = 3;
            grid.Squares.ElementAt(5).Value = 8;
            grid.Squares.ElementAt(8).Value = 6;
            grid.Squares.ElementAt(10).Value = 1;
            grid.Squares.ElementAt(15).Value = 2;
            grid.Squares.ElementAt(16).Value = 5;
            grid.Squares.ElementAt(19).Value = 4;
            grid.Squares.ElementAt(22).Value = 3;
            grid.Squares.ElementAt(23).Value = 5;
            grid.Squares.ElementAt(27).Value = 8;
            grid.Squares.ElementAt(29).Value = 5;
            grid.Squares.ElementAt(30).Value = 4;
            grid.Squares.ElementAt(32).Value = 6;
            grid.Squares.ElementAt(38).Value = 2;
            grid.Squares.ElementAt(42).Value = 1;
            grid.Squares.ElementAt(48).Value = 2;
            grid.Squares.ElementAt(50).Value = 7;
            grid.Squares.ElementAt(51).Value = 8;
            grid.Squares.ElementAt(53).Value = 5;
            grid.Squares.ElementAt(57).Value = 3;
            grid.Squares.ElementAt(58).Value = 7;
            grid.Squares.ElementAt(61).Value = 2;
            grid.Squares.ElementAt(64).Value = 2;
            grid.Squares.ElementAt(65).Value = 4;
            grid.Squares.ElementAt(70).Value = 7;
            grid.Squares.ElementAt(72).Value = 7;
            grid.Squares.ElementAt(75).Value = 8;
            grid.Squares.ElementAt(80).Value = 4;
        }

        [TestMethod]
        public void CheckForSolvedTest()
        {
            var squares = from s in grid.Squares
                          where s.Block == 1
                          select s;
            IEnumerable<SudokuSquare> solvableSquares = null;
            IEnumerable<int> solvedValues = null;
            bool solvedOne = grid.CheckForSolved(squares, out solvableSquares, out solvedValues);
            Assert.IsTrue(solvedOne);
            Assert.IsTrue(solvedValues.Contains(2));
            Assert.IsTrue(solvableSquares.Contains(grid.Squares.ElementAt(4)));
        }

        [TestMethod]
        public void SolveASquareTest()
        {
            int solved = grid.SolvedCount;

            grid.SolveASquare();

            Assert.AreEqual(solved + 2, grid.SolvedCount);
        }
    }
}
