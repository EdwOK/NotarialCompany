using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ServicesPage
{
    public class ServicesViewModel : ViewModelBase
    {
        private readonly DbScope dbScope;

        public ServicesViewModel(DbScope dbScope)
        {
            OpenDetailsCommand = new RelayCommand(OpenDetailsCommandExecute);

            this.dbScope = dbScope;
            Services = dbScope.GetServices();
        }

        public List<Service> Services { get; set; }

        public Service SelectedServise { get; set; }

        public ICommand OpenDetailsCommand { get; set; }

        private void OpenDetailsCommandExecute()
        {
            var a = 5;
        }
    }
}
