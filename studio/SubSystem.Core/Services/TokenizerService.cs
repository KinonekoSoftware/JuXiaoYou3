using System.Linq;
using DryIoc;
using NReco.Text;

namespace Acorisoft.FutureGL.MigaStudio.Core
{
    public interface ITokenizerService : IInMemoryDatabaseService
    {
        void Invalidate();
        IEnumerable<DocumentCache> Tokenize(string document);
    }
    
    public class TokenizerService : ITokenizerService
    {
        private readonly Dictionary<string, DocumentCache>         _dictionary = new Dictionary<string, DocumentCache>();
        private readonly AhoCorasickDoubleArrayTrie<DocumentCache> _parser     = new AhoCorasickDoubleArrayTrie<DocumentCache>();
        
        
        public void Start(IDatabaseManager databaseManager)
        {
            Engine          = databaseManager.GetEngine<DocumentEngine>();
            TrackingVersion = Engine.Version;
            InvalidateDataSource();
        }

        public void InvalidateDataSource()
        {
            _dictionary.Clear();

            foreach (var cache in Engine.GetCaches())
            {
                _dictionary.TryAdd(cache.Name, cache);
            }
            
            //
            _parser.Build(_dictionary, true);
        }

        public void Register(IContainer container)
        {
            container.Use<TokenizerService, ITokenizerService>(this);
        }

        public void Unregister(IContainer container)
        {
            container.Unregister<TokenizerService>(this);
            container.Unregister<ITokenizerService>(this);
        }

        public void Stop()
        {
            _dictionary.Clear();
            TrackingVersion = -1;
        }

        public void Invalidate()
        {
            if (TrackingVersion != Engine.Version)
            {
                TrackingVersion = Engine.Version;
                InvalidateDataSource();
            }
        }

        public IEnumerable<DocumentCache> Tokenize(string document)
        {
            if (string.IsNullOrEmpty(document))
            {
                return null;
            }
            return _parser.ParseText(document)
                          .Select(x => x.Value)
                          .Distinct();
        }
        public int TrackingVersion { get;private set; }
        public DocumentEngine Engine { get; private set; }
    }
}