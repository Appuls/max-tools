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

    public class NodeInfo
    {
        public readonly string Name;
        public readonly IReadOnlyList<(string, string)> Properties;

        public NodeInfo(string name, IReadOnlyList<(string, string)> properties)
        {
            Name = name;
            Properties = properties;
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
        event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        OnInitializedBehavior OnInitializedBehavior { get; }
        OnClosingBehavior OnClosingBehavior { get; }
        void AttachOwnerWindow(Window window);
        Task RunOnMaxThread(Action action);
        void ObserveSelectionChanged(bool enabled);
    }
}
