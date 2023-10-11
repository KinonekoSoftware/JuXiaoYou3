using System.Windows;
using System.Windows.Data;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Converters;

namespace Forest.Tests.Converters
{
    [TestClass, TestCategory("Converters")]
    public class BooleanConverterUnitTest
    {
        [TestMethod]
        public void Test_VisibilityWorkCorrection()
        {
            IValueConverter converter = new BooleanTrueToVisibilityConverter();
            
            Assert.AreEqual(converter.Convert(Boxing.True, null, null, null), (object)Visibility.Visible);
            Assert.AreEqual(converter.Convert(Boxing.False, null, null, null), (object)Visibility.Collapsed);
            Assert.AreEqual(converter.Convert(true, null, null, null), (object)Visibility.Visible);
            Assert.AreEqual(converter.Convert(false, null, null, null), (object)Visibility.Collapsed);
            Assert.AreEqual(converter.Convert(null, null, null, null), (object)Visibility.Collapsed);
            
            
            converter = new BooleanFalseToVisibilityConverter();
            
            Assert.AreEqual(converter.Convert(Boxing.True, null, null, null), (object)Visibility.Collapsed);
            Assert.AreEqual(converter.Convert(Boxing.False, null, null, null), (object)Visibility.Visible);
            Assert.AreEqual(converter.Convert(true, null, null, null), (object)Visibility.Collapsed);
            Assert.AreEqual(converter.Convert(false, null, null, null), (object)Visibility.Visible);
            Assert.AreEqual(converter.Convert(null, null, null, null), (object)Visibility.Collapsed);
        }
    }
}