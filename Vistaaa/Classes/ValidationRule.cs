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
        public string ValidationMessage { get; set; } = "";

        public bool Check(T value) =>
            value is string str && !string.IsNullOrWhiteSpace(str);
    }
    public partial class EmailRule<T> : IValidationRule<T>
    {
        private readonly Regex _regex = EmailRegex();

        public string ValidationMessage { get; set; } = "";

        public bool Check(T value) =>
            value is string str && _regex.IsMatch(str);
        [GeneratedRegex(@"^([w.-]+)@([w-]+)((.(w){2,3})+)$")]
        private static partial Regex EmailRegex();
    }
}
