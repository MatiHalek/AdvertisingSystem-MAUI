using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vistaaa.Classes
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "Należy uzupełnić to pole.";

        public bool Check(T value) =>
            value is string str && !string.IsNullOrWhiteSpace(str);
    }
    public class EmailExistsRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "Ten e-mail jest już przypisany do innego konta";
        private readonly Database Database = new();

        public bool Check(T value) =>
            value is string str && !Task.Run(() => Database.EmailExists(str)).Result;           
    }
    public class CompanyNameExistsRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "Firma o takiej nazwie już istnieje";
        private readonly Database Database = new();

        public bool Check(T value) =>
            value is string str && !Task.Run(() => Database.CompanyNameExists(str)).Result;
    }
    public partial class EmailRule<T> : IValidationRule<T>
    {
        private readonly Regex _regex = EmailRegex();

        public string ValidationMessage { get; set; } = "Proszę podać poprawny adres e-mail";

        public bool Check(T value) =>
            value is string str && _regex.IsMatch(str);
        [GeneratedRegex(@"^([\w.-]+)@([\w-]+)((.(\w){2,3})+)$")]
        private static partial Regex EmailRegex();
    }
    public partial class  PasswordRule<T> : IValidationRule<T>
    {
        private readonly Regex _regex = PasswordRegex();

        public string ValidationMessage { get; set; } = "Hasło musi zawierać od 8 do 255 znaków (w tym cyfry, małe i wielkie litery oraz znaki specjalne)";

        public bool Check(T value) =>
            value is string str && _regex.IsMatch(str);
        [GeneratedRegex(@"^(?=.*[\p{L}])(?=.*\d)(?=.*[!@#$%^&*()])[\p{L}\d!@#$%^&*()]{8,255}$")]
        private static partial Regex PasswordRegex();
    }
    public partial class PostalCodeRule<T> : IValidationRule<T>
    {
        private readonly Regex _regex = PostalCodeRegex();

        public string ValidationMessage { get; set; } = "Proszę wpisać poprawny kod pocztowy";

        public bool Check(T value) =>
            value is string str && _regex.IsMatch(str);
        [GeneratedRegex(@"^[0-9]{2}-[0-9]{3}$")]
        private static partial Regex PostalCodeRegex();
    }
    public partial class DateRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; } = "Aby założyć konto, musisz mieć ukończone co najmniej 18 lat.";

        public bool Check(T value) =>
            value is DateTime str && DateTime.Today.AddYears(-18) >= str;
    }

}
