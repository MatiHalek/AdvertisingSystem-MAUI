using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void CancelLoginButton_Clicked(object sender, EventArgs e)
    {
        _ = loginModal.TranslateTo(0, 200, 300, Easing.SinInOut);
        await loginModal.FadeTo(0, 300, Easing.SinInOut);
        await Navigation.PopModalAsync();
    }

    private async void LoginPage_Appearing(object sender, EventArgs e)
    {
        _ = loginModal.TranslateTo(0, 0, 500, Easing.SinInOut);
        await loginModal.FadeTo(1, 500, Easing.SinInOut);
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        Database database = new();
        User? user = await database.VerifyUserAsync(emailEntry.Text, passwordEntry.Text);
        if (user is not null)
        {
            Preferences.Set("userId", user.Id.ToString());
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password!", "OK");
        }
    }
}