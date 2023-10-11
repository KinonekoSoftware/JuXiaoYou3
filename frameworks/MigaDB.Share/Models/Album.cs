namespace Acorisoft.FutureGL.MigaDB.Models
{
    public class Album : StorageUIObject
    {
        private string _name;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    return Source;
                }

                return _name;
            }
            set => SetValue(ref _name, value);
        }
        public string Source { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
    }

}