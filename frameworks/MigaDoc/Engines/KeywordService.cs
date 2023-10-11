using System.Collections;
using Acorisoft.Miga.Doc.Keywords;

namespace Acorisoft.Miga.Doc.Engines
{
    [GeneratedModules]
    public class KeywordService : StorageService, IRefreshSupport
    {
        public KeywordService()
        {
            IdMapping   = new Dictionary<string, Keyword>();
            NameMapping = new Dictionary<string, Keyword>();
        }

        public void Refresh()
        {
            IdMapping.Clear();
        }


        public bool GetKeyword(string id, out Keyword keyword)
        {
            return IdMapping.TryGetValue(id, out keyword);
        }


        public bool Add(string newName, string owner)
        {
            if (newName is null)
            {
                return false;
            }

            if (NameMapping.ContainsKey(newName))
            {
                return false;
            }


            var keyword = new Keyword
            {
                Name  = newName,
                Id    = ShortGuidString.GetId(),
                Owner = owner
            };


            return true;
        }
        
        private void Add(Keyword keyword)
        {
            if (keyword is null)
            {
                return;
            }
        }

        public bool Rename(Keyword keyword, string oldName, string newName)
        {
            if (NameMapping.ContainsKey(newName))
            {
                return false;
            }

            NameMapping.Remove(oldName);
            NameMapping.Add(newName, keyword);
            keyword.Name = newName;
            Add(keyword);
            return true;
        }

        public void Add(Sight sight)
        {
            if (sight is null)
            {
                return;
            }


        }

        public bool Add(KeywordMapping mapping)
        {
            if (mapping is null)
            {
                return false;
            }

            // confuse :
            // 如果标签不存在就返回
            if (!IdMapping.ContainsKey(mapping.Keyword))
            {
                return false;
            }

            return true;
        }

        public void Remove(Keyword keyword)
        {
            if (keyword is null)
            {
                return;
            }

        }

        public void Remove(string labelName, string id)
        {
            if (!NameMapping.TryGetValue(labelName, out var keyword))
            {
                return;
            }

        }

        public void Remove(Sight sight)
        {
            if (sight is null)
            {
                return;
            }

        }

        public void Remove(DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }
        }

        public void Remove(string labelName, DocumentIndex index)
        {
            if (index is null)
            {
                return;
            }

            if (string.IsNullOrEmpty(labelName))
            {
                return;
            }

            if (!IdMapping.TryGetValue(labelName, out var keyword))
            {
                return;
            }

        }

        protected internal override void OnRepositoryOpening(RepositoryContext context, RepositoryProperty property)
        {
            // KeywordDB = context.Database.GetCollection<Keyword>(Constants.cn_keyword);
            // RelDB     = context.Database.GetCollection<KeywordMapping>(Constants.cn_keywordMapping);
            // SightDB   = context.Database.GetCollection<Sight>(Constants.cn_sight);

            Refresh();
        }

        protected internal override void OnRepositoryClosing(RepositoryContext context)
        {
            NameMapping.Clear();
            IdMapping.Clear();
        }

        public Dictionary<string, Keyword> IdMapping { get; }
        public Dictionary<string, Keyword> NameMapping { get; }
    }
}