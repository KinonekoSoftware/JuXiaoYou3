global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Threading.Tasks;

global using Acorisoft.FutureGL.Forest;
global using Acorisoft.FutureGL.Forest.Attributes;
global using Acorisoft.FutureGL.Forest.Services;
global using Acorisoft.FutureGL.Forest.Styles;
global using Acorisoft.FutureGL.Forest.ViewModels;
global using Acorisoft.FutureGL.Forest.Interfaces;

global using Acorisoft.FutureGL.MigaStudio.Models;
global using Acorisoft.FutureGL.MigaStudio.ViewModels;
global using Acorisoft.FutureGL.MigaStudio.Pages.Commons;

global using Acorisoft.FutureGL.MigaUtils;
global using Acorisoft.FutureGL.MigaUtils.Collections;

global using Acorisoft.FutureGL.MigaDB.Core;
global using Acorisoft.FutureGL.MigaDB.Data.DataParts;
global using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
global using Acorisoft.FutureGL.MigaDB.Documents;
global using Acorisoft.FutureGL.MigaDB.Interfaces;
global using Acorisoft.FutureGL.MigaDB.Models;
global using Acorisoft.FutureGL.MigaDB.Services;
global using Acorisoft.FutureGL.MigaDB.Utils;

using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page,
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page,
    // app, or any theme specific resource dictionaries)
)]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Controls")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Controls.Socials")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Resources.Behaviors")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Resources.Converters")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Resources.Panels")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Resources.EmptyState")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Models")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.Windows")]
[assembly: XmlnsDefinition("urn:acorisoft/studio", "Acorisoft.FutureGL.MigaStudio.ViewModels")]