namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public class SpaceConceptRelationship
    {
        [BsonId]
        public string SpaceConceptID { get; init; }
        public string ParentID { get; init; }
        public int Height { get; init; }
    }
}