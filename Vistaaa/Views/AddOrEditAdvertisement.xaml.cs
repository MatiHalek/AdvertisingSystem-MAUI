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
}