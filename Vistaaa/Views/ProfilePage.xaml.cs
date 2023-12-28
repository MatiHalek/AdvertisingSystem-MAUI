using CommunityToolkit.Maui.Views;
using Vistaaa.Classes;
using Vistaaa.Controls;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class ProfilePage : ContentPage
{
    private ProfileView? ProfileView = null;
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
            if (ProfileView is not null)
                profilePage.Remove(ProfileView);
            form.IsVisible = false;
            Database database = new();
            ProfileView = new ProfileView(await database.GetUserAsync(uint.Parse(Preferences.Get("userId", null) ?? "")));
            ProfileView.logoutButton.Clicked += (object? sender, EventArgs e) =>
            {
                Preferences.Set("userId", null);
                profilePage.Remove(ProfileView);
                form.IsVisible = true;
            };  
            profilePage.Add(ProfileView);
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