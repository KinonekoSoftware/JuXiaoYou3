using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acorisoft.FutureGL.Forest.Models;
using Acorisoft.FutureGL.MigaUtils.Collections;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public abstract class EntityTabViewModel<TEntity> : TabViewModel
    {
        private TEntity _selected;

        protected EntityTabViewModel()
        {
            ApprovalRequired = false;
            Collection       = new ObservableCollection<TEntity>();
            AddCommand       = AsyncCommand(AddEntityImpl);
            EditCommand      = AsyncCommand<TEntity>(EditEntityImpl);
            RemoveCommand    = AsyncCommand<TEntity>(RemoveEntityImpl);
            ClearCommand     = AsyncCommand(ClearEntityImpl);
            SaveCommand      = Command(Save);
            ShiftUpCommand   = Command<TEntity>(ShiftUpEntityImpl);
            ShiftDownCommand = Command<TEntity>(ShiftDownEntityImpl);
        }

        private async Task AddEntityImpl()
        {
            var r = await Add();
            if (!r.IsFinished)
            {
                return;
            }

            Collection.Add(r.Value);
        }


        private async Task EditEntityImpl(TEntity entity)
        {
            if (entity is null)
            {
                return;
            }
            await Edit(entity);
            SetDirtyState();
        }

        private async Task RemoveEntityImpl(TEntity entity)
        {
            if (entity is null)
            {
                return;
            }
            
            if (!await this.Error(Language.GetText("text.AreYouSureRemoveIt")))
            {
                return;
            }

            Remove(entity);
            SetDirtyState();

            //
            //
            if (ReferenceEquals(Selected, entity))
            {
                Selected = default(TEntity);
            }
        }

        private async Task ClearEntityImpl()
        {
            if (!await this.Error(Language.GetText("text.AreYouSureClearIt")))
            {
                return;
            }

            ClearEntity(Collection.ToArray());
            Collection.Clear();
            SetDirtyState();
        }

        private void ShiftUpEntityImpl(TEntity entity)
        {
            if (entity is null)
            {
                return;
            }
            Collection.ShiftUp(entity, ShiftUp);
            SetDirtyState();
        }


        private void ShiftDownEntityImpl(TEntity entity)
        {
            if (entity is null)
            {
                return;
            }
            Collection.ShiftDown(entity, ShiftDown);
            SetDirtyState();
        }

        #region OnStart / OnResume

        /// <summary>
        /// 需要同步数据源
        /// </summary>
        /// <returns>true为需要，false为不需要</returns>
        protected abstract bool NeedDataSourceSynchronize();

        /// <summary>
        /// 请求同步数据源
        /// </summary>
        /// <param name="dataSource">需要同步的数据源</param>
        protected abstract void OnRequestDataSourceSynchronize(ICollection<TEntity> dataSource);

        protected override void OnStart()
        {
            OnRequestDataSourceSynchronize(Collection);
            base.OnStart();
        }

        protected override void OnResume()
        {
            if (NeedDataSourceSynchronize())
            {
                OnRequestDataSourceSynchronize(Collection);
            }
        }

        #endregion

        protected abstract void Save();
        protected abstract Task<Op<TEntity>> Add();
        protected abstract Task Edit(TEntity entity);
        protected abstract void Remove(TEntity entity);
        protected abstract void ShiftUp(TEntity entity, int oldIndex, int newIndex);
        protected abstract void ShiftDown(TEntity entity, int oldIndex, int newIndex);
        protected abstract void ClearEntity(TEntity[] entities);

        protected void UpdateCommands()
        {
            AddCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
            ClearCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
            ShiftUpCommand.NotifyCanExecuteChanged();
            ShiftDownCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public TEntity Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                UpdateCommands();
            }
        }

        public ObservableCollection<TEntity> Collection { get; }
        public AsyncRelayCommand AddCommand { get; }
        public AsyncRelayCommand<TEntity> EditCommand { get; }
        public AsyncRelayCommand<TEntity> RemoveCommand { get; }
        public AsyncRelayCommand ClearCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand<TEntity> ShiftUpCommand { get; }
        public RelayCommand<TEntity> ShiftDownCommand { get; }
    }

    public enum MultipleEntityOperator
    {
        Entity1,
        Entity2,
        Entity3,
        Entity4,
        Entity5,
    }
    
    public abstract class EntityTabViewModel<TEntity, TEntity1, TEntity2> : TabViewModel
        where TEntity : class
        where TEntity1 : TEntity
        where TEntity2 : TEntity

    {
        private TEntity _selected;

        protected EntityTabViewModel()
        {
            ApprovalRequired = false;
            Collection       = new ObservableCollection<TEntity>();
            AddCommand       = AsyncCommand<MultipleEntityOperator>(AddEntityImpl);
            EditCommand      = AsyncCommand<TEntity>(EditEntityImpl);
            RemoveCommand    = AsyncCommand<TEntity>(RemoveEntityImpl);
            ClearCommand     = AsyncCommand(ClearEntityImpl);
            SaveCommand      = Command(Save);
            ShiftUpCommand   = Command<TEntity>(ShiftUpEntityImpl);
            ShiftDownCommand = Command<TEntity>(ShiftDownEntityImpl);
        }

        private async Task AddEntityImpl(MultipleEntityOperator type)
        {
            TEntity value = default(TEntity);
            
            if (type == MultipleEntityOperator.Entity1)
            {
                var r = await AddEntity1();
                if (!r.IsFinished)
                {
                    return;
                }

                value = r.Value;
            }
            
            
            if (type == MultipleEntityOperator.Entity2)
            {
                var r = await AddEntity2();
                if (!r.IsFinished)
                {
                    return;
                }


                value = r.Value;
            }
            
            Collection.Add(value);
        }
        
        private async Task EditEntityImpl(TEntity entity)
        {
            await Edit(entity);
        }

        private async Task RemoveEntityImpl(TEntity entity)
        {
            if (!await this.Error(Language.GetText("text.AreYouSureRemoveIt")))
            {
                return;
            }

            Remove(entity);

            //
            //
            if (ReferenceEquals(Selected, entity))
            {
                Selected = default(TEntity);
            }
        }

        private async Task ClearEntityImpl()
        {
            if (!await this.Error(Language.GetText("text.AreYouSureClearIt")))
            {
                return;
            }

            ClearEntity(Collection.ToArray());
            Collection.Clear();
        }

        private void ShiftUpEntityImpl(TEntity entity)
        {
            Collection.ShiftUp(entity, ShiftUp);
        }


        private void ShiftDownEntityImpl(TEntity entity)
        {
            Collection.ShiftDown(entity, ShiftDown);
        }

        #region OnStart / OnResume

        /// <summary>
        /// 需要同步数据源
        /// </summary>
        /// <returns>true为需要，false为不需要</returns>
        protected abstract bool NeedDataSourceSynchronize();

        /// <summary>
        /// 请求同步数据源
        /// </summary>
        /// <param name="dataSource">需要同步的数据源</param>
        protected abstract void OnRequestDataSourceSynchronize(ICollection<TEntity> dataSource);

        protected override void OnStart()
        {
            OnRequestDataSourceSynchronize(Collection);
            base.OnStart();
        }

        protected override void OnResume()
        {
            if (NeedDataSourceSynchronize())
            {
                OnRequestDataSourceSynchronize(Collection);
            }
        }

        #endregion

        protected abstract void Save();
        protected abstract Task<Op<TEntity1>> AddEntity1();
        protected abstract Task<Op<TEntity2>> AddEntity2();
        protected abstract Task Edit(TEntity entity);
        protected abstract void Remove(TEntity entity);
        protected abstract void ShiftUp(TEntity entity, int oldIndex, int newIndex);
        protected abstract void ShiftDown(TEntity entity, int oldIndex, int newIndex);
        protected abstract void ClearEntity(TEntity[] entities);

        protected void UpdateCommands()
        {
            AddCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
            ClearCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
            ShiftUpCommand.NotifyCanExecuteChanged();
            ShiftDownCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 获取或设置 <see cref="Selected"/> 属性。
        /// </summary>
        public TEntity Selected
        {
            get => _selected;
            set
            {
                SetValue(ref _selected, value);
                UpdateCommands();
            }
        }

        public ObservableCollection<TEntity> Collection { get; }
        public AsyncRelayCommand<MultipleEntityOperator> AddCommand { get; }
        public AsyncRelayCommand<TEntity> EditCommand { get; }
        public AsyncRelayCommand<TEntity> RemoveCommand { get; }
        public AsyncRelayCommand ClearCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand<TEntity> ShiftUpCommand { get; }
        public RelayCommand<TEntity> ShiftDownCommand { get; }
    }
}