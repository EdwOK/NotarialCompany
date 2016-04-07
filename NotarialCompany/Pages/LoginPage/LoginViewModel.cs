using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Security;

namespace NotarialCompany.Pages.LoginPage
{
    public class LoginViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IAuthenticationService authenticationService;

        private string errorMessage;

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;

            LoginCommand = new RelayCommand(LoginCommandExecute);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { Set(ref errorMessage, value); }
        }

        public bool HasError => ErrorMessage != null; 

        public ICommand LoginCommand { get; set; }

        private void LoginCommandExecute()
        {
            if (!authenticationService.ValidatePassword(Login, Password))
            {
                ErrorMessage = "Username or Password is incorrect";
                return;
            }
            Messenger.Default.Send(new OpenViewArgs(new ServicesView(), nameof(ServicesViewModel)));
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Login) && string.IsNullOrWhiteSpace(Login))
                {
                    return "Login is required";
                }
                if (columnName == nameof(Password) && string.IsNullOrWhiteSpace(Password))
                {
                    return "Password is required";
                }
                return string.Empty;
            }
        }

        public string Error => string.Empty;
    }
}
