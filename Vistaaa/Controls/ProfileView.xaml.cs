using System.Reflection;
using Vistaaa.Models;
using Vistaaa.Views;

namespace Vistaaa.Controls;

public partial class ProfileView : ContentView
{
    private readonly Database Database = new();
    User? User { get; set; } = null;
	Company? Company { get; set; } = null;
	public ProfileView(User user)
	{
		InitializeComponent();
		User = user;
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/User/{user.Id}"));
        LoadUserData();
        LoadProfileImage();
        LoadUserListView();
	}
    public ProfileView(Company company)
    {
        InitializeComponent();
        editUserDataButton.IsVisible = false;
        Company = company;
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/Company/{company.Id}"));
        nameAndSurnameLabel.Text = Company.Name;
        emailLabel.Text = Company.Email;
        LoadProfileImage();
    }
    private async void LoadUserListView()
    {
        List<AdvertisementApplying> advertisements = await Database.GetApplyingAdvertisements();
        advertisements = advertisements.Where(x => x.UserId == User?.Id).ToList();
        ListView.ItemsSource = advertisements;
    }
    private void LoadUserData()
    {
        if (!string.IsNullOrWhiteSpace(User?.FirstName))
        {
            nameAndSurnameLabel.Text = User.FirstName + " " + (User.LastName ?? "");
            emailLabel.Text = User.Email;
            emailLabel.IsVisible = true;
        }
        else
        {
            nameAndSurnameLabel.Text = User?.Email;
            emailLabel.IsVisible = false;
        }
    }
    private void LoadProfileImage()
    {
        if(User is not null)
        {
            if (Directory.EnumerateFileSystemEntries(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/User/{User.Id}")).Any())
            {
                FileStream fs = new(Directory.EnumerateFileSystemEntries(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/User/{User.Id}")).First(), FileMode.Open, FileAccess.Read);
                profileImage.Source = ImageSource.FromStream(() => fs);
            }
        }
        else
        {
            if (Directory.EnumerateFileSystemEntries(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/Company/{Company?.Id}")).Any())
            {
                FileStream fs = new(Directory.EnumerateFileSystemEntries(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/Company/{Company?.Id}")).First(), FileMode.Open, FileAccess.Read);
                profileImage.Source = ImageSource.FromStream(() => fs);
            }
        }
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
        profileImage.Source = null;
        if(User is not null)
        {
            using var fileStream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/User/{User.Id}/image.png"), FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            stream.Close();
            fileStream.Close();
        }
        else
        {
            using var fileStream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Vistaaa/Company/{Company?.Id}/image.png"), FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            stream.Close();
            fileStream.Close();
        }      
        LoadProfileImage();
    }

    private async void EditUserDataButton_Clicked(object sender, EventArgs e)
    {
        string result = await Application.Current!.MainPage!.DisplayPromptAsync("Edytuj dane", "Imiê", "OK", "Anuluj", "Imiê", 50, Keyboard.Default, User?.FirstName);
        if(result is not null)
        {
            User!.FirstName = result;
            result = await Application.Current!.MainPage!.DisplayPromptAsync("Edytuj dane", "Nazwisko", "OK", "Anuluj", "Nazwisko", 100, Keyboard.Default, User?.LastName);
            if(result is not null)
            {
                User!.LastName = result;
                await Database.UpdateUser(User);
                LoadUserData();
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        AdvertisementApplying advertisementApplying = (AdvertisementApplying)button.CommandParameter;
        //Navigation.PushAsync(new AdvertisementPage(advertisementApplying.AdvertisementId)); 
    }
}