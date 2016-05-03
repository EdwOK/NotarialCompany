using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Security.Authorization;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientDetailsViewModel : ValidationViewModel
    {
        private readonly IAuthorizationService authorizationService;

        private MetroContentControl parentView;
        private string parentViewModelName;

        public ClientDetailsViewModel(DbScope dbScope, IAuthorizationService authorizationService) : base(dbScope)
        {
            this.authorizationService = authorizationService;

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);

            ValidatingProperties = new List<string>
            {
                nameof(FirstName),
                nameof(SecondName),
                nameof(MiddleName),
                nameof(Occupation),
                nameof(Address),
                nameof(PhoneNumber)
            };

            Messenger.Default.Register<SendViewModelParamArgs<Client>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(ClientDetailsViewModel))
                {
                    return;
                }
                AllowValidation = false;

                parentView = args.ParentView;
                parentViewModelName = args.ParentViewModelName;

                Client = args.Parameter ?? new Client();

                RaisePropertyChanged(() => FirstName);
                RaisePropertyChanged(() => SecondName);
                RaisePropertyChanged(() => MiddleName);
                RaisePropertyChanged(() => Occupation);
                RaisePropertyChanged(() => Address);
                RaisePropertyChanged(() => PhoneNumber);
            });
        }

        public Client Client { get; set; }

        public bool CanUpdateClient { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        public ICommand LoadedCommand { get; set; }

        public string FirstName
        {
            get { return Client?.FirstName; }
            set { Client.FirstName = value; }
        }

        public string SecondName
        {
            get { return Client?.SecondName; }
            set { Client.SecondName = value; }
        }

        public string MiddleName
        {
            get { return Client?.MiddleName; }
            set { Client.MiddleName = value; }
        }

        public string Occupation
        {
            get { return Client?.Occupation; }
            set { Client.Occupation = value; }
        }

        public string Address
        {
            get { return Client?.Address; }
            set { Client.Address = value; }
        }

        public string PhoneNumber
        {
            get { return Client?.PhoneNumber; }
            set { Client.PhoneNumber = value; }
        }

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(FirstName) && string.IsNullOrWhiteSpace(FirstName))
            {
                return "FirstName is required";
            }
            if (propertyName == nameof(SecondName) && string.IsNullOrWhiteSpace(SecondName))
            {
                return "SecondName is required";
            }
            if (propertyName == nameof(MiddleName) && string.IsNullOrWhiteSpace(MiddleName))
            {
                return "MiddleName is required";
            }
            if (propertyName == nameof(Occupation) && string.IsNullOrWhiteSpace(Occupation))
            {
                return "Occupation is required";
            }
            if (propertyName == nameof(Address) && string.IsNullOrWhiteSpace(Address))
            {
                return "Address is required";
            }
            if (propertyName == nameof(PhoneNumber) && string.IsNullOrWhiteSpace(PhoneNumber))
            {
                return "PhoneNumber is required";
            }
            return null;
        }

        private void SaveCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }

            DbScope.UpdateClient(Client);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }

        private void LoadedCommandExecute()
        {
            CanUpdateClient = authorizationService.CheckAccess(typeof(Client), ResourceAction.Update);
            RaisePropertyChanged(() => CanUpdateClient);
        }
    }
}
