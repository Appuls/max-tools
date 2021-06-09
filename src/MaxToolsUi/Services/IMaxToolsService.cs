using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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

    public class PropertyInfo
    {
        public string Name { get; }
        public string Value { get; }

        public PropertyInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class NodeInfo
    {
        public string Name { get; }
        public IReadOnlyList<PropertyInfo> Properties { get; }

        public NodeInfo(string name, IReadOnlyList<PropertyInfo> properties)
        {
            Name = name;
            Properties = properties ?? new List<PropertyInfo>();
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
