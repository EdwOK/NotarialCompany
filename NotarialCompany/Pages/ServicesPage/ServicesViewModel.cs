﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Security.Authorization;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServicesViewModel : BasePageViewModel
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IDialogCoordinator dialogCoordinator;
        private string searchText;

        private IList<Service> services;

        private ICollectionView servicesViews;

        public ServicesViewModel(DbScope dbScope, IAuthorizationService authorizationService,
            IDialogCoordinator dialogCoordinator)
            : base(dbScope)
        {
            this.authorizationService = authorizationService;
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

        public bool CanDeleteService { get; set; }
        public bool CanCreateService { get; set; }

        protected override void LoadedCommandExecute()
        {
            CanDeleteService = authorizationService.CheckAccess(typeof(Service), ResourceAction.Delete);
            RaisePropertyChanged(() => CanDeleteService);

            CanCreateService = authorizationService.CheckAccess(typeof(Service), ResourceAction.Create);
            RaisePropertyChanged(() => CanCreateService);

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
            var result = await dialogCoordinator.ShowMessageAsync(
                this, "Delete service", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);

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