using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Max.MaxPlus;
using MaxToolsUi;
using MaxToolsUi.Services;
using Environment = System.Environment;
using INode = Autodesk.Max.Plugins.INode;

namespace MaxToolsLib
{
    public class MaxToolsService : IMaxToolsService
    {
        private App _app;
        private readonly Dispatcher _maxDispatcher;
        private readonly Thread _wpfThread;
        private readonly TaskCompletionSource<bool> _windowAttached;
        private bool _isObservingSelectionChanged = false;

        public event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;
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

        public async void ObserveSelectionChanged(bool enabled)
        {
            if (enabled && _isObservingSelectionChanged)
                return;

            await RunOnMaxThread(() => Core.ExecuteMAXScript($"maxTools_observeSelectionChanged {enabled}"));
            _isObservingSelectionChanged = enabled;
        }

        public Task RunOnWpfThread(Action action)
            => _app.Dispatcher.InvokeAsync(action).Task;

        public Task RunOnMaxThread(Action action)
            => _maxDispatcher?.InvokeAsync(action).Task;

        public void ShowDialog()
            => RunOnWpfThread(() => _app.ShowDialog());

        private static readonly IReadOnlyList<(string, string)> DefaultProps = new (string,string)[] {};

        public static IReadOnlyList<(string, string)> GetProperties(Autodesk.Max.MaxPlus.INode node)
        {
            var buffer = new WStr();
            node.GetUserPropBuffer(buffer);
            var rawProperties = buffer.Contents();

            if (string.IsNullOrEmpty(rawProperties))
            {
                return DefaultProps;
            }

            // Split the lines.
            var lines = rawProperties.Split(new [] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return DefaultProps;
            }

            // Split each token.
            return lines.Select(l =>
            {
                var tokens = l.Split(new []{ '=' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToArray();
                
                switch (tokens.Length)
                {
                    case 0:
                        return ("", "");
                    case 1:
                        return (tokens[0], "");
                    case 2:
                        return (tokens[0], tokens[1]);
                    default:
                        return (tokens[0], string.Join("=", tokens.Skip(1)));
                }
            }).ToList();
        }

        public void HandleSelectionChanged()
        {
            var selection = SelectionManager.Nodes.ToList();
            var nodeInfo = selection.Select(n => new NodeInfo(n.Name, GetProperties(n))).ToList();
            RunOnWpfThread(() => OnSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(nodeInfo)));
        }
    }
}
