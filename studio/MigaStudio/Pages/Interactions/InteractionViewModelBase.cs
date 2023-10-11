using System.Collections.Generic;
using System.Linq;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Documents;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Interactions
{
    public abstract class InteractionViewModelBase : TabViewModel
    {

        public static string GetOwnerName() => Language.GetText("text.Interaction.Owner");
        public static string GetManagerName() => Language.GetText("text.Interaction.Manager");

        public static string GetCharacterTitle(string id, MemberRole role,
            IReadOnlyDictionary<string, string> titleMapping)
        {
            if (role == MemberRole.Manager)
            {
                return GetManagerName();
            }

            if (role == MemberRole.Owner)
            {
                return GetOwnerName();
            }

            if (role == MemberRole.Special)
            {
                return titleMapping.TryGetValue(id, out var title) ? title : string.Empty;
            }

            return string.Empty;
        }
        public static string GetCharacterTitle(string id, 
            IReadOnlyDictionary<string, MemberRole> roleMapping,
            IReadOnlyDictionary<string, string> titleMapping,
            out MemberRole role)
        {
            if (roleMapping.TryGetValue(id, out role))
            {
                if (role == MemberRole.Manager)
                {
                    return GetManagerName();
                }

                if (role == MemberRole.Owner)
                {
                    return GetOwnerName();
                }

                if (role == MemberRole.Special)
                {
                    return titleMapping.TryGetValue(id, out var title) ? title : string.Empty;
                }
            }

            return string.Empty;
        }
        
        public static MemberRole GetCharacterRole(string id, 
            IReadOnlyDictionary<string, MemberRole> roleMapping)
        {
            return roleMapping.TryGetValue(id, out var role) ? role : MemberRole.Member;
        }
        

        public static string GetCharacterName(SocialCharacter ch, Dictionary<string, string> aliasMapping)
        {
            return aliasMapping.TryGetValue(ch.Id, out var alias) ? alias : ch.Name;
        }

        public Dictionary<string, SocialCharacter> CharacterMapper { get; internal set; }
    }
}