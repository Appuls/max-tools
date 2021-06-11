using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;

namespace MaxToolsUi.Controls
{
    /// <summary>
    /// Interaction logic for AddPropertyEntryUC.xaml
    /// </summary>
    public partial class AddPropertyEntryUC : UserControl
    {
        public class AddCommandEventArgs
        {
            public readonly string Name;
            public readonly string Value;

            public AddCommandEventArgs(string name, string value)
                => (Name, Value) = (name, value);
        }

        public static readonly DependencyProperty AddCommandProperty
            = DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(AddPropertyEntryUC));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        private DelegateCommand _addPropertyEntryCommandInternal;

        public DelegateCommand AddPropertyEntryCommandInternal
            => _addPropertyEntryCommandInternal ?? (_addPropertyEntryCommandInternal = new DelegateCommand(AddPropertyEntryCommandInternalExecute));

        public void AddPropertyEntryCommandInternalExecute()
        {
            AddCommand.Execute(new AddCommandEventArgs(TextBoxName.Text, TextBoxValue.Text));
            TextBoxName.Text = "";
            TextBoxValue.Text = "";
        }

        public AddPropertyEntryUC()
        {
            InitializeComponent();
        }
    }
}
