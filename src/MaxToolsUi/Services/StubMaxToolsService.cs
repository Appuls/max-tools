using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaxToolsUi.ViewModels;

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

        private const int NumStubNodes = 3;
        public static IReadOnlyList<NodeModel> StubNodeInfos =>
            Enumerable.Repeat(0, NumStubNodes)
            .Select((_, i) => new NodeModel($"node_info_{i}", new List<PropertyModel>
            {
                new PropertyModel($"abc_{i}", "xyz"),
                new PropertyModel("hotdog", "mustard"),
                new PropertyModel("jojo", $"juju_{i}"),
                new PropertyModel("color", "red"),
                new PropertyModel("single_key", ""),
            })).ToArray();

        public readonly List<NodeModel> CurrentSelection = new List<NodeModel>();

        public void Select(IEnumerable<NodeModel> nodes, bool add = false)
        {
            if (!add)
            {
                CurrentSelection.Clear();
            }

            CurrentSelection.AddRange(nodes);
            OnSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(CurrentSelection));
        }

        public void RefreshSelection()
            => Select(StubNodeInfos);

        public void SelectByProperty(string name, string value, bool add)
            => Select(StubNodeInfos.Where(n => n.Properties.Any(p => 
                p.Name == name && (value == PropertyModel.VariesCandidate || p.Value == value))), add);
    }
}
