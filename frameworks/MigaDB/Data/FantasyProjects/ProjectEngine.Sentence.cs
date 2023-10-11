
namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {

        #region Sentence

        public void AddSentence(Sentence item)
        {
            if (item is null)
            {
                return;
            }

            SentenceDB.Upsert(item);
        }

        public void RemoveSentence(Sentence item)
        {
            if (item is null)
            {
                return;
            }

            SentenceDB.Delete(item.Id);
        }

        public IEnumerable<Sentence> GetSentences(DocumentCache source)
        {
            if (source is null)
            {
                return GetSentences();
            }

            return SentenceDB
                   .Include(y => y.Source)
                   .Find(x => x.Source.Id == source.Id)
                   .Where(x => x.Source is not null &&
                               !x.Source.IsDeleted);
        }

        public IEnumerable<Sentence> GetSentences() => SentenceDB.FindAll();

        #endregion


        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Sentence> SentenceDB { get; private set; }
    }
}