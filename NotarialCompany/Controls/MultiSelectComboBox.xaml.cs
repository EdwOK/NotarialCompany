using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NotarialCompany.Controls
{
    /// <summary>
    ///     Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private readonly ObservableCollection<Node> nodeList;

        public MultiSelectComboBox()
        {
            InitializeComponent();
            nodeList = new ObservableCollection<Node>();
        }

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof (Dictionary<string, object>), typeof (MultiSelectComboBox),
                new FrameworkPropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof (Dictionary<string, object>),
                typeof (MultiSelectComboBox),
                new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (MultiSelectComboBox),
                new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof (string), typeof (MultiSelectComboBox),
                new UIPropertyMetadata(string.Empty));


        public Dictionary<string, object> ItemsSource
        {
            get { return (Dictionary<string, object>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Dictionary<string, object> SelectedItems
        {
            get { return (Dictionary<string, object>) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string) GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        #endregion

        #region Events

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSelectComboBox) d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSelectComboBox) d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var clickedBox = (CheckBox) sender;

            if (ReferenceEquals(clickedBox.Content, "All"))
            {
                if (clickedBox.IsChecked.HasValue)
                {
                    foreach (var node in nodeList)
                    {
                        node.IsSelected = true;
                    }
                }
                else
                {
                    foreach (var node in nodeList)
                    {
                        node.IsSelected = false;
                    }
                }
            }
            else
            {
                var selectedCount = nodeList.Count(s => s.IsSelected && s.Title != "All");
                var node = nodeList.FirstOrDefault(i => i.Title == "All");
                if (node != null)
                {
                    node.IsSelected = selectedCount == nodeList.Count - 1;
                }
            }
            SetSelectedItems();
            SetText();
        }

        #endregion

        #region Methods

        private void SelectNodes()
        {
            foreach (var keyValue in SelectedItems)
            {
                var node = nodeList.FirstOrDefault(i => i.Title == keyValue.Key);
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
        }

        private void SetSelectedItems()
        {
            if (SelectedItems == null)
            {
                SelectedItems = new Dictionary<string, object>();
            }

            SelectedItems.Clear();

            var newSelectedItems = new Dictionary<string, object>();

            foreach (var node in nodeList)
            {
                if (node.IsSelected && node.Title != "All")
                {
                    if (ItemsSource.Count > 0)
                    {
                        newSelectedItems.Add(node.Title, ItemsSource[node.Title]);
                    }
                }
            }

            SelectedItems = newSelectedItems;
        }

        private void DisplayInControl()
        {
            nodeList.Clear();
            if (ItemsSource.Count > 0)
            {
                nodeList.Add(new Node("All"));
            }

            foreach (var keyValue in ItemsSource)
            {
                var node = new Node(keyValue.Key);
                nodeList.Add(node);
            }
            MultiSelectCombo.ItemsSource = nodeList;
        }

        private void SetText()
        {
            if (SelectedItems != null)
            {
                var displayText = new StringBuilder();
                foreach (var node in nodeList)
                {
                    if (node.IsSelected && node.Title == "All")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("All");
                        break;
                    }
                    if (node.IsSelected && node.Title != "All")
                    {
                        displayText.Append(node.Title);
                        displayText.Append(',');
                    }
                }
                Text = displayText.ToString().TrimEnd(',');
            }

            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(Text))
            {
                Text = DefaultText;
            }
        }

        #endregion
    }

    public class Node : INotifyPropertyChanged
    {
        private bool isSelected;
        private string title;

        #region Ctor

        public Node(string title)
        {
            Title = title;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        #endregion
    }
}