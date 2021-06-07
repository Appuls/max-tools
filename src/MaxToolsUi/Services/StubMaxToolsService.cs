using System;
using System.Threading.Tasks;
using System.Windows;

namespace MaxToolsUi.Services
{
    public class StubMaxToolsService : IMaxToolsService
    {
        public event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
        public OnInitializedBehavior OnInitializedBehavior => OnInitializedBehavior.ShowDialog;
        public OnClosingBehavior OnClosingBehavior => OnClosingBehavior.None;

        public void AttachOwnerWindow(Window _) { }

        public Task RunOnMaxThread(Action action)
            => Task.Run(action);

        public void ObserveSelectionChanged(bool enabled) { }
    }
}
