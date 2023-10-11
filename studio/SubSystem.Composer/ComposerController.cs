using System.Reactive.Concurrency;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Composer
{
    public class ComposerController : TabController
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Id => ComposerSubSystem.Id;
    }
}