using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public partial class ProjectEngine : KnowledgeEngine
    {
        public override Knowledge GetKnowledge(string id)
        {
            return new Knowledge
            {
            };
        }

        protected override void OnDatabaseOpeningOverride(DatabaseSession session)
        {
            var database = session.Database;
            Preshapes     = database.GetCollection<Preshape>(Constants.Name_Preshape);
            AppraiseDB    = database.GetCollection<Appraise>(Constants.Name_Appraise);
            SentenceDB    = database.GetCollection<Sentence>(Constants.Name_Sentence);
            TimelineDB    = database.GetCollection<TimelineConcept>(Constants.Name_Timeline);
            TerminologyDB = database.GetCollection<Terminology>(Constants.Name_Terminology);
            SpaceDB       = database.GetCollection<SpaceConcept>(Constants.Name_SpaceConcept);
            SpaceRelDB    = database.GetCollection<SpaceConceptRelationship>(Constants.Name_Relationship_SpaceConcept);
            PrototypeDB   = database.GetCollection<Prototype>(Constants.Name_Prototype);
        }

        protected override void OnDatabaseClosingOverride()
        {
            PrototypeDB   = null;
            Preshapes     = null;
            AppraiseDB    = null;
            TimelineDB    = null;
            TerminologyDB = null;
            SpaceDB       = null;
            SpaceRelDB    = null;
        }

        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Preshape> Preshapes { get; private set; }

        /// <summary>
        /// 术语
        /// </summary>
        public ILiteCollection<Terminology> TerminologyDB { get; private set; }
    }
}