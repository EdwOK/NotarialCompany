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

namespace NotarialCompany.Pages.UsersPage
{
    public class UsersViewModel : BasePageViewModel
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private string searchText;

        private ICollectionView usersViews;

        private IList<User> users; 

        public UsersViewModel(DbScope dbScope, IDialogCoordinator dialogCoordinator)
            : base(dbScope)
        {
            this.dialogCoordinator = dialogCoordinator;
        }

        public ICollectionView UsersViews
        {
            get { return usersViews; }
            set { Set(ref usersViews, value); }
        }

        public User SelectedUser { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                UsersViews?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            UsersViews = CollectionViewSource.GetDefaultView(users = DbScope.GetUsers());
            UsersViews.Filter = Filter;
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new UserDetailsView(), nameof(UserDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<User>(new UsersView(), nameof(UsersViewModel),
                nameof(UserDetailsViewModel), SelectedUser));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new UserDetailsView(), nameof(UserDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<User>(new UsersView(), nameof(UsersViewModel),
                nameof(UserDetailsViewModel), new User()));
        }

        protected override async void RemoveItemCommandExecute()
        {
            MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(
                this, "Delete user", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);

            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }

            DbScope.DeleteUser(SelectedUser.Id);
            users.Remove(SelectedUser);
            usersViews.Refresh();
        }


        private bool Filter(object obj)
        {
            var data = obj as User;
            if (data == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                return data.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Role.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Employee.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Employee.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Employee.MiddleName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Employee.EmployeesPosition.Postition.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}