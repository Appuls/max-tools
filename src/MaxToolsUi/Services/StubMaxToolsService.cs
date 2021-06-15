using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MaxToolsUi.Models;

namespace MaxToolsUi.Services
{
    public class StubMaxToolsService : IMaxToolsService
    {
        public bool IsStub => true;
        public event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        public OnInitializedBehavior OnInitializedBehavior => OnInitializedBehavior.ShowDialog;
        public OnClosingBehavior OnClosingBehavior => OnClosingBehavior.None;

        public void AttachOwnerWindow(Window _) { }

        public void ObserveSelectionChanged(bool enabled) { }

        private const int NumStubNodes = 3;

        public IReadOnlyList<NodeModel> StubNodeModels;

        public StubMaxToolsService()
        {
            StubNodeModels = Enumerable.Repeat(0, NumStubNodes)
                .Select((_, i) => new NodeModel($"node_model_{i}", new List<PropertyModel>
                {
                    new PropertyModel($"abc_{i}", "xyz"),
                    new PropertyModel("hotdog", "mustard"),
                    new PropertyModel("jojo", $"juju_{i}"),
                    new PropertyModel("color", "red"),
                    new PropertyModel("single_key", ""),
                })).ToArray();
        }

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
            => Select(StubNodeModels);

        public void SelectByProperty(string name, string value, bool add)
            => Select(StubNodeModels.Where(n => n.HasProperty(name, value)), add);

        public void SelectByAbsentProperties(bool add)
            => Select(StubNodeModels.Where(n => n.Properties.Count == 0));

        public void AddProperty(string name, string value)
        {
            foreach (var n in CurrentSelection)
            {
                n.AddProperty(name, value);
            }
            Select(CurrentSelection.ToArray());
        }

        public void ApplyNodeModels()
        {
            // do nothing
        }
    }
}
