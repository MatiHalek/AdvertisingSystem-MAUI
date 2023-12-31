using Vistaaa.Views;

namespace Vistaaa
{
    public partial class AppShell : Shell
    {

        public string AppVersion { get; } = "1.0.0";
        public DateOnly ReleaseDate { get; } = new DateOnly(2024, 1, 2);
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
