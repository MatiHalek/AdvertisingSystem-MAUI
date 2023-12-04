using System.Timers;

namespace Vistaaa.Views;

public partial class HomePage : ContentPage
{
    int number = 6549;
    Random r = new();
    public HomePage()
	{
		InitializeComponent();
        var myTimer = new System.Timers.Timer();
        myTimer.Elapsed += new ElapsedEventHandler(IncreaseOffers);
        myTimer.Interval = 2000;  
        myTimer.Enabled = true;
    }
    private void IncreaseOffers(object? source, ElapsedEventArgs e)
    {
         number += r.Next(-3, 6);
        SetCounter();
    }
    private void SetCounter()
    {
        string numberString = number.ToString();
        for (int i = 0; i < numberString.Length; i++)
        {
            var stackLayout = (StackLayout)counter.Children[i];
            var grid = (Grid)stackLayout.Children[0];
            _ = grid.TranslateTo(0, -50 * int.Parse(numberString[i].ToString()), 500 + 100 * (uint)i, Easing.SinInOut);
        }
    }
}