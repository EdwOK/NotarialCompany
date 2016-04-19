using System;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.Pages.ClientsPage;
using NotarialCompany.Pages.DealsPage;
using NotarialCompany.Pages.LoginPage;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Pages.UsersPage;
using NotarialCompany.Security;

namespace NotarialCompany
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private MetroContentControl currentView;

        private readonly UsersView usersView;
        private readonly ServicesView servicesView;
        private readonly DealsView dealsView;
        private readonly ClientsView clientsView;
        private readonly LoginView loginView;

        public MainViewModel(IAuthenticationService authenticationService, UsersView usersView,
            ServicesView servicesView, DealsView dealsView, ClientsView clientsView, LoginView loginView)
        {
            this.authenticationService = authenticationService;
            this.usersView = usersView;
            this.servicesView = servicesView;
            this.dealsView = dealsView;
            this.clientsView = clientsView;
            this.loginView = loginView;
            CurrentView = new LoginView();

            OpenDealsCommand = new RelayCommand(OpenDealsCommandExecute);
            OpenServicesCommand = new RelayCommand(OpenServicesCommandExecute);
            OpenClientsCommand = new RelayCommand(OpenClientsCommandExecute);
            OpenUsersCommand = new RelayCommand(OpenUsersCommandExecute);
            LogoutCommand = new RelayCommand(LogoutCommandExecute);

            Messenger.Default.Register<OpenViewArgs>(this, args =>
            {
                CurrentView = args.View;
                RaisePropertyChanged(() => IsAuthenticated);
            });
        }

        public bool IsAuthenticated => authenticationService.IsAuthenticated();

        public MetroContentControl CurrentView
        {
            get { return currentView; }
            set { Set(ref currentView, value); }
        }

        public ICommand OpenServicesCommand { get; set; }
        public ICommand OpenDealsCommand { get; set; }
        public ICommand OpenClientsCommand { get; set; }
        public ICommand OpenUsersCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

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

        private void LogoutCommandExecute()
        {
            authenticationService.Logout();
            CurrentView = loginView;
            RaisePropertyChanged(() => IsAuthenticated);
        }
    }
}