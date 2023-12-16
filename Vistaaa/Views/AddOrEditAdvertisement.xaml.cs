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

    private async void FilePickerButton_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Wybierz zdjêcie",
            FileTypes = FilePickerFileType.Images
        });
        if (result is null)
            return;
        var stream = await result.OpenReadAsync();
           
    }
}