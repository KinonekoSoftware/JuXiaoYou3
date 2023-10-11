namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {
        public void AddSpace(SpaceConcept space)
        {
        }

        public void AddSpace(SpaceConcept space, SpaceConcept parent)
        {
            if (space is null ||
                parent is null)
            {
                return;
            }
            
            SpaceDB.Insert(space);
            var parentRels = SpaceRelDB.Find(x => x.SpaceConceptID == parent.Id)
                                       .Select(x => new SpaceConceptRelationship
                                       {
                                           SpaceConceptID = space.Id,
                                           ParentID       = x.ParentID,
                                           Height         = x.Height + 1
                                       });
            SpaceRelDB.Insert(parentRels);
            SpaceRelDB.Insert(new SpaceConceptRelationship
            {
                SpaceConceptID = space.Id,
                ParentID       = parent.Id,
                Height         = parent.Height + 1
            });
        }

        public ILiteCollection<SpaceConcept> SpaceDB { get; private set; }
        public ILiteCollection<SpaceConceptRelationship> SpaceRelDB { get; private set; }
    }
}