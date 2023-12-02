namespace Vistaaa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void ShellContent_Appearing(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "This is an alert.", "OK");
        }
        private void ShellContent_Appearing2(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "This is an alert2.", "OK");
        }
    }
}
