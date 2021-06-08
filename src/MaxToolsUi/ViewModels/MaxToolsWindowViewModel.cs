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
        public const string VariesCandidate = "Varies...";

        public string EntryName { get; }
        public ObservableCollection<string> CandidateValues { get; } = new ObservableCollection<string>();

        public PropertyEntry(string entryName, IReadOnlyList<string> candidateValues)
        {
            EntryName = entryName;
            
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
        private readonly List<NodeInfo> _nodeInfos = new List<NodeInfo>();
        public ObservableCollection<PropertyEntry> PropertyEntries { get; } = new ObservableCollection<PropertyEntry>();

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

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _nodeInfos.Clear();
            _nodeInfos.AddRange(args.NodeInfo);

            PropertyEntries.Clear();
            var propEntries = _nodeInfos
                .SelectMany(n => n.Properties)
                .GroupBy(t => t.entryName)
                .Select(g => new PropertyEntry(g.Key, g.Select(t => t.value).Distinct().OrderBy(v => v).ToArray()))
                .OrderBy(p => p.EntryName);
            PropertyEntries.AddRange(propEntries);
        }

        public void SubscribeToEvents()
        {
            _maxToolsService.OnSelectionChanged += HandleSelectionChanged;
        }
    }
}
