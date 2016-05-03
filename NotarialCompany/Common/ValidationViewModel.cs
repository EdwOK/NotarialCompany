using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using NotarialCompany.DataAccess;

namespace NotarialCompany.Common
{
    public class ValidationViewModel : ViewModelBase, IDataErrorInfo
    {
        protected bool AllowValidation;

        protected readonly DbScope DbScope;

        protected MetroContentControl ParentView;
        protected string ParentViewModelName;

        protected ValidationViewModel(DbScope dbScope) 
        {
            this.DbScope = dbScope;
        }

        public ICollection<string> ValidatingProperties { get; set; }

        public string Error
        {
            get
            {
                if (!AllowValidation)
                {
                    return null;
                }

                var propertiesWithErrors = ValidatingProperties.Where(p => ((IDataErrorInfo) this)[p] != null).ToList();
                if (propertiesWithErrors.Count == 0)
                {
                    return null;
                }

                foreach (var property in propertiesWithErrors)
                {
                    RaisePropertyChanged(property);
                }

                return "error";
            }
        }

        public string this[string propertyName] => AllowValidation ? GetValidationError(propertyName) : null;

        protected string EnableValidationAndGetError()
        {
            AllowValidation = true;
            var error = ((IDataErrorInfo) this).Error;
            return string.IsNullOrEmpty(error) ? null : error;
        }

        protected virtual string GetValidationError(string propertyName)
        {
            return null;
        }
    }
}
