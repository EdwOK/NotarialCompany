using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Security.Authorization;

namespace NotarialCompany.Pages.EmployeesPage
{
    public class EmployeeDetailsViewModel : ValidationViewModel
    {
        private readonly IAuthorizationService authorizationService;

        public EmployeeDetailsViewModel(DbScope dbScope, IAuthorizationService authorizationService) : base(dbScope)
        {
            this.authorizationService = authorizationService;
            this.EmployeesPositions = dbScope.GetEmployeesPosition();

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);

            ValidatingProperties = new List<string>
            {
                nameof(FirstName),
                nameof(MiddleName),
                nameof(LastName),
                nameof(PhoneNumber),
                nameof(Address),
                nameof(EmploymentDate),
                nameof(SelectedEmployeesPosition)
            };

            Messenger.Default.Register<SendViewModelParamArgs<Employee>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(EmployeeDetailsViewModel))
                {
                    return;
                }

                AllowValidation = false;

                ParentView = args.ParentView;
                ParentViewModelName = args.ParentViewModelName;

                Employee = args.Parameter ?? new Employee();
                SelectedEmployeesPosition = EmployeesPositions.Find(r => r.Id == Employee.EmployeesPositionId);

                RaisePropertyChanged(() => FirstName);
                RaisePropertyChanged(() => MiddleName);
                RaisePropertyChanged(() => LastName);
                RaisePropertyChanged(() => Address);
                RaisePropertyChanged(() => PhoneNumber);
                RaisePropertyChanged(() => EmploymentDate);
                RaisePropertyChanged(() => SelectedEmployeesPosition);
            });
        }

        public Employee Employee { get; set; }

        public EmployeesPosition SelectedEmployeesPosition { get; set; }

        public List<EmployeesPosition> EmployeesPositions { get; set; }

        public bool CanUpdateEmployee { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        public string FirstName
        {
            get { return Employee?.FirstName; }
            set { Employee.FirstName = value; }
        }

        public string LastName
        {
            get { return Employee?.LastName; }
            set { Employee.LastName = value; }
        }

        public string MiddleName
        {
            get { return Employee?.MiddleName; }
            set { Employee.MiddleName = value; }
        }

        public string Address
        {
            get { return Employee?.Address; }
            set { Employee.Address = value; }
        }

        public string PhoneNumber
        {
            get { return Employee?.PhoneNumber; }
            set { Employee.PhoneNumber = value; }
        }

        public DateTime EmploymentDate { get; set; }

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(FirstName) && string.IsNullOrWhiteSpace(FirstName))
            {
                return "FirstName is required";
            }
            if (propertyName == nameof(MiddleName) && string.IsNullOrWhiteSpace(MiddleName))
            {
                return "MiddleName is required";
            }
            if (propertyName == nameof(LastName) && string.IsNullOrWhiteSpace(LastName))
            {
                return "LastName is required";
            }
            if (propertyName == nameof(Address) && string.IsNullOrWhiteSpace(Address))
            {
                return "Address is required";
            }
            if (propertyName == nameof(PhoneNumber) && string.IsNullOrWhiteSpace(PhoneNumber))
            {
                return "PhoneNumber is required";
            }
            if (propertyName == nameof(SelectedEmployeesPosition) && SelectedEmployeesPosition == null)
            {
                return "Position is required";
            }
            return null;
        }

        private void SaveCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }

            Employee.EmployeesPosition = SelectedEmployeesPosition;
            Employee.EmployeesPositionId = SelectedEmployeesPosition.Id;

            DbScope.UpdateEmployee(Employee);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(ParentView, ParentViewModelName));
        }

        private void LoadedCommandExecute()
        {
            EmploymentDate = Employee.Id == 0 ? DateTime.Now.Date : Employee.EmploymentDate;
            RaisePropertyChanged(() => EmploymentDate);

            AllowValidation = false;

            CanUpdateEmployee = authorizationService.CheckAccess(typeof(Employee), ResourceAction.Update);
            RaisePropertyChanged(() => CanUpdateEmployee);
        }
    }
}
