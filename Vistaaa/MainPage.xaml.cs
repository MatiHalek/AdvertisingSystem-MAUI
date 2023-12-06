using Vistaaa.Models;
using Vistaaa.Views;

namespace Vistaaa
{
    public partial class MainPage : ContentPage
    {
        readonly Database database;
        const uint ADVERTISEMENTS_PER_PAGE = 5;
        uint currentPage = 1;
        public List<Advertisement>? AdvertisementList { get; set; }
        public MainPage(Database database)
        {
            InitializeComponent();
            this.database = database; 
            LoadData();
            //database.CreateCompanyAsync(new Company("Vistaaa", "Najlepsza firma zajmująca się wspaniałym systemem Windows Vista!", "Zielona", "5", "Limanowa", "34-600"));
            //database.CreateAdvertisementAsync(new Advertisement("Programowanie w Scratchu", 1, "Programista", "Junior Developer", "Umowa o pracę", "Pełen etat", "Praca zdalna", 10000.07m, 20000.78m, "aaa", DateTime.Now, DateTime.Now, "aaa", "aaa", "aaa"));         
        }
        private async void LoadData()
        {
            loading.IsRunning = true;
            string searchBarText = string.Empty;
            if (searchBar.Text != null)
                searchBarText = searchBar.Text;
            int advertisementCount = (await database.GetAdvertisementsAsync(searchBarText)).Count;
            if (ADVERTISEMENTS_PER_PAGE * currentPage == advertisementCount + ADVERTISEMENTS_PER_PAGE)
                currentPage--;
            AdvertisementList = await database.GetAdvertisementsAsync(currentPage, ADVERTISEMENTS_PER_PAGE, searchBarText);
            AdvertisementCollectionView.ItemsSource = AdvertisementList;
            if (currentPage < 2)
                previousPageBtn.IsEnabled = false;
            else
                previousPageBtn.IsEnabled = true;
            if (currentPage == Math.Ceiling(advertisementCount / (float)ADVERTISEMENTS_PER_PAGE))
                nextPageBtn.IsEnabled = false;
            else
                nextPageBtn.IsEnabled = true;
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
    }

}
