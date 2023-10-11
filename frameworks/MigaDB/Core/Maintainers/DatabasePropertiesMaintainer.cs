using System.Collections.ObjectModel;
using Acorisoft.FutureGL.MigaDB.Data;
// ReSharper disable All

namespace Acorisoft.FutureGL.MigaDB.Core.Maintainers
{
    public class DatabasePropertiesMaintainer : IDatabaseMaintainer
    {
        public void Maintain(IDatabase database)
        {
            var timeOfBoth = DateTime.Now;

            database.IfSet<DatabaseProperty>(new DatabaseProperty
            {
                Author      = "Test",
                Name        = "Test",
                ForeignName = "Test"
            });


            database.IfSet<ColorServiceProperty>(new ColorServiceProperty
            {
                Mappings = new List<ColorMapping>(),
            });
            
            database.IfSet<DatabaseVersion>(new DatabaseVersion
            {
                TimeOfModified = timeOfBoth,
                TimeOfCreated  = timeOfBoth,
                Version        = Constants.MinVersion
            });
        }
    }
}