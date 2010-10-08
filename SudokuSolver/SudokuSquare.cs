﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SudokuSolver
{
    sealed class ValueSetEventArgs : EventArgs
    {
        private readonly int value;
        public int Value { get { return value; } }
        public ValueSetEventArgs(int value)
        {
            this.value = value;
        }
    }

    sealed class SudokuSquare: INotifyPropertyChanged, IComparable<SudokuSquare>, IComparable
    {
        private readonly List<int> possibleValues = new List<int>();
        private readonly List<int> possibleValuesBackup = new List<int>();
        private readonly int row;
        private readonly int column;
        private readonly int block;
        private bool setByHand;

        public event EventHandler<ValueSetEventArgs> ValueSet;
        public event EventHandler<ValueSetEventArgs> ValueUnset;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<int> PossibleValues { get { return possibleValues; } }

        public int? Value
        {
            get
            {
                if (possibleValues.Count == 1)
                {
                    return possibleValues[0];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                setByHand = true;
                SetValue(value);
            }
        }

        public int Row { get { return row; } }
        public int Column { get { return column; } }
        public int Block { get { return block; } }

        public SudokuSquare(int row, int column, int block)
        {
            possibleValues.AddRange(Enumerable.Range(1, 9));
            this.row = row;
            this.column = column;
            this.block = block;
        }

        private void RaiseValueSetEvent(int value)
        {
            if (ValueSet != null)
            {
                ValueSet(this, new ValueSetEventArgs(value));
            }
        }

        private void RaiseValueUnsetEvent(int value)
        {
            if (ValueUnset != null)
            {
                ValueUnset(this, new ValueSetEventArgs(value));
            }
        }

        private void RaisePropertyChangedEvent(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public void SetValue(int? value)
        {
            if (value.HasValue)
            {
                possibleValuesBackup.AddRange(possibleValues);
                possibleValues.Clear();
                possibleValues.Add(value.Value);
                RaisePropertyChangedEvent("Value");
                RaiseValueSetEvent(value.Value);
            }
            else
            {
                RaiseValueUnsetEvent(possibleValues[0]);
                possibleValues.Clear();
                possibleValues.AddRange(possibleValuesBackup);
                setByHand = false;
            }
        }

        public void RemovePossibleValue(object sender, ValueSetEventArgs arg)
        {
            if (possibleValues.Count > 1)
            {
                possibleValues.Remove(arg.Value);
                if (Value != null)
                {
                    RaisePropertyChangedEvent("Value");
                    RaiseValueSetEvent(Value.Value);
                }
            }
            else if(setByHand)
            {
                possibleValuesBackup.Remove(arg.Value);
            }
        }

        public void AddPossibleValue(object sender, ValueSetEventArgs arg)
        {
            if (!possibleValues.Contains(arg.Value) && !setByHand)
            {
                possibleValues.Add(arg.Value);
            }
            else if (!possibleValuesBackup.Contains(arg.Value) && setByHand)
            {
                possibleValuesBackup.Add(arg.Value);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(SudokuSquare other)
        {
            if (other == null)
            {
                return 1;
            }

            int thisScore = possibleValues.Count;
            int otherScore = other.possibleValues.Count;
            thisScore += row + column + block;
            otherScore += other.row + other.column + other.block;
            return thisScore.CompareTo(otherScore);
        }

        public int CompareTo(object obj)
        {
            SudokuSquare other = obj as SudokuSquare;
            return CompareTo(other);
        }
    }
}