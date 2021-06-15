using System.Collections.ObjectModel;
using System.Linq;
using MaxToolsUi.Controls;
using MaxToolsUi.Models;
using MaxToolsUi.Services;
using MaxToolsUi.Utilities;
using Prism.Commands;
using Prism.Mvvm;

namespace MaxToolsUi.ViewModels
{
    public class MaxToolsWindowViewModel : BindableBase
    {
        private readonly IMaxToolsService _maxToolsService;
        public ObservableCollection<NodeModel> NodeModels { get; } = new ObservableCollection<NodeModel>();
        public ObservableCollection<PropertyEntry> PropertyEntries { get; } = new ObservableCollection<PropertyEntry>();
        public bool IsStub => _maxToolsService.IsStub;

        public MaxToolsWindowViewModel(IMaxToolsService maxToolsService)
        {
            _maxToolsService = maxToolsService;
            SubscribeToEvents();
        }

        public PropertyEntry GetPropertyEntry(string guid)
            => PropertyEntries.FirstOrDefault(p => p.Guid == guid);

        private DelegateCommand<string> _removeCommand;

        public DelegateCommand<string> RemoveCommand
            => _removeCommand ?? (_removeCommand = new DelegateCommand<string>(RemoveCommandExecute));

        private void RemoveCommandExecute(string guid)
        {
            var propertyEntry = GetPropertyEntry(guid);
            if (propertyEntry == null)
                return;

            PropertyEntries.Remove(propertyEntry);

            foreach (var n in NodeModels)
            {
                n.RemoveProperty(propertyEntry.Name);
            }

            _maxToolsService.ApplyNodeModels();
        }

        public class SelectArgs
        {
            public readonly string Name;
            public readonly string Value;
            public readonly bool Add;

            public SelectArgs(string name, string value, bool add)
            {
                Name = name;
                Value = value;
                Add = add;
            }
        }

        private DelegateCommand<SelectArgs> _selectCommand;

        public DelegateCommand<SelectArgs> SelectCommand
            => _selectCommand ?? (_selectCommand = new DelegateCommand<SelectArgs>(SelectCommandExecute));

        private void SelectCommandExecute(SelectArgs args)
            => _maxToolsService.SelectByProperty(args.Name, args.Value, args.Add);

        private DelegateCommand<bool?> _selectByAbsentPropertiesCommand;

        public DelegateCommand<bool?> SelectByAbsentPropertiesCommand
            => _selectByAbsentPropertiesCommand ?? (_selectByAbsentPropertiesCommand = new DelegateCommand<bool?>(SelectByAbsentPropertiesCommandExecute));

        private void SelectByAbsentPropertiesCommandExecute(bool? add)
            => _maxToolsService.SelectByAbsentProperties(add ?? false);

        public class ValueChangedArgs
        {
            public readonly string Guid;
            public readonly string Value;

            public ValueChangedArgs(string guid, string value)
            {
                Guid = guid;
                Value = value;
            }
        }

        private DelegateCommand<ValueChangedArgs> _valueChangedCommand;

        public DelegateCommand<ValueChangedArgs> ValueChangedCommand
            => _valueChangedCommand ?? (_valueChangedCommand = new DelegateCommand<ValueChangedArgs>(ValueChangedCommandExecute));

        private void ValueChangedCommandExecute(ValueChangedArgs args)
        {
            var guid = args.Guid;
            var value = args.Value;

            var propertyEntry = GetPropertyEntry(guid);
            if (propertyEntry == null)
                return;

            var propInfos = NodeModels
                .SelectMany(n => n.Properties)
                .Where(p => p.Name == propertyEntry.Name);
            foreach (var p in propInfos)
            {
                p.Value = value == PropertyModel.VariesCandidate ? p.OriginalValue : value;
            }

            _maxToolsService.ApplyNodeModels();
        }

        public bool HasNodeModels => NodeModels.Count > 0;

        public bool HasPropertyEntries => PropertyEntries.Count > 0;

        private DelegateCommand<AddPropertyEntryUC.AddCommandEventArgs> _addCommand;

        public DelegateCommand<AddPropertyEntryUC.AddCommandEventArgs> AddCommand
            => _addCommand ?? (_addCommand = new DelegateCommand<AddPropertyEntryUC.AddCommandEventArgs>(AddCommandExecute));

        public void AddCommandExecute(AddPropertyEntryUC.AddCommandEventArgs args)
            => _maxToolsService.AddProperty(args.Name, args.Value);

        private DelegateCommand _refreshSelectionCommand;

        public DelegateCommand RefreshSelectionCommand
            => _refreshSelectionCommand ?? (_refreshSelectionCommand = new DelegateCommand(RefreshSelectionCommandExecute));

        private void RefreshSelectionCommandExecute()
            => _maxToolsService.RefreshSelection();

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            NodeModels.Clear();
            NodeModels.AddRange(args.NodeModels);
            RaisePropertyChanged(nameof(HasNodeModels));

            PropertyEntries.Clear();
            var propEntries = NodeModels
                .SelectMany(n => n.Properties)
                .GroupBy(t => t.Name)
                .Select(g =>
                {
                    var name = g.Key;
                    var candidateValues = g.Select(t => t.Value).Distinct().OrderBy(v => v).ToArray();
                    var isGlobal = g.Count() == NodeModels.Count;
                    return new PropertyEntry(name, candidateValues, isGlobal);
                }).OrderBy(p => p.Name);
            PropertyEntries.AddRange(propEntries);
            RaisePropertyChanged(nameof(HasPropertyEntries));
        }

        public void SubscribeToEvents()
        {
            _maxToolsService.OnSelectionChanged += HandleSelectionChanged;
        }

        private DelegateCommand _exportToCsvCommand;

        public DelegateCommand ExportToCsvCommand
            => _exportToCsvCommand ?? (_exportToCsvCommand = new DelegateCommand(ExportToCsvCommandExecute));

        public void ExportToCsvCommandExecute()
            => NodeModels.WriteToCsvWithDialog();
    }
}
