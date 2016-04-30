using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NotarialCompany.DataAccess;

namespace NotarialCompany.Common
{
    public abstract class BasePageViewModel : ViewModelBase
    {
        protected readonly DbScope DbScope;

        protected BasePageViewModel(DbScope dbScope)
        {
            this.DbScope = dbScope;
            OpenDetailsViewCommand = new RelayCommand(OpenDetailsViewCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);
            AddNewItemCommand = new RelayCommand(AddNewItemCommandExecute);
            RemoveItemCommand = new RelayCommand(RemoveItemCommandExecute);
        }

        public ICommand OpenDetailsViewCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand AddNewItemCommand { get; set; }

        public ICommand RemoveItemCommand { get; set; }

        protected abstract void LoadedCommandExecute();

        protected abstract void OpenDetailsViewCommandExecute();

        protected abstract void AddNewItemCommandExecute();

        protected abstract void RemoveItemCommandExecute();
    }
}
