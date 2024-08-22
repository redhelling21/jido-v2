using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Jido.UI.Components.Pages.InventoryManagement
{
    public class InventoryOverlay : Window
    {
        private const int GridWidth = 12;
        private const int GridHeight = 5;

        private bool[][] _cells;

        public InventoryOverlay(int width, int height)
        {
            Title = "Inventory";
            Background = new SolidColorBrush(Colors.Transparent);
            //TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;

            Width = width;
            Height = height;
            _cells = new bool[GridWidth][];
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions(string.Join(",", Enumerable.Repeat("*", GridWidth))),
                RowDefinitions = new RowDefinitions(string.Join(",", Enumerable.Repeat("*", GridHeight)))
            };
            for (int col = 0; col < GridWidth; col++)
            {
                _cells[col] = new bool[GridHeight];
                for (int row = 0; row < GridHeight; row++)
                {
                    var cell = new Border
                    {
                        Background = new SolidColorBrush(Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(1)
                    };

                    cell.PointerPressed += Cell_PointerPressed;

                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);
                    grid.Children.Add(cell);
                }
            }
            Content = grid;
        }

        public void GetInventoryConfig()
        {
        }

        private void Cell_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender is Border cell)
            {
                var x = Grid.GetColumn(cell);
                var y = Grid.GetRow(cell);
                _cells[x][y] = !_cells[x][y];
                if (_cells[x][y])
                {
                    cell.Background = new SolidColorBrush(Colors.Red, 0.7);
                }
                else
                {
                    cell.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
        }
    }
}
