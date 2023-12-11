using System.Timers;

namespace Vistaaa.Views;

public partial class HomePage : ContentPage
{
    int number = 6549;
    readonly Random r = new();
    readonly List<string> list = ["programowanie", "kierowca", "elektryk"];
    readonly string stringList = "";
    int iterator = 0;
    public HomePage()
	{
		InitializeComponent();
        var myTimer = new System.Timers.Timer();
        myTimer.Elapsed += new ElapsedEventHandler(IncreaseOffers);
        myTimer.Interval = 3000;  
        myTimer.Enabled = true;

        stringList = string.Join(';', list);
        var searchTimer = new System.Timers.Timer();    
        searchTimer.Elapsed += new ElapsedEventHandler(ChangeSearch);
        searchTimer.Interval = 800;
        searchTimer.Enabled = true;
    }

    private void IncreaseOffers(object? source, ElapsedEventArgs e)
    {
        number += r.Next(-3, 6);
        SetCounter();
    }

    private void ChangeSearch(object? source, ElapsedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => {
            if(iterator == stringList.Length)
            {
                searchAnimationLabel.Text = string.Empty;
                iterator = 0;
                return;
            }    
            if (stringList[iterator] == ';')
            {
                searchAnimationLabel.Text = string.Empty;
                iterator++;
                return;
            } 
            searchAnimationLabel.Text += stringList[iterator];
            iterator++;                         
        });
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

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//offers");
    }
}