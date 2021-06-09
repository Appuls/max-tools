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

    public class PropertyInfo: BindableBase
    {
        public string Name { get; }

        public string OriginalValue { get; }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public PropertyInfo(string name, string value)
        {
            Name = name;
            Value = value;
            OriginalValue = Value;
        }
    }

    public class NodeInfo
    {
        public string Name { get; }
        public ObservableCollection<PropertyInfo> Properties { get; } = new ObservableCollection<PropertyInfo>();

        public NodeInfo(string name, IReadOnlyList<PropertyInfo> properties)
        {
            Name = name;
            Properties.AddRange(properties);
        }
    }

    public class SelectionChangedEventArgs
    {
        public readonly IReadOnlyList<NodeInfo> NodeInfo;

        public SelectionChangedEventArgs(IReadOnlyList<NodeInfo> nodeInfo)
            => NodeInfo = nodeInfo ?? new List<NodeInfo>();
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
    }
}
