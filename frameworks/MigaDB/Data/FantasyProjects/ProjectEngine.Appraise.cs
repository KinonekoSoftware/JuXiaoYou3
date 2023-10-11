using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {
        

        #region Appraise

        public void AddAppraise(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            AppraiseDB.Upsert(item);
        }

        public void RemoveAppraise(Appraise item)
        {
            if (item is null)
            {
                return;
            }

            AppraiseDB.Delete(item.Id);
        }
        
        public void RemoveAppraise()
        {
            AppraiseDB.DeleteAll();
        }

        public IEnumerable<Appraise> GetAppraises(DocumentCache source)
        {
            if (source is null)
            {
                return AppraiseDB.Include(y => y.Source)
                                 .Include(a => a.Target)
                                 .Find(x => !x.Source.IsDeleted &&
                                            !x.Target.IsDeleted);
            }

            return AppraiseDB
                   .Include(y => y.Source)
                   .Include(a => a.Target)
                   .Find(x => x.Source.Id == source.Id &&
                              !x.Source.IsDeleted &&
                              !x.Target.IsDeleted);
        }

        #endregion

        #region MessageBoard

        

        public IEnumerable<Sentence> GetMessages()
        {
            var a = AppraiseDB.Include(y => y.Source)
                              .Include(a => a.Target)
                              .Find(x => !x.Source.IsDeleted ||
                                         !x.Target.IsDeleted);
            
            var b = SentenceDB.Include(y => y.Source)
                              .Find(x => !x.Source.IsDeleted);
            
            // ReSharper disable once InvokeAsExtensionMethod
            return Enumerable.Concat(a, b);
        } 
        #endregion

        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<Appraise> AppraiseDB { get; private set; }
    }
}