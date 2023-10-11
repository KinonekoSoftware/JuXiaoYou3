using System.Diagnostics;
using System.Linq;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class FantasyProjectSpaceConceptViewModel : TabViewModel
    {
        public FantasyProjectSpaceConceptViewModel()
        {
            Children   = new ObservableCollection<SpaceConceptUI>();
            
            AddCommand = AsyncCommand<SpaceConceptUI>(AddImpl);
        }
        
        protected override void OnStart(Parameter parameter)
        {
            var a    = parameter.Args;
            CurrentItem = (SpaceConceptUI)a[0];
            ParentItem  = CurrentItem?.Parent as SpaceConceptUI;
            Children.AddMany(CurrentItem.Children
                                        .Cast<SpaceConceptUI>(), true);
            base.OnStart(parameter);
        }

        /*
         * - root
         * -- space  
         */

        private async Task AddImpl(SpaceConceptUI parent)
        {
            var r = await SingleLineViewModel.String("Add");

            if (!r.IsFinished)
            {
                return;
            }

            SpaceConceptUI ui;
            parent ??= ParentItem;
            
            if (parent is null)
            {
                var spaceConcept = new SpaceConcept
                {
                    Id     = ID.Get(),
                    Name   = r.Value,
                    Height = 0
                };

                ui = new SpaceConceptUI
                {
                    Id = ID.Get(),
                    Source   = spaceConcept,
                    Expression    = FantasyProjectStartupViewModel.CreateSpaceConceptExpr,
                    ViewModelType = typeof(FantasyProjectSpaceConceptViewModel),
                };

                Children.Add(ui);
                CurrentItem.Add(ui);
            }
            else
            {
#if DEBUG
                Debug.WriteLine($"handle: {CurrentItem.Name}, parent: {ParentItem.Name}");
#endif
                if (CurrentItem.Source.Height > 16)
                {
                    // 宇宙 星系 星球 大陆 国家 省份 城市 区县 乡镇 村 屯
                    await this.WarningNotification("不支持超过16级的空间");
                    return;
                }
                
                var spaceConcept = new SpaceConcept
                {
                    Id     = ID.Get(),
                    Name   = r.Value,
                    Height = CurrentItem.Source.Height + 1,
                    Owner = CurrentItem.Source.Id,
                };

                ui = new SpaceConceptUI
                {
                    Source        = spaceConcept,
                    Expression    = FantasyProjectStartupViewModel.CreateSpaceConceptExpr,
                    ViewModelType = typeof(FantasyProjectSpaceConceptViewModel),
                };

                Children.Add(ui);
                CurrentItem.Add(ui);
            }
        }
        
        public AsyncRelayCommand<SpaceConceptUI> AddCommand { get; }
        public AsyncRelayCommand<SpaceConceptUI> EditCommand { get; }
        public AsyncRelayCommand<SpaceConceptUI> RemoveCommand { get; }
        
        public SpaceConceptUI CurrentItem { get; private set; }
        public SpaceConceptUI ParentItem { get; private set; }
        
        public ObservableCollection<SpaceConceptUI> Children { get; }
    }
}