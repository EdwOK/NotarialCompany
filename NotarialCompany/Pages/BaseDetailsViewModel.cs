using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.DataAccess;
using NotarialCompany.MessagesArgs;

namespace NotarialCompany.Pages
{
    public abstract class BaseDetailsViewModel : ViewModelBase
    {
        protected readonly DbScope DbScope;

        protected ContentControl ParentView;
        protected string ParentViewModelName;

        protected BaseDetailsViewModel(DbScope dbScope)
        {
            this.DbScope = dbScope;
            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
        }

        protected virtual void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(ParentView, ParentViewModelName));
        }

        protected virtual void SaveCommandExecute()
        {
            NavigateBackCommandExecute();
        }

        public RelayCommand NavigateBackCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }
    }
}
