using CommunityToolkit.Maui.Views;
using Vistaaa.Classes;

namespace Vistaaa.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		DisplayAlert("Welcome", Preferences.ContainsKey("userId").ToString(), "OK");
		Preferences.Set("userId", null);
		DisplayAlert("Test", PasswordHasher.Verify("Test", PasswordHasher.Hash("test")).ToString(), "OK");
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		var loginPopup = new LoginPage
		{
			CanBeDismissedByTappingOutsideOfPopup = false
		};
		this.ShowPopup(loginPopup);
    }
}