using System.Windows;
using Acorisoft.FutureGL.MigaStudio.Pages.Universe.Models;
using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;
using Microsoft.CodeAnalysis;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    partial class FantasyProjectStartupViewModel : TabViewModel
    {
        private ProjectItem _spaceRoot;

        private void CreateProjectItems()
        {
            ProjectElements.Clear();
            DocumentElements.Clear();
            OtherElements.Clear();

            //
            //
            CreateWorldViewItems(ProjectElements);
            CreateDocumentItems(DocumentElements);
            CreateOtherItems(OtherElements);
        }

        private void CreateWorldViewItems(ICollection<ProjectItem> collection)
        {
            // 世界划分
            CreateSpaceConcept(collection);

            // 世界机制
            collection.Add(Create<FantasyProjectMechanismViewModel>("text.Project.Mechanism"));

            // 世界法则
            collection.Add(Create<FantasyProjectRuleViewModel>("text.Project.Rule"));

            // 时间轴
            collection.Add(Create<FantasyProjectTimelineViewModel>("text.Project.Timeline"));
        }

        private void CreateSpaceConcept(ICollection<ProjectItem> collection)
        {
            _spaceRoot = new SpaceConceptUI
            {
                Source        = null,
                Name          = Language.GetText("text.Project.SpaceConcept"),
                ViewModelType = typeof(FantasyProjectSpaceConceptViewModel),
                Property      = new ObservableCollection<ProjectItem>(),
                Expression    = CreateSpaceConceptExpr
            };

            _spaceRoot.Parameter1 = _spaceRoot;


            collection.Add(_spaceRoot);
        }

        private FrameworkElement DefaultCreateViewExpr(ProjectItem item)
        {
            //
            //
            item.ViewModel ??= Xaml.GetViewModel<TabViewModel>(item.ViewModelType);

            //
            //
            var page = (FrameworkElement)Xaml.Connect(item.ViewModel);

            //
            //
            if (page is null)
            {
                return null;
            }

            page.DataContext = item.ViewModel;

            return page;
        }

        internal static FrameworkElement CreateSpaceConceptExpr(ProjectItem item)
        {
            //
            //
            item.ViewModel ??= Xaml.GetViewModel<TabViewModel>(item.ViewModelType);

            item.ViewModel
                .Startup(new RoutingEventArgs
                {
                    Parameter = new Parameter
                    {
                        Args = new object[]
                        {
                            item,
                        }
                    }
                });

            //
            //
            var page = (FrameworkElement)Xaml.Connect(item.ViewModel);

            //
            //
            if (page is null)
            {
                return null;
            }

            page.DataContext = item.ViewModel;

            return page;
        }


        private static void CreateDocumentItems(ICollection<ProjectItem> collection)
        {
            // 人物
            collection.Add(CreateCharacters());

            // 地理
            collection.Add(Create<FantasyProjectGeographyViewModel>("text.Project.Geography"));

            // 能力
            collection.Add(Create<FantasyProjectSkillViewModel>("text.Project.Skill"));

            // 物品
            collection.Add(CreateItems());

            // 政治团体
            collection.Add(CreatePoliticalBlocs());

            // 知识
            collection.Add(CreateKnowledge());
        }

        private static void CreateOtherItems(ICollection<ProjectItem> collection)
        {
            // 其他
            collection.Add(Create<FantasyProjectOtherViewModel>("text.Project.Other"));

            // 关系
            collection.Add(Create<FantasyProjectRelativesViewModel>("text.Project.Relatives"));
        }

        private static ProjectItem CreatePoliticalBlocs()
        {
            var parent = Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs");
            parent.Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Country", PoliticalBlocs.Country));
            parent.Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Force", PoliticalBlocs.Force));
            parent.Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Organization", PoliticalBlocs.Organization));
            parent.Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Tribe", PoliticalBlocs.Tribe));
            parent.Add(Create<FantasyProjectPoliticalBlocsViewModel>("text.Project.PoliticalBlocs.Clan", PoliticalBlocs.Clan));
            return parent;
        }

        private static ProjectItem CreateCharacters()
        {
            var parent = Create<FantasyProjectCharacterViewModel>("text.Project.Character");
            parent.Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.All", Character.All));
            parent.Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.Primary", Character.Primary));
            parent.Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.Secondary", Character.Secondary));
            parent.Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.NPC", Character.NPC));
            parent.Add(Create<FantasyProjectCharacterViewModel>("text.Project.Character.Binding", Character.Binding));
            return parent;
        }

        private static ProjectItem CreateItems()
        {
            var parent = Create<FantasyProjectItemViewModel>("text.Project.Item");
            parent.Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Product", ItemType.Product));
            parent.Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Artifact", ItemType.Artifact));
            parent.Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Material", ItemType.Material));
            parent.Add(Create<FantasyProjectItemViewModel>("text.Project.Item.UnprocessedMaterial", ItemType.UnprocessedMaterial));
            parent.Add(Create<FantasyProjectItemViewModel>("text.Project.Item.Equipment", ItemType.Equipment));
            return parent;
        }

        private static ProjectItem CreateKnowledge()
        {
            var parent = Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge");
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Planets", KnowledgeType.Planets));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Creatures", KnowledgeType.Creatures));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Gods", KnowledgeType.Gods));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Devils", KnowledgeType.Devils));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Poison", KnowledgeType.Poison));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Calamity", KnowledgeType.Calamity));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Religion", KnowledgeType.Religion));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Thread", KnowledgeType.Thread));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Class", KnowledgeType.Class));
            parent.Add(Create<FantasyProjectKnowledgeViewModel>("text.Project.Knowledge.Tale", KnowledgeType.Tale));
            return parent;
        }

        private static ProjectItem Create<TViewModel>(string id) where TViewModel : TabViewModel
        {
            return new ProjectEntryItem
            {
                Name          = Language.GetText(id),
                ViewModelType = typeof(TViewModel),
                Property      = new ObservableCollection<ProjectItem>(),
            };
        }

        private static ProjectItem Create<TViewModel>(string id, object type) where TViewModel : TabViewModel
        {
            return new ProjectEntryItem
            {
                Name          = Language.GetText(id),
                ViewModelType = typeof(TViewModel),
                Parameter1    = type,
                Property      = new ObservableCollection<ProjectItem>(),
            };
        }
    }
}