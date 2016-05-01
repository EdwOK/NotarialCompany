using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using NotarialCompany.DataAccess;
using NotarialCompany.Pages.ClientsPage;
using NotarialCompany.Pages.DealsPage;
using NotarialCompany.Pages.EmployeesPage;
using NotarialCompany.Pages.LoginPage;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Pages.UsersPage;
using NotarialCompany.Security;
using NotarialCompany.Security.Authentication;
using NotarialCompany.Security.Authorization;

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

            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<UserDetailsViewModel>();

            SimpleIoc.Default.Register<ClientsViewModel>();
            SimpleIoc.Default.Register<ClientDetailsViewModel>();

            SimpleIoc.Default.Register<ServicesViewModel>();
            SimpleIoc.Default.Register<ServiceDetailsViewModel>();

            SimpleIoc.Default.Register<DealsViewModel>();

            SimpleIoc.Default.Register<EmployeesViewModel>();
            SimpleIoc.Default.Register<EmployeeDetailsViewModel>();

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<IAuthorizationService, AuthorizationService>();
            SimpleIoc.Default.Register<DbScope>();

            SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();

            SimpleIoc.Default.Register<ServicesView>();
            SimpleIoc.Default.Register<DealsView>();
            SimpleIoc.Default.Register<ClientsView>();
            SimpleIoc.Default.Register<UsersView>();
            SimpleIoc.Default.Register<LoginView>();
            SimpleIoc.Default.Register<EmployeesView>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public UsersViewModel UsersViewModel => ServiceLocator.Current.GetInstance<UsersViewModel>();
        public UserDetailsViewModel UserDetailsViewModel => ServiceLocator.Current.GetInstance<UserDetailsViewModel>();

        public ClientsViewModel ClientsViewModel => ServiceLocator.Current.GetInstance<ClientsViewModel>();
        public ClientDetailsViewModel ClientDetailsViewModel => ServiceLocator.Current.GetInstance<ClientDetailsViewModel>();

        public ServicesViewModel ServicesViewModel => ServiceLocator.Current.GetInstance<ServicesViewModel>();
        public ServiceDetailsViewModel ServiceDetailsViewModel => ServiceLocator.Current.GetInstance<ServiceDetailsViewModel>();

        public DealsViewModel DealsViewModel => ServiceLocator.Current.GetInstance<DealsViewModel>();

        public EmployeesViewModel EmployeesViewModel => ServiceLocator.Current.GetInstance<EmployeesViewModel>();
        public EmployeeDetailsViewModel EmployeeDetailsViewModel => ServiceLocator.Current.GetInstance<EmployeeDetailsViewModel>();

        public ServicesView ServicesView => ServiceLocator.Current.GetInstance<ServicesView>();
        public DealsView DealsView => ServiceLocator.Current.GetInstance<DealsView>();
        public ClientsView ClientsView => ServiceLocator.Current.GetInstance<ClientsView>();
        public UsersView UsersView => ServiceLocator.Current.GetInstance<UsersView>();
        public LoginView LoginView => ServiceLocator.Current.GetInstance<LoginView>();
        public EmployeesView EmployeesView => ServiceLocator.Current.GetInstance<EmployeesView>();


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}