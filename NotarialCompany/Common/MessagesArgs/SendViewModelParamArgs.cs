using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace NotarialCompany.Common.MessagesArgs
{
    public class SendViewModelParamArgs<T>
    {
        public SendViewModelParamArgs(MetroContentControl parentView, string parentViewModelName, string childViewModelName, T parameter)
        {
            ParentView = parentView;
            ChildViewModelName = childViewModelName;
            Parameter = parameter;
            ParentViewModelName = parentViewModelName;
        }

        public MetroContentControl ParentView { get; private set; }

        public string ParentViewModelName { get; private set; }

        public string ChildViewModelName { get; private set; }

        public T Parameter { get; private set; }
    }
}
