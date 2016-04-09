using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Pages.ServicesPage;

namespace NotarialCompany.Pages.UsersPage
{
    public class UserDetailsViewModel : ValidationViewModel
    {
        private readonly DbScope dbScope;

        private ContentControl parentView;
        private string parentViewModelName;

        public UserDetailsViewModel(DbScope dbScope)
        {
            this.dbScope = dbScope;
            this.Roles = dbScope.GetRoles();

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);

            ValidatingProperties = new List<string> {nameof(Username), nameof(Password), nameof(Salt)};

            Messenger.Default.Register<SendViewModelParamArgs<User>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(UserDetailsViewModel))
                {
                    return;
                }

                AllowValidation = false;

                parentView = args.ParentView;
                parentViewModelName = args.ParentViewModelName;

                User = args.Parameter ?? new User();
                SelectedRole = Roles.Find(r => r.Id == User.RoleId);

                RaisePropertyChanged(() => Username);
                RaisePropertyChanged(() => Password);
                RaisePropertyChanged(() => Salt);
                RaisePropertyChanged(() => SelectedRole);
            });
        }

        public User User { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand NavigateBackCommand { get; set; }

        public string Username
        {
            get { return User?.Username; }
            set { User.Username = value; }
        }

        public string Password
        {
            get { return User?.Password; }
            set { User.Password = value; }
        }

        public string Salt
        {
            get { return User?.Salt; }
            set { User.Salt = value; }
        }

        public Employee Employee
        {
            get { return User?.Employee; }
            set { User.Employee = value; }
        }

        public Role SelectedRole { get; set; }

        public List<Role> Roles { get; set; }

        public List<Employee> Employees { get; set; }
         
        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(Username) && string.IsNullOrWhiteSpace(Username))
            {
                return "Name is required";
            }
            if (propertyName == nameof(Password) && string.IsNullOrWhiteSpace(Password))
            {
                return "Password is required";
            }
            if (propertyName == nameof(Salt) && string.IsNullOrWhiteSpace(Salt))
            {
                return "Salt is required";
            }
            return null;
        }

        private void SaveCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }

            User.EmployeeId = User.Employee.Id;
            User.RoleId = User.RoleId;

            dbScope.UpdateUser(User);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }
    }
}
