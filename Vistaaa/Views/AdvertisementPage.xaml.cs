using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class AdvertisementPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();
		CheckIfSaved();		
    }
	private readonly Database Database = new();
    private Advertisement? Advertisement { get; set;}
    public AdvertisementPage(Advertisement? advertisement)
	{
		InitializeComponent();
		Advertisement = advertisement;
        advertisementTitle.Text = Advertisement?.Title;
		advertisementId.Text = " (#" + Advertisement?.Id.ToString().PadLeft(7, '0') + ")";
		advertisementCategory.Text = Advertisement?.CategoryName;
		advertisementDateAdded.Text = "Dodano " + Advertisement?.CreationDate.ToString("d MMM yyyy H:mm");
		advertisementDateExpire.Text = "Wa¿ne do " + Advertisement?.ExpirationDate.ToString("d MMM yyyy H:mm");
		advertisementEarnings.Text = (Advertisement?.LowestSalary is not null ? Advertisement?.LowestSalary?.ToString("N2") + " z³ - " : "") + Advertisement?.HighestSalary.ToString("N2") + " z³ / mies.";
		map.Pins.Add(new Pin()
		{
			Location = new Location(49.699936, 20.417650),
			Label = Advertisement?.CompanyName ?? "",
			Address = "ul. Zielona 1, 00-000 Warszawa"
		});
		map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(49.699936, 20.417650), Distance.FromKilometers(10)));
		
	}
	private async void CheckIfSaved()
	{
		if(Preferences.ContainsKey("userId") && await Database.CheckIfUserAdvertisementExists(uint.Parse(Preferences.Get("userId", null) ?? ""), Advertisement?.Id ?? 0) is not null)
		{
            saveButton.Text = "Usuñ z zapisanych";
            saveButton.TextColor = Colors.Red;
            saveButton.BorderColor = Colors.Red;

        }
        else
		{
            saveButton.Text = "Zapisz";
            saveButton.TextColor = Colors.DodgerBlue;
            saveButton.BorderColor = Colors.DodgerBlue;
        }
		saveButton.IsEnabled = true;
	}

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if(Preferences.ContainsKey("userId"))
		{
            UserAdvertisement? userAdvertisement = await Database.CheckIfUserAdvertisementExists(uint.Parse(Preferences.Get("userId", null) ?? ""), Advertisement?.Id ?? 0);
			if(userAdvertisement is not null)
				await Database.DeleteUserAdvertisementAsync(userAdvertisement);
            else
				await Database.CreateUserAdvertisementAsync(new UserAdvertisement(uint.Parse(Preferences.Get("userId", null) ?? ""), Advertisement?.Id ?? 0));
			CheckIfSaved();
		}
		else
			_= Shell.Current.GoToAsync("//profile");
    }

}