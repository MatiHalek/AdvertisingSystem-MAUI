using System.Windows.Input;
using Vistaaa.Classes;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class RegistrationPage : ContentPage
{
    private readonly Database Database = new();
    //public ValidatableObject<string> UserName { get; private set; } = new();
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
        //AddValidations();UserName.Value = " ";
        //ValidateUsername = new Command(ValidateUsername2);
    }

    //public ICommand ValidateUsername { get; }

    private void ValidateUsername2()
    {
        //DisplayAlert("Test", UserName.IsValid.ToString(), "OK");    
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
        uint insertedId = await Database.CreateUser(new User(        
            null, null,
            birthDatePicker.Date,
            emailEntry.Text,
            PasswordHasher.Hash(passwordEntry.Text),
            null, null, null, null, null, null, null, false
        ));
        Preferences.Set("userId", insertedId.ToString());
        await Navigation.PopModalAsync();
    }
}