namespace Vistaaa
{
    public partial class AppShell : Shell
    {

        public string AppVersion { get; } = "1.0.0";
        public DateOnly ReleaseDate { get; } = new DateOnly(2023, 12, 19);
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;            
        }
    }
}
