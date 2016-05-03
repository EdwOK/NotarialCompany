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
using NotarialCompany.Security.Authorization;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.EmployeesPositionsPage
{
    public class EmployeesPositionsViewModel : BasePageViewModel
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IDialogCoordinator dialogCoordinator;
        private string searchText;

        private ICollectionView employeesPositionsViews;

        private IList<EmployeesPosition> employeesPositions;

        public EmployeesPositionsViewModel(DbScope dbScope, IAuthorizationService authorizationService, IDialogCoordinator dialogCoordinator) 
            : base(dbScope)
        {
            this.authorizationService = authorizationService;
            this.dialogCoordinator = dialogCoordinator;
        }

        public ICollectionView EmployeesPositionsView
        {
            get { return employeesPositionsViews; }
            set { Set(ref employeesPositionsViews, value); }
        }

        public EmployeesPosition SelectedEmployeesPosition { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                EmployeesPositionsView?.Refresh();
            }
        }

        public bool CanDeleteEmployeePosition { get; set; }
        public bool CanCreateEmployeePosition { get; set; }

        protected override void LoadedCommandExecute()
        {
            CanDeleteEmployeePosition = authorizationService.CheckAccess(typeof(EmployeesPosition), ResourceAction.Delete);
            RaisePropertyChanged(() => CanDeleteEmployeePosition);

            CanCreateEmployeePosition = authorizationService.CheckAccess(typeof(EmployeesPosition), ResourceAction.Create);
            RaisePropertyChanged(() => CanCreateEmployeePosition);

            EmployeesPositionsView = CollectionViewSource.GetDefaultView(employeesPositions = DbScope.GetEmployeesPosition());
            EmployeesPositionsView.Filter = Filter;
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new EmployeePositonDetailsView(), nameof(EmployeesPositionsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<EmployeesPosition>(new EmployeesPositionsView(), nameof(EmployeesPositionsViewModel),
                nameof(EmployeesPositionsViewModel), SelectedEmployeesPosition));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new EmployeePositonDetailsView(), nameof(EmployeesPositionsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<EmployeesPosition>(new EmployeesPositionsView(), nameof(EmployeesPositionsViewModel),
                nameof(EmployeesPositionsViewModel), new EmployeesPosition()));
        }

        protected override async void RemoveItemCommandExecute()
        {
            MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(
                this, "Delete employee position", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);

            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }

            DbScope.DeleteEmployee(SelectedEmployeesPosition.Id);
            employeesPositions.Remove(SelectedEmployeesPosition);
            employeesPositionsViews.Refresh();
        }

        private bool Filter(object obj)
        {
            var data = obj as EmployeesPosition;
            if (data == null)
            {
                return false;
            }

            return string.IsNullOrEmpty(SearchText) ||
                   data.Position.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   (data.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);
        }
    }
}
