using System.Windows;
using NotarialCompany.Configuration;

namespace NotarialCompany
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            AutoMapperConfig.Configure();
        }
    }
}
