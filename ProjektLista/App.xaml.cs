using ProjektLista.Pages;

namespace ProjektLista
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new StronaGlowna();
        }
    }
}
