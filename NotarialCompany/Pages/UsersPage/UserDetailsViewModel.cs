using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Security;

namespace NotarialCompany.Pages.UsersPage
{
    public class UserDetailsViewModel : ValidationViewModel
    {
        private readonly DbScope dbScope;
        private readonly IAuthenticationService authenticationService;

        private MetroContentControl parentView;
        private string parentViewModelName;

        public UserDetailsViewModel(DbScope dbScope, IAuthenticationService authenticationService)
        {
            this.dbScope = dbScope;
            this.authenticationService = authenticationService;

            this.Roles = dbScope.GetRoles();
            this.Employees = dbScope.GetEmployees();

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);

            ValidatingProperties = new List<string> { nameof(Username), nameof(Password) };

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
                SelectedEmployee = Employees.Find(e => e.Id == User.EmployeeId);

                RaisePropertyChanged(() => Username);
                RaisePropertyChanged(() => Password);
                RaisePropertyChanged(() => SelectedRole);
                RaisePropertyChanged(() => SelectedEmployee);
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

        public Employee SelectedEmployee { get; set; }

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
            return null;
        }

        private void SaveCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }

            User.Employee = SelectedEmployee;
            User.EmployeeId = SelectedEmployee.Id;

            User.Role = SelectedRole;
            User.RoleId = SelectedRole.Id;

            var saveUser = authenticationService.GenerateCredentials(User);
            dbScope.UpdateUser(saveUser);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }
    }
}
