using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientsViewModel : BasePageViewModel
    {
        private List<Client> clients;

        public ClientsViewModel(DbScope dbScope) : base(dbScope)
        {
        }

        public List<Client> Clients
        {
            get { return clients; }
            set { Set(ref clients, value); }
        }

        public Client SelectedClient { get; set; }

        protected override void LoadedCommandExecute()
        {
            Clients = DbScope.GetClients();
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ClientDetailsView(), nameof(ClientDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Client>(new ClientsView(), nameof(ClientsViewModel),
                nameof(ClientDetailsViewModel), SelectedClient));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ClientDetailsView(), nameof(ClientDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Client>(new ClientsView(), nameof(ClientsViewModel),
                nameof(ClientDetailsViewModel), new Client()));
        }
    }
}
