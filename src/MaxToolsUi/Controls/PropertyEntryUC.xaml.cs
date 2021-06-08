using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaxToolsUi.Controls
{
    /// <summary>
    /// Interaction logic for PropertyEntry.xaml
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class PropertyEntryUC : UserControl
    {
        public static readonly DependencyProperty EntryNameProperty
            = DependencyProperty.Register(nameof(EntryName), typeof(string), typeof(PropertyEntryUC));

        public string EntryName
        {
            get => (string)GetValue(EntryNameProperty);
            set => SetValue(EntryNameProperty, value);
        }

        public static readonly DependencyProperty CandidateValuesProperty
            = DependencyProperty.Register(nameof(CandidateValues), typeof(ObservableCollection<string>), typeof(PropertyEntryUC));

        public ObservableCollection<string> CandidateValues
        {
            get => (ObservableCollection<string>)GetValue(CandidateValuesProperty);
            set => SetValue(CandidateValuesProperty, value);
        }

        public static readonly DependencyProperty RemoveCommandProperty
            = DependencyProperty.Register(nameof(RemoveCommand), typeof(ICommand), typeof(PropertyEntryUC));

        public ICommand RemoveCommand
        {
            get => (ICommand)GetValue(RemoveCommandProperty);
            set => SetValue(RemoveCommandProperty, value);
        }

        public static readonly DependencyProperty SelectCommandProperty
            = DependencyProperty.Register(nameof(SelectCommand), typeof(ICommand), typeof(PropertyEntryUC));

        public ICommand SelectCommand
        {
            get => (ICommand)GetValue(SelectCommandProperty);
            set => SetValue(SelectCommandProperty, value);
        }

        public static readonly DependencyProperty AddToSelectionCommandProperty
            = DependencyProperty.Register(nameof(AddToSelectionCommand), typeof(ICommand), typeof(PropertyEntryUC));

        public ICommand AddToSelectionCommand
        {
            get => (ICommand)GetValue(AddToSelectionCommandProperty);
            set => SetValue(AddToSelectionCommandProperty, value);
        }


        public PropertyEntryUC()
        {
            InitializeComponent();
        }
    }
}
