using Vistaaa.Models;

namespace Vistaaa.Controls;

public partial class ProfileView : ContentView
{
	User User { get; set; }
	public ProfileView(User user)
	{
		InitializeComponent();
		User = user;
		label.Text = User.FirstName;
	}
}