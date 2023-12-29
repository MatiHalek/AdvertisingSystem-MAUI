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

        public RegistrationViewModel()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "A username is required."
            });
        }
        [RelayCommand]
        void ValidateEmail()
        {
            Email.Validate();
            Debug.WriteLine("test" + Email.Value);
        }
    }
}
