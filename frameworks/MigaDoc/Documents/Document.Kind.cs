using System.ComponentModel;

namespace Acorisoft.Miga.Doc.Documents
{
    public enum OldDocumentKind
    {
        Character,
        Skills,
        Materials,
        Map,
        Assets,
        Custom,
        None
    }

    public enum SkillType
    {
        None,
        Passive,
        Cost,
        Faith,
        Regular
    }

    public enum EntityType
    {
        Document,
        Compose,
        Inspiration,
        Grouping,
        Entity,
        Author,
    }

    public enum ComposeType
    {
        None,
        Story,
        Tales,
        Ova,
        IF,
        Ex
    }
}