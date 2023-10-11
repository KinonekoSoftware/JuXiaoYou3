using System.Diagnostics.CodeAnalysis;

namespace Acorisoft.Miga.Doc.Labels
{
    public interface IObjectLabelCollection<T> : INonredundant where T : class, IObjectLabel
    {
        void AddLabel(T label, [AllowNull] T parent = null);
        bool HasLabel(string labelName);
        bool GetLabel(string labelName, out T label);
        IEnumerable<Label> GetLabels();
        void RemoveLabel(T label);
    }
}