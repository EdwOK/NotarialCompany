using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.DataAccess;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServiceDetailsViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly DbScope dbScope;

        private ContentControl parentView;
        private string parentViewModelName;

        public ServiceDetailsViewModel(DbScope dbScope)
        {
            this.dbScope = dbScope;

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);

            Messenger.Default.Register<SendViewModelParamArgs<Service>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(ServiceDetailsViewModel))
                {
                    return;
                }
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

        private void SaveCommandExecute()
        {
            dbScope.UpdateService(Service);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Name) && string.IsNullOrWhiteSpace(Name))
                {
                    return "Name is required";
                }
                if (columnName == nameof(Description) && string.IsNullOrWhiteSpace(Description))
                {
                    return "Description is required";
                }
                return string.Empty;
            }
        }

        public string Error => string.Empty;
    }
}
