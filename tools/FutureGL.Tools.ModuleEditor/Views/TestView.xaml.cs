using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;

namespace Acorisoft.FutureGL.Tools.ModuleEditor.Views
{
    public class ModuleBlockViewModel
    {
        public ModuleBlockViewModel()
        {
            Blocks = new ObservableCollection<ModuleBlockDataUI>(
                ModuleBlockFactory.CreateBlocks()
                                  .Where(x => x is not ColorBlock)
                                  .Select(ModuleBlockFactory.GetDataUI));
        }
        public ObservableCollection<ModuleBlockDataUI> Blocks { get; init; }
    }
    
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
        }
    }
}