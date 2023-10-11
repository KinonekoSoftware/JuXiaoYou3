global using System;
global using System.Runtime.CompilerServices;
global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Media;
global using System.Windows.Input;
global using Acorisoft.FutureGL.Forest;
global using Acorisoft.FutureGL.Forest.Services;
global using Acorisoft.FutureGL.Forest.Styles;
global using Acorisoft.FutureGL.MigaUtils;
using System.Reflection;
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
[assembly: InternalsVisibleTo("Forest.Tests")]