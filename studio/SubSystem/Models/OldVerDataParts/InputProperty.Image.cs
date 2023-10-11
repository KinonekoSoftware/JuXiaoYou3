using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("image")]
    public class ImageProperty2 : InputProperty2
    {
        protected sealed override InputProperty2 CreateInstanceOverride()
        {
            return new ImageProperty2();
        }

        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("image");

            //
            //
            Write(element);

            return element;
        }
    }
}