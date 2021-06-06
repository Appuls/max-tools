using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_maxToolsService.OnClosingBehavior == OnClosingBehavior.Hide)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void CloseWindow()
        {
            if (_maxToolsService.OnClosingBehavior == OnClosingBehavior.Hide)
            {
                Hide();
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
