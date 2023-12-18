using System.Timers;

namespace Vistaaa.Views;

public partial class HomePage : ContentPage
{
    private readonly Database Database = new();
    int number = 6549;
    readonly Random r = new();
    readonly List<string> CategoryList = [];
    readonly string stringList = "";
    int iterator = 0;
    public HomePage()
	{
		InitializeComponent();
        var myTimer = new System.Timers.Timer();
        myTimer.Elapsed += new ElapsedEventHandler(IncreaseOffers);
        myTimer.Interval = 3000;  
        myTimer.Enabled = true;

        CategoryList = Task.Run(Database.GetCategories).Result.Select(item => item.Name.ToLower()).ToList();
        stringList = string.Join(';', CategoryList);
        IDispatcherTimer searchTimer = Dispatcher.CreateTimer();
        searchTimer.Tick += SearchTimer_Tick;
        searchTimer.Interval = TimeSpan.FromMilliseconds(800);
        searchTimer.Start();
    }

    private void SearchTimer_Tick(object? sender, EventArgs e)
    {
        if (iterator == stringList.Length)
        {
            MainThread.BeginInvokeOnMainThread(() => searchAnimationLabel.Text = string.Empty);
            iterator = 0;

            return;
        }
        if (stringList[iterator] == ';')
        {
            MainThread.BeginInvokeOnMainThread(() => searchAnimationLabel.Text = string.Empty);
            iterator++;
            return;
        }
        MainThread.BeginInvokeOnMainThread(() => searchAnimationLabel.Text += stringList[iterator]);
        iterator++;
    }

    private void IncreaseOffers(object? source, ElapsedEventArgs e)
    {
        number += r.Next(-3, 6);
        SetCounter();
    }

    private void ChangeSearch(object? source, ElapsedEventArgs e)
    {
        DisplayAlert("Test", iterator.ToString(), "OK");
        if (iterator == stringList.Length)
        {
            searchAnimationLabel.Dispatcher.Dispatch(() => searchAnimationLabel.Text = string.Empty);
            iterator = 0;
            
            return;
        }
        if (stringList[iterator] == ';')
        {
            searchAnimationLabel.Dispatcher.Dispatch(() => searchAnimationLabel.Text = string.Empty);
            iterator++;
            return;
        }   
        searchAnimationLabel.Dispatcher.Dispatch(() => searchAnimationLabel.Text += stringList[iterator]);
        iterator++;     
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

        Shell.Current.GoToAsync($"//offers?category=aaa");
       
    }
}