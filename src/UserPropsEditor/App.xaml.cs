
using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using System.Windows.Threading;
using UserPropsEditor.Services;
using UserPropsEditor.Views;

namespace UserPropsEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public readonly MaxService MaxService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public App(MaxService maxService)
            => MaxService= maxService;

        /// <summary>
        /// Constructor used for stub testing.
        /// </summary>
        private App()
            : this(new MaxService(OnInitializedBehavior.ShowDialog, true))
        { }

        /// <summary>
        /// Invoked by max-tools-bootstrap.ms
        /// </summary>
        public static App Create()
            => new App(new MaxService(OnInitializedBehavior.None, false));

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
            => containerRegistry.RegisterInstance(typeof(MaxService), MaxService);

        protected override void OnInitialized()
        {
            if (MaxService.OnInitializedBehavior == OnInitializedBehavior.ShowDialog)
            {
                MainWindow?.ShowDialog();
            }
        }

        protected override Window CreateShell()
            => ContainerLocator.Container.Resolve<ToolWindow>();

        public bool? ShowDialog()
            => MainWindow?.ShowDialog();

        public Window GetMainWindow()
            => MainWindow;
    }
}
