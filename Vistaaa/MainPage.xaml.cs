using Vistaaa.Classes;
using Vistaaa.Views;

namespace Vistaaa
{
    public partial class MainPage : ContentPage
    {
        readonly Database database;
        public List<Advertisement>? Advertisements { get; set; }

        public MainPage(Database database)
        {
            InitializeComponent();
            this.database = database;
            LoadData();
        }
        private async void LoadData()
        {
            Advertisements = await database.GetAdvertisementsAsync();
            collectionView.ItemsSource = Advertisements;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var advertisement = button?.BindingContext as Advertisement;
            await Navigation.PushAsync(new AdvertisementPage(advertisement));
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///dogs");
        }
    }

}
