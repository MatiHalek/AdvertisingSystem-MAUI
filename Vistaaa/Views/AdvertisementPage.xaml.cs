using Vistaaa.Classes;

namespace Vistaaa.Views;

public partial class AdvertisementPage : ContentPage
{
    private Advertisement? Advertisement { get; set;}
    public AdvertisementPage(Advertisement? advertisement)
	{
		InitializeComponent();
		Advertisement = advertisement;
        advertisementTitle.Text = Advertisement?.Title;
	}
}