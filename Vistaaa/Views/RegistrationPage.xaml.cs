using System.Windows.Input;
using Vistaaa.Classes;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class RegistrationPage : ContentPage
{
    private readonly Database Database = new();

    public ValidatableObject<string>? Email { get; private set; }
    public ICommand ValidateEmailCommand => new Command(ValidateEmail);
    private VerticalStackLayout? IndividualUserStackLayout;
    private VerticalStackLayout? CompanyStackLayout;
    //public ValidatableObject<string> Password { get; private set; }
    /*private void AddValidations()
    {
        UserName.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = "A username is required."
        });

        /*Password.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = "A password is required."
        });
    }*/
    public RegistrationPage()
	{
		InitializeComponent();
        picker.ItemsSource = ["Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6"];
        picker.SelectedIndices = [2, 4, 5];
        /*ValidatableObject<string> UserName = new()
        {
            Value = "ggg ",
        };
        UserName.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = "A username is required."
        });
        UserName.Validate();
        DisplayAlert("Test", UserName.IsValid.ToString(), "OK");*/

        //AddValidations();UserName.Value = " ";
    }

    //public ICommand ValidateUsername { get; }

    private void ValidateEmail()
    {
        //DisplayAlert("Test", UserName.IsValid.ToString(), "OK");
        DisplayAlert("Test", Email?.Value, "OK");
        Email?.Validate();    
    }

    private async void CancelRegistrationButton_Clicked(object sender, EventArgs e)
    {
        _= registrationModal.TranslateTo(0, 200, 300, Easing.SinInOut);
        await registrationModal.FadeTo(0, 300, Easing.SinInOut);
        await Navigation.PopModalAsync();
    }

    private async void RegistrationPage_Appearing(object sender, EventArgs e)
    {
        _= registrationModal.TranslateTo(0, 0, 500, Easing.SinInOut);
        await registrationModal.FadeTo(1, 500, Easing.SinInOut);
    }

    private async void RegistrationButton_Clicked(object sender, EventArgs e)
    {
        //VerticalStackLayout? stackLayout = (VerticalStackLayout?)((Button)sender).Parent;
        //carouselView.CurrentItem = 1;
        //await DisplayAlert("Test", ((Entry?)stackLayout?.Children[1])?.Text, "OK");    
        //await DisplayAlert("Test", ((Entry)st1.Children[1]).Text, "OK");
        if(carouselView.CurrentItem.ToString() == "IndividualUser")
        {
            uint insertedId = await Database.CreateUser(new User(        
                null, null,
                ((DatePicker)IndividualUserStackLayout!.Children[8]).Date,
                ((Entry)IndividualUserStackLayout!.Children[2]).Text,
                PasswordHasher.Hash(((Entry)IndividualUserStackLayout!.Children[4]).Text),
                null, null, null, null, null, null, null, false
            ));
            Preferences.Set("userId", insertedId.ToString());
            await Navigation.PopModalAsync();
        }       
    }

    private void VerticalStackLayout_Loaded(object sender, EventArgs e)
    {
        IndividualUserStackLayout = (VerticalStackLayout)sender;
    }

    private void VerticalStackLayout_Loaded_1(object sender, EventArgs e)
    {
        CompanyStackLayout = (VerticalStackLayout)sender;
    }
}