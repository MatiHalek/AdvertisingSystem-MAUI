using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistaaa.Selectors
{
    public class RegistrationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IndividualUserTemplate { get; set; } = new();
        public DataTemplate CompanyTemplate { get; set; } = new();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if ((string)item == "IndividualUser")
                return IndividualUserTemplate;
            else
                return CompanyTemplate;
        }
    }
}
