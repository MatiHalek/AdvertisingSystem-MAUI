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
		GetAdvertisementData();
	}
	private async void GetAdvertisementData()
	{
        advertisementTitle.Text = Advertisement?.Title;
        advertisementId.Text = " (#" + Advertisement?.Id.ToString().PadLeft(7, '0') + ")";
        advertisementCategory.Text = Advertisement?.CategoryName;
        advertisementDateAdded.Text = "Dodano " + Advertisement?.CreationDate.ToString("d MMM yyyy H:mm");
        advertisementDateExpire.Text = "Wa¿ne do " + Advertisement?.ExpirationDate.ToString("d MMM yyyy H:mm");
        advertisementEarnings.Text = (Advertisement?.LowestSalary is not null ? Advertisement?.LowestSalary?.ToString("N2") + " z³ - " : "") + Advertisement?.HighestSalary.ToString("N2") + " z³ / mies.";
		Company? company = await Database.GetCompany(Advertisement?.CompanyId ?? 0);
        map.Pins.Add(new Pin()
        {
            Location = new Location(49.699936, 20.417650),
            Label = Advertisement?.CompanyName ?? "",
            Address = $"ul. {company?.StreetName} {company?.StreetNumber}, {company?.PostalCode} {company?.City}"
        });
        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(49.699936, 20.417650), Distance.FromKilometers(10)));
        positionNameLabel.Text = Advertisement?.PositionName;
        positionLevelLabel.Text = Advertisement?.PositionLevel;
		contractTypeLabel.Text = await Database.GetContractType(Advertisement?.ContractType ?? 0);
		employmentTypeLabel.Text = await Database.GetEmploymentType(Advertisement?.EmploymentType ?? 0);
		workTypeLabel.Text = "Praca " + (await Database.GetWorkType(Advertisement?.WorkType ?? 0)).ToLower();
        workHoursLabel.Text = "Godziny pracy:\n" + Advertisement?.WorkDays;
        string[] separator = ["\r\n", "\r", "\n"];
        List<string>? responsibilities = Advertisement?.Responsibilities.Split(separator,
    StringSplitOptions.None).ToList();
		if(responsibilities is not null)
			for(int i = 0; i < responsibilities.Count; i++)
			{
				Grid grid = [];
				grid.ColumnSpacing = 8;
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
				Label iconLabel = new() { Text = "\uf058;", FontSize = 18, TextColor = Colors.LawnGreen, FontFamily = "FAS" };
				Grid.SetRow(iconLabel, i);
                grid.Children.Add(iconLabel);
				Label textLabel = new() { Text = responsibilities[i], FontSize = 16, TextColor = Colors.DarkBlue, FontFamily = "Franklin Gothic", FontAttributes = FontAttributes.Bold};
				Grid.SetColumn(textLabel, 1);
				Grid.SetRow(textLabel, i);
				grid.Children.Add(textLabel);
				responsibilitiesStackLayout.Children.Add(grid);
			}
		List<string>? requirements = Advertisement?.Requirements.Split(separator,
    StringSplitOptions.None).ToList();
		if(requirements is not null)
			for(int i = 0; i < requirements.Count; i++)
			{
				Grid grid = [];
				grid.ColumnSpacing = 8;
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
				Label iconLabel = new() { Text = "\uf058;", FontSize = 18, TextColor = Colors.LawnGreen, FontFamily = "FAS" };
				Grid.SetRow(iconLabel, i);
				grid.Children.Add(iconLabel);
				Label textLabel = new() { Text = requirements[i], FontSize = 16, TextColor = Colors.DarkBlue, FontFamily = "Franklin Gothic", FontAttributes = FontAttributes.Bold};
				Grid.SetColumn(textLabel, 1);
				Grid.SetRow(textLabel, i);
				grid.Children.Add(textLabel);
				requirementsStackLayout.Children.Add(grid);
			}
		List<string>? benefits = Advertisement?.Offer.Split(separator,
    StringSplitOptions.None).ToList();
		if(benefits is not null)
			for(int i = 0; i < benefits.Count; i++)
			{
				Grid grid = [];
				grid.ColumnSpacing = 8;
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
				Label iconLabel = new(){ Text = "\uf058;", FontSize = 18, TextColor = Colors.LawnGreen, FontFamily = "FAS" };
				Grid.SetRow(iconLabel, i);
				grid.Children.Add(iconLabel);
				Label label = new() { Text = benefits[i], FontSize = 16, TextColor = Colors.DarkBlue, FontFamily = "Franklin Gothic", FontAttributes = FontAttributes.Bold};
				Grid.SetColumn(label, 1);
				Grid.SetRow(label, i);
				grid.Children.Add(label);
				offerStackLayout.Children.Add(grid);
			}
    }
	private async void CheckIfSaved()
	{	if(Preferences.ContainsKey("userId") && Preferences.Get("userType", "") == "Company")
		{
			saveButton.IsEnabled = false;
			applyButton.IsEnabled = false;
			advertisementButtonsFlexLayout.IsVisible = false;
			expiredAdvertisementLabel.Text = "Aplikowanie i zapisywanie og³oszeñ nie jest mo¿liwe w przypadku kont firmowych.";
			expiredAdvertisementLabel.IsVisible = true;
			return;
		}
        if (Preferences.ContainsKey("userId") && await Database.CheckIfApplyingAdvertisementExists(uint.Parse(Preferences.Get("userId", null) ?? ""), Advertisement?.Id ?? 0) is not null)
        {
			applyButton.Text = "Aplikowano";
			applyButton.IsEnabled = false;
        }
        if (Advertisement?.ExpirationDate < DateTime.Now)
		{
            expiredAdvertisementLabel.IsVisible = true;
			applyButton.IsVisible = false;
        }
		else
		{
			expiredAdvertisementLabel.IsVisible = false;
			saveButton.IsVisible = true;
		}
		if(Preferences.ContainsKey("userId") && await Database.CheckIfUserAdvertisementExists(uint.Parse(Preferences.Get("userId", null) ?? ""), Advertisement?.Id ?? 0) is not null)
		{
            saveButton.Text = "Usuñ z zapisanych";
            saveButton.TextColor = Colors.Red;
            saveButton.BorderColor = Colors.Red;
        }
        else
		{
			if(Advertisement?.ExpirationDate < DateTime.Now)
			{
				saveButton.IsVisible = false;
			}
			else
			{
				saveButton.Text = "Zapisz";
				saveButton.TextColor = Colors.DodgerBlue;
				saveButton.BorderColor = Colors.DodgerBlue;
			}           
        }
		if(!applyButton.IsVisible && !saveButton.IsVisible)
			advertisementButtonsFlexLayout.IsVisible = false;
		else
			advertisementButtonsFlexLayout.IsVisible = true;
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

    private async void ApplyButton_Clicked(object sender, EventArgs e)
    {
        if (Preferences.ContainsKey("userId"))
        {
            User user = await Database.GetUserAsync(uint.Parse(Preferences.Get("userId", null) ?? ""));
			if(string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
				await DisplayAlert("Uzupe³nij dane", "Aby aplikowaæ na og³oszenia musisz uzupe³niæ swoje dane w zak³adce profilu.", "OK");
			else
			{
				bool result = await DisplayAlert("Aplikowanie", "Czy na pewno chcesz aplikowaæ na to og³oszenie?", "Tak", "Nie");
				if(result)
				{
                    await Database.CreateApplyingAdvertisement(new AdvertisementApplying(uint.Parse(Preferences.Get("userId", null) ?? ""), Advertisement?.Id ?? 0));
                    await DisplayAlert("Aplikowanie", "Aplikacja zosta³a wys³ana.", "OK");
					CheckIfSaved();
                }
			}
        }
        else
            _ = Shell.Current.GoToAsync("//profile");
    }
}