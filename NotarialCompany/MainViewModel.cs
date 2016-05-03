using System;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.Models;
using NotarialCompany.Pages.ClientsPage;
using NotarialCompany.Pages.DealsPage;
using NotarialCompany.Pages.EmployeesPage;
using NotarialCompany.Pages.EmployeesPositionsPage;
using NotarialCompany.Pages.LoginPage;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Pages.UsersPage;
using NotarialCompany.Security.Authentication;
using NotarialCompany.Security.Authorization;

namespace NotarialCompany
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IAuthorizationService authorizationService;
        private MetroContentControl currentView;

        private readonly UsersView usersView;
        private readonly ServicesView servicesView;
        private readonly DealsView dealsView;
        private readonly ClientsView clientsView;
        private readonly EmployeesView employeeView;
        private readonly EmployeesPositionsView employeesPositionsView;
        private readonly LoginView loginView;

        public MainViewModel(IAuthenticationService authenticationService, IAuthorizationService authorizationService,
            UsersView usersView, ServicesView servicesView, DealsView dealsView, ClientsView clientsView, 
            EmployeesView employeeView, EmployeesPositionsView employeesPositionsView, LoginView loginView)
        {
            CurrentView = loginView;

            this.authenticationService = authenticationService;
            this.authorizationService = authorizationService;

            this.usersView = usersView;
            this.servicesView = servicesView;
            this.dealsView = dealsView;
            this.clientsView = clientsView;
            this.employeeView = employeeView;
            this.employeesPositionsView = employeesPositionsView;
            this.loginView = loginView;

            OpenDealsCommand = new RelayCommand(OpenDealsCommandExecute);
            OpenServicesCommand = new RelayCommand(OpenServicesCommandExecute);
            OpenClientsCommand = new RelayCommand(OpenClientsCommandExecute);
            OpenUsersCommand = new RelayCommand(OpenUsersCommandExecute);
            OpenEmployeeCommand = new RelayCommand(OpenEmployeesCommandExecute);
            OpenEmployeesPotitionsCommand = new RelayCommand(OpenEmployeesPositionsCommandExecute);
            LogoutCommand = new RelayCommand(LogoutCommandExecute);
            OpenUserProfileCommand = new RelayCommand(OpenUserProfileCommandExecute);

            Messenger.Default.Register<OpenViewArgs>(this, args =>
            {
                CurrentView = args.View;
                RaisePropertyChanged(() => IsAuthenticated);
                RaisePropertyChanged(() => HasUserAccess);
                RaisePropertyChanged(() => HasEmployeesPositionAccess);
            });
        }

        public bool IsAuthenticated => authenticationService.IsAuthenticated();

        public bool HasUserAccess => authorizationService.CheckAccess(typeof (User), AccessPolicyBase.StandartResourceActions);
        public bool HasEmployeesPositionAccess => authorizationService.CheckAccess(typeof(EmployeesPosition), AccessPolicyBase.StandartResourceActions);

        public MetroContentControl CurrentView
        {
            get { return currentView; }
            set { Set(ref currentView, value); }
        }

        public ICommand OpenServicesCommand { get; set; }
        public ICommand OpenDealsCommand { get; set; }
        public ICommand OpenClientsCommand { get; set; }
        public ICommand OpenUsersCommand { get; set; }
        public ICommand OpenEmployeeCommand { get; set; }
        public ICommand OpenEmployeesPotitionsCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand OpenUserProfileCommand { get; set; }

        public void OpenUsersCommandExecute()
        {
            CurrentView = usersView;
        }

        private void OpenServicesCommandExecute()
        {
            CurrentView = servicesView;
        }

        private void OpenDealsCommandExecute()
        {
            CurrentView = dealsView;
        }

        private void OpenClientsCommandExecute()
        {
            CurrentView = clientsView;
        }

        private void OpenEmployeesCommandExecute()
        {
            CurrentView = employeeView;
        }

        private void OpenEmployeesPositionsCommandExecute()
        {
            CurrentView = employeesPositionsView;
        }

        private void OpenUserProfileCommandExecute()
        {
            MetroContentControl view = CurrentView;
            Messenger.Default.Send(new OpenViewArgs(new UserDetailsView(), nameof(UserDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<User>(view, nameof(MainViewModel), nameof(UserDetailsViewModel), authenticationService.CurrentUser));
        }

        private void LogoutCommandExecute()
        {
            authenticationService.Logout();
            CurrentView = loginView;
            RaisePropertyChanged(() => IsAuthenticated);
        }
    }
}