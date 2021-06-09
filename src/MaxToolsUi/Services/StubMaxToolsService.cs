using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MaxToolsUi.Services
{
    public class StubMaxToolsService : IMaxToolsService
    {
        public bool IsStub => true;
        public event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        public OnInitializedBehavior OnInitializedBehavior => OnInitializedBehavior.ShowDialog;
        public OnClosingBehavior OnClosingBehavior => OnClosingBehavior.None;

        public void AttachOwnerWindow(Window _) { }

        public Task RunOnMaxThread(Action action)
            => Task.Run(action);

        public void ObserveSelectionChanged(bool enabled) { }

        private const int NumStubNodes = 5;
        public static IReadOnlyList<NodeInfo> StubNodeInfos =
            Enumerable.Repeat(0, NumStubNodes)
            .Select((_, i) => new NodeInfo($"node_info_{i}", new List<PropertyInfo>
            {
                new PropertyInfo($"abc_{i}", "xyz"),
                new PropertyInfo("hotdog", "mustard"),
                new PropertyInfo("jojo", $"juju_{i}"),
                new PropertyInfo("color", "red"),
                new PropertyInfo("single_key", ""),
            })).ToArray();

        public void FireStubSelection()
            => OnSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(StubNodeInfos));
    }
}
