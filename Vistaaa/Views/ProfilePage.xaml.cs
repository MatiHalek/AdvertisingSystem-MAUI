namespace Vistaaa.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		DisplayAlert("Welcome", Preferences.ContainsKey("userId").ToString(), "OK");
		Preferences.Set("userId", "aaa");
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Navigation.PushModalAsync(new LoginPage());
    }
}