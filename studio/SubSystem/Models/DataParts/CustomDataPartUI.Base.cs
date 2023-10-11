using System.ComponentModel;

namespace Acorisoft.FutureGL.MigaStudio.Models.DataParts
{
    public interface IPartOfDetailUI : INotifyPropertyChanged
    {
        
    }

    public abstract class PartOfDetailUI : ObservableObject, IPartOfDetailUI
    {
        
    }
}