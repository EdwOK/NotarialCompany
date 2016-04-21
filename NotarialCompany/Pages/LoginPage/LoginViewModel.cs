using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using NotarialCompany.Common;
using NotarialCompany.Common.MessagesArgs;
using NotarialCompany.Pages.ServicesPage;
using NotarialCompany.Security;

namespace NotarialCompany.Pages.LoginPage
{
    public class LoginViewModel : ValidationViewModel
    {
        private readonly IAuthenticationService authenticationService;

        private readonly IDialogCoordinator dialogCoordinator;

        public LoginViewModel(IAuthenticationService authenticationService, IDialogCoordinator dialogCoordinator)
        {
            this.authenticationService = authenticationService;
            this.dialogCoordinator = dialogCoordinator;

            ValidatingProperties = new List<string> {nameof(Login), nameof(Password)};

            LoginCommand = new RelayCommand(LoginCommandExecute);
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }

        private void LoginCommandExecute()
        {
            if (EnableValidationAndGetError() != null)
            {
                return;
            }

            AllowValidation = false;

            var status = authenticationService.ValidatePassword(Login, Password);
            if (!status)
            {
                dialogCoordinator.ShowMessageAsync(this, "Authorization", "Username or Password is incorrect!");
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
