using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs;
using Acorisoft.FutureGL.MigaStudio.Pages.Composes;
using Acorisoft.FutureGL.MigaStudio.Pages.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;
using Acorisoft.FutureGL.MigaStudio.Pages.Relatives;
using Acorisoft.FutureGL.MigaStudio.Pages.Services;
using Acorisoft.FutureGL.MigaStudio.Pages.Templates;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe.Dialogs;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public static class SubSystem
    {
        public static Task<Op<DocumentCache>> NewDocument()
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, NewDocumentViewModel>();
        }

        public static void ImageView(string fileName)
        {
            var vm = new ImageViewModel();
            vm.Startup(new RoutingEventArgs
            {
                Parameter = new Parameter
                {
                    Args = new object[] { fileName }
                }
            });
            new ImageView
            {
                DataContext = vm
            }.Show();
        }


        public static Task<Op<DocumentCache>> Select(IEnumerable<DocumentCache> documents)
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }
        
        public static Task<Op<IEnumerable<DocumentCache>>> MultiSelect(IEnumerable<DocumentCache> documents)
        {
            return Xaml.Get<IDialogService>()
                       .Dialog<IEnumerable<DocumentCache>, MultiDocumentPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }

        public static Task<Op<DocumentCache>> Select(DocumentType type)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(type);
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }
        
        public static Task<Op<IEnumerable<DocumentCache>>> MultiSelect(DocumentType type)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(type);
            return Xaml.Get<IDialogService>()
                       .Dialog<IEnumerable<DocumentCache>, MultiDocumentPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }

        public static Task<Op<DocumentCache>> SelectExclude(DocumentType type, ISet<string> pool)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCachesExclude(type, pool);
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }
        
        public static Task<Op<DocumentCache>> SelectExclude(DocumentType type, string target)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCachesExclude(type, target);
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }
        
        public static Task<Op<IEnumerable<DocumentCache>>> MultiSelectExclude(DocumentType type, ISet<string> pool)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCachesExclude(type, pool);
            return Xaml.Get<IDialogService>()
                       .Dialog<IEnumerable<DocumentCache>, MultiDocumentPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }

        public static Task<Op<DocumentCache>> Select(DocumentType type, ISet<string> idPool)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(type, idPool);
            return Xaml.Get<IDialogService>()
                       .Dialog<DocumentCache, DocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }
        
        public static Task<Op<IEnumerable<DocumentCache>>> MultiSelect(DocumentType type, ISet<string> idPool)
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches(type, idPool);
            return Xaml.Get<IDialogService>()
                       .Dialog<IEnumerable<DocumentCache>, MultiDocumentPickerViewModel>(new Parameter
                       {
                           Args = new object[]
                           {
                               documents
                           }
                       });
        }

        public static Task<Op<IEnumerable<DocumentCache>>> MultiSelect()
        {
            var documents = Studio.Engine<DocumentEngine>()
                                  .GetCaches();
            return Xaml.Get<IDialogService>()
                       .Dialog<IEnumerable<DocumentCache>, MultiDocumentPickerViewModel>(new Parameter
            {
                Args = new object[]
                {
                    documents
                }
            });
        }

        /// <summary>
        /// 选择选项视图
        /// </summary>
        /// <param name="title">视图的标题</param>
        /// <param name="selected">选择的选项</param>
        /// <param name="options">所有选项</param>
        /// <typeparam name="TOption">选项类型</typeparam>
        /// <returns>返回一个操作结果</returns>
        public static Task<Op<TOption>> Selection<TOption>(string title, object selected, IEnumerable<object> options)
        {
            var optionVM = Xaml.GetViewModel<OptionSelectionViewModel>();
            var parameter = new Parameter
            {
                Args = new[]
                {
                    selected,
                    options
                }
            };

            optionVM.Title = title;
            return Xaml.Get<IDialogService>()
                       .Dialog<TOption>(optionVM, parameter);
        }

        /// <summary>
        /// 选择选项视图
        /// </summary>
        /// <param name="title">视图的标题</param>
        /// <param name="options">所有选项</param>
        /// <typeparam name="TOption">选项类型</typeparam>
        /// <returns>返回一个操作结果</returns>
        public static Task<Op<TOption>> Selection<TOption>(string title, IEnumerable<object> options)
        {
            // ReSharper disable PossibleMultipleEnumeration
            return Selection<TOption>(title, options.First(), options);
        }

        public static void InstallLanguages()
        {
            var fileName = Language.Culture switch
            {
                CultureArea.English  => "SubSystem.en.ini",
                CultureArea.French   => "SubSystem.fr.ini",
                CultureArea.Japanese => "SubSystem.jp.ini",
                CultureArea.Korean   => "SubSystem.kr.ini",
                CultureArea.Russian  => "SubSystem.ru.ini",
                _                    => "SubSystem.cn.ini",
            };

            Language.AppendLanguageSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages", fileName));
        }

        public static void InstallViews()
        {
            //
            // Commons
            Xaml.InstallView<ImageEditView, ImageEditViewModel>();
            Xaml.InstallView<MusicPlayerView, MusicPlayerViewModel>();
            Xaml.InstallView<MusicPage, MusicViewModel>();
            Xaml.InstallView<OptionSelectionView, OptionSelectionViewModel>();
            Xaml.InstallView<DocumentPickerView, DocumentPickerViewModel>();
            Xaml.InstallView<MultiDocumentPickerView, MultiDocumentPickerViewModel>();

            //
            // Compose
            Xaml.InstallView<ComposeEditorPage, ComposeEditorViewModel>();

            //
            // Commons
            Xaml.InstallView<NewDocumentView, NewDocumentViewModel>();
            Xaml.InstallView<DocumentGalleryExPage, DocumentGalleryViewModelEx>();

            //
            // Document
            Xaml.InstallView<DetailPartSelectorView, DetailPartSelectorViewModel>();
            Xaml.InstallView<ManageSurveyView, ManageSurveyViewModel>();
            Xaml.InstallView<NewPresentationView, NewPresentationViewModel>();
            Xaml.InstallView<NewSurveyView, NewSurveyViewModel>();
            Xaml.InstallView<EditPresentationView, EditPresentationViewModel>();
            Xaml.InstallView<EditStringPresentationView, EditStringPresentationViewModel>();
            Xaml.InstallView<EditChartPresentationView, EditChartPresentationViewModel>();
            Xaml.InstallView<EditPlainTextView, EditPlainTextViewModel>();
            Xaml.InstallView<ModuleSelectorView, ModuleSelectorViewModel>();
            Xaml.InstallView<DocumentEditorPage, SkillDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, CharacterDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, ItemDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, OtherDocumentViewModel>();
            Xaml.InstallView<DocumentEditorPage, GeographyDocumentViewModel>();

            //
            // Relationships
            Xaml.InstallView<NewRelativeView, NewRelativeViewModel>();
            Xaml.InstallView<NewRelativePresetView, NewRelativePresetViewModel>();
            Xaml.InstallView<RelativePresetPage, RelativePresetViewModel>();
            Xaml.InstallView<CharacterPedigreePage, CharacterPedigreeViewModel>();
            Xaml.InstallView<CharacterRelationshipPage, CharacterRelationshipViewModel>();

            //
            // Template
            Xaml.InstallView<EditBlockView, EditBlockViewModel>();
            Xaml.InstallView<NewBlockView, NewBlockViewModel>();
            Xaml.InstallView<NewElementView, NewElementViewModel>();
            Xaml.InstallView<TemplateEditorPage, TemplateEditorViewModel>();
            Xaml.InstallView<TemplateGalleryPage, TemplateGalleryViewModel>();
            Xaml.InstallView<ModuleManifestView, ModuleManifestViewModel>();

            //
            // FantasyProject
            Xaml.InstallView<FantasyProjectPoliticalBlocsPage, FantasyProjectPoliticalBlocsViewModel>();
            Xaml.InstallView<FantasyProjectStartupPage, FantasyProjectStartupViewModel>();
            Xaml.InstallView<FantasyProjectCharacterPage, FantasyProjectCharacterViewModel>();
            Xaml.InstallView<FantasyProjectItemPage, FantasyProjectItemViewModel>();
            Xaml.InstallView<FantasyProjectGeographyPage, FantasyProjectGeographyViewModel>();
            Xaml.InstallView<FantasyProjectSpacePage, FantasyProjectSpaceConceptViewModel>();
            Xaml.InstallView<FantasyProjectSettingPage, FantasyProjectSettingViewModel>();
            Xaml.InstallView<FantasyProjectRulePage, FantasyProjectRuleViewModel>();
            Xaml.InstallView<FantasyProjectMechanismPage, FantasyProjectMechanismViewModel>();
            Xaml.InstallView<FantasyProjectSkillPage, FantasyProjectSkillViewModel>();
            Xaml.InstallView<FantasyProjectKnowledgePage, FantasyProjectKnowledgeViewModel>();
            
            //
            // Timeline
            Xaml.InstallView<FantasyProjectTimelinePage, FantasyProjectTimelineViewModel>();
            Xaml.InstallView<NewTimelineView, NewTimelineViewModel>();
            
            Xaml.InstallView<FantasyProjectOtherPage, FantasyProjectOtherViewModel>();
            Xaml.InstallView<FantasyProjectRelativesPage, FantasyProjectRelativesViewModel>();

            ServiceViewContainer.Initialize();
        }
    }
}