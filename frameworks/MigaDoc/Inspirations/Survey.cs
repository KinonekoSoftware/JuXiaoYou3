using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Inspirations
{
    public class SurveySet
    {
        
        private string _summary;

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }

        public ObservableCollection<Survey> Surveys { get; init; }
    }

    public class Survey
    {
        private string _value;
        
        private string _summary;

        /// <summary>
        /// 获取或设置 <see cref="Summary"/> 属性。
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value { get; set; }
    }
}