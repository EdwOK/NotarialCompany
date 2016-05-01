using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientsViewModel : BasePageViewModel
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private string searchText;

        private ICollectionView clientsViews;

        private IList<Client> clients;

        public ClientsViewModel(DbScope dbScope, IDialogCoordinator dialogCoordinator) : base(dbScope)
        {
            this.dialogCoordinator = dialogCoordinator;
        }

        public ICollectionView ClientsView
        {
            get { return clientsViews; }
            set { Set(ref clientsViews, value); }
        }

        public Client SelectedClient { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                ClientsView?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            ClientsView = CollectionViewSource.GetDefaultView(clients = DbScope.GetClients());
            ClientsView.Filter = Filter;
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

        protected override async void RemoveItemCommandExecute()
        {
            MessageDialogResult result =
                await
                    dialogCoordinator.ShowMessageAsync(this, "Delete client", "Are you sure?",
                        MessageDialogStyle.AffirmativeAndNegative);

            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }

            DbScope.DeleteClient(SelectedClient.Id);
            clients.Remove(SelectedClient);
            clientsViews.Refresh();
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
