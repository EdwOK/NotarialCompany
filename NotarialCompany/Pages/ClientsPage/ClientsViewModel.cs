using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientsViewModel : BasePageViewModel
    {
        private string searchText;

        private ICollectionView clients;

        public ClientsViewModel(DbScope dbScope) : base(dbScope)
        {
        }

        public ICollectionView Clients
        {
            get { return clients; }
            set { Set(ref clients, value); }
        }

        public Client SelectedClient { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                Clients?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            Clients = CollectionViewSource.GetDefaultView(DbScope.GetClients());
            Clients.Filter = Filter;
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

        protected override void RemoveItemCommandExecute()
        {
            throw new NotImplementedException();
        }

        private bool Filter(object obj)
        {
            var data = obj as Client;
            if (data == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                return data.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.MiddleName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.SecondName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Address.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Occupation.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}
