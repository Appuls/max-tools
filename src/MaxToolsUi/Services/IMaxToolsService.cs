using System;
using System.Threading.Tasks;
using System.Windows;

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

    public interface IMaxToolsService
    {
        event EventHandler OnSelectionChanged;
        OnInitializedBehavior OnInitializedBehavior { get; }
        OnClosingBehavior OnClosingBehavior { get; }
        void AttachOwnerWindow(Window window);
        Task RunOnMaxThread(Action action);
    }
}
