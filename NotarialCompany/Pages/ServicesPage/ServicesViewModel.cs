using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServicesViewModel : BasePageViewModel
    {
        private List<Service> services;

        public ServicesViewModel(DbScope dbScope) : base(dbScope)
        {
        }

        public List<Service> Services
        {
            get { return services; }
            set { Set(ref services, value); }
        }

        public Service SelectedServise { get; set; }

        protected override void LoadedCommandExecute()
        {
            Services = DbScope.GetServices();
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ServiceDetailsView(), nameof(ServiceDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Service>(new ServicesView(), nameof(ServicesViewModel),
                nameof(ServiceDetailsViewModel), SelectedServise));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new ServiceDetailsView(), nameof(ServiceDetailsViewModel)));
            Messenger.Default.Send(new SendViewModelParamArgs<Service>(new ServicesView(), nameof(ServicesViewModel),
                nameof(ServiceDetailsViewModel), new Service()));
        }
    }
}
