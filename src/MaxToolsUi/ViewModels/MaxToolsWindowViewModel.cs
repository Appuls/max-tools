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
        public const string VariesCandidate = "[Varies]";

        public string Guid { get; }
        public string Name { get; }
        public ObservableCollection<string> CandidateValues { get; } = new ObservableCollection<string>();

        public PropertyEntry(string name, IReadOnlyList<string> candidateValues)
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = name;

            if (candidateValues.Count > 1)
            {
                CandidateValues.Add(VariesCandidate);
            }
            CandidateValues.AddRange(candidateValues);
        }
    }

    public class MaxToolsWindowViewModel
    {
        private readonly IMaxToolsService _maxToolsService;
        public ObservableCollection<NodeInfo> NodeInfos { get; } = new ObservableCollection<NodeInfo>();
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

        private DelegateCommand<string> _selectCommand;

        public DelegateCommand<string> SelectCommand
            => _selectCommand ?? (_selectCommand = new DelegateCommand<string>(SelectCommandExecute));

        private void SelectCommandExecute(string entryName)
        {
            // TODO
        }

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
                p.Value = value == PropertyEntry.VariesCandidate ? p.OriginalValue : value;
            }
        }

        private DelegateCommand<string> _addToSelectionCommand;

        public DelegateCommand<string> AddToSelectionCommand
            => _addToSelectionCommand ?? (_addToSelectionCommand = new DelegateCommand<string>(AddToSelectionCommandExecute));

        private void AddToSelectionCommandExecute(string entryName)
        {
            // TODO
        }

        private DelegateCommand _stubCommand;

        public DelegateCommand StubCommand
            => _stubCommand ?? (_stubCommand = new DelegateCommand(StubCommandExecute));

        private void StubCommandExecute()
        {
            if (!(_maxToolsService is StubMaxToolsService stubService))
                return;
            stubService.FireStubSelection();
        }

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
