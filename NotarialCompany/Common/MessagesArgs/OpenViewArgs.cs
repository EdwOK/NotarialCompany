using System.Windows.Controls;

namespace NotarialCompany.MessagesArgs
{
    public class OpenViewArgs
    {
        public OpenViewArgs(ContentControl view, string viewModelName)
        {
            View = view;
            ViewModelName = viewModelName;
        }

        public ContentControl View { get; private set; }

        public string ViewModelName { get; private set; }
    }
}
