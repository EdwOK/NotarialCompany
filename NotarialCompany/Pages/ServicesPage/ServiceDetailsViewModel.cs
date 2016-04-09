using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServiceDetailsViewModel : ValidationViewModel
    {
        private readonly DbScope dbScope;

        private ContentControl parentView;
        private string parentViewModelName;

        public ServiceDetailsViewModel(DbScope dbScope)
        {
            this.dbScope = dbScope;

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);

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
    }
}
