using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.UnitTests;

namespace Acorisoft.FutureGL.MigaDB.Tests
{
    class DatabaseProperty
    {
        public int v { get; set; }
    }
    
    [TestClass, TestCategory("Database")]
    public class DatabaseUnitTest
    {
        [TestMethod]
        public void SetObjectAndGetObjectCorrection()
        {
            var db = new DatabaseAdapter();
            db.Open();

            var dp = db.Database.Set(new DatabaseProperty { v = 1 });
            Assert.IsTrue(db.Database.Get<DatabaseProperty>().v == dp.v);
            Assert.IsTrue(dp.v == 1);
            
            
            db.Reopen();
            dp = db.Database.Set(new DatabaseProperty { v = 1 });
            Assert.IsTrue(db.Database.Get<DatabaseProperty>().v == dp.v);
            Assert.IsTrue(dp.v == 1);
        }
    }
}