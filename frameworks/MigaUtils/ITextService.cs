namespace Acorisoft.FutureGL.MigaUtils
{
    public interface ITextService
    {
        /// <summary>
        /// 是否使用语言服务
        /// </summary>
        /// <returns></returns>
        bool UseLanguageService();
        
        /// <summary>
        /// 获得文本源
        /// </summary>
        /// <returns></returns>
        string GetTextSource();
    }
}