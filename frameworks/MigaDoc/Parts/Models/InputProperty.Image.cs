using System.Xml.Linq;
using Acorisoft.Miga.Xml;

namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("image")]
    public class ImageProperty : InputProperty
    {
        protected sealed override InputProperty CreateInstanceOverride()
        {
            return new ImageProperty();
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