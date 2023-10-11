namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class PlaceholderViewModel : TabViewModel
    {
        public PlaceholderViewModel()
        {
            PageId = 
                Title = GetHashCodeIntern().ToString();
            ApprovalRequired = false;
        }
        
        public override bool Removable => true;
        public override bool Uniqueness => false;
    }
}