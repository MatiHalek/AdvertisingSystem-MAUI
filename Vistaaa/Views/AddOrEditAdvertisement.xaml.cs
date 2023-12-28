using CommunityToolkit.Maui.Storage;
using System.Reflection;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class AddOrEditAdvertisement : ContentPage
{
	private readonly Database Database = new();
    public Advertisement? Advertisement{ get; set; }

    public AddOrEditAdvertisement()
	{
		InitializeComponent();
	}
    public AddOrEditAdvertisement(Advertisement advertisement)
    {
        InitializeComponent();
        addOrEditAdvertisementContentPage.Title = "Edytuj og³oszenie";
        submitButton.Text = "ZatwierdŸ zmiany";
        Advertisement = advertisement;
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
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Vistaaa/1"));
        using var fileStream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Vistaaa/1/image.png"), FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
        image.Source = ImageSource.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Vistaaa/1/image.png"));
        //await FileSaver.Default.SaveAsync("test.png", stream);
        //await DisplayAlert("test", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "OK");

    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        await Database.CreateAdvertisementAsync(new Advertisement(
            titleEntry.Text,
            1,
            1,
            positionNameEntry.Text,
            positionLevelEntry.Text,
            contractTypeEntry.Text,
            employmentTypeEntry.Text,
            workTypeEntry.Text,
            null,
            2,
            workDaysEntry.Text,
            DateTime.Now,
            expirationDatePicker.Date,
            responsibilitiesEntry.Text,
            requirementsEntry.Text,
            offerEntry.Text
            ));
        await Navigation.PopAsync();
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void AddNewCategoryButton_Clicked(object sender, EventArgs e)
    {
        DisplayPromptAsync("Dodaj now¹ kategoriê", "WprowadŸ nazwê nowej kategorii", "Dodaj", "Anuluj", "Nazwa kategorii", 100, Keyboard.Text, "Nowa kategoria").ContinueWith((task) =>
        {
            if (task.Result is not null)
            {
                //Database.CreateCategoryAsync(new Category(task.Result));
            }
        }); 
    }
}