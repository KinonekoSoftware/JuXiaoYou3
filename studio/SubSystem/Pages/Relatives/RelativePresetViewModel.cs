using Acorisoft.FutureGL.MigaDB;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data;
using Acorisoft.FutureGL.MigaDB.Data.Relatives;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relatives
{
    public class RelativePresetViewModel : EntityTabViewModel<RelativePreset>
    {
        protected override bool NeedDataSourceSynchronize() => true;

        protected override void OnRequestDataSourceSynchronize(ICollection<RelativePreset> dataSource)
        {
            var db = Studio.DatabaseManager()
                         .Database
                         .CurrentValue;
            var property = db.Get<PresetProperty>();
            dataSource.AddMany(property.RelativePresets, true);
        }

        protected override void Save()
        { 
            var db = Studio.DatabaseManager()
                       .Database
                       .CurrentValue;
            var property = db.Get<PresetProperty>();
            property.RelativePresets.AddMany(Collection, true);
            db.Upsert(property);
            SetDirtyState(false);
        }

        protected override Task<Op<RelativePreset>> Add()
        {
            return NewRelativePresetViewModel.New();
        }

        protected override async Task Edit(RelativePreset entity)
        {
            await NewRelativePresetViewModel.Edit(entity);
        }

        protected override void Remove(RelativePreset entity)
        {
            Collection.Remove(entity);
        }

        protected override void ShiftUp(RelativePreset entity, int oldIndex, int newIndex)
        {
            Collection.Move(oldIndex, newIndex);
        }

        protected override void ShiftDown(RelativePreset entity, int oldIndex, int newIndex)
        {
            Collection.Move(oldIndex, newIndex);
        }

        protected override void ClearEntity(RelativePreset[] entities)
        {
        }
        
        
        public sealed override bool Uniqueness => true;
    }
}