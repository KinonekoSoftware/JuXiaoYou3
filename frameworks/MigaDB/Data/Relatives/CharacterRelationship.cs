using Acorisoft.FutureGL.MigaDB.Documents;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaDB.Data.Relatives
{
    /// <summary>
    /// 人物关系
    /// </summary>
    public class CharacterRelationship : StorageUIObject, IEdge<DocumentCache>
    {
        private string        _callOfSource;
        private string        _callOfTarget;
        private int           _friendliness;
        private bool          _directRelative;
        private bool          _collateralRelative;
        private bool          _conjugalRelative;
        private DocumentCache _target;
        private DocumentCache _source;

        public CharacterRelationship Switch()
        {
            IsSwitch                     = true;
            (CallOfTarget, CallOfSource) = (CallOfSource, CallOfTarget);
            (_target, _source)           = (_source, _target);
            return this;
        }

        public void Reset()
        {
            IsSwitch                     = false;
            (CallOfTarget, CallOfSource) = (CallOfSource, CallOfTarget);
            (_target, _source)           = (_source, _target);
        }

        public bool IsSwitch { get; private set; }

        /// <summary>
        /// 旁系亲属
        /// </summary>
        public bool CollateralRelative
        {
            get => _collateralRelative;
            set => SetValue(ref _collateralRelative, value);
        }

        /// <summary>
        /// 夫妻关系
        /// </summary>
        public bool ConjugalRelative
        {
            get => _conjugalRelative;
            set => SetValue(ref _conjugalRelative, value);
        }

        /// <summary>
        /// 直系亲属
        /// </summary>
        public bool DirectRelative
        {
            get => _directRelative;
            set => SetValue(ref _directRelative, value);
        }

        /// <summary>
        /// 类型
        /// </summary>
        public DocumentType Type { get; init; }

        /// <summary>
        /// 友善度
        /// </summary>
        public int Friendliness
        {
            get => _friendliness;
            set => SetValue(ref _friendliness, value);
        }

        /// <summary>
        /// 右边的称呼
        /// </summary>
        public string CallOfTarget
        {
            get => _callOfTarget;
            set => SetValue(ref _callOfTarget, value);
        }

        /// <summary>
        /// 左边的称呼
        /// </summary>
        public string CallOfSource
        {
            get => _callOfSource;
            set => SetValue(ref _callOfSource, value);
        }

        /// <summary>
        /// 源
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Source
        {
            get => _source;
            init => _source = value;
        }

        /// <summary>
        /// 目标
        /// </summary>
        [BsonRef(Constants.Name_Cache_Document)]
        public DocumentCache Target
        {
            get => _target;
            init => _target = value;
        }
    }
}