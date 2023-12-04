using Vistaaa.Classes;
using Vistaaa.Views;

namespace Vistaaa
{
    public partial class MainPage : ContentPage
    {
        readonly Database database;
        const uint ADVERTISEMENTS_PER_PAGE = 50;
        public List<Advertisement>? Advertisements { get; set; }
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
            Advertisements = await database.GetAdvertisementsAsync(searchBar.Text);          
            collectionView.ItemsSource = Advertisements;
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
            LoadData();
        }

        private void PreviousPage_Clicked(object sender, EventArgs e)
        {

        }

        private void NextPage_Clicked(object sender, EventArgs e)
        {

        }
    }

}
