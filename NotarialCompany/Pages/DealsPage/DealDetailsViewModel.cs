using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Pages.DealsPage
{
    public class DealDetailsViewModel : ValidationViewModel
    {
        private decimal basePrice;

        private Client selectedClient;

        private Employee selectedEmployee;

        private Dictionary<string, object> selectedServices;

        private Dictionary<string, object> services;

        private decimal totalPrice;

        public DealDetailsViewModel(DbScope dbScope) : base(dbScope)
        {
            SaveCommand = new RelayCommand(SaveCommandExecute);
            NavigateBackCommand = new RelayCommand(NavigateBackCommandExecute);
            LoadedCommand = new RelayCommand(LoadedCommandExecute);

            ValidatingProperties = new List<string>
            {
                nameof(Description),
                nameof(SelectedClient),
                nameof(SelectedEmployee),
                nameof(SelectedServices)
            };

            Messenger.Default.Register<SendViewModelParamArgs<Deal>>(this, args =>
            {
                if (args.ChildViewModelName != nameof(DealDetailsViewModel))
                {
                    return;
                }

                AllowValidation = false;

                ParentView = args.ParentView;
                ParentViewModelName = args.ParentViewModelName;

                Deal = args.Parameter ?? new Deal { Bill = new Bill(), ServiceIds = new List<int>() };
            });
        }

        public ICommand SaveCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }
        public ICommand LoadedCommand { set; get; }

        public Dictionary<string, object> Services
        {
            get { return services; }
            set { Set(ref services, value); }
        }

        public Dictionary<string, object> SelectedServices
        {
            get { return selectedServices; }
            set
            {
                Set(ref selectedServices, value);
                CalculatePrices();
            }
        }

        public Deal Deal { get; set; }

        public string Description
        {
            get { return Deal?.Description; }
            set { Deal.Description = value; }
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                Set(ref selectedEmployee, value);
                CalculatePrices();
            }
        }

        public List<Employee> Employees { get; set; }

        public Client SelectedClient
        {
            get { return selectedClient; }
            set { Set(ref selectedClient, value); }
        }

        public List<Client> Clients { get; set; }

        public decimal TotalPrice
        {
            get { return totalPrice; }
            set { Set(ref totalPrice, value); }
        }

        public decimal BasePrice
        {
            get { return basePrice; }
            set { Set(ref basePrice, value); }
        }

        public DateTime BillDate { get; set; }

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(Description) && string.IsNullOrWhiteSpace(Description))
            {
                return "Description is required";
            }
            if (propertyName == nameof(SelectedClient) && SelectedClient == null)
            {
                return "Client is required";
            }
            if (propertyName == nameof(SelectedEmployee) && SelectedEmployee == null)
            {
                return "Employee is required";
            }
            if (propertyName == nameof(SelectedServices) && SelectedServices?.Count == 0)
            {
                return "Services is required";
            }
            return null;
        }

        private void LoadedCommandExecute()
        {
            Employees = DbScope.GetEmployees();
            Clients = DbScope.GetClients();

            SelectedEmployee = Employees.Find(e => e.Id == Deal.EmployeeId);
            SelectedClient = Clients.Find(e => e.Id == Deal.ClientId);

            RaisePropertyChanged(() => Clients);
            RaisePropertyChanged(() => Employees);

            TotalPrice = Deal.Bill.TotalPrice;
            BasePrice = Deal.Bill.BasePrice;

            BillDate = Deal.Id == 0 ? DateTime.Now.Date : Deal.Bill.DateTime;

            RaisePropertyChanged(() => Description);
            RaisePropertyChanged(() => BillDate);

            AllowValidation = false;

            var servicesList = DbScope.GetServices().ToDictionary(s => s.Name, s => (object) s);
            Services = new Dictionary<string, object>(servicesList);

            SelectedServices = new Dictionary<string, object>(
                Services.Where(s => Deal.ServiceIds.Contains(((Service) s.Value).Id))
                    .ToDictionary(x => x.Key, x => x.Value));
        }

        private void SaveCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }
            UpdateDealModel();
            DbScope.CreateOrUpdateDeal(Deal);
            NavigateBackCommandExecute();
        }

        private void NavigateBackCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(ParentView, ParentViewModelName));
        }

        private void CalculatePrices()
        {
            if (selectedServices == null)
            {
                return;
            }

            BasePrice = selectedServices.Sum(p => ((Service) p.Value).Cost);
            if (SelectedEmployee != null)
            {
                TotalPrice = BasePrice/100*SelectedEmployee.EmployeesPosition.Commission + BasePrice;
            }
        }

        private void UpdateDealModel()
        {
            Deal.ClientId = SelectedClient.Id;
            Deal.EmployeeId = SelectedEmployee.Id;
            Deal.ServiceIds = SelectedServices.Values.Select(s => ((Service) s).Id).ToList();

            Deal.Bill.DateTime = BillDate;
            Deal.Bill.BasePrice = BasePrice;
            Deal.Bill.TotalPrice = TotalPrice;
        }
    }
}