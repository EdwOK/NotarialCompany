using System;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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
        private ContentControl currentContent;

        public MainViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            CurrentContent = new LoginView();

            OpenDealsCommand = new RelayCommand(OpenDealsCommandExecute);
            OpenServicesCommand = new RelayCommand(OpenServicesCommandExecute);
            OpenClientsCommand = new RelayCommand(OpenClientsCommandExecute);

            Messenger.Default.Register<int>(this, i =>
            {
                CurrentContent = new DealsView();
                RaisePropertyChanged(() => IsAuthenticated);
            });
        }

        public bool IsAuthenticated => authenticationService.IsAuthenticated();

        public ContentControl CurrentContent
        {
            get { return currentContent; }
            set { Set(ref currentContent, value); }
        }

        public ICommand OpenServicesCommand { get; set; }
        public ICommand OpenDealsCommand { get; set; }
        public ICommand OpenClientsCommand { get; set; }
        

        private void OpenServicesCommandExecute()
        {
            CurrentContent = new ServicesView();
        }

        private void OpenDealsCommandExecute()
        {
            CurrentContent = new DealsView();
        }

        private void OpenClientsCommandExecute()
        {
            CurrentContent = new ClientsView();
        }
    }
}