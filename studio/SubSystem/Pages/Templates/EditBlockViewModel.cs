namespace Acorisoft.FutureGL.MigaStudio.Pages.Templates
{
    public class EditBlockViewModel : ImplicitDialogVM
    {
        private ModuleBlockEditUI _block;

        protected override void OnStart(RoutingEventArgs parameter)
        {
            Block = parameter.Parameter
                             .Args[0] as ModuleBlockEditUI;
            base.OnStart(parameter);
        }

        /// <summary>
        /// 获取或设置 <see cref="Block"/> 属性。
        /// </summary>
        public ModuleBlockEditUI Block
        {
            get => _block;
            set => SetValue(ref _block, value);
        }
        
        public static Task<Op<ModuleBlockEditUI>> Edit(ModuleBlockEditUI element)
        {
            return DialogService().Dialog<ModuleBlockEditUI>(new EditBlockViewModel(), new Parameter
                       {
                           Args = new object[]
                           {
                               element
                           }
                       });
        }

        protected override bool IsCompleted() => true;

        protected override void Finish()
        {
            Result = Block;
        }

        protected override string Failed() => SubSystemString.Unknown;
    }
}