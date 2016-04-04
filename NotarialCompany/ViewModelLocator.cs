using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using NotarialCompany.DataAccess;
using NotarialCompany.Pages.DealsPage;
using NotarialCompany.Pages.LoginPage;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Security;

namespace NotarialCompany
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<DealsViewModel>();
            SimpleIoc.Default.Register<ServicesViewModel>();
            SimpleIoc.Default.Register<ServiceDetailsVewModel>();

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<DbScope>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();
        public DealsViewModel DealsViewModel => ServiceLocator.Current.GetInstance<DealsViewModel>();
        public ServicesViewModel ServicesViewModel => ServiceLocator.Current.GetInstance<ServicesViewModel>();
        public ServiceDetailsVewModel ServiceDetailsVewModel => ServiceLocator.Current.GetInstance<ServiceDetailsVewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}