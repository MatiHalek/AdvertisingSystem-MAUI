using CommunityToolkit.Maui.Views;
using Vistaaa.Classes;
using Vistaaa.Controls;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		//DisplayAlert("Welcome", Preferences.Get("userId", "").ToString(), "OK");
		//Preferences.Set("userId", null);
		//DisplayAlert("Test", PasswordHasher.Verify("Test", PasswordHasher.Hash("test")).ToString(), "OK");
		UpdateView();
	}
	private async void UpdateView()
	{
        if (Preferences.ContainsKey("userId"))
        {
            profilePage.Clear();
            Database database = new();
            profilePage.Add(new ProfileView(await database.GetUserAsync(uint.Parse(Preferences.Get("userId", null) ?? ""))));
        }
    }

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LoginPage());
		UpdateView();
    }

    private void RegistrationButton_Clicked(object sender, EventArgs e)
    {
		Navigation.PushModalAsync(new RegistrationPage());
    }
}