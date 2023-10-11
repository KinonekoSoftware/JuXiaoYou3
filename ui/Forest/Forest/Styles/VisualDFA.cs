namespace Acorisoft.FutureGL.Forest.Styles
{
    /// <summary>
    /// 表示一个视觉DFA状态机。
    /// </summary>
    public class VisualDFA
    {
        /// <summary>
        /// 表示一个迁移函数。
        /// </summary>
        public class Transition
        {
            /// <summary>
            /// 获取当前迁移函数的源状态
            /// </summary>
            public VisualState FromState { get; init; }
            
            /// <summary>
            /// 迁移函数
            /// </summary>
            internal Dictionary<VisualStateTrigger, VisualState> InternalTransitions { get; init; }

            /// <summary>
            /// 
            /// </summary>
            public IReadOnlyDictionary<VisualStateTrigger, VisualState> Transitions => InternalTransitions;
        }

        private readonly Dictionary<VisualState, Transition> _transitions = new Dictionary<VisualState, Transition>();

        private bool        _init;
        private VisualState _last;
        private VisualState _now;
        
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="now">当前状态</param>
        /// <param name="next">下一个状态</param>
        /// <param name="function">迁移函数</param>
        /// <param name="canBackward">是否默认添加后退</param>
        /// <returns>返回操作的结果，成功返回true，否则返回false。</returns>
        public bool AddState(VisualState now, VisualState next, VisualStateTrigger function, bool canBackward = true)
        {
            Transition transition;
            
            if (canBackward)
            {
                if (!_transitions.TryGetValue(next, out transition))
                {
                    transition = new Transition
                    {
                        FromState = next,
                        InternalTransitions = new Dictionary<VisualStateTrigger, VisualState>
                        {
                            { VisualStateTrigger.Back, now }
                        }
                    };
                
                    _transitions.TryAdd(next, transition);
                }
                else
                {
                    transition.InternalTransitions.TryAdd(VisualStateTrigger.Back, now);
                }
            }

            if (!_transitions.TryGetValue(now, out transition))
            {
                transition = new Transition
                {
                    FromState = now,
                    InternalTransitions = new Dictionary<VisualStateTrigger, VisualState>
                    {
                        { function, next }
                    }
                };
                
                return _transitions.TryAdd(now, transition);
            }

            return transition.InternalTransitions.TryAdd(function, next);
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="now">当前状态</param>
        /// <param name="next">下一个状态</param>
        /// <param name="function">迁移函数</param>
        /// <returns>返回操作的结果，成功返回true，否则返回false。</returns>
        public bool RemoveState(VisualState now, VisualState next, VisualStateTrigger function)
        {
            var result = false;
            
            if (_transitions.TryGetValue(now, out var transition))
            {
                result = transition.InternalTransitions.Remove(function);
            }

            if (!result)
            {
                return false;
            }
            
            if (_transitions.TryGetValue(next, out transition))
            {
                result |= transition.InternalTransitions.Remove(VisualStateTrigger.Back);
            }


            return result;
        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        /// <returns>返回操作的结果，成功返回true，否则返回false。</returns>
        public void NextState()
        {
            _init = true;
            _last = _now = VisualState.Normal;
            StateChangedHandler?.Invoke(false, VisualState.Normal, VisualState.Normal, VisualStateTrigger.Next);
        }

        /// <summary>
        /// 尝试进入下一个状态。
        /// </summary>
        /// <param name="trigger">触发器</param>
        /// <returns>返回操作的结果，成功返回true，否则返回false。</returns>
        public bool NextState(VisualStateTrigger trigger)
        {
            if (!_transitions.TryGetValue(_now, out var transition))
            {
                return false;
            }

            if (!transition.InternalTransitions.TryGetValue(trigger, out var nextState))
            {
                return false;
            }

            _last = _now;
            _now  = nextState;
            StateChangedHandler?.Invoke(_init, _last, _now, trigger);
            return true;
        }

        public void NextState(VisualState state, bool force = true)
        {
            _init = true;
            _last = _now;
            _now  = state;

            if (force)
            {
                StateChangedHandler?.Invoke(_init, _last, _now, VisualStateTrigger.Next);
            }
        }

        /// <summary>
        /// 重新进入相同的状态
        /// </summary>
        public void GotoState()
        {
            StateChangedHandler?.Invoke(true, _last, _now, VisualStateTrigger.Next);
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        /// <returns>返回操作的结果，成功返回true，否则返回false。</returns>
        public void ResetState()
        {
            _last = _now;
            _now  = VisualState.Normal;
            StateChangedHandler?.Invoke(_init, _last, _now, VisualStateTrigger.Next);
        }
        
        /// <summary>
        /// 状态变更时的函数
        /// </summary>
        public NewVisualStateHandler StateChangedHandler { get; set; }

        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool HasInitialized => _init;

        /// <summary>
        /// 上一个状态
        /// </summary>
        public VisualState? LastState => _init ? _last : null;
        
        /// <summary>
        /// 当前状态
        /// </summary>
        public VisualState? CurrentState  => _init ? _now : null;

        /// <summary>
        /// 所有迁移函数
        /// </summary>
        public IReadOnlyDictionary<VisualState, Transition> Transitions => _transitions;

        public override string ToString()
        {
            return CurrentState.ToString();
        }
    }
}