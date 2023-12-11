namespace Vistaaa.Views;

public partial class AddOrEditAdvertisement : ContentPage
{
	public AddOrEditAdvertisement()
	{
		InitializeComponent();
	}

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync();
    }
}