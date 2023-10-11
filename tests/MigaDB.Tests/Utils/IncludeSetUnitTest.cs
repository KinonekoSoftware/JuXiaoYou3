using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Tests.Utils
{
    [TestClass]
    public class IncludeSetUnitTest
    {
        [TestMethod]
        public void Include()
        {
            /*
             *          -> a_a_a
             *   -> a_a -> a_a_b
             * a -> a_b -> a_b_a
             *   -> a_c -> a_c_a
             *          -> a_c_b
             */
            var a     = "a";
            var a_a   = "a_a";
            var a_b   = "a_b";
            var a_c   = "a_c";
            var a_a_a = "a_a_a";
            var a_a_b = "a_a_b";
            var a_b_a = "a_b_a";
            var a_c_a = "a_c_a";
            var a_c_b = "a_c_b";

            var set = new IncludeSet();
            set.Add(a);
            set.Add(a, a_a);
            set.Add(a, a_b);
            set.Add(a, a_c);
            set.Add(a_a, a_a_a);
            set.Add(a_a, a_a_b);
            set.Add(a_b, a_b_a);
            set.Add(a_c, a_c_a);
            set.Add(a_c, a_c_b);
            
            HashSet<string> h;
            
            h = set.GetChildren(a);
            
            Assert.IsTrue(h.Contains(a_a_a));
            Assert.IsTrue(h.Contains(a_a_b));
            
            h = set.GetChildren(a_a);
            Assert.IsTrue(h.Contains(a_a_a));
            
            
            h = set.GetChildren(a_b);
            Assert.IsFalse(h.Contains(a_a_a));
        }
    }
}