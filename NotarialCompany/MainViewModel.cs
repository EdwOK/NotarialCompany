using System;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Pages.ClientsPage;
using NotarialCompany.Pages.DealsPage;
using NotarialCompany.Pages.LoginPage;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Security;

namespace NotarialCompany
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private ContentControl currentView;

        public MainViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            CurrentView = new LoginView();

            OpenDealsCommand = new RelayCommand(OpenDealsCommandExecute);
            OpenServicesCommand = new RelayCommand(OpenServicesCommandExecute);
            OpenClientsCommand = new RelayCommand(OpenClientsCommandExecute);
            Messenger.Default.Register<OpenViewArgs>(this, args =>
            {
                CurrentView = args.View;
                RaisePropertyChanged(() => IsAuthenticated);
            });
        }

        public bool IsAuthenticated => authenticationService.IsAuthenticated();

        public ContentControl CurrentView
        {
            get { return currentView; }
            set { Set(ref currentView, value); }
        }

        public ICommand OpenServicesCommand { get; set; }
        public ICommand OpenDealsCommand { get; set; }
        public ICommand OpenClientsCommand { get; set; }
        
        private void OpenServicesCommandExecute()
        {
            CurrentView = new ServicesView();
        }

        private void OpenDealsCommandExecute()
        {
            CurrentView = new DealsView();
        }

        private void OpenClientsCommandExecute()
        {
            CurrentView = new ClientsView();
        }
    }
}