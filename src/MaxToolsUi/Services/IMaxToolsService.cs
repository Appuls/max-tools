using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Prism.Mvvm;

namespace MaxToolsUi.Services
{
    public enum OnInitializedBehavior
    {
        None,
        ShowDialog,
    }

    public enum OnClosingBehavior
    {
        None,
        Hide,
    }

    public class PropertyModel: BindableBase
    {
        public const string VariesCandidate = "[Varies]";
        public string Name { get; }

        public string OriginalValue { get; }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public PropertyModel(string name, string value)
        {
            Name = name;
            Value = value;
            OriginalValue = Value;
        }
    }

    public class NodeModel
    {
        public string Name { get; }
        public ObservableCollection<PropertyModel> Properties { get; } = new ObservableCollection<PropertyModel>();

        public NodeModel(string name, IReadOnlyList<PropertyModel> properties)
        {
            Name = name;
            Properties.AddRange(properties);
        }
    }

    public class SelectionChangedEventArgs
    {
        public readonly IReadOnlyList<NodeModel> NodeInfo;

        public SelectionChangedEventArgs(IReadOnlyList<NodeModel> nodeInfo)
            => NodeInfo = nodeInfo ?? new List<NodeModel>();
    }

    public interface IMaxToolsService
    {
        bool IsStub { get; }
        event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        OnInitializedBehavior OnInitializedBehavior { get; }
        OnClosingBehavior OnClosingBehavior { get; }
        void AttachOwnerWindow(Window window);
        Task RunOnMaxThread(Action action);
        void ObserveSelectionChanged(bool enabled);
        void SelectByProperty(string name, string value, bool add);
        void RefreshSelection();
    }
}
