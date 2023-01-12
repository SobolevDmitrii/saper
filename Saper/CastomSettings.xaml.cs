using System;
using System.Windows;

namespace Saper;

public partial class CastomSettings : Window
{
    public int x = 10;
    public int y = 10;
    public double? percent = 0.1;
    public double? countBombs = 10;
    public Window _mainWindow;
    public CastomSettings(Window mainWindow)
    {
        _mainWindow = mainWindow;
        InitializeComponent();
    }

    private void SaveButtonClick(object sender, RoutedEventArgs e)
    {
        x = Convert.ToInt32(XCount.Text);
        y = Convert.ToInt32(YCount.Text);
        if (PercentBombs.Text != "")
        {
            percent = Convert.ToDouble(PercentBombs.Text) / 100;
        }
        else
        {
            percent = null;
        }

        if (CountBombs.Text != "")
        {
            countBombs = Convert.ToDouble(CountBombs.Text);
        }
        else
        {
            countBombs = null;
        }
        MainWindow.SelfRef.GetSettingsCastomGame(x,y,countBombs,percent);
        this.Close();
    }
    
    private void ExitButtonClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
    
    
}