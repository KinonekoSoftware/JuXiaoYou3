namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface ISizable
    {
        /// <summary>
        /// 初次加载的时候期望的宽度。
        /// </summary>
        int Width { get; set; }
        
        /// <summary>
        /// 初次加载的时候期望的高度。
        /// </summary>
        int Height { get; set; }
    }
}