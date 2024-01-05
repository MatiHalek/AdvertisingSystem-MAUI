using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistaaa.Classes;

namespace Vistaaa.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        [ObservableProperty]
        ValidatableObject<string> email = new();
        [ObservableProperty]
        ValidatableObject<string> password = new();
        [ObservableProperty]
        ValidatableObject<DateTime> date = new();
        [ObservableProperty]
        ValidatableObject<string> companyName = new();
        [ObservableProperty]
        ValidatableObject<string> companyEmail = new();
        [ObservableProperty]
        ValidatableObject<string> companyPassword = new();
        [ObservableProperty]
        ValidatableObject<string> companyStreetName = new();
        [ObservableProperty]
        ValidatableObject<string> companyStreetNumber = new();
        [ObservableProperty]
        ValidatableObject<string> companyPostalCode = new();
        [ObservableProperty]
        ValidatableObject<string> companyCity = new();

        public RegistrationViewModel()
        {
            Date.Value = DateTime.Now;
            Email.Validations.Add(new EmailRule<string>());
            Email.Validations.Add(new EmailExistsRule<string>());
            Password.Validations.Add(new PasswordRule<string>());
            Date.Validations.Add(new DateRule<DateTime>());
            companyName.Validations.Add(new IsNotNullOrEmptyRule<string>());
            companyName.Validations.Add(new CompanyNameExistsRule<string>());
            CompanyEmail.Validations.Add(new EmailRule<string>());
            CompanyEmail.Validations.Add(new EmailExistsRule<string>());
            CompanyPassword.Validations.Add(new PasswordRule<string>());
            CompanyStreetName.Validations.Add(new IsNotNullOrEmptyRule<string>());
            CompanyStreetNumber.Validations.Add(new IsNotNullOrEmptyRule<string>());
            CompanyPostalCode.Validations.Add(new PostalCodeRule<string>());
            CompanyCity.Validations.Add(new IsNotNullOrEmptyRule<string>());
        }
        [RelayCommand]
        void ValidateEmail()
        {
            Email.Validate();
        }
        [RelayCommand]
        void ValidatePassword()
        {
            Password.Validate();
        }
        [RelayCommand]
        void ValidateDate()
        {
            Date.Validate();
        }
        [RelayCommand]
        void ValidateCompanyName()
        {
            CompanyName.Validate();
        }
        [RelayCommand]
        void ValidateCompanyEmail()
        {
            CompanyEmail.Validate();
        }
        [RelayCommand]
        void ValidateCompanyPassword()
        {
            CompanyPassword.Validate();
        }
        [RelayCommand]
        void ValidateCompanyStreetName()
        {
            CompanyStreetName.Validate();
        }
        [RelayCommand]
        void ValidateCompanyStreetNumber()
        {
            CompanyStreetNumber.Validate();
        }
        [RelayCommand]
        void ValidateCompanyPostalCode()
        {
            CompanyPostalCode.Validate();
        }
        [RelayCommand]
        void ValidateCompanyCity()
        {
            CompanyCity.Validate();
        }
    }
}
