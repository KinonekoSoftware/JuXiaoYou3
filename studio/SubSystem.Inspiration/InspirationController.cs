using System;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Inspirations.Pages;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations
{
    public class InspirationControllerBindingProxy : BindingProxy<InspirationController>
    {
        
    }
    
    public class InspirationController : ShellCore
    {
        private bool _isCharacterSelected;
        
        protected override void StartOverride()
        {
            RequireStartupTabViewModel();

        }

        protected sealed override void OnStart(RoutingEventArgs arg)
        {
            var p = arg.Parameter;
            GlobalStudioContext = (GlobalStudioContext)p.Args[0];

            if (GlobalStudioContext is null)
            {
                throw new ArgumentNullException(nameof(GlobalStudioContext));
            }
            
            base.OnStart(arg);
        }

        public override void Resume()
        {
            var db = Studio.Engine<DocumentEngine>()
                           .DocumentCacheDB;

            var list = GlobalStudioContext.FavoriteCharacterList;
            
            //
            // check character is delete?
            IsCharacterSelected = GlobalStudioContext is not null &&
                                  GlobalStudioContext.Character is not null &&
                                  db.HasID(GlobalStudioContext.Character.Id);

            //
            // remove deleted character
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                
                if (!db.HasID(item.Id))
                {
                    list.RemoveAt(i);
                }
            }
            
            base.Resume();
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
        }

        public GlobalStudioContext GlobalStudioContext { get; private set; }
        
        /// <summary>
        /// 获取或设置 <see cref="IsCharacterSelected"/> 属性。
        /// </summary>
        public bool IsCharacterSelected
        {
            get => _isCharacterSelected;
            set => SetValue(ref _isCharacterSelected, value);
        }
        
        public override string Id => InspirationSubSystem.Id;
    }
}