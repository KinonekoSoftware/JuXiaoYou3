using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.UnitTests;
using Acorisoft.FutureGL.MigaDB.Utils;
// ReSharper disable JoinDeclarationAndInitializer

namespace Acorisoft.FutureGL.MigaDB.Tests.Engines.Documents
{
    [TestClass, TestCategory("Engines")]
    public class DocumentEngineUnitTest
    {
        [TestMethod]
        public void AddDocumentWasCorrection()
        {
            var adapter = new DataEngineAdapter<DocumentEngine>(new DocumentEngine());
            var engine = adapter.Engine;
            adapter.Start();

            
            adapter.Stop();
        }
        
        
        [TestMethod]
        public void UpdateDocumentWasCorrection()
        {
            var adapter = new DataEngineAdapter<DocumentEngine>(new DocumentEngine());
            var engine = adapter.Engine;
            adapter.Start();

           
            adapter.Stop();
        }
        
        [TestMethod]
        public void RemoveDocumentWasCorrection()
        {
            var adapter = new DataEngineAdapter<DocumentEngine>(new DocumentEngine());
            var engine = adapter.Engine;
            adapter.Start();
            
            adapter.Stop();
        }
    }
}