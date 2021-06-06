using System.Threading.Tasks;

namespace MaxToolsLib
{
    public static class Bootstrap
    {
        private static MaxToolsService _maxToolsService;

        public static async Task<MaxToolsService> GetInstance()
            => _maxToolsService ?? (_maxToolsService = await MaxToolsService.CreateInstance());

        /// <summary>
        /// Invoked by max-tools-bootstrap.ms
        /// </summary>
        public static async void OpenDialog()
            => (await GetInstance()).ShowDialog();

        public static async void HandleSelectionChanged()
            => (await GetInstance()).HandleSelectionChanged();
    }
}
