using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using MaxToolsUi.ViewModels;

namespace MaxToolsUi.Controls
{
    /// <summary>
    /// Interaction logic for PropertyEntry.xaml
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class PropertyEntryUC : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static readonly DependencyProperty GuidProperty
            = DependencyProperty.Register(nameof(Guid), typeof(string), typeof(PropertyEntryUC));

        public string Guid
        {
            get => (string)GetValue(GuidProperty);
            set => SetValue(GuidProperty, value);
        }

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

        public static readonly DependencyProperty ValueChangedCommandProperty
            = DependencyProperty.Register(nameof(ValueChangedCommand), typeof(ICommand), typeof(PropertyEntryUC));

        public ICommand ValueChangedCommand
        {
            get => (ICommand)GetValue(ValueChangedCommandProperty);
            set => SetValue(ValueChangedCommandProperty, value);
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public PropertyEntryUC()
        {
            InitializeComponent();
        }

        private void PropertyEntryUC_OnLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("loaded!");
        }

        private void TriggerValueChanged()
            => ValueChangedCommand?.Execute(new MaxToolsWindowViewModel.ValueChangedArgs(Guid, CandidateValues[ComboBox.SelectedIndex]));

        private void ComboBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;

            var comboBox = (ComboBox)sender;

            var text = comboBox.Text.Trim();
            
            var index = comboBox.SelectedIndex;
            var previous = index == -1 ? null : CandidateValues[index];
            if (previous == text)
                return;

            if (!CandidateValues.Contains(text))
                CandidateValues.Add(text);

            // Note: this triggers ComboBox_OnSelectionChanged
            comboBox.SelectedIndex = CandidateValues.IndexOf(text);
        }

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            var comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex == -1)
                return;

            TriggerValueChanged();
        }
    }
}
