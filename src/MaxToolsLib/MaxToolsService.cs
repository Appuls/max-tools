using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MaxToolsUi;
using MaxToolsUi.Services;

namespace MaxToolsLib
{
    public class MaxToolsService : IMaxToolsService
    {
        private App _app;
        private readonly Dispatcher _maxDispatcher;
        private readonly Thread _wpfThread;

        public event EventHandler OnSelectionChanged;
        public OnInitializedBehavior OnInitializedBehavior => OnInitializedBehavior.None;
        public OnClosingBehavior OnClosingBehavior => OnClosingBehavior.Hide;

        public MaxToolsService()
        {
            _maxDispatcher = Dispatcher.FromThread(Thread.CurrentThread);

            _wpfThread = new Thread(() =>
            {
                _app = new App(this);
                Dispatcher.Run();
            });
            _wpfThread.SetApartmentState(ApartmentState.STA);
            _wpfThread.IsBackground = true;
            _wpfThread.Start();
        }

        public void AttachOwnerWindow(Window window)
        {
            var windowHandle = new System.Windows.Interop.WindowInteropHelper(window);
            windowHandle.Owner = ManagedServices.AppSDK.GetMaxHWND();
            ManagedServices.AppSDK.ConfigureWindowForMax(window);
        }

        public void ShowDialog()
            => _app.Dispatcher.BeginInvoke((Action)(() => _app.ShowDialog()));

        public Task RunOnMaxThread(Action action)
            => _maxDispatcher?.InvokeAsync(action).Task;
    }
}
