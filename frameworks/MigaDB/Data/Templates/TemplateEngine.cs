namespace Acorisoft.FutureGL.MigaDB.Data.Templates
{
    public partial class TemplateEngine : DataEngine
    {
        protected override void OnDatabaseOpening(DatabaseSession session)
        {
            ModuleOpening(session.Database);
        }

        protected override void OnDatabaseClosing()
        {
            ModuleClosing();
        }
    }
}