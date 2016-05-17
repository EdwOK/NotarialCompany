using MahApps.Metro.Controls;

namespace NotarialCompany.Common.MessagesArgs
{
    public class OpenViewArgs
    {
        public OpenViewArgs(MetroContentControl view, string viewModelName)
        {
            View = view;
            ViewModelName = viewModelName;
        }

        public MetroContentControl View { get; private set; }

        public string ViewModelName { get; private set; }
    }
}
