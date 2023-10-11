using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Composer
{
    public class ComposerSubSystem : SubSystemModule
    {
        public override ITabViewController GetController() => new ComposerController();


        protected override string GetLanguageFilePrefix() => "Acorisoft.FutureGL.MigaStudio.Composer.Languages.";

        protected override void InstallView(IContainer container)
        {
        }

        protected override void InstallServices(IContainer container)
        {
        }

        public override string FriendlyName => Language switch
        {
            _ => "建造模式"
        };
        public override string Intro => Language switch
        {
            _ => "建造模式"
        };
        public override string ModuleId => Id;
        public const string Id = "__Composer";
    }
}