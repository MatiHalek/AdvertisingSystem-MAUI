using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Windows.Input;
using Vistaaa.Classes;
using Vistaaa.Models;
using Vistaaa.ViewModel;

namespace Vistaaa.Views;

public partial class RegistrationPage : ContentPage
{
    private readonly Database Database = new();
    private VerticalStackLayout? IndividualUserStackLayout;
    private VerticalStackLayout? CompanyStackLayout;

    private int? verificationCode = null;
    public bool IsCompany { get; set; }

    public RegistrationPage(bool IsCompany = false)
	{
		InitializeComponent();
        this.IsCompany = IsCompany;
        BindingContext = new RegistrationViewModel();
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
        if(IsCompany)
            carouselView.ScrollTo(1, -1, ScrollToPosition.End);
    }

    private async void RegistrationButton_Clicked(object sender, EventArgs e)
    {
        if(!registrationCheckbox.IsChecked)
        {
            await DisplayAlert("B³¹d rejestracji", "Proszê zaakceptowaæ regulamin!", "OK");
            return;
        }
        if(carouselView.CurrentItem.ToString() == "IndividualUser")
        {
            if (!((RegistrationViewModel)BindingContext).Email.Validate() || !((RegistrationViewModel)BindingContext).Password.Validate() || !((RegistrationViewModel)BindingContext).Date.Validate() || !CheckPasswordForIndividualUser())
                return;
            string email = ((Entry)IndividualUserStackLayout!.Children[2]).Text;
            verificationCode = new Random().Next(1000, 10000);
            MailMessage mail = new("vistaaa_advertising@outlook.com", email, "System og³oszeniowy Vistaaa - powtwierdŸ rejestracjê", $"Oto kod s³u¿¹cy do dokoñczenia rejestracji w systemie og³oszeniowym Vistaaa: {verificationCode}.");
            SmtpClient client = new("smtp-mail.outlook.com", 587)
            {
                Credentials = new NetworkCredential("vistaaa_advertising@outlook.com", "dontgiveyoupassword"),
                EnableSsl = true,
                UseDefaultCredentials = false
            };
            client.Send(mail);
            string result = await DisplayPromptAsync("Dokoñcz rejestracjê", $"WprowadŸ kod weryfikacyjny, który wys³aliœmy na adres e-mail: {email}.\n\nJe¿eli kod nie doszed³, spróbuj ponownie, poprzez ponowne klikniêcie przycisku rejestracji.", "OK", "Anuluj", "Kod weryfikacyjny", 4, Keyboard.Numeric, "");
            if (result is null)
                return;
            if (int.Parse(result) != verificationCode)
            {
                await DisplayAlert("B³¹d rejestracji", "Wprowadzony kod jest niepoprawny!", "OK");
                return;
            }
            uint insertedId = await Database.CreateUser(new User(        
                null, null,
                ((DatePicker)IndividualUserStackLayout!.Children[11]).Date,
                email,
                PasswordHasher.Hash(((Entry)IndividualUserStackLayout!.Children[5]).Text),
                null, null, null, null, null, null, null, false
            ));
            Preferences.Set("userId", insertedId.ToString());
            Preferences.Set("userType", "IndividualUser");
            await Navigation.PopModalAsync();
        }         
        else
        {
            if (!((RegistrationViewModel)BindingContext).CompanyEmail.Validate() || !((RegistrationViewModel)BindingContext).CompanyName.Validate() || !((RegistrationViewModel)BindingContext).CompanyPassword.Validate() || !((RegistrationViewModel)BindingContext).CompanyPostalCode.Validate() || !((RegistrationViewModel)BindingContext).CompanyStreetName.Validate() || !((RegistrationViewModel)BindingContext).CompanyStreetNumber.Validate() || !((RegistrationViewModel)BindingContext).CompanyCity.Validate() || !CheckPasswordForCompany())
                return;
            string email = ((Entry)CompanyStackLayout!.Children[2]).Text;
            verificationCode = new Random().Next(1000, 10000);
            MailMessage mail = new("vistaaa_advertising@outlook.com", email, "System og³oszeniowy Vistaaa - powtwierdŸ rejestracjê", $"Oto kod s³u¿¹cy do dokoñczenia rejestracji w systemie og³oszeniowym Vistaaa: {verificationCode}.");
            SmtpClient client = new("smtp-mail.outlook.com", 587)
            {
                Credentials = new NetworkCredential("vistaaa_advertising@outlook.com", "dontgiveyoupassword"),
                EnableSsl = true,
                UseDefaultCredentials = false
            };
            client.Send(mail);
            string result = await DisplayPromptAsync("Dokoñcz rejestracjê", $"WprowadŸ kod weryfikacyjny, który wys³aliœmy na adres e-mail: {email}.\n\nJe¿eli kod nie doszed³, spróbuj ponownie, poprzez ponowne klikniêcie przycisku rejestracji.", "OK", "Anuluj", "Kod weryfikacyjny", 4, Keyboard.Numeric, "");
            if (result is null)
                return;
            if (int.Parse(result) != verificationCode)
            {
                await DisplayAlert("B³¹d rejestracji", "Wprowadzony kod jest niepoprawny!", "OK");
                return;
            }
            Grid streetGrid = (Grid)CompanyStackLayout!.Children[13];
            VerticalStackLayout streetNameStackLayout = (VerticalStackLayout)streetGrid.Children[0];
            VerticalStackLayout streetNumberStackLayout = (VerticalStackLayout)streetGrid.Children[1];
            Grid cityGrid = (Grid)CompanyStackLayout!.Children[14];
            VerticalStackLayout postalCodeStackLayout = (VerticalStackLayout)cityGrid.Children[0];
            VerticalStackLayout cityStackLayout = (VerticalStackLayout)cityGrid.Children[1];
            uint insertedId = await Database.CreateCompanyAsync(new Company(
                ((Entry)CompanyStackLayout!.Children[2]).Text,
                PasswordHasher.Hash(((Entry)CompanyStackLayout!.Children[8]).Text),
                ((Entry)CompanyStackLayout!.Children[5]).Text,
                null,
                ((Entry)streetNameStackLayout!.Children[1]).Text,
                ((Entry)streetNumberStackLayout!.Children[1]).Text,
                ((Entry)postalCodeStackLayout!.Children[1]).Text,
                ((Entry)cityStackLayout!.Children[1]).Text
            ));
            Preferences.Set("userId", insertedId.ToString());
            Preferences.Set("userType", "Company");
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

    private void RegistrationCheckboxLabel_Tapped(object sender, TappedEventArgs e)
    {
        registrationCheckbox.IsChecked = !registrationCheckbox.IsChecked;
    }

    private void RepeatPasswordEntry_TextChanged(object sender, TextChangedEventArgs e) => CheckPasswordForIndividualUser();
    private void RepeatCompanyPasswordEntry_TextChanged(object sender, TextChangedEventArgs e) => CheckPasswordForCompany();
    private bool CheckPasswordForIndividualUser()
    {
        if (!((Entry)IndividualUserStackLayout!.Children[5]).Text.Equals(((Entry)IndividualUserStackLayout!.Children[8]).Text))
        {
            ((Entry)IndividualUserStackLayout!.Children[8]).BackgroundColor = Color.FromArgb("#77FF0000");
            ((Label)IndividualUserStackLayout!.Children[9]).IsVisible = true;
            return false;
        }
        ((Entry)IndividualUserStackLayout!.Children[8]).BackgroundColor = Color.FromArgb("#DDD");
        ((Label)IndividualUserStackLayout!.Children[9]).IsVisible = false;
        return true;
    }
    private bool CheckPasswordForCompany()
    {
        if (!((Entry)CompanyStackLayout!.Children[8]).Text.Equals(((Entry)CompanyStackLayout!.Children[11]).Text))
        {
            ((Entry)CompanyStackLayout!.Children[11]).BackgroundColor = Color.FromArgb("#77FF0000");
            ((Label)CompanyStackLayout!.Children[12]).IsVisible = true;
            return false;
        }
        ((Entry)CompanyStackLayout!.Children[11]).BackgroundColor = Color.FromArgb("#DDD");
        ((Label)CompanyStackLayout!.Children[12]).IsVisible = false;
        return true;
    }

    
}