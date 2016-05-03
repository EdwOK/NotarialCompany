using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;
using NotarialCompany.Pages.UsersPage;
using NotarialCompany.Utilities;

namespace NotarialCompany.Pages.DealsPage
{
    public class DealsViewModel : BasePageViewModel
    {
        public DealsViewModel(DbScope dbScope) 
            : base(dbScope)
        {
        }

        public IList<Deal> Deals { get; set; }

        public Deal SelectedDeal { get; set; }

        private ICollectionView dealsViews;
        public ICollectionView DealsViews
        {
            get { return dealsViews; }
            set { Set(ref dealsViews, value); }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                Set(ref searchText, value);
                DealsViews?.Refresh();
            }
        }

        protected override void LoadedCommandExecute()
        {
            Deals = DbScope.GetDeals();
            DealsViews = CollectionViewSource.GetDefaultView(Deals);
            DealsViews.Filter = Filter;
        }

        protected override void OpenDetailsViewCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new DealDetailsView(), nameof(DealDetailsView)));
            Messenger.Default.Send(new SendViewModelParamArgs<Deal>(new DealsView(), nameof(DealsViewModel),
                nameof(DealDetailsViewModel), SelectedDeal));
        }

        protected override void AddNewItemCommandExecute()
        {
            Messenger.Default.Send(new OpenViewArgs(new DealDetailsView(), nameof(DealDetailsView)));
            Messenger.Default.Send(new SendViewModelParamArgs<Deal>(new DealsView(), nameof(DealsViewModel),
                nameof(DealDetailsViewModel), new Deal {Bill = new Bill(), ServiceIds = new List<int>() }));
        }

        protected override void RemoveItemCommandExecute()
        {
            throw new NotImplementedException();
        }

        private bool Filter(object obj)
        {
            var data = obj as Deal;
            if (data == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                return data.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Employee.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                       || data.Client.SecondName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}
