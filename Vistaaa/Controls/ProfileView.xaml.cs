using Vistaaa.Models;

namespace Vistaaa.Controls;

public partial class ProfileView : ContentView
{
	User User { get; set; }
	public ProfileView(User user)
	{
		InitializeComponent();
		User = user;
		if(User.FirstName is not null)
			nameAndSurnameLabel.Text = User.FirstName + " " + User.LastName;
		else
			nameAndSurnameLabel.Text = User.Email;
	}
}