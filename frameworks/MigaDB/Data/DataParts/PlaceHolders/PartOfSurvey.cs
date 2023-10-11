using System.Collections.ObjectModel;

namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class SurveySet : StorageUIObject
    {
        private string _name;
        private string _intro;

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
        
        public ObservableCollection<Survey> Items { get; init; }
    }

    public class Survey : StorageUIObject
    {
        private string _name;
        private string _intro;
        private string _value;

        /// <summary>
        /// 获取或设置 <see cref="Value"/> 属性。
        /// </summary>
        public string Value
        {
            get => _value;
            set => SetValue(ref _value, value);
        }
        
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
    
    public class PartOfSurvey : PartOfEditableDetail
    {
        public PartOfSurvey()
        {
            Id = Constants.IdOfSurveyPart;
        }

        public List<SurveySet> Items { get; set; }
    }
}