using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly DbScope dbScope;

        public ClientsViewModel(DbScope dbScope)
        {
            OpenClientCommand = new RelayCommand(OpenClientsCommandExecute);

            this.dbScope = dbScope;
            Clients = dbScope.GetClients();
        }

        public List<Models.Client> Clients { get; set; }

        public Models.Client SelectedClient { get; set; }

        public ICommand OpenClientCommand { get; set; }

        private void OpenClientsCommandExecute()
        {
            var a = 5;
        }
    }
}
