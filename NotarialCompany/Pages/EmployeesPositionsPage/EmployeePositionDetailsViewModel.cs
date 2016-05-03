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

namespace NotarialCompany.Pages.EmployeesPositionsPage
{
    public class EmployeePositionDetailsViewModel : ValidationViewModel
    {
        private readonly IAuthorizationService authorizationService;

        private MetroContentControl parentView;
        private string parentViewModelName;

        public EmployeePositionDetailsViewModel(DbScope dbScope, IAuthorizationService authorizationService) : base(dbScope)
        {
            this.authorizationService = authorizationService;

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);

            ValidatingProperties = new List<string>
            {
                nameof(Position),
                nameof(Salary),
                nameof(Commission)
            };

            Messenger.Default.Register<SendViewModelParamArgs<EmployeesPosition>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(EmployeesPositionsViewModel))
                {
                    return;
                }

                AllowValidation = false;

                parentView = args.ParentView;
                parentViewModelName = args.ParentViewModelName;

                EmployeesPosition = args.Parameter ?? new EmployeesPosition();

                RaisePropertyChanged(() => Position);
                RaisePropertyChanged(() => Salary);
                RaisePropertyChanged(() => Commission);
            });
        }

        public EmployeesPosition EmployeesPosition { get; set; }

        public bool CanUpdateEmployeesPosition { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        public string Position
        {
            get { return EmployeesPosition?.Position; }
            set { EmployeesPosition.Position = value; }
        }

        public decimal Salary
        {
            get { return EmployeesPosition?.Salary ?? 0; }
            set { EmployeesPosition.Salary = value; }
        }

        public int Commission
        {
            get { return EmployeesPosition?.Commission ?? 0; }
            set { EmployeesPosition.Commission = value; }
        }

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(Position) && string.IsNullOrWhiteSpace(Position))
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

            DbScope.UpdateEmployeesPosition(EmployeesPosition);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }

        private void LoadedCommandExecute()
        {
            CanUpdateEmployeesPosition = authorizationService.CheckAccess(typeof(EmployeesPosition), ResourceAction.Update);
            RaisePropertyChanged(() => CanUpdateEmployeesPosition);
        }
    }
}
