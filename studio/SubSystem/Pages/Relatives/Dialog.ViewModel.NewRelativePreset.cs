using Acorisoft.FutureGL.MigaDB.Data.Relatives;

// ReSharper disable RedundantArgumentDefaultValue

namespace Acorisoft.FutureGL.MigaStudio.Pages.Relatives
{
    public class NewRelativePresetViewModel : ImplicitDialogVM
    {
        public static Task<Op<RelativePreset>> New()
        {
            return DialogService().Dialog<RelativePreset, NewRelativePresetViewModel>();
        }

        public static Task<Op<RelativePreset>> Edit(RelativePreset rel)
        {
            return DialogService().Dialog<RelativePreset, NewRelativePresetViewModel>(new Parameter
            {
                Args = new object[]
                {
                    rel
                }
            });
        }


        private string _callOfSource;
        private string _callOfTarget;
        private bool   _isBidirection;
        private int    _friendliness;
        private bool   _directRelative;
        private bool   _conjugalRelative;
        private string _name;
        private bool   _collateralRelative;
        private bool   _otherRelative = true;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;
            IsEditMode = a.Length > 0 && a[0] is RelativePreset;

            if (IsEditMode)
            {
                Entity       = (RelativePreset)a[0];
                CallOfSource = Entity.CallOfSource;
                CallOfTarget = Entity.CallOfTarget;
            }

            base.OnStart(parameter);
        }


        protected override bool IsCompleted()
        {
            return !string.IsNullOrEmpty(CallOfSource) &&
                   !string.IsNullOrEmpty(CallOfTarget);
        }

        protected override void Finish()
        {
            if (IsEditMode)
            {
                Entity.CallOfSource       = CallOfSource;
                Entity.CallOfTarget       = CallOfTarget;
                Entity.CollateralRelative = CollateralRelative;
                Entity.DirectRelative     = DirectRelative;
                Entity.ConjugalRelative   = ConjugalRelative;
                Entity.Name               = Name;
                Result                    = Entity;
            }
            else
            {
                Result = new RelativePreset
                {
                    Id               = ID.Get(), CollateralRelative = CollateralRelative,
                    CallOfSource     = CallOfSource,
                    CallOfTarget     = CallOfTarget,
                    ConjugalRelative = ConjugalRelative,
                    DirectRelative   = DirectRelative,
                    Name             = Name
                };
            }
        }

        protected override string Failed()
        {
            if (string.IsNullOrEmpty(CallOfSource))
            {
                return SubSystemString.EmptyName;
            }

            if (string.IsNullOrEmpty(CallOfTarget))
            {
                return SubSystemString.EmptyName;
            }

            return SubSystemString.Unknown;
        }

        public RelativePreset Entity { get; private set; }
        

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

         /// <summary>
        /// 旁系亲属
        /// </summary>
        public bool CollateralRelative
        {
            get => _collateralRelative;
            set
            {
                SetValue(ref _collateralRelative, value);
                
                
                _conjugalRelative = false;
                _otherRelative      = false;
                _directRelative     = false;
                RaiseUpdated(nameof(DirectRelative));
                RaiseUpdated(nameof(OtherRelative));
                RaiseUpdated(nameof(ConjugalRelative));
            }
        }

        /// <summary>
        /// 夫妻关系
        /// </summary>
        public bool ConjugalRelative
        {
            get => _conjugalRelative;
            set
            {
                SetValue(ref _conjugalRelative, value);
                
                _collateralRelative = false;
                _otherRelative      = false;
                _directRelative     = false;
                RaiseUpdated(nameof(DirectRelative));
                RaiseUpdated(nameof(OtherRelative));
                RaiseUpdated(nameof(CollateralRelative));
            }
        }

        /// <summary>
        /// 直系亲属
        /// </summary>
        public bool DirectRelative
        {
            get => _directRelative;
            set
            {
                SetValue(ref _directRelative, value);
                
                _collateralRelative = false;
                _conjugalRelative   = false;
                _otherRelative     = false;
                RaiseUpdated(nameof(OtherRelative));
                RaiseUpdated(nameof(ConjugalRelative));
                RaiseUpdated(nameof(CollateralRelative));
            }
        }


        /// <summary>
        /// 获取或设置 <see cref="OtherRelative"/> 属性。
        /// </summary>
        public bool OtherRelative
        {
            get => _otherRelative;
            set
            {
                SetValue(ref _otherRelative, value);
                _collateralRelative = false;
                _conjugalRelative   = false;
                _directRelative     = false;
                RaiseUpdated(nameof(DirectRelative));
                RaiseUpdated(nameof(ConjugalRelative));
                RaiseUpdated(nameof(CollateralRelative));
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="Friendliness"/> 属性。
        /// </summary>
        public int Friendliness
        {
            get => _friendliness;
            set => SetValue(ref _friendliness, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="IsBidirection"/> 属性。
        /// </summary>
        public bool IsBidirection
        {
            get => _isBidirection;
            set
            {
                SetValue(ref _isBidirection, value);

                var ns = string.IsNullOrEmpty(_callOfSource);
                var nt = string.IsNullOrEmpty(_callOfTarget);

                if (ns && nt)
                {
                    return;
                }

                if (ns)
                {
                    _callOfSource = _callOfTarget;
                    RaiseUpdated(nameof(CallOfSource));
                }
                else
                {
                    _callOfTarget = _callOfSource;
                    RaiseUpdated(nameof(CallOfTarget));
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="CallOfTarget"/> 属性。
        /// </summary>
        public string CallOfTarget
        {
            get => _callOfTarget;
            set
            {
                if (IsBidirection)
                {
                    _callOfTarget = value;
                    _callOfSource = value;
                    RaiseUpdated(nameof(CallOfTarget));
                    RaiseUpdated(nameof(CallOfSource));
                }
                else
                {
                    SetValue(ref _callOfTarget, value);
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="CallOfSource"/> 属性。
        /// </summary>
        public string CallOfSource
        {
            get => _callOfSource;
            set
            {
                if (IsBidirection)
                {
                    _callOfTarget = value;
                    _callOfSource = value;
                    RaiseUpdated(nameof(CallOfTarget));
                    RaiseUpdated(nameof(CallOfSource));
                }
                else
                {
                    SetValue(ref _callOfSource, value);
                }
            }
        }
    }
}