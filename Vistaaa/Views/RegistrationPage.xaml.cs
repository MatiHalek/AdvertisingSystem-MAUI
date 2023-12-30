using CommunityToolkit.Mvvm.ComponentModel;
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