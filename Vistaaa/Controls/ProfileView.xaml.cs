using System.Reflection;
using Vistaaa.Models;

namespace Vistaaa.Controls;

public partial class ProfileView : ContentView
{
	User User { get; set; }
	public ProfileView(User user)
	{
		InitializeComponent();
		User = user;
		if(User.FirstName is not null)
			nameAndSurnameLabel.Text = User.FirstName + " " + User.LastName;
		else
			nameAndSurnameLabel.Text = User.Email;
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
        profileImage.Source = ImageSource.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Vistaaa/1/image.png"));
        //await FileSaver.Default.SaveAsync("test.png", stream);
        //await DisplayAlert("test", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "OK");

    }
}