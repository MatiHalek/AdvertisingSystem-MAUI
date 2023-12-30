using System.Windows.Input;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class OffersPage : ContentPage
{
    private readonly Database Database = new();
    uint ADVERTISEMENTS_PER_PAGE = 10;
    uint currentPage = 1;
    public List<Advertisement>? AdvertisementList { get; set; }
    protected override void OnAppearing()
    {
        base.OnAppearing();        
        if(Preferences.ContainsKey("userId"))
            onlySavedStackLayout.IsEnabled = true;
        else
        {
            onlySavedStackLayout.IsEnabled = false;
            onlySavedCheckbox.IsChecked = false;
        }   
        LoadData();        
    }
    public OffersPage()
    {
        InitializeComponent();
        picker.ItemsSource = ["Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6"];
        picker.SelectedIndices = [2, 4, 5];
        advertisementsOnPagePicker.ItemsSource = new List<uint> { 10, 20, 40, 100 };
        advertisementsOnPagePicker.SelectedIndex = 0;
        advertisementsOnPagePicker.SelectedIndexChanged += AdvertisementsOnPagePicker_SelectedIndexChanged;
        sortTypePicker.ItemsSource = new List<Tuple<string, SortBy>> { new("Od najnowszych", SortBy.Date), new("Od najlepiej p�atnych", SortBy.Salary) };
        sortTypePicker.SelectedIndex = 0;
        sortTypePicker.SelectedIndexChanged += SortTypePicker_SelectedIndexChanged;
        ICommand RefreshCommand = new Command(() =>
        {
            LoadData();
            refreshView.IsRefreshing = false;
        });
        refreshView.Command = RefreshCommand;
        //refreshView.IsRefreshing = true;
    }

    private async void LoadData()
    {
        loading.IsRunning = true;
        deleteAllButton.IsEnabled = false;
        deleteAllButton.Text = "Usu� zaznaczone (0)";
        loading.IsVisible = true;
        string searchBarText = string.Empty;
        if (searchBar.Text != null)
            searchBarText = searchBar.Text;
        int advertisementCount = (await Database.GetAdvertisementsAsync(searchBarText.Trim(), ((Tuple<string, SortBy>)sortTypePicker.SelectedItem).Item2, onlySavedCheckbox.IsChecked)).Count;
        if (ADVERTISEMENTS_PER_PAGE * currentPage == advertisementCount + ADVERTISEMENTS_PER_PAGE)
            currentPage--;
        AdvertisementList = await Database.GetAdvertisementsAsync(currentPage, ADVERTISEMENTS_PER_PAGE, searchBarText.Trim(), ((Tuple<string, SortBy>)sortTypePicker.SelectedItem).Item2, onlySavedCheckbox.IsChecked);
        AdvertisementCollectionView.ItemsSource = AdvertisementList;
        pageButtons.Children.Clear();
        if (AdvertisementList.Count > 0)
        {
            headerCollectionViewLabel.Text = $"Znalezione oferty: {advertisementCount}";
            headerCollectionViewLabel.IsVisible = true;
            pageButtons.Children.Add(new Button()
            {
                Text = currentPage.ToString(),
            });
            footerCollectionViewStackLayout.IsVisible = true;
        }
        else
        {
            headerCollectionViewLabel.IsVisible = false;
            footerCollectionViewStackLayout.IsVisible = false;
        }
        if (currentPage < 2)
        {
            previousPageBtn.IsEnabled = false;
            firstPageBtn.IsEnabled = false;
        }
        else
        {
            previousPageBtn.IsEnabled = true;
            firstPageBtn.IsEnabled = true;
        }
        if (currentPage == Math.Ceiling(advertisementCount / (float)ADVERTISEMENTS_PER_PAGE))
        {
            nextPageBtn.IsEnabled = false;
            lastPageBtn.IsEnabled = false;
        }
        else
        {
            nextPageBtn.IsEnabled = true;
            lastPageBtn.IsEnabled = true;
        }
        if (!emptyCollectionViewPlaceholder.IsVisible)
            emptyCollectionViewPlaceholder.IsVisible = true;
        loading.IsVisible = false;
        loading.IsRunning = false;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        currentPage = 1;
        LoadData();
    }
    private void PreviousPageButton_Clicked(object sender, EventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            LoadData();
        }
    }
    private void NextPageButton_Clicked(object sender, EventArgs e)
    {
        currentPage++;
        LoadData();
    }

    private void FirstPageButton_Clicked(object sender, EventArgs e)
    {
        currentPage = 1;
        LoadData();
    }

    private void LastPageButton_Clicked(object sender, EventArgs e)
    {
        refreshView.IsRefreshing = true;
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddOrEditAdvertisement());
    }

    private void AdvertisementCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        deleteAllButton.Text = $"Usu� zaznaczone ({AdvertisementCollectionView.SelectedItems.Count})";
        if(AdvertisementCollectionView.SelectedItems.Count > 0)          
            deleteAllButton.IsEnabled = true;
        else
            deleteAllButton.IsEnabled = false;
    }

    private void AdvertisementsOnPagePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        currentPage = 1;
        ADVERTISEMENTS_PER_PAGE = (uint)advertisementsOnPagePicker.SelectedItem;
        LoadData();
    }
    private void SortTypePicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadData();
    }

    private void EditAdvertisementMenuItem_Clicked(object? sender, EventArgs e)
    {
        Advertisement advertisement = new();
        if (sender is MenuFlyoutItem menuItem)
            advertisement = (Advertisement)menuItem.CommandParameter;
        if (sender is SwipeItem swipeItem)
            advertisement = (Advertisement)swipeItem.CommandParameter;
        Navigation.PushAsync(new AddOrEditAdvertisement(advertisement));
    }

    private void DetailsButton_Clicked(object sender, EventArgs e)
    {
        if (AdvertisementCollectionView.SelectionMode is not SelectionMode.None)
            return;
        Button button = (Button)sender;
        Advertisement advertisement = (Advertisement)button.BindingContext;
        Navigation.PushAsync(new AdvertisementPage(advertisement));
    }

    private void DeleteSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if(e.Value == true)
            AdvertisementCollectionView.SelectionMode = SelectionMode.Multiple;
        else
        {
            AdvertisementCollectionView.SelectedItems.Clear();
            AdvertisementCollectionView.SelectionMode = SelectionMode.None;
        }            
    }

    private void OnlySavedLabel_Tapped(object sender, TappedEventArgs e)
    {
        if(onlySavedCheckbox.IsEnabled)
            onlySavedCheckbox.IsChecked = !onlySavedCheckbox.IsChecked;
    }

    private void OnlySavedCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        LoadData();
    }

    private async void DeleteAllButton_Clicked(object sender, EventArgs e)
    {
        bool alertResult = await DisplayAlert("Usuwanie og�osze�", $"Czy na pewno chcesz usun�� wszystkie zaznaczone og�oszenia: {AdvertisementCollectionView.SelectedItems.Count}?\n\nTej operacji nie mo�na cofn��!", "Tak", "Nie");
        if (!alertResult)
            return;
        foreach(Advertisement advertisement in AdvertisementCollectionView.SelectedItems.Cast<Advertisement>())
        {
            List<UserAdvertisement> userAdvertisements = await Database.GetUserAdvertisements(advertisement.Id);
            foreach(UserAdvertisement userAdvertisement in userAdvertisements)
                await Database.DeleteUserAdvertisementAsync(userAdvertisement);
            await Database.DeleteAdvertisementAsync(advertisement);
        }
        LoadData();
    }

    private async void AdvertisementBorder_Loaded(object sender, EventArgs e)
    {
        if (!Preferences.ContainsKey("userId"))
            return;
        User currentUser = await Database.GetUserAsync(uint.Parse(Preferences.Get("userId", null) ?? ""));
        if (!currentUser.IsAdmin)
            return;
        MenuFlyoutItem menuFlyoutItem = new()
        {
            Text = "Edytuj to og�oszenie"
        };
        menuFlyoutItem.Clicked += EditAdvertisementMenuItem_Clicked;
        menuFlyoutItem.CommandParameter = (Advertisement)((Border)sender).BindingContext;
        MenuFlyout menuFlyout = [];
        menuFlyout.Add(menuFlyoutItem);
        FlyoutBase.SetContextFlyout(sender as Border, menuFlyout);
    }

    private async void AdvertisementSwipeView_Loaded(object sender, EventArgs e)
    {
        if (!Preferences.ContainsKey("userId"))
            return;
        User currentUser = await Database.GetUserAsync(uint.Parse(Preferences.Get("userId", null) ?? ""));
        if (!currentUser.IsAdmin)
            return;
        SwipeItems swipeItems = [];
        SwipeItem swipeItem = new()
        {
            Text = "Edytuj",
            BackgroundColor = Colors.LightGreen,
            CommandParameter = (Advertisement)((SwipeView)sender).BindingContext,
        };
        swipeItem.Invoked += EditAdvertisementMenuItem_Clicked;
        swipeItems.Add(swipeItem);
        ((SwipeView)sender).LeftItems = swipeItems;    
    }
}