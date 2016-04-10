using System.Windows;
using System.Windows.Controls;

namespace NotarialCompany.Controls
{
    public class DropdownButtonControl : Button
    {
        public DropdownButtonControl()
        {
            Click += DropdownButtonControl_Click;
        }

        private void DropdownButtonControl_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            button.ContextMenu.IsEnabled = true;
            button.ContextMenu.PlacementTarget = (Button)sender;
            button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            button.ContextMenu.IsOpen = true;
        }
    }
}
