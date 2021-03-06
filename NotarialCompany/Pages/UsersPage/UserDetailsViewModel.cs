﻿using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Security;
using NotarialCompany.Security.Authentication;
using NotarialCompany.Security.Authorization;

namespace NotarialCompany.Pages.UsersPage
{
    public class UserDetailsViewModel : ValidationViewModel
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IAuthorizationService authorizationService;

        private string savedPassword;

        public UserDetailsViewModel(DbScope dbScope, IAuthenticationService authenticationService, IAuthorizationService authorizationService) : base(dbScope)
        {
            this.authenticationService = authenticationService;
            this.authorizationService = authorizationService;

            this.Roles = dbScope.GetRoles();
            this.Employees = dbScope.GetEmployees();

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);

            ValidatingProperties = new List<string>
            {
                nameof(Username),
                nameof(Password),
                nameof(SelectedRole),
                nameof(SelectedEmployee)
            };

            Messenger.Default.Register<SendViewModelParamArgs<User>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(UserDetailsViewModel))
                {
                    return;
                }

                AllowValidation = false;

                ParentView = args.ParentView;
                ParentViewModelName = args.ParentViewModelName;

                User = args.Parameter ?? new User();
                savedPassword = User.Password;
                User.Password = null;

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

        public ICommand LoadedCommand { get; set; }

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

        public bool CanUpdateUser { get; set; }

        public Employee SelectedEmployee { get; set; }

        public Role SelectedRole { get; set; }

        public List<Role> Roles { get; set; }

        public List<Employee> Employees { get; set; }

        public bool IsCreationMode => User?.Id == 0;

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(Username) && string.IsNullOrWhiteSpace(Username))
            {
                return "Name is required";
            }
            if (propertyName == nameof(Password) && string.IsNullOrWhiteSpace(Password) && IsCreationMode)
            {
                return "Password is required";
            }
            if (propertyName == nameof(SelectedRole) && SelectedRole == null)
            {
                return "Role is required";
            }
            if (propertyName == nameof(SelectedEmployee) && SelectedEmployee == null)
            {
                return "Employee is required";
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

            if (IsCreationMode || User.Password != null)
            {
                authenticationService.GenerateCredentials(User);
            }
            else
            {
                User.Password = savedPassword;
            }
            DbScope.CreateOrUpdateUser(User);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(ParentView, ParentViewModelName));
        }

        private void LoadedCommandExecute()
        {
            CanUpdateUser = authorizationService.CheckAccess(typeof (User), ResourceAction.Update);
            RaisePropertyChanged(() => CanUpdateUser);
        }
    }
}
