using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public class Appraise : Sentence
    {

        /// <summary>
        /// 发言的目标对象
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Target { get; init; }
    }
}