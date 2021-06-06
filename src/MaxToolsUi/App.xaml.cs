
using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using System.Windows.Threading;
using MaxToolsUi.Services;
using MaxToolsUi.Views;

namespace MaxToolsUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public readonly IMaxToolsService MaxToolsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public App(IMaxToolsService maxToolsService)
            => MaxToolsService= maxToolsService;

        /// <summary>
        /// Constructor used for stub testing.
        /// </summary>
        public App()
            : this(new StubMaxToolsService())
        { }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
            => containerRegistry.RegisterInstance(typeof(IMaxToolsService), MaxToolsService);

        protected override void OnInitialized()
        {
            if (MaxToolsService.OnInitializedBehavior == OnInitializedBehavior.ShowDialog)
            {
                MainWindow?.ShowDialog();
            }
        }

        protected override Window CreateShell()
            => ContainerLocator.Container.Resolve<MaxToolsWindow>();

        public bool? ShowDialog()
            => MainWindow?.ShowDialog();

        public Window GetMainWindow()
            => MainWindow;
    }
}
