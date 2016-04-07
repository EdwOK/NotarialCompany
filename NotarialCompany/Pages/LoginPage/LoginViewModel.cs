using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NotarialCompany.Common;
using NotarialCompany.MessagesArgs;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Security;

namespace NotarialCompany.Pages.LoginPage
{
    public class LoginViewModel : ValidationViewModel
    {
        private readonly IAuthenticationService authenticationService;

        private string loginErrorMessage;

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;

            ValidatingProperties = new List<string> {nameof(Login), nameof(Password)};

            LoginCommand = new RelayCommand(LoginCommandExecute);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public string LoginErrorMessage
        {
            get { return loginErrorMessage; }
            set { Set(ref loginErrorMessage, value); }
        }

        public bool HasLoginError => LoginErrorMessage != null;

        public ICommand LoginCommand { get; set; }

        private void LoginCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }

            if (!authenticationService.ValidatePassword(Login, Password))
            {
                LoginErrorMessage = "Username or Password is incorrect";
                return;
            }
            Messenger.Default.Send(new OpenViewArgs(new ServicesView(), nameof(ServicesViewModel)));
        }

        protected override string GetValidationError(string propertyName)
        {
            if (propertyName == nameof(Login) && string.IsNullOrWhiteSpace(Login))
            {
                return "Login is required";
            }
            if (propertyName == nameof(Password) && string.IsNullOrWhiteSpace(Password))
            {
                return "Password is required";
            }
            return null;
        }
    }
}
