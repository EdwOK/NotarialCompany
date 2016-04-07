using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.DataAccess;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServicesViewModel : ViewModelBase
    {
        private readonly DbScope dbScope;
        private List<Service> services;

        public ServicesViewModel(DbScope dbScope)
        {
            this.dbScope = dbScope;
            OpenDetailsViewCommand = new RelayCommand(OpenDetailsViewCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);
            AddNewItemCommand = new RelayCommand(AddNewItemCommandExecute);
        }

        public List<Service> Services
        {
            get { return services; }
            set { Set(ref services, value); }
        }

        public Service SelectedServise { get; set; }

        public ICommand OpenDetailsViewCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand AddNewItemCommand { get; set; }

        private void LoadedCommandExecute()
        {
            Services = dbScope.GetServices();
        }

        private void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ServiceDetailsView(), nameof(ServiceDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Service>(new ServicesView(), nameof(ServicesViewModel),
                nameof(ServiceDetailsViewModel), SelectedServise));
        }

        private void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ServiceDetailsView(), nameof(ServiceDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Service>(new ServicesView(), nameof(ServicesViewModel),
                nameof(ServiceDetailsViewModel), new Service()));
        }
    }
}
