namespace MaxToolsLib
{
    public static class Bootstrap
    {
        private static MaxToolsService _maxToolsService;

        private static MaxToolsService MaxToolsService
            => _maxToolsService ?? (_maxToolsService = new MaxToolsService());

        public static void OpenDialog()
            => MaxToolsService.ShowDialog();
    }
}
