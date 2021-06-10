using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxToolsUi.Services;
using Prism.Commands;
using Prism.Events;

namespace MaxToolsUi.ViewModels
{
    public class PropertyEntry
    {
        public string Guid { get; }
        public string Name { get; }
        public ObservableCollection<string> CandidateValues { get; } = new ObservableCollection<string>();

        public PropertyEntry(string name, IReadOnlyList<string> candidateValues)
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = name;

            if (candidateValues.Count > 1)
            {
                CandidateValues.Add(PropertyModel.VariesCandidate);
            }
            CandidateValues.AddRange(candidateValues);
        }
    }

    public class MaxToolsWindowViewModel
    {
        private readonly IMaxToolsService _maxToolsService;
        public ObservableCollection<NodeModel> NodeInfos { get; } = new ObservableCollection<NodeModel>();
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

            foreach (var n in NodeInfos)
            {
                var toRemove = n.Properties.FirstOrDefault(p => p.Name == propertyEntry.Name);
                if (toRemove == null)
                    continue;
                n.Properties.Remove(toRemove);
            }
        }

        public class SelectArgs
        {
            public readonly string Name;
            public readonly string Value;

            public SelectArgs(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        public void Select(SelectArgs args, bool add)
            => _maxToolsService.SelectByProperty(args.Name, args.Value, add);

        private DelegateCommand<SelectArgs> _selectCommand;

        public DelegateCommand<SelectArgs> SelectCommand
            => _selectCommand ?? (_selectCommand = new DelegateCommand<SelectArgs>(SelectCommandExecute));

        private void SelectCommandExecute(SelectArgs args)
            => Select(args, false);


        private DelegateCommand<SelectArgs> _addToSelectionCommand;

        public DelegateCommand<SelectArgs> AddToSelectionCommand
            => _addToSelectionCommand ?? (_addToSelectionCommand = new DelegateCommand<SelectArgs>(AddToSelectionCommandExecute));

        private void AddToSelectionCommandExecute(SelectArgs args)
            => Select(args, true);

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

            var propInfos = NodeInfos.SelectMany(n => n.Properties).Where(p => p.Name == propertyEntry.Name);
            foreach (var p in propInfos)
            {
                p.Value = value == PropertyModel.VariesCandidate ? p.OriginalValue : value;
            }
        }

        private DelegateCommand _refreshSelectionCommand;

        public DelegateCommand RefreshSelectionCommand
            => _refreshSelectionCommand ?? (_refreshSelectionCommand = new DelegateCommand(RefreshSelectionCommandExecute));

        private void RefreshSelectionCommandExecute()
            => _maxToolsService.RefreshSelection();

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            NodeInfos.Clear();
            NodeInfos.AddRange(args.NodeInfo);

            PropertyEntries.Clear();
            var propEntries = NodeInfos
                .SelectMany(n => n.Properties)
                .GroupBy(t => t.Name)
                .Select(g => new PropertyEntry(g.Key, g.Select(t => t.Value).Distinct().OrderBy(v => v).ToArray()))
                .OrderBy(p => p.Name);
            PropertyEntries.AddRange(propEntries);
        }

        public void SubscribeToEvents()
        {
            _maxToolsService.OnSelectionChanged += HandleSelectionChanged;
        }
    }
}
