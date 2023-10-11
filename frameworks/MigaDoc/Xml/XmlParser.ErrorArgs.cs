namespace Acorisoft.Miga.Xml
{
    public class ErrorArgs : EventArgs
    {
        public int LineNumber { get; init; }
        public int LinePosition { get; init; }
        public string Message { get; init; }

        public override string ToString()
        {
            return $"{LineNumber}:{LinePosition}，{Message}";
        }
    }
}