using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class AdvertisementPage : ContentPage
{
    private Advertisement? Advertisement { get; set;}
    public AdvertisementPage(Advertisement? advertisement)
	{
		InitializeComponent();
		Advertisement = advertisement;
        advertisementTitle.Text = Advertisement?.Title;
		advertisementId.Text = " (#" + Advertisement?.Id.ToString().PadLeft(7, '0') + ")";
		advertisementCategory.Text = Advertisement?.CategoryName;
		advertisementDateAdded.Text = "Dodano " + Advertisement?.CreationDate.ToString("d MMM yyyy H:m");
		advertisementDateExpire.Text = "Wa¿ne do " + Advertisement?.ExpirationDate.ToString("d MMM yyyy H:m");
		advertisementEarnings.Text = (Advertisement?.LowestSalary is not null ? Advertisement?.LowestSalary?.ToString("N2") + " z³ - " : "") + Advertisement?.HighestSalary.ToString("N2") + " z³ / mies.";
		map.Pins.Add(new Pin()
		{
			Location = new Location(50, 6),
			Label = Advertisement?.CompanyName ?? "",
			Address = "ul. Zielona 1, 00-000 Warszawa"
		});
		map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(50, 6), Distance.FromKilometers(10)));
	}
}