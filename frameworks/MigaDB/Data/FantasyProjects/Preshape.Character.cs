using Acorisoft.FutureGL.MigaDB.Enums;

namespace Acorisoft.FutureGL.MigaDB.Data.FantasyProjects
{
    public class CharacterPreshape : ItemPreshape
    {
        private CharacterRole _role;

        /// <summary>
        /// 是否为NPC
        /// </summary>
        public CharacterRole Role
        {
            get => _role;
            set => SetValue(ref _role, value);
        }
    }
}