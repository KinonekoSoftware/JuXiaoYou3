using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaStudio.Pages.Interactions;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class InteractionController : ShellCore
    {
        protected override void RequireStartupTabViewModel()
        {
            New<InteractionStartupViewModel>();
        }

        protected override void OnCurrentViewModelChanged(ITabViewModel oldViewModel, ITabViewModel newViewModel)
        {
            base.OnCurrentViewModelChanged(oldViewModel, newViewModel);
        }
        
        /// <summary>
        /// ID
        /// </summary>
        public sealed override string Id => AppViewModel.IdOfInteractionController;
    }
    
    
}