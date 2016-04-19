using System;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.UsersPage
{
    public class UsersViewModel : BasePageViewModel
    {
        private string searchText;

        private ICollectionView users;

        public UsersViewModel(DbScope dbScope) : base(dbScope)
        {
        }

        public ICollectionView Users
        {
            get { return users; }
            set { Set(ref users, value); }
        }

        public User SelectedUser { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                Users?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            Users = CollectionViewSource.GetDefaultView(DbScope.GetUsers());
            Users.Filter = Filter;
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