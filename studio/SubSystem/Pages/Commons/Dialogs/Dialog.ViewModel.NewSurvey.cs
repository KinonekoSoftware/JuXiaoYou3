namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class NewSurveyViewModel : ImplicitDialogVM
    {
        private string _name;
        private string _intro;

        protected override bool IsCompleted()
        {
            if (Source is null)
            {
                return !string.IsNullOrEmpty(Name) &&
                       !string.IsNullOrEmpty(Intro);
            }

            return true;
        }

        public static Task<Op<SurveySet>> NewSet()
        {
            return DialogService().Dialog<SurveySet, NewSurveyViewModel>(new NewSurveyViewModel(), new Parameter
            {
                Args = new[]
                {
                    Boxing.True,
                }
            });
        }

        public static Task<Op<Survey>> New()
        {
            return DialogService().Dialog<Survey, NewSurveyViewModel>(new NewSurveyViewModel(), new Parameter
            {
                Args = new[]
                {
                    Boxing.False,
                }
            });
        }

        public static Task<Op<SurveySet>> Edit(SurveySet set)
        {
            return DialogService().Dialog<SurveySet, NewSurveyViewModel>(new NewSurveyViewModel(), new Parameter
            {
                Args = new[]
                {
                    Boxing.True,
                    set
                }
            });
        }

        public static Task<Op<Survey>> Edit(Survey item)
        {
            return DialogService().Dialog<Survey, NewSurveyViewModel>(new NewSurveyViewModel(), new Parameter
            {
                Args = new[]
                {
                    Boxing.False,
                    item
                }
            });
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            IsSet = (bool)a[0];

            if (a.Length > 1 && a[1] is not null)
            {
                Source = a[1];

                if (Source is SurveySet ss)
                {
                    Name  = ss.Name;
                    Intro = ss.Intro;
                }

                if (Source is Survey s)
                {
                    Name  = s.Name;
                    Intro = s.Intro;
                }
            }
        }

        protected override void Finish()
        {
            if (Source is SurveySet se)
            {
                se.Intro = Intro;
                se.Name  = Name;
                Result   = Source;
                return;
            }

            if (Source is Survey s)
            {
                s.Intro = Intro;
                s.Name  = Name;
                Result  = Source;
                return;
            }

            if (IsSet)
            {
                Result = new SurveySet
                {
                    Id    = ID.Get(),
                    Items = new ObservableCollection<Survey>(),
                    Name  = Name,
                    Intro = Intro,
                };
            }
            else
            {
                Result = new Survey
                {
                    Id    = ID.Get(),
                    Name  = Name,
                    Intro = Intro,
                };
            }
        }

        protected override string Failed()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return SubSystemString.EmptyName;
            }

            return string.IsNullOrEmpty(Intro) ? SubSystemString.EmptyIntro : SubSystemString.Unknown;
        }

        /// <summary>
        /// 源
        /// </summary>
        public object Source { get; private set; }

        public bool IsSet { get; private set; }

        /// <summary>
        /// 获取或设置 <see cref="Intro"/> 属性。
        /// </summary>
        public string Intro
        {
            get => _intro;
            set { SetValue(ref _intro, value); }
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set { SetValue(ref _name, value); }
        }
    }
}