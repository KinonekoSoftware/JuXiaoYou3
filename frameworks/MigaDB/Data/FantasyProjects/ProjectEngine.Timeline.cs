using System.Diagnostics;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    partial class ProjectEngine
    {

        #region Timelines

        public void AddTimeline(TimelineConcept item)
        {
            if (item is null)
            {
                return;
            }

#if DEBUG
            Debug.WriteLine($"last:{item.LastItem}\ncurrent:{item.Id}\nnext:{item.NextItem}\n");
#endif

            TimelineDB.Upsert(item);
        }

        public void RemoveTimeline(TimelineConcept item)
        {
            if (item is null)
            {
                return;
            }

            TimelineDB.Delete(item.Id);
        }

        public IEnumerable<TimelineConcept> GetTimelines() => TimelineDB.FindAll();

        #endregion
        
        

        /// <summary>
        /// 时间线
        /// </summary>
        public ILiteCollection<TimelineConcept> TimelineDB { get; private set; }
    }
}