using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;

namespace MaxToolsUi.Controls
{
    /// <summary>
    /// Interaction logic for UnassignedPropertyUC.xaml
    /// </summary>
    public partial class UnassignedPropertyUC : UserControl
    {
        public static readonly DependencyProperty SelectCommandProperty
            = DependencyProperty.Register(nameof(SelectCommand), typeof(ICommand), typeof(UnassignedPropertyUC));

        public ICommand SelectCommand
        {
            get => (ICommand)GetValue(SelectCommandProperty);
            set => SetValue(SelectCommandProperty, value);
        }

        private DelegateCommand<object> _innerSelectCommand;

        public DelegateCommand<object> InnerSelectCommand
            => _innerSelectCommand ?? (_innerSelectCommand = new DelegateCommand<object>(InnerSelectCommandExecute));

        public void InnerSelectCommandExecute(object addObj)
        {
            var add = addObj != null && addObj is bool b && b;
            SelectCommand.Execute(add);
        }

        public UnassignedPropertyUC()
        {
            InitializeComponent();
        }
    }
}
