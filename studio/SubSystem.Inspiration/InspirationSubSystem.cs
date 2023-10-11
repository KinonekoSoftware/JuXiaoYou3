using System;
using System.Collections.Generic;
using System.IO;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Inspirations.Pages;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations
{
    
    public class InspirationSubSystem : SubSystemModule
    {
        
        public override ITabViewController GetController() => new InspirationController();

        protected override string GetLanguageFilePrefix() => "Acorisoft.FutureGL.MigaStudio.Inspirations.Languages.";

        protected override void InstallView(IContainer container)
        {
            Xaml.InstallView<InspirationView, InspirationController>();
            Xaml.InstallView<HomePage, HomeViewModel>();
        }

        protected override void InstallServices(IContainer container)
        {
        }

        public override string FriendlyName => Language switch
        {
            _ => "灵感模式"
        };
        public override string Intro => Language switch
        {
            _ => "灵感模式"
        };
        public override string ModuleId => Id;
        public const string Id = "__Inspiration";
    }
}