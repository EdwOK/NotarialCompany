using System;
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
    public class ServiceDetailsViewModel : ViewModelBase
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
                RaisePropertyChanged(() => Service);
            });
        }

        public Service Service { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }

        private void SaveCommandExecute()
        {
            dbScope.UpdateService(Service);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }
    }
}
