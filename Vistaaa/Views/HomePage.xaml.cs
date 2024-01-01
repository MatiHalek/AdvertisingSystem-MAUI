using System.Timers;

namespace Vistaaa.Views;

public partial class HomePage : ContentPage
{
    public static bool Navigated { get; set; } = false;
    private readonly Database Database = new();
    int number = 6549;
    readonly Random r = new();
    List<string> CategoryList = [];
    string stringList = "";
    int iterator = 0;
    int currentCategory = 0;
    public HomePage()
	{
		InitializeComponent();
        var myTimer = new System.Timers.Timer();
        myTimer.Elapsed += new ElapsedEventHandler(IncreaseOffers);
        myTimer.Interval = 3000;  
        myTimer.Enabled = true;
        UpdateCategories();
        IDispatcherTimer searchTimer = Dispatcher.CreateTimer();
        searchTimer.Tick += SearchTimer_Tick;
        searchTimer.Interval = TimeSpan.FromMilliseconds(800);
        searchTimer.Start();
    }

    public void UpdateCategories()
    {
        CategoryList = Task.Run(Database.GetCategories).Result.Select(item => item.Name).ToList();
        stringList = string.Join(';', CategoryList);
    }

    private void SearchTimer_Tick(object? sender, EventArgs e)
    {
        if (iterator == stringList.Length)
        {
            MainThread.BeginInvokeOnMainThread(() => searchAnimationLabel.Text = string.Empty);
            iterator = 0;
            UpdateCategories();
            currentCategory = 0;
            return;
        }
        if (stringList[iterator] == ';')
        {
            MainThread.BeginInvokeOnMainThread(() => searchAnimationLabel.Text = string.Empty);
            iterator++;
            currentCategory++;
            return;
        }
        MainThread.BeginInvokeOnMainThread(() => searchAnimationLabel.Text += stringList[iterator].ToString().ToLower());
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
        Navigated = true;
        Shell.Current.GoToAsync($"//offers?Category={CategoryList[currentCategory]}");       
    }
}