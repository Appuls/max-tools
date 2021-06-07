using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxToolsUi.Services;
using Prism.Events;

namespace MaxToolsUi.ViewModels
{
    public class MutableProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MutableProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class MutableNodeInfo
    {
        public string Name { get; set; }
        public ObservableCollection<MutableProperty> Properties { get; } = new ObservableCollection<MutableProperty>();

        public MutableNodeInfo(NodeInfo nodeInfo)
        {
            Name = nodeInfo.Name;
            Properties.AddRange(nodeInfo.Properties.Select(p => new MutableProperty(p.Item1, p.Item2)));
        }
    }

    public class MaxToolsWindowViewModel
    {
        private readonly IMaxToolsService _maxToolsService;
        public ObservableCollection<MutableNodeInfo> NodeInfos { get; } = new ObservableCollection<MutableNodeInfo>();

        public MaxToolsWindowViewModel(IMaxToolsService maxToolsService)
        {
            _maxToolsService = maxToolsService;
            SubscribeToEvents();
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            NodeInfos.Clear();
            NodeInfos.AddRange(args.NodeInfo.Select(n => new MutableNodeInfo(n)));
        }

        public void SubscribeToEvents()
        {
            _maxToolsService.OnSelectionChanged += HandleSelectionChanged;
        }
    }
}
