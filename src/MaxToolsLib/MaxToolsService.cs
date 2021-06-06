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
        private Dispatcher _maxDispatcher;
        private Thread _wpfThread;
        private TaskCompletionSource<bool> _windowAttached;

        public event EventHandler OnSelectionChanged;
        public OnInitializedBehavior OnInitializedBehavior => OnInitializedBehavior.None;
        public OnClosingBehavior OnClosingBehavior => OnClosingBehavior.Hide;

        private MaxToolsService()
        {
            _windowAttached = new TaskCompletionSource<bool>();

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

        private Task WaitForAttached()
            => _windowAttached.Task;

        public static async Task<MaxToolsService> CreateInstance()
        {
            var service = new MaxToolsService();
            await service.WaitForAttached();
            return service;
        }

        public void AttachOwnerWindow(Window window)
        {
            var windowHandle = new System.Windows.Interop.WindowInteropHelper(window);
            windowHandle.Owner = ManagedServices.AppSDK.GetMaxHWND();
            ManagedServices.AppSDK.ConfigureWindowForMax(window);
            _windowAttached.SetResult(true);
        }

        public void ShowDialog()
            => _app.Dispatcher.BeginInvoke((Action)(() => _app.ShowDialog()));

        public Task RunOnMaxThread(Action action)
            => _maxDispatcher?.InvokeAsync(action).Task;
    }
}
