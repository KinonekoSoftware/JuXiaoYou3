namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public static class StateDriven
    {
        /// <summary>
        /// 完成构造。
        /// </summary>
        /// <param name="builder">构造器。</param>
        public static Animator FinalFinish(this IStateDrivenAnimationBuilder builder)
        {
            return builder.Finish()
                   .Finish();
        }
    }
}