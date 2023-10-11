using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Forest.Tests
{
    [TestClass, TestCategory("Infrastructure")]
    public class TabControllerUnitTest
    {
        class TestTabController : TabController
        {
            
        }

        class Tab1 : TabViewModel
        {
            public override bool Uniqueness => true;
        }

        class Tab2 : TabViewModel
        {
        }

        class Tab3 : TabViewModel
        {
        }
        
        public static TViewModel Test<TViewModel>(TabController controller) where TViewModel : TabViewModel
        {
            var vm = Activator.CreateInstance<TViewModel>();
            vm.Start(NavigationParameter.New(vm, controller));
            controller.Start(vm);
            return vm;
        }

        [TestMethod]
        public void AddTabItem()
        {
            var ttc  = new TestTabController();
            var tab1 = Test<Tab1>(ttc);
            
            //
            // 当前
            Assert.IsTrue(ReferenceEquals(tab1, ttc.CurrentViewModel));
            
            var tab2 = Test<Tab2>(ttc);
            Assert.IsTrue(ReferenceEquals(tab2, ttc.CurrentViewModel));
            
            //
            // 避免不是唯一实例却是相同的引用
            var tab2_1 = Test<Tab2>(ttc);
            Assert.IsTrue(ReferenceEquals(tab2_1, ttc.CurrentViewModel));
            Assert.IsFalse(ReferenceEquals(tab2, ttc.CurrentViewModel));
            
            //
            // 确定唯一实例仍然是相同引用
            ttc.Start(tab1);
            Assert.IsTrue(ReferenceEquals(tab1, ttc.CurrentViewModel));
            
            //
            // 避免不是唯一实例却是相同的引用
            var tab3 = Test<Tab3>(ttc);
            Assert.IsTrue(ReferenceEquals(tab3, ttc.CurrentViewModel));
        }

        [TestMethod]
        public void RemoveTabWithPromise()
        {
            var ttc  = new TestTabController();
            var tab1 = Test<Tab2>(ttc);
            var tab2 = Test<Tab2>(ttc);
            var tab3 = Test<Tab2>(ttc);
            var tab4 = Test<Tab2>(ttc);
            var tab5 = new Tab2();


            var count = ttc.Workspace.Count;
            ttc.RemoveTabWithPromise(tab5);
            
            //
            // 并没有减少
            Assert.IsTrue(count == ttc.Workspace.Count);
            
            //
            // 确定索引
            Assert.IsTrue(ReferenceEquals(tab1, ttc.Workspace[0]));
            Assert.IsTrue(ReferenceEquals(tab2, ttc.Workspace[1]));
            Assert.IsTrue(ReferenceEquals(tab3, ttc.Workspace[2]));
            Assert.IsTrue(ReferenceEquals(tab4, ttc.Workspace[3]));
            
            //
            // First-Index Remove
            ttc.CurrentViewModel = ttc.Workspace[0];
            ttc.RemoveTabWithPromise(tab1);
            Assert.IsTrue(ReferenceEquals(tab2, ttc.CurrentViewModel));
            
            //
            // restore
            ttc.Workspace.Add(tab1);
            
            //
            // Last-Index Remove
            ttc.CurrentViewModel = ttc.Workspace[ttc.Workspace.IndexOf(tab3)];
            ttc.RemoveTabWithPromise(tab4);
            Assert.IsTrue(ReferenceEquals(tab3, ttc.CurrentViewModel));
            
            
            //
            // restore
            ttc.Workspace.Add(tab4);


            //
            // center-index Remove
            ttc.RemoveTabWithPromise(tab2);
            Assert.IsTrue(ReferenceEquals(tab3, ttc.CurrentViewModel));
            
            //
            // restore
            ttc.Workspace.Add(tab2);
        }
    }
}