namespace Acorisoft.FutureGL.MigaDB.Utils
{
    public class ObservableState : ObservableProperty<bool>, IObservableState
    {
        public ObservableState() : base(false)
        {
            
        }
    }
}