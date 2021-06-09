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

        public string Name { get; }
        public ObservableCollection<string> CandidateValues { get; } = new ObservableCollection<string>();

        public PropertyEntry(string name, IReadOnlyList<string> candidateValues)
        {
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

        private DelegateCommand<string> _removeCommand;

        public DelegateCommand<string> RemoveCommand
            => _removeCommand ?? (_removeCommand = new DelegateCommand<string>(RemoveCommandExecute));

        private void RemoveCommandExecute(string entryName)
        {
            // TODO
        }

        private DelegateCommand<string> _selectCommand;

        public DelegateCommand<string> SelectCommand
            => _selectCommand ?? (_selectCommand = new DelegateCommand<string>(SelectCommandExecute));

        private void SelectCommandExecute(string entryName)
        {
            // TODO
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
