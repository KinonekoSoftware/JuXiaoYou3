using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Styles;

namespace Forest.Tests
{
    [TestClass, TestCategory("Infrastructure")]
    public class VisualStateUnitTest
    {
        /// <summary>
        /// 测试添加状态是否成功
        /// </summary>
        [TestMethod]
        public void AddStateWasIndeedAction()
        {
            var dfa = new VisualDFA();
            
            dfa.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            dfa.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            
            Assert.IsTrue(dfa.Transitions.ContainsKey(VisualState.Normal));
            Assert.IsTrue(dfa.Transitions.ContainsKey(VisualState.Highlight1));
            Assert.IsTrue(dfa.Transitions.ContainsKey(VisualState.Highlight2));
        }

        /// <summary>
        /// 测试状态迁移是否成功
        /// </summary>
        [TestMethod]
        public void StateTransitionWasCorrection()
        {
            var dfa = new VisualDFA();
            
            dfa.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            dfa.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            
            //
            dfa.NextState();
            Assert.IsTrue(dfa.CurrentState == VisualState.Normal);
            
            //
            NextStateAndBiDirectionCheck(dfa, VisualState.Normal, VisualState.Highlight1);
            NextStateAndBiDirectionCheck(dfa, VisualState.Highlight1, VisualState.Highlight2);
        }

        private static void NextStateAndCheck(VisualDFA dfa, VisualState state)
        {
            Assert.IsTrue(dfa.NextState(VisualStateTrigger.Next));
            Assert.IsTrue(dfa.CurrentState == state);
        }
        
        private static void NextStateAndBiDirectionCheck(VisualDFA dfa, VisualState lastState, VisualState nextState)
        {
            NextStateAndCheck(dfa, nextState);
            Assert.IsTrue(dfa.NextState(VisualStateTrigger.Back));
            Assert.IsTrue(dfa.CurrentState == lastState);
            Assert.IsTrue(dfa.NextState(VisualStateTrigger.Next));
        }
    }
}