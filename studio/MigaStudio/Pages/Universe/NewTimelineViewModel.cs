using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.MigaDB.Data.FantasyProjects;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class NewTimelineViewModel : ImplicitDialogVM
    {
        private string _name;
        private string _intro;

        public static Task<Op<TimelineConcept>> NewAge()
        {
            return DialogService().Dialog<TimelineConcept, NewTimelineViewModel>(new Parameter
            {
                Args = new[]
                {
                    Boxing.True
                }
            });
        }

        public static Task<Op<TimelineConcept>> NewEvent()
        {
            return DialogService().Dialog<TimelineConcept, NewTimelineViewModel>(new Parameter
            {
                Args = new[]
                {
                    Boxing.False
                }
            });
        }

        public static Task<Op<TimelineConcept>> Edit(TimelineConcept concept)
        {
            return DialogService().Dialog<TimelineConcept, NewTimelineViewModel>(new Parameter
            {
                Args = new object[]
                {
                    concept
                }
            });
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;

            IsEditMode = a.Length > 0 && a[0] is TimelineConcept;

            if (IsEditMode)
            {
                Source = (TimelineConcept)a[0];
                Name   = Source.Name;
                Intro  = Source.Intro;
            }
            else
            {
                IsAge = a[0] is bool b && b;
            }
        }

        protected override bool IsCompleted() => !string.IsNullOrEmpty(Name) &&
                                                 !string.IsNullOrEmpty(Intro);

        protected override void Finish()
        {
            if (IsEditMode)
            {
                Source.Name  = Name;
                Source.Intro = Intro;
                Result       = Source;
                return;
            }

            #if DEBUG
            
            if (IsAge)
            {
                Result = new TimelineAge
                {
                    Id    = Name,
                    Name  = Name,
                    Intro = Intro
                };
                return;
            }

            Result = new TimelineEvent
            {
                Id    = Name,
                Name  = Name,
                Intro = Intro
            };
            
            #else
            
            if (IsAge)
            {
                Result = new TimelineAge
                {
                    Id    = ID.Get(),
                    Name  = Name,
                    Intro = Intro
                };
                return;
            }

            Result = new TimelineEvent
            {
                Id    = ID.Get(),
                Name  = Name,
                Intro = Intro
            };
            #endif
        }

        protected override string Failed()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return SubSystemString.EmptyName;
            }
            
            if (string.IsNullOrEmpty(Intro))
            {
                return SubSystemString.EmptyIntro;
            }

            return SubSystemString.Unknown;
        }


        public bool IsAge { get; private set; }
        public TimelineConcept Source { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set => SetValue(ref _intro, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
    }
}