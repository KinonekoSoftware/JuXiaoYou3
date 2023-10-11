namespace Acorisoft.FutureGL.Forest.Styles.Animations
{
    public class ColorPropertyBuilder : StateDrivenPropertyBuilder<Color?>
    {
        protected override IPropertyAnimationRunner CreateAnimation()
        {
            return new ColorAnimationRuner
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
    
    public class ThicknessPropertyBuilder : StateDrivenPropertyBuilder<Thickness?>
    {
        protected override IPropertyAnimationRunner CreateAnimation()
        {
            return new ThicknessAnimationRuner
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
    
    public class DoublePropertyBuilder : StateDrivenPropertyBuilder<double?>
    {
        protected override IPropertyAnimationRunner CreateAnimation()
        {
            return new DoubleAnimationRuner
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
    
    public class ObjectPropertyBuilder : StateDrivenPropertyBuilder<object>
    {
        protected override IPropertyAnimationRunner CreateAnimation()
        {
            return new ObjectAnimationRuner
            {
                Duration     = Duration,
                PropertyPath = PropertyPath,
                Mapper       = Mapper
            };
        }
    }
}