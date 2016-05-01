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
using NotarialCompany.Pages.ClientsPage;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.EmployeesPage
{
    public class EmployeesViewModel : BasePageViewModel
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private string searchText;

        private ICollectionView employessViews;

        private IList<Employee> employees;

        public EmployeesViewModel(DbScope dbScope, IDialogCoordinator dialogCoordinator) : base(dbScope)
        {
            this.dialogCoordinator = dialogCoordinator;
        }

        public ICollectionView EmployeesView
        {
            get { return employessViews; }
            set { Set(ref employessViews, value); }
        }

        public Employee SelectedEmployee { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                EmployeesView?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            EmployeesView = CollectionViewSource.GetDefaultView(employees = DbScope.GetEmployees());
            EmployeesView.Filter = Filter;
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new EmployeeDetailsView(), nameof(EmployeeDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Employee>(new EmployeesView(), nameof(EmployeeDetailsViewModel),
                nameof(EmployeeDetailsViewModel), SelectedEmployee));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new EmployeeDetailsView(), nameof(EmployeeDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Employee>(new EmployeesView(), nameof(EmployeeDetailsViewModel),
                nameof(EmployeeDetailsViewModel), new Employee()));
        }

        protected override async void RemoveItemCommandExecute()
        {
            MessageDialogResult result =
                await
                    dialogCoordinator.ShowMessageAsync(this, "Delete employee", "Are you sure?",
                        MessageDialogStyle.AffirmativeAndNegative);

            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }

            DbScope.DeleteEmployee(SelectedEmployee.Id);
            employees.Remove(SelectedEmployee);
            employessViews.Refresh();
        }

        private bool Filter(object obj)
        {
            var data = obj as Employee;
            if (data == null)
            {
                return false;
            }
            
            return string.IsNullOrEmpty(SearchText) ||
                   data.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   data.MiddleName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   data.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   data.Address.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   data.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || 
                   data.EmployeesPosition.Position.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
