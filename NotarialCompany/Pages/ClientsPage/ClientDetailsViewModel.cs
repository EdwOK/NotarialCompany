using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.DataAccess;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientDetailsViewModel : ValidationViewModel
    {
        private readonly DbScope dbScope;

        private ContentControl parentView;
        private string parentViewModelName;

        public ClientDetailsViewModel(DbScope dbScope) 
        {
            this.dbScope = dbScope;

            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);

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

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }

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
            dbScope.UpdateClient(Client);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(parentView, parentViewModelName));
        }
    }
}
