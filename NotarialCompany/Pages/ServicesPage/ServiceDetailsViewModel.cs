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

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServiceDetailsViewModel : ValidationViewModel
    {
        private readonly DbScope dbScope;
        private readonly IAuthorizationService authorizationService;

        private MetroContentControl parentView;
        private string parentViewModelName;

        public ServiceDetailsViewModel(DbScope dbScope, IAuthorizationService authorizationService)
        {
            this.dbScope = dbScope;
            this.authorizationService = authorizationService;

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);

            ValidatingProperties = new List<string> {nameof(Name), nameof(Description)};

            Messenger.Default.Register<SendViewModelParamArgs<Service>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(ServiceDetailsViewModel))
                {
                    return;
                }
                AllowValidation = false;

                parentView = args.ParentView;
                parentViewModelName = args.ParentViewModelName;

                Service = args.Parameter ?? new Service();

                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => Description);
                RaisePropertyChanged(() => Cost);
            });
        }

        public Service Service { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        public string Name
        {
            get { return Service?.Name; }
            set { Service.Name = value; }
        }

        public string Description
        {
            get { return Service?.Description; }
            set { Service.Description = value; }
        }

        public decimal Cost
        {
            get { return Service?.Cost ?? 0; }
            set { Service.Cost = value; }
        }

        public bool CanUpdateService { get; set; }

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(Name) && string.IsNullOrWhiteSpace(Name))
            {
                return "Name is required";
            }
            if (propertyName == nameof(Description) && string.IsNullOrWhiteSpace(Description))
            {
                return "Description is required";
            }
            return null;
        }

        private void SaveCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }
            dbScope.UpdateService(Service);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }

        private void LoadedCommandExecute()
        {
            CanUpdateService = authorizationService.CheckAccess(typeof (Service), ResourceAction.Update);
            RaisePropertyChanged(nameof(CanUpdateService));
        }
    }
}
