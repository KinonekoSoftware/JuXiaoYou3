using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;
// ReSharper disable All

namespace Acorisoft.FutureGL.MigaDB.Core.Migrations
{
    public class UpdaterOfVer001 : DatabaseUpdater
    {
        protected override void Execute(IDatabase database)
        {
            var db = database.GetLiteDatabase();
            db.DropCollection(Constants.Name_FileTable);

        }

        private static void ExecuteUpdate_RelationshipDefinition(IDatabase database)
        {

        }

        public override int TargetVersion => 0;
        
        public override int ResultVersion => 1;
    }
}