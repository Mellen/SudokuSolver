using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SudokuSolver
{
    sealed class SudokuGrid
    {
        private readonly ObservableCollection<SudokuSquare> squares = new ObservableCollection<SudokuSquare>();

        public IEnumerable<SudokuSquare> Squares
        {
            get { return squares; }
        }

        public bool IsCompletelySolved
        {
            get
            {
                return !squares.Any(s => s.Value == null);
            }
        }

        public int SolvedCount
        {
            get
            {
                return squares.Count(s => s.Value != null);
            }
        }

        public void SolveASquare()
        {
            var sortedSquares = squares.Where(s => s.PossibleValues.Count() > 1).OrderBy(s => s.PossibleValues.Count()); 
            bool solvedOne = false;
            List<SudokuSquare> checkedBlockSquares = new List<SudokuSquare>();
            List<SudokuSquare> checkedRowSquares = new List<SudokuSquare>();
            List<SudokuSquare> checkedColSquares = new List<SudokuSquare>();
            IEnumerable<SudokuSquare> solvableSquares = null;
            IEnumerable<int> solvedValues = null;
            foreach (var square in sortedSquares)
            {
                bool rowChecked = checkedRowSquares.Contains(square);
                bool colChecked = checkedColSquares.Contains(square);
                bool blockChecked = checkedBlockSquares.Contains(square);

                if (rowChecked && colChecked && blockChecked)
                    continue;

                if (!blockChecked)
                {
                    var blockSquares = squares.Where(s => s.PossibleValues.Count() > 1 && s.Block == square.Block);
                    checkedBlockSquares.AddRange(blockSquares);
                    solvedOne = CheckForSolved(blockSquares, out solvableSquares, out solvedValues);
                }

                if (solvedOne)
                    break;

                if (!colChecked)
                {
                    var colSquares = squares.Where(s => s.PossibleValues.Count() > 1 && s.Column == square.Column);
                    checkedColSquares.AddRange(colSquares);
                    solvedOne = CheckForSolved(colSquares, out solvableSquares, out solvedValues);
                }

                if (solvedOne)
                    break;

                if (!rowChecked)
                {
                    var rowSquares = squares.Where(s => s.PossibleValues.Count() > 1 && s.Row == square.Row);
                    checkedRowSquares.AddRange(rowSquares);
                    solvedOne = CheckForSolved(rowSquares, out solvableSquares, out solvedValues);
                }

                if (solvedOne)
                    break;
            }

            if (solvedOne)
            {
                foreach (var s in solvableSquares)
                {
                    s.Value = solvedValues.Where(sv => s.PossibleValues.Contains(sv)).First();
                }
            }
        }

        public bool CheckForSolved(IEnumerable<SudokuSquare> squares, out IEnumerable<SudokuSquare> solvableSquares, out IEnumerable<int> solvedValues)
        {
            var pvs = from s in squares
                      from pv in s.PossibleValues
                      select pv;
            solvedValues = from pv in pvs.GroupBy(p => p)
                           where pv.Count() == 1
                           select pv.FirstOrDefault();

            var tempsolved = solvedValues.ToList();

            solvableSquares = from s in squares
                              from spv in tempsolved
                              where s.PossibleValues.Contains(spv)
                              select s;

            return solvableSquares.Count() > 0;
        }

        public SudokuGrid()
        {
            foreach(var row in Enumerable.Range(0,9))
            {
                int startblock = 3 * (row / 3);
                foreach(var col in Enumerable.Range(0,9))
                {
                    int block = startblock + (col / 3);
                    squares.Add(new SudokuSquare(row, col, block));
                }
            }

            foreach (var square in squares)
            {
                var relatedSquares = from s in squares
                                     where (s.Row == square.Row
                                     || s.Column == square.Column
                                     || s.Block == square.Block)
                                     && s != square
                                     select s;
                foreach(var s in relatedSquares)
                {
                    square.ValueSet += s.RemovePossibleValue;
                    square.ValueUnset += s.AddPossibleValue;
                }
            }
        }
    }
}
