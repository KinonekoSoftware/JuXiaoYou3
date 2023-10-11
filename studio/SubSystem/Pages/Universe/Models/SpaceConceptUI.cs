using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaStudio.ViewModels.FantasyProject;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe.Models
{
    public class SpaceConceptUI : ProjectItem
    {
        private string _name;

        public SpaceConcept Source { get; init; }

        /// <summary>
        /// 名字
        /// </summary>
        public override string Name
        {
            get => Source?.Name ?? _name;
            set
            {
                if (Source is null)
                {
                    _name = value;
                }
                else
                {
                    Source.Name = value;
                }
                RaiseUpdated();
            }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public override string ToolTips
        {
            get => Source.Intro;
            set
            {
                Source.Intro = value;
                RaiseUpdated();
            }
        }
    }
}