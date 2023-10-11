using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Models;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class GlobalStudioContext : ObservableObject
    {
        private DocumentCache _character;
        
        public void SwitchController(ITabViewController controller)
        {
            if(controller is null ||
               ControllerSetter is null) return;
            ControllerSetter(controller);
            CurrentController = controller;
        }
        
        public ObservableCollection<DocumentCache> FavoriteCharacterList { get; init; }

        /// <summary>
        /// 获取或设置 <see cref="Character"/> 属性。
        /// </summary>
        public DocumentCache Character
        {
            get => _character;
            set => SetValue(ref _character, value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ControllerManifest> ControllerList { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ITabViewController CurrentController { get; private set; }
        
        /// <summary>
        /// 是否打开数据库
        /// </summary>
        public bool IsDatabaseOpen { get; set; }
        
        /// <summary>
        /// 是否为第一次启动
        /// </summary>
        public bool FirstTimeStartup { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Action<ITabViewController> ControllerSetter { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public ITabViewController MainController { get; init; }
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<ITabViewController> Controllers { get; init; }
        
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, ITabViewController> ControllerMaps { get; init; }
        
        public bool IsUpdated { get; set; }
    }
}