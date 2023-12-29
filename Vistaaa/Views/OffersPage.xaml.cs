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
        sortTypePicker.ItemsSource = new List<Tuple<string, SortBy>> { new("Od najnowszych", SortBy.Date), new("Od najlepiej p³atnych", SortBy.Salary) };
        sortTypePicker.SelectedIndex = 0;
        sortTypePicker.SelectedIndexChanged += SortTypePicker_SelectedIndexChanged;
        ICommand RefreshCommand = new Command(() =>
        {
            LoadData();
            refreshView.IsRefreshing = false;
        });
        refreshView.Command = RefreshCommand;
        //refreshView.IsRefreshing = true;
        //database.CreateCompanyAsync(new Company("Vistaaa", "Najlepsza firma zajmuj¹ca siê wspania³ym systemem Windows Vista!", "Zielona", "5", "Limanowa", "34-600"));
        //database.CreateAdvertisementAsync(new Advertisement("Programowanie w Scratchu", 1, "Programista", "Junior Developer", "Umowa o pracê", "Pe³en etat", "Praca zdalna", 10000.07m, 20000.78m, "aaa", DateTime.Now, DateTime.Now, "aaa", "aaa", "aaa"));
        //database.CreateUserAsync(new User("Mateusz", "MarmuŸniak", new DateTime(2005, 2, 7), "mateusz.marmuzniak.poland@gmail.com", PasswordHasher.Hash("zaq1@WSX"), "123456789", "Limanowa", "Polska", "34-600", "Limanowa", "Zielona", "5", true));
    }

    private async void LoadData()
    {
        loading.IsRunning = true;
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
        deleteAllButton.Text = $"Usuñ zaznaczone ({AdvertisementCollectionView.SelectedItems.Count})";
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

    private void EditAdvertisementMenuItem_Clicked(object sender, EventArgs e)
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
        bool alertResult = await DisplayAlert("Usuwanie og³oszeñ", $"Czy na pewno chcesz usun¹æ wszystkie zaznaczone og³oszenia: {AdvertisementCollectionView.SelectedItems.Count}?\n\nTej operacji nie mo¿na cofn¹æ!", "Tak", "Nie");
        if (!alertResult)
            return;
        foreach(Advertisement advertisement in AdvertisementCollectionView.SelectedItems.Cast<Advertisement>())
        {
            await Database.DeleteAdvertisementAsync(advertisement);
        }
        LoadData();
    }
}