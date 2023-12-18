using System.Windows.Input;
using Vistaaa.Classes;
using Vistaaa.Models;
using Vistaaa.Views;

namespace Vistaaa
{
    public partial class MainPage : ContentPage
    {
        readonly Database database;
        uint ADVERTISEMENTS_PER_PAGE = 10;
        uint currentPage = 1;
        public List<Advertisement>? AdvertisementList { get; set; }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();            
        }
        public MainPage(Database database)
        {
            InitializeComponent();
            advertisementsOnPagePicker.ItemsSource = new List<uint> { 10, 20, 40, 100 };
            advertisementsOnPagePicker.SelectedIndex = 0;
            advertisementsOnPagePicker.SelectedIndexChanged += AdvertisementsOnPagePicker_SelectedIndexChanged;
            sortTypePicker.ItemsSource = new List<SortBy> { SortBy.Date, SortBy.Salary};
            sortTypePicker.SelectedIndex = 0;
            sortTypePicker.SelectedIndexChanged += SortTypePicker_SelectedIndexChanged;
            this.database = database;
            ICommand RefreshCommand = new Command(() =>
            {
                LoadData();
                refreshView.IsRefreshing = false;
            });
            refreshView.Command = RefreshCommand;
            //refreshView.IsRefreshing = true;
            //database.CreateCompanyAsync(new Company("Vistaaa", "Najlepsza firma zajmująca się wspaniałym systemem Windows Vista!", "Zielona", "5", "Limanowa", "34-600"));
            //database.CreateAdvertisementAsync(new Advertisement("Programowanie w Scratchu", 1, "Programista", "Junior Developer", "Umowa o pracę", "Pełen etat", "Praca zdalna", 10000.07m, 20000.78m, "aaa", DateTime.Now, DateTime.Now, "aaa", "aaa", "aaa"));
            //database.CreateUserAsync(new User("Mateusz", "Marmuźniak", new DateTime(2005, 2, 7), "mateusz.marmuzniak.poland@gmail.com", PasswordHasher.Hash("zaq1@WSX"), "123456789", "Limanowa", "Polska", "34-600", "Limanowa", "Zielona", "5", true));
        }

        private async void LoadData()
        {
            loading.IsRunning = true;
            loading.IsVisible = true;
            string searchBarText = string.Empty;
            if (searchBar.Text != null)
                searchBarText = searchBar.Text;
            int advertisementCount = (await database.GetAdvertisementsAsync(searchBarText.Trim(), (SortBy)sortTypePicker.SelectedItem)).Count;
            if (ADVERTISEMENTS_PER_PAGE * currentPage == advertisementCount + ADVERTISEMENTS_PER_PAGE)
                currentPage--;
            AdvertisementList = await database.GetAdvertisementsAsync(currentPage, ADVERTISEMENTS_PER_PAGE, searchBarText.Trim(), (SortBy)sortTypePicker.SelectedItem);
            AdvertisementCollectionView.ItemsSource = AdvertisementList;
            if(AdvertisementList.Count > 0)
            {                
                headerCollectionViewLabel.Text = "Znalezione oferty (" + AdvertisementList.Count + ")";
                headerCollectionViewLabel.IsVisible = true;
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
                nextPageBtn.IsEnabled = false;
            else
                nextPageBtn.IsEnabled = true;
            if(!emptyCollectionViewPlaceholder.IsVisible)
                emptyCollectionViewPlaceholder.IsVisible = true;
            loading.IsVisible = false;
            loading.IsRunning = false;           
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var advertisement = button?.BindingContext as Advertisement;
            await Navigation.PushAsync(new AdvertisementPage(advertisement));
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 1;
            LoadData();
        }
        private void PreviousPageButton_Clicked(object sender, EventArgs e)
        {
            if(currentPage > 1)
            {
                currentPage--;
                AdvertisementScrollView.ScrollToAsync(0, 0, true);
                LoadData();
            }
        }
        private void NextPageButton_Clicked(object sender, EventArgs e)
        {
            currentPage++;
            AdvertisementScrollView.ScrollToAsync(0, 0, true);
            LoadData();
        }

        private void FirstPageButton_Clicked(object sender, EventArgs e)
        {
            currentPage = 1;
            AdvertisementScrollView.ScrollToAsync(0, 0, true);
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
            //AdvertisementCollectionView.SelectionMode = SelectionMode.Multiple;
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
    }

}
