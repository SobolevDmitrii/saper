using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Saper.Field;
using Saper.GameUnits;

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int ConstSize = 22;
        private double _countBomb = Math.Round(_xField * _yField * percent);
        private double _countWinBomb = Math.Round(_xField * _yField * percent);
        private static int _xField = 10;
        private static int _yField = 10;
        private static double percent = 0.1;

        private double CountBomb
        {
            get => _countBomb;
            set
            {
                _countBomb = value;
                CountBombsTextBox.Text = _countBomb.ToString();
            }
        }

        public static MainWindow SelfRef { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            SelfRef = this;
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            var gameField = new BackGroundField().CreateGameField(_xField, _yField);
            CreateButtonField(_xField, _yField, gameField);
        }

        private void CreateButtonField(int xField, int yField, Unit[,] unit)
        {
            double left = 0;
            var top = 0;

            for (int i = 0; i < xField; i++)
            {
                for (int j = 0; j < yField; j++)
                {
                    var button = new Button
                    {
                        Height = ConstSize,
                        Width = ConstSize,
                    };
                    button.Name = $"Button_{i}_{j}";
                    var buttonMargin = button.Margin;
                    buttonMargin.Left = left;
                    buttonMargin.Top = top;
                    button.Margin = buttonMargin;
                    button.Click += LeftButtonClick;
                    button.MouseRightButtonDown += RightButttonClick;
                    button.Tag = unit[i, j];
                    CanvasGame.Children.Add(button);
                    left += ConstSize - 2;
                }

                top += ConstSize - 2;
                left = 0;
            }
        }

        private void RightButttonClick(object sender, MouseButtonEventArgs e)
        {
            var countBombs = 0;
            if (e.RightButton == MouseButtonState.Pressed)
            {
                //MessageBox.Show($"right");
                var button = (Button) sender;
                if (button.Content == null || button.Content.ToString() == "10")
                {
                    button.Content = "F";
                    button.Click -= LeftButtonClick;
                    CountBomb--;

                    foreach (var children in CanvasGame.Children)
                    {
                        if (children.GetType() == typeof(Button))
                        {
                            var butChildren = (Button) children;
                            if (((Unit) butChildren.Tag)._bomb == true && butChildren.Content == "F")
                            {
                                countBombs++;
                            }
                        }
                    }

                    if (countBombs == _countWinBomb)
                    {
                        MessageBox.Show("You win");
                        foreach (var children in CanvasGame.Children)
                        {
                            if (children.GetType() == typeof(Button))
                            {
                                var butChildren = (Button) children;
                                butChildren.IsEnabled = false;
                                if (((Unit) butChildren.Tag).countBombs == 10)
                                {
                                    Image img = new Image();
                                    img.Source = new BitmapImage(new Uri(@"D:\project\Saper\Saper\bombs.png"));

                                    StackPanel stackPnl = new StackPanel();
                                    stackPnl.Orientation = Orientation.Horizontal;
                                    butChildren.Content = img;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (button.Content.ToString() == "F")
                    {
                        button.Content = null;
                        if (((Unit) button.Tag)._bomb == true)
                        {
                            countBombs--;
                        }

                        CountBomb++;
                        button.Click += LeftButtonClick;
                    }
                }
            }
        }

        public void ColorNumber(Button button)
        {
            switch (button.Content.ToString())
            {
                case "1":
                    button.Foreground = Brushes.Green;
                    button.FontWeight = FontWeights.UltraBold;
                    break;
                case "2":
                    button.Foreground = Brushes.Blue;
                    button.FontWeight = FontWeights.UltraBold;
                    break;
                case "3":
                    button.Foreground = Brushes.Red;
                    button.FontWeight = FontWeights.UltraBold;
                    break;
                case "4":
                    button.Foreground = Brushes.Orange;
                    button.FontWeight = FontWeights.UltraBold;
                    break;
                case "5":
                    button.Foreground = Brushes.Gold;
                    button.FontWeight = FontWeights.UltraBold;
                    break;
                case "6":
                    button.Foreground = Brushes.Black;
                    button.FontWeight = FontWeights.UltraBold;
                    break;
            }
        }

        
        public void OpenField(List<int> groupT)
        {
            var tempGroup = groupT;
            foreach (var children in CanvasGame.Children)
            {
                if (children.GetType() == typeof(Button))
                {
                    var butChildren = (Button) children;

                    var color = butChildren.Background;
                    foreach (var @group in groupT.Where(@group =>
                                 ((Unit) butChildren.Tag).@group.Any(q => q == @group)))
                    {
                        if (((Unit) butChildren.Tag).countBombs == 0)
                        {
                            if (butChildren.Content == null || butChildren.Content.ToString() == "0")
                            {
                                butChildren.Content = "";
                                butChildren.Background = Brushes.White;
                            }
                        }
                        else
                        {
                            if (butChildren.Content == null || butChildren.Content.ToString() != "F")
                            {
                                butChildren.Content = ((Unit) butChildren.Tag).countBombs;
                                butChildren.Background = Brushes.White;
                                ColorNumber(butChildren);
                            }
                        }

                        if (butChildren.Content.ToString() == "F")
                        {
                            butChildren.Content = "F";
                            butChildren.Background = color;
                        }
                    }

                    var ti = new List<int>(groupT);
                    foreach (var t in ti)
                    {
                        if (tempGroup.Any(q => q == t)!)
                            tempGroup.Add(t);
                    }

                    /*if (((Unit) butChildren.Tag).group.Any(q => groupT.Contains(q)) && ((Unit) butChildren.Tag).countBombs == 0)
                    {
                        tempGroup.AddRange(groupT);
                    }
                    */
                    /*if (((Unit) butChildren.Tag).countBombs == 0)
                    {
                        foreach (var chBut in ((Unit) butChildren.Tag).group)
                        {
                            if (groupT.Any(q => q != chBut))
                            {
                                tempGroup.Add(chBut);
                            }
                        }
                    }*/

                    tempGroup = tempGroup.Distinct().ToList();
                    /*if (((Unit) butChildren.Tag).group.Any(q => groupTemp.Contains(q)))
                     {
                         butChildren.Content = ((Unit) butChildren.Tag).countBombs;
                         //butChildren.IsEnabled = false;
                         /*if (((Unit) butChildren.Tag).group.Count > 1 && ((Unit) butChildren.Tag).countBombs == 0)
                         {
                             groupTemp = new List<int>(((Unit) butChildren.Tag).group);
                         }#1#
                         /*if (((Unit) butChildren.Tag).countBombs == 0)
                         {
                             groupTemp= new List<int>(((Unit) butChildren.Tag).group);
                         }#1#
                     }*/
                }
            }

            if (tempGroup.Count > groupT.Count)
                OpenField(tempGroup);
        }

        private void LeftButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            if (button.Content == "F")
            {
                return;
            }

            var unit = (Unit) button.Tag;
            button.Content = unit.countBombs;
            button.Background = Brushes.White;
            ColorNumber(button);
            if (unit.countBombs == 0)
            {
                var groupTemp = unit.group;
                var tempGroup = new List<int>();
                //OpenGroup(xField,yField,group);
                OpenField(groupTemp);
                /*foreach (var children in CanvasGame.Children)
                {
                    if (children.GetType() == typeof(Button))
                    {
                        var butChildren = (Button) children;

                        foreach (var @group in groupTemp.Where(@group =>
                                     ((Unit) butChildren.Tag).@group.Any(q => q == @group)))
                        {
                            if (((Unit) butChildren.Tag).countBombs == 0)
                            {
                                butChildren.Content = "";
                                butChildren.Background = Brushes.White;
                            }
                            else
                            {
                                butChildren.Content = ((Unit) butChildren.Tag).countBombs;
                                butChildren.Background = Brushes.White;
                                ColorNumber(butChildren);
                            }
                        }

                        if (((Unit) butChildren.Tag).group.Any(q => groupTemp.Contains(q)) && ((Unit) butChildren.Tag).countBombs == 0)
                        {
                            
                        }
                        /*if (((Unit) butChildren.Tag).group.Any(q => groupTemp.Contains(q)))
                         {
                             butChildren.Content = ((Unit) butChildren.Tag).countBombs;
                             //butChildren.IsEnabled = false;
                             /*if (((Unit) butChildren.Tag).group.Count > 1 && ((Unit) butChildren.Tag).countBombs == 0)
                             {
                                 groupTemp = new List<int>(((Unit) butChildren.Tag).group);
                             }#2#
                             /*if (((Unit) butChildren.Tag).countBombs == 0)
                             {
                                 groupTemp= new List<int>(((Unit) butChildren.Tag).group);
                             }#2#
                         }#1#
                    }
                }*/
            }

            if (unit.countBombs == 10)
            {
                foreach (var children in CanvasGame.Children)
                {
                    if (children.GetType() == typeof(Button))
                    {
                        var butChildren = (Button) children;
                        butChildren.IsEnabled = false;
                        if (((Unit) butChildren.Tag).countBombs == 10 && butChildren.Content == "F")
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"D:\project\Saper\Saper\bombs.png"));
                            butChildren.Content = img;
                        }

                        if (((Unit) butChildren.Tag).countBombs == 10 &&
                            (butChildren.Content == null || butChildren.Content.ToString() == "10"))
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"D:\project\Saper\Saper\bombsX.png"));
                            butChildren.Content = img;
                        }
                    }
                }

                MessageBox.Show($"you lose");
            }
        }

        private void EasyButtonClick(object sender, RoutedEventArgs e)
        {
            _xField = 10;
            _yField = 10;
            CountBomb = Math.Round(_xField * _yField * percent);
            _countWinBomb = Math.Round(_xField * _yField * percent);
            CountBombsTextBox.Text = CountBomb.ToString();
            CanvasGame.Children.Clear();
            var gameField = new BackGroundField().CreateGameField(_xField, _yField);
            CreateButtonField(_xField, _yField, gameField);
            SizeGameField();
            
        }

        private void NormalButtonClick(object sender, RoutedEventArgs e)
        {
            _xField = 16;
            _yField = 16;
            CountBomb = Math.Round(_xField * _yField * percent);
            _countWinBomb = Math.Round(_xField * _yField * percent);
            CountBombsTextBox.Text = CountBomb.ToString();
            CanvasGame.Children.Clear();
            var gameField = new BackGroundField().CreateGameField(_xField, _yField);
            CreateButtonField(_xField, _yField, gameField);
            SizeGameField();
        }

        private void HardButtonClick(object sender, RoutedEventArgs e)
        {
            _xField = 16;
            _yField = 30;
            CountBomb = Math.Round(_xField * _yField * percent);
            _countWinBomb = Math.Round(_xField * _yField * percent);
            CountBombsTextBox.Text = CountBomb.ToString();
            CanvasGame.Children.Clear();
            var gameField = new BackGroundField().CreateGameField(_xField, _yField);
            CreateButtonField(_xField, _yField, gameField);
            SizeGameField();
        }

        public void SizeGameField()
        {
            CanvasGame.Height = (ConstSize-2) * _xField;
            CanvasGame.Width = (ConstSize-2) * _yField;
            CanvasGame.Background = Brushes.Aqua;
            this.Height = 90 + CanvasGame.Height;
            this.Width = 25+CanvasGame.Width;
        }

        
        private void CastomClick(object sender, RoutedEventArgs e)
        {
            var castomForm = new CastomSettings(this);
            castomForm.Show();
            
        }

        public void GetSettingsCastomGame(int x, int y, double? count = null, double? percent = null)
        {
            _xField = x;
            _yField = y;
            if (percent != null)
            {
                CountBomb = Math.Round((double) (_xField * _yField * percent));
                _countWinBomb = Math.Round((double) (_xField * _yField * percent));
            }

            if (count != null)
            {
                CountBomb = (double) count;
                _countWinBomb = (double) count;
            }

            CountBombsTextBox.Text = CountBomb.ToString();
            CanvasGame.Children.Clear();
            var gameField = new BackGroundField().CreateGameField(_xField, _yField);
            CreateButtonField(_xField, _yField, gameField);
            SizeGameField();
        }

    }
}