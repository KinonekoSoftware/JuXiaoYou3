using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Acorisoft.Miga.Xml;
using Koyashiro.PngChunkUtil;

namespace Acorisoft.Miga.Doc.Parts
{
    public class DataPartReader
    {
        private readonly XmlParser _parser;

        public DataPartReader()
        {
            _parser = XmlParser.GetParser(new []
            {
                typeof(Module),
                typeof(ColorProperty2),
                typeof(GroupProperty2),
                typeof(ImageProperty2),
                typeof(NumberProperty2),
                typeof(OptionProperty2),
                typeof(ReferenceProperty2),
                typeof(SequenceProperty2),
                typeof(TextProperty2),
                typeof(PageProperty2),
                typeof(Value),
                typeof(DegreeProperty2)
            });
        }

        public Compilation<Module> Read(string fileName)
        {
            var buffer = File.ReadAllText(fileName);
            var document = XDocument.Load(new StringReader(buffer), LoadOptions.SetLineInfo);
            var compilation = _parser.Parse<Module>(document);

            return compilation;
        }

        
        public async Task<Compilation<Module>> ReadAsync(string fileName)
        {
            var buffer = await File.ReadAllTextAsync(fileName);
            var document = XDocument.Load(new StringReader(buffer), LoadOptions.SetLineInfo);
            var compilation = _parser.Parse<Module>(document);

            return compilation;
        }
        
        public  Compilation<Module> ReadFrom(string fileName)
        {
            var dataPackets = File.ReadAllBytes(fileName);
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer1 = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();

            try
            {

                var reader = XmlReader.Create(new MemoryStream(buffer1));
                var document = XDocument.Load(reader, LoadOptions.SetLineInfo);
                var compilation = _parser.Parse<Module>(document);

                return compilation;
            }
            catch
            {
                return new Compilation<Module>
                {
                    IsFinished = false,
                    
                };
            }
        }
        
        public async Task<Compilation<Module>> ReadFromAsync(string fileName)
        {
            var dataPackets = await File.ReadAllBytesAsync(fileName);
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();
            var payload = Encoding.UTF8.GetString(buffer); 
            try
            {
                var document = XDocument.Load(new StringReader(payload), LoadOptions.SetLineInfo);
                var compilation = _parser.Parse<Module>(document);

                return compilation;
            }
            catch
            {
                return new Compilation<Module>
                {
                    IsFinished = false,
                };
            }
        }
        
        public Compilation<Module> ReadFromAsync(byte[] dataPackets)
        {
            var chunks = PngReader.ReadBytes(dataPackets);
            var IEND = chunks.Last();
            var buffer = IEND.Bytes.Slice(8, IEND.Length ?? 0).ToArray();
            var payload = Encoding.UTF8.GetString(buffer); 
            try
            {
                var document = XDocument.Load(new StringReader(payload), LoadOptions.SetLineInfo);
                var compilation = _parser.Parse<Module>(document);

                return compilation;
            }
            catch
            {
                return new Compilation<Module>
                {
                    IsFinished = false,
                };
            }
        }
    }
}