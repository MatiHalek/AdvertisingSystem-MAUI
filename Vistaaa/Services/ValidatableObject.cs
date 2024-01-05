using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Classes
{
    public class ValidatableObject<T> : ObservableObject
    {
        private IEnumerable<string> _errors;
        private string? _firstError;
        private bool _isValid;
        private T _value = default!;
        public List<IValidationRule<T>> Validations { get; } = [];
        public IEnumerable<string> Errors
        {
            get => _errors;
            private set => SetProperty(ref _errors, value);
        }
        public string? FirstError
        {
            get => _firstError;
            private set => SetProperty(ref _firstError, value);
        }
        public bool IsValid
        {
            get => _isValid;
            private set => SetProperty(ref _isValid, value);
        }
        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        public ValidatableObject()
        {
            _isValid = true;
            _errors = Enumerable.Empty<string>();
        }
        public bool Validate()
        {
            Errors = Validations
                ?.Where(v => !v.Check(Value))
                ?.Select(v => v.ValidationMessage)
                ?.ToArray()
                ?? Enumerable.Empty<string>();
            IsValid = !Errors.Any();
            FirstError = Errors.FirstOrDefault();
            return IsValid;
        }
    }
}
