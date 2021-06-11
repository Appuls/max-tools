using System;
using System.Windows;
using MaxToolsUi.Services;
using Prism.Events;

namespace MaxToolsUi.Views
{
    public partial class MaxToolsWindow : Window
    {
        public class Events
        {
            public class CloseRequested : PubSubEvent { }
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly IMaxToolsService _maxToolsService;

        public MaxToolsWindow(IEventAggregator eventAggregator, IMaxToolsService maxToolsService)
        {
            _eventAggregator = eventAggregator;
            _maxToolsService = maxToolsService;
            _maxToolsService.AttachOwnerWindow(this);
            InitializeComponent();
            SubscribeToEvents();
        }

        protected override void OnActivated(EventArgs _)
        {
            _maxToolsService.ObserveSelectionChanged(true);
            _maxToolsService.RefreshSelection();
        }

        private void InnerHide()
        {
            _maxToolsService.ObserveSelectionChanged(false);
            Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_maxToolsService.OnClosingBehavior == OnClosingBehavior.Hide)
            {
                e.Cancel = true;
                InnerHide();
            }
        }

        private void CloseWindow()
        {
            if (_maxToolsService.OnClosingBehavior == OnClosingBehavior.Hide)
            {
                InnerHide();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Subscribe to the events.
        /// </summary>
        private void SubscribeToEvents()
            => _eventAggregator.GetEvent<Events.CloseRequested>().Subscribe(CloseWindow);
    }
}
