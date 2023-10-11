namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public interface IPresentationViewModel
    {
        public string PresentationIntro { get; }
        public string PresentationContractList { get; }
        public string PresentationAuthorList { get; }
        public string PresentationName { get; }
        public IEnumerable<ModuleBlockDataUI> Presentations { get; }
    }
}