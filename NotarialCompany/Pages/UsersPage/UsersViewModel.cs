using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.UsersPage
{
    public class UsersViewModel : BasePageViewModel
    {
        private List<User> users;

        public UsersViewModel(DbScope dbScope) : base(dbScope)
        {
        }

        public List<User> Users
        {
            get { return users; }
            set { Set(ref users, value); }
        }

        public User SelectedUser { get; set; }

        protected override void LoadedCommandExecute()
        {
            Users = DbScope.GetUsers();
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
    }
}
