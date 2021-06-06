using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaxToolsUi.Services
{
    public class StubMaxToolsService : IMaxToolsService
    {
        public event EventHandler OnSelectionChanged;
        public OnInitializedBehavior OnInitializedBehavior => OnInitializedBehavior.ShowDialog;
        public OnClosingBehavior OnClosingBehavior => OnClosingBehavior.None;

        public void AttachOwnerWindow(Window _) { }

        public Task RunOnMaxThread(Action action)
            => Task.Run(action);
    }
}
