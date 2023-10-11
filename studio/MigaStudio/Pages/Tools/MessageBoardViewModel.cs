using System.Collections.Generic;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Views;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{
    public class MessageBoardViewModel : EntityTabViewModel<Sentence, Sentence, Appraise>
    {
        public MessageBoardViewModel()
        {
            ProjectEngine    = Studio.Engine<ProjectEngine>();
            ApprovalRequired = false;
        }

        protected sealed override bool NeedDataSourceSynchronize()
        {
            return Version != ProjectEngine.Version;
        }

        protected override void OnRequestDataSourceSynchronize(ICollection<Sentence> dataSource)
        {
            var collection = ProjectEngine.GetMessages();
            dataSource.AddMany(collection, true);
        }

        protected override void Save()
        {
        }

        protected override async Task<Op<Sentence>> AddEntity1()
        {
            var source = await SubSystem.Select(DocumentType.Character);

            if (!source.IsFinished)
            {
                return Op<Sentence>.Failed("用户取消");
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle);

            if (!r1.IsFinished)
            {
                return Op<Sentence>.Failed("用户取消");
            }
            
            var Sentence = new Sentence
            {
                Id      = ID.Get(),
                Source  = source.Value,
                Content = r1.Value 
            };
            
            ProjectEngine.AddSentence(Sentence);
            return Op<Sentence>.Success(Sentence);
        }
        
        protected override async Task<Op<Appraise>> AddEntity2()
        {
            var source = await SubSystem.Select(DocumentType.Character);

            if (!source.IsFinished)
            {
                return Op<Appraise>.Failed("用户取消");
            }

            var target = await SubSystem.SelectExclude(DocumentType.Character, source.Value
                                                                                     .Id);
            
            if (!target.IsFinished)
            {
                return Op<Appraise>.Failed("用户取消");
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle);

            if (!r1.IsFinished)
            {
                return Op<Appraise>.Failed("用户取消");
            }
            
            var Sentence = new Appraise
            {
                Id      = ID.Get(),
                Target  = target.Value,
                Source  = source.Value,
                Content = r1.Value 
            };
            
            ProjectEngine.AddSentence(Sentence);
            return Op<Appraise>.Success(Sentence);
        }
        

        protected override async Task Edit(Sentence item)
        {
            if (item is null)
            {
                return;
            }

            var r1 = await MultiLineViewModel.String(SubSystemString.EditValueTitle, item.Content);

            if (!r1.IsFinished)
            {
                return;
            }
            
            item.Content = r1.Value;
            
            if (item is Appraise a)
            {
                ProjectEngine.AddAppraise(a);
            }
            else
            {
                ProjectEngine.AddSentence(item);
            }
        }

        protected override void Remove(Sentence entity)
        {
            if (entity is Appraise a)
            {
                ProjectEngine.RemoveSentence(a);
                ProjectEngine.RemoveAppraise(a);
            }
            else
            {
                ProjectEngine.RemoveSentence(entity);
            }

            Collection.Remove(entity);
        }

        protected override void ShiftUp(Sentence entity, int oldIndex, int newIndex)
        {
            
        }

        protected override void ShiftDown(Sentence entity, int oldIndex, int newIndex)
        {
        }

        protected override void ClearEntity(Sentence[] entities)
        {
            ProjectEngine.RemoveAppraise();
        }
        
        public ProjectEngine ProjectEngine { get; }
        
        
        
        public override bool Uniqueness => true;
    }
}