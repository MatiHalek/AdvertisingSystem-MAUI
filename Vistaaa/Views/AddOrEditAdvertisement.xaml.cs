using CommunityToolkit.Maui.Storage;
using System.Reflection;
using Vistaaa.Models;

namespace Vistaaa.Views;

public partial class AddOrEditAdvertisement : ContentPage
{
	private readonly Database Database = new();
    public Advertisement? Advertisement{ get; set; }

    public AddOrEditAdvertisement()
	{
		InitializeComponent();
        FillPickers();
	}
    public AddOrEditAdvertisement(Advertisement advertisement) : this()
    {
        addOrEditAdvertisementContentPage.Title = "Edytuj og³oszenie";
        submitButton.Text = "ZatwierdŸ zmiany";
        Advertisement = advertisement;        
    }
    private bool Validate()
    {
        bool success = true;
        if(string.IsNullOrWhiteSpace(titleEntry.Text))
        {
            titleValidationLabel.Text = "Tytu³ nie mo¿e byæ pusty";
            titleValidationLabel.IsVisible = true;
            success = false;
        }
        else
            titleValidationLabel.IsVisible = false;
        if(categoryPicker.SelectedIndex == -1)
        {
            categoryValidationLabel.Text = "Wybierz kategoriê";
            categoryValidationLabel.IsVisible = true;
            success = false;
        }
        else
            categoryValidationLabel.IsVisible = false;
        if(companyPicker.SelectedIndex == -1)
        {
            companyValidationLabel.Text = "Wybierz firmê";
            companyValidationLabel.IsVisible = true;
            success = false;
        }
        else
            companyValidationLabel.IsVisible = false;
        if(string.IsNullOrWhiteSpace(positionNameEntry.Text))
        {
            positionNameValidationLabel.Text = "Nazwa stanowiska nie mo¿e byæ pusta";
            positionNameValidationLabel.IsVisible = true;
            success = false;
        }
        else
            positionNameValidationLabel.IsVisible = false;
        if(string.IsNullOrWhiteSpace(positionLevelEntry.Text))
        {
            positionLevelValidationLabel.Text = "Poziom stanowiska nie mo¿e byæ pusty";
            positionLevelValidationLabel.IsVisible = true;
            success = false;
        }
        else
            positionLevelValidationLabel.IsVisible = false;
        if(contractTypePicker.SelectedIndex == -1)
        {
            contractTypeValidationLabel.Text = "Wybierz rodzaj umowy";
            contractTypeValidationLabel.IsVisible = true;
            success = false;
        }
        else
            contractTypeValidationLabel.IsVisible = false;
        if(employmentTypePicker.SelectedIndex == -1)
        {
            employmentTypeValidationLabel.Text = "Wybierz wymiar zatrudnienia";
            employmentTypeValidationLabel.IsVisible = true;
            success = false;
        }
        else
            employmentTypeValidationLabel.IsVisible = false;
        if(workTypePicker.SelectedIndex == -1)
        {
            workTypeValidationLabel.Text = "Wybierz rodzaj pracy";
            workTypeValidationLabel.IsVisible = true;
            success = false;
        }
        else
            workTypeValidationLabel.IsVisible = false;
        if(expirationDatePicker.Date.Add(expirationTimePicker.Time) < DateTime.Now)
        {
            dateValidationLabel.Text = "Data wygaœniêcia nie mo¿e byæ wczeœniejsza ni¿ obecna";
            dateValidationLabel.IsVisible = true;
            success = false;
        }
        else
            dateValidationLabel.IsVisible = false;
        if (!string.IsNullOrWhiteSpace(lowestSalaryEntry.Text) && (!decimal.TryParse(lowestSalaryEntry.Text, out _) || decimal.Parse(lowestSalaryEntry.Text) < 0.01m || decimal.Parse(lowestSalaryEntry.Text) > 999999.99m || lowestSalaryEntry.Text.ToString().Replace(",", ".").TrimEnd('0').SkipWhile(c => c != '.').Skip(1).Count() > 2))
        {
            salaryValidationLabel.Text = "Podaj poprawn¹ wartoœæ najni¿szego wynagrodzenia";
            salaryValidationLabel.IsVisible = true;
        }
        else if(!decimal.TryParse(highestSalaryEntry.Text, out _) || decimal.Parse(highestSalaryEntry.Text) < 0.01m || decimal.Parse(highestSalaryEntry.Text) > 999999.99m || highestSalaryEntry.Text.ToString().Replace(",", ".").TrimEnd('0').SkipWhile(c => c != '.').Skip(1).Count() > 2)
        {
            salaryValidationLabel.Text = "Podaj poprawn¹ wartoœæ najwy¿szego wynagrodzenia";
            salaryValidationLabel.IsVisible = true;
        }
        else if(decimal.Parse(lowestSalaryEntry.Text) >= decimal.Parse(highestSalaryEntry.Text))
        {
            salaryValidationLabel.Text = "Wartoœæ najwy¿szego wynagrodzenia powinna byæ wiêksza od wartoœci najni¿szego wynagrodzenia";
            salaryValidationLabel.IsVisible = true;
        }
        else
            salaryValidationLabel.IsVisible = false;
        if (string.IsNullOrWhiteSpace(workDaysValidationLabel.Text))
        {
            workDaysValidationLabel.Text = "Podaj dni oraz godziny pracy";
            workDaysValidationLabel.IsVisible = true;
            success = false;
        }
        else
            workDaysValidationLabel.IsVisible = false;
        if (string.IsNullOrWhiteSpace(responsibilitiesEditor.Text))
        {
            responsibilitiesValidationLabel.Text = "Podaj obowi¹zki pracownika";
            responsibilitiesValidationLabel.IsVisible = true;
            success = false;
        }
        else
            responsibilitiesValidationLabel.IsVisible = false;
        if(string.IsNullOrWhiteSpace(requirementsEditor.Text))
        {
            requirementsValidationLabel.Text = "Podaj wymagania wobec kandydata";
            requirementsValidationLabel.IsVisible = true;
            success = false;
        }
        else
            requirementsValidationLabel.IsVisible = false;
        if(string.IsNullOrWhiteSpace(offerEditor.Text))
        {
            offerValidationLabel.Text = "Podaj ofertê pracodawcy";
            offerValidationLabel.IsVisible = true;
            success = false;
        }
        else
            offerValidationLabel.IsVisible = false;
        return success;
    }

    private async void FillPickers()
    {
        categoryPicker.ItemsSource = await Task.Run(Database.GetCategories);
        companyPicker.ItemsSource = await Task.Run(Database.GetCompanies);
        workTypePicker.ItemsSource = await Task.Run(Database.GetWorkTypes);
        employmentTypePicker.ItemsSource = await Task.Run(Database.GetEmploymentTypes);
        contractTypePicker.ItemsSource = await Task.Run(Database.GetContractTypes);
        if(Advertisement is not null)
            FillData();
    }
    private void FillData()
    {
        titleEntry.Text = Advertisement?.Title;
        categoryPicker.SelectedItem = categoryPicker.ItemsSource.Cast<Category>().First(category => category.Id == Advertisement?.CategoryId);
        companyPicker.SelectedItem = companyPicker.ItemsSource.Cast<Company>().First(company => company.Id == Advertisement?.CompanyId);
        positionNameEntry.Text = Advertisement?.PositionName;
        positionLevelEntry.Text = Advertisement?.PositionLevel;
        contractTypePicker.SelectedItem = contractTypePicker.ItemsSource.Cast<ContractType>().First(contractType => contractType.ContractTypeId == Advertisement?.ContractType);
        employmentTypePicker.SelectedItem = employmentTypePicker.ItemsSource.Cast<EmploymentType>().First(employmentType => employmentType.EmploymentTypeId == Advertisement?.EmploymentType);
        workTypePicker.SelectedItem = workTypePicker.ItemsSource.Cast<WorkType>().First(workType => workType.WorkTypeId == Advertisement?.WorkType);
        lowestSalaryEntry.Text = Advertisement?.LowestSalary.ToString();
        highestSalaryEntry.Text = Advertisement?.HighestSalary.ToString();
        workDaysEntry.Text = Advertisement?.WorkDays;
        expirationDatePicker.Date = (Advertisement!.ExpirationDate);
        expirationTimePicker.Time = Advertisement!.ExpirationDate.TimeOfDay;
        responsibilitiesEditor.Text = Advertisement?.Responsibilities;
        requirementsEditor.Text = Advertisement?.Requirements;
        offerEditor.Text = Advertisement?.Offer; 
    }

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        if(!Validate())
            return;
        if(Advertisement is null)
        await Database.CreateAdvertisementAsync(new Advertisement(
            titleEntry.Text.Trim(),
            ((Company)companyPicker.SelectedItem).Id,
            ((Category)categoryPicker.SelectedItem).Id,
            positionNameEntry.Text,
            positionLevelEntry.Text,
            ((ContractType)contractTypePicker.SelectedItem).ContractTypeId,
            ((EmploymentType)employmentTypePicker.SelectedItem).EmploymentTypeId,
            ((WorkType)workTypePicker.SelectedItem).WorkTypeId,
            decimal.Parse(lowestSalaryEntry.Text),
            decimal.Parse(highestSalaryEntry.Text),
            workDaysEntry.Text,
            DateTime.Now,
            expirationDatePicker.Date.Add(expirationTimePicker.Time),
            responsibilitiesEditor.Text,
            requirementsEditor.Text,
            offerEditor.Text
        ));
        else
        {
            Advertisement.Title = titleEntry.Text.Trim();
            Advertisement.CompanyId = ((Company)companyPicker.SelectedItem).Id;
            Advertisement.CategoryId = ((Category)categoryPicker.SelectedItem).Id;
            Advertisement.PositionName = positionNameEntry.Text;
            Advertisement.PositionLevel = positionLevelEntry.Text;
            Advertisement.ContractType = ((ContractType)contractTypePicker.SelectedItem).ContractTypeId;
            Advertisement.EmploymentType = ((EmploymentType)employmentTypePicker.SelectedItem).EmploymentTypeId;
            Advertisement.WorkType = ((WorkType)workTypePicker.SelectedItem).WorkTypeId;
            Advertisement.LowestSalary = decimal.Parse(lowestSalaryEntry.Text);
            Advertisement.HighestSalary = decimal.Parse(highestSalaryEntry.Text);
            Advertisement.WorkDays = workDaysEntry.Text;
            Advertisement.ExpirationDate = expirationDatePicker.Date.Add(expirationTimePicker.Time);
            Advertisement.Responsibilities = responsibilitiesEditor.Text;
            Advertisement.Requirements = requirementsEditor.Text;
            Advertisement.Offer = offerEditor.Text;
            await Database.UpdateAdvertisement(Advertisement);
        }
        await Navigation.PopAsync();
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void AddNewCategoryButton_Clicked(object sender, EventArgs e)
    {
        string promptResult = await DisplayPromptAsync("Dodaj now¹ kategoriê", "WprowadŸ nazwê nowej kategorii:", "Dodaj", "Anuluj", "Nazwa kategorii", 100, Keyboard.Text, "Nowa kategoria");
        //if(string.IsNullOrEmpty(promptResult))
            //return;
        if(string.IsNullOrWhiteSpace(promptResult))
        {
            await DisplayAlert("B³¹d dodawania kategorii", "Nazwa kategorii nie mo¿e byæ pusta!", "OK");
            return;
        }
        if((await Task.Run(Database.GetCategories)).Any(category => category.Name.ToLower() == promptResult.Trim().ToLower()))
        {
            await DisplayAlert("B³¹d dodawania kategorii", "Taka kategoria ju¿ istnieje!", "OK");
            return;
        }
        int? selectedCategoryId = null;
        if(categoryPicker.SelectedItem is not null)
            selectedCategoryId = (int?)((Category)categoryPicker.SelectedItem).Id;
        await Database.CreateCategory(new Category(promptResult.Trim()));
        await DisplayAlert("Dodano kategoriê", $"Pomyœlnie dodano kategoriê {promptResult}.", "OK");
        categoryPicker.ItemsSource = await Task.Run(Database.GetCategories); 
        if(selectedCategoryId is not null)
            categoryPicker.SelectedItem = categoryPicker.ItemsSource.Cast<Category>().First(category => category.Id == selectedCategoryId);
    }

    private async void AddNewCompanyButton_Clicked(object sender, EventArgs e)
    {
        RegistrationPage registrationPage = new(true);
        await Navigation.PushModalAsync(registrationPage);
        companyPicker.ItemsSource = await Task.Run(Database.GetCompanies);
    }
}