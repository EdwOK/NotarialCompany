using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.DataAccess;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Models;
using NotarialCompany.Pages.ServicesPage;

namespace NotarialCompany.Pages.ClientsPage
{
    public class ClientDetailsViewModel : BaseDetailsViewModel, IDataErrorInfo
    {
        public ClientDetailsViewModel(DbScope dbScope) : base(dbScope)
        {
            Messenger.Default.Register<SendViewModelParamArgs<Client>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(ClientDetailsViewModel))
                {
                    return;
                }
                ParentView = args.ParentView;
                ParentViewModelName = args.ParentViewModelName;

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

        protected override void SaveCommandExecute()
        {
            DbScope.UpdateClient(Client);
            base.SaveCommandExecute();
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FirstName) && string.IsNullOrWhiteSpace(FirstName))
                {
                    return "FirstName is required";
                }
                if (columnName == nameof(SecondName) && string.IsNullOrWhiteSpace(SecondName))
                {
                    return "SecondName is required";
                }
                if (columnName == nameof(Occupation) && string.IsNullOrWhiteSpace(Occupation))
                {
                    return "Occupation is required";
                }
                if (columnName == nameof(PhoneNumber) && string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    return "PhoneNumber is required";
                }
                return string.Empty;
            }
        }

        public string Error => string.Empty;
    }
}
