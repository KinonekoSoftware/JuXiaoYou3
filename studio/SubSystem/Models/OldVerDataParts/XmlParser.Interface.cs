namespace Acorisoft.Miga.Xml
{

    public interface IAddChild
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        void Accept(object instance);
    }

    public interface ITextNode
    {
        void AddText(string text);
    }

}