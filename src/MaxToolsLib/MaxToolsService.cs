using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Autodesk.Max.MaxPlus;
using MaxToolsUi;
using MaxToolsUi.Services;
using MaxToolsUi.ViewModels;
using Environment = System.Environment;
using INode = Autodesk.Max.Plugins.INode;
using Object = Autodesk.Max.MaxPlus.Object;

namespace MaxToolsLib
{
    public class MaxToolsService : IMaxToolsService
    {
        private App _app;
        private readonly Dispatcher _maxDispatcher;
        private readonly Thread _wpfThread;
        private readonly TaskCompletionSource<bool> _windowAttached;
        private bool _isObservingSelectionChanged = false;

        public bool IsStub => false;

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

        public static IReadOnlyList<PropertyModel> GetProperties(Autodesk.Max.MaxPlus.INode node)
        {
            var buffer = new WStr();
            node.GetUserPropBuffer(buffer);
            var rawProperties = buffer.Contents();

            if (string.IsNullOrEmpty(rawProperties))
            {
                return null;
            }

            // Split the lines.
            var lines = rawProperties.Split(new [] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return null;
            }

            // Split each token.
            return lines.Select(l =>
            {
                var tokens = l.Split(new []{ '=' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToArray();
                
                switch (tokens.Length)
                {
                    case 0:
                        return new PropertyModel("", "");
                    case 1:
                        return new PropertyModel(tokens[0], "");
                    case 2:
                        return new PropertyModel(tokens[0], tokens[1]);
                    default:
                        return new PropertyModel(tokens[0], string.Join("=", tokens.Skip(1)));
                }
            }).ToList();
        }

        private static readonly IReadOnlyList<PropertyModel> DefaultProps = new PropertyModel[] { };

        public void HandleSelectionChanged()
        {
            var selection = SelectionManager.Nodes.ToList();
            var nodeInfo = selection.Select(n => new NodeModel(n.Name, GetProperties(n) ?? DefaultProps)).ToList();
            RunOnWpfThread(() => OnSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(nodeInfo)));
        }

        public async void SelectByProperty(string name, string value, bool add)
        {
            await RunOnMaxThread(() =>
            {
                if (!add)
                {
                    SelectionManager.ClearNodeSelection(true);
                }

                var nodeTab = new INodeTab();
                foreach (var n in Core.GetRootNode().Children)
                {
                    var properties = GetProperties(n);
                    if (properties == null)
                        continue;

                    if (properties.Any(p =>
                        p.Name == name && (value == PropertyModel.VariesCandidate || p.Value == value)))
                        nodeTab.Append(n, false);
                }

                SelectionManager.SelectNodes(nodeTab);
            });
        }

        public async void RefreshSelection()
            => await RunOnMaxThread(HandleSelectionChanged);
    }
}
