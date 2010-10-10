using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace SudokuSolver
{
    [ValueConversion(typeof(int?), typeof(String))]
    class StringToNullableIntConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(String.IsNullOrEmpty((value as string).Trim()))
            {
                return null;
            }

            return System.Convert.ToInt32(value);
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MemoryStream iconstream = new MemoryStream();
            Properties.Resources.sudokusolver.Save(iconstream);
            iconstream.Seek(0, SeekOrigin.Begin);
            this.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(iconstream);
            InitializeComponent();
            DataContext = new SudokuGrid();
            foreach (var square in (DataContext as SudokuGrid).Squares)
            {
                TextBox sudokuSquare = new TextBox();
                sudokuSquare.DataContext = square;
                sudokuSquare.SetBinding(TextBox.TextProperty, new Binding("Value") { Converter = new StringToNullableIntConverter(), Mode = BindingMode.TwoWay });
                sudokuSquare.Width = 25;
                sudokuSquare.FontFamily = new System.Windows.Media.FontFamily("Tahoma");
                sudokuSquare.FontSize = 14;
                sudokuGrid.Children.Add(sudokuSquare);
                sudokuSquare.SetValue(Grid.RowProperty, square.Row);
                sudokuSquare.SetValue(Grid.ColumnProperty, square.Column);
                double bottomThickness = (square.Row == 2 || square.Row == 5) ? 2 : 0;
                double rightThickness = (square.Column == 2 || square.Column == 5) ? 2 : 0;
                sudokuSquare.Margin = new Thickness(0, 0, rightThickness, bottomThickness);
                sudokuSquare.SetBinding(TextBox.ToolTipProperty, "PossibleValuesString");
            }
        }

        private void SolveClick(object sender, RoutedEventArgs e)
        {
            SudokuGrid grid = DataContext as SudokuGrid;
            while (!grid.IsCompletelySolved)
            {
                int lastCount = grid.SolvedCount;
                grid.SolveASquare();
                if (grid.SolvedCount == lastCount)
                {
                    MessageBox.Show("There are not enough solved squares to solved this grid automatically. Please fill some more in.");
                    break;
                }
            }
        }
    }
}
