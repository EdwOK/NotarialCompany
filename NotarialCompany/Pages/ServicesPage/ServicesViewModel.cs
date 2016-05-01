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

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServicesViewModel : BasePageViewModel
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private string searchText;

        private ICollectionView servicesViews;

        private IList<Service> services;

        public ServicesViewModel(DbScope dbScope, IDialogCoordinator dialogCoordinator) : base(dbScope)
        {
            this.dialogCoordinator = dialogCoordinator;
        }

        public ICollectionView ServicesView
        {
            get { return servicesViews; }
            set { Set(ref servicesViews, value); }
        }

        public Service SelectedServise { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                ServicesView?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            ServicesView = CollectionViewSource.GetDefaultView(services = DbScope.GetServices());
            ServicesView.Filter = Filter;
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ServiceDetailsView(), nameof(ServiceDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Service>(new ServicesView(), nameof(ServicesViewModel),
                nameof(ServiceDetailsViewModel), SelectedServise));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ServiceDetailsView(), nameof(ServiceDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Service>(new ServicesView(), nameof(ServicesViewModel),
                nameof(ServiceDetailsViewModel), new Service()));
        }

        protected override async void RemoveItemCommandExecute()
        {
            MessageDialogResult result =
                await
                    dialogCoordinator.ShowMessageAsync(this, "Delete service", "Are you sure?",
                        MessageDialogStyle.AffirmativeAndNegative);

            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }

            DbScope.DeleteService(SelectedServise.Id);
            services.Remove(SelectedServise);
            servicesViews.Refresh();
        }

        private bool Filter(object obj)
        {
            var data = obj as Service;
            if (data == null)
            {
                return false;
            }

            return string.IsNullOrEmpty(SearchText) ||
                   data.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
