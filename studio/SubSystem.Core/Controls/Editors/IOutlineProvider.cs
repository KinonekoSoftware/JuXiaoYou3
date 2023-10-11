using Acorisoft.FutureGL.MigaStudio.Editors.Models;

namespace Acorisoft.FutureGL.MigaStudio.Editors
{
    public interface IOutlineProvider
    {
        IEnumerable<IOutlineModel> GetOutlineModels();
    }
}