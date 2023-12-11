using CommunityToolkit.Maui.Views;
using Vistaaa.Classes;
using Vistaaa.Controls;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class ProfilePage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateView();
    }

    public ProfilePage()
	{
		InitializeComponent();
	}

	private async void UpdateView()
	{
        if (Preferences.ContainsKey("userId"))
        {
            form.IsVisible = false;
            Database database = new();
            var profileView = new ProfileView(await database.GetUserAsync(uint.Parse(Preferences.Get("userId", null) ?? "")));
            profileView.logoutButton.Clicked += (object? sender, EventArgs e) =>
            {
                Preferences.Set("userId", null);
                profilePage.Remove(profileView);
                form.IsVisible = true;
            };  
            profilePage.Add(profileView);
        }
    }

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LoginPage());
    }

    private void RegistrationButton_Clicked(object sender, EventArgs e)
    {
		Navigation.PushModalAsync(new RegistrationPage());
    }
}