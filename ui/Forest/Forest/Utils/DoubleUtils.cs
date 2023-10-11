namespace Acorisoft.FutureGL.Forest.Utils
{
    public class DoubleUtils
    {
        internal const double DBL_EPSILON = 2.220446049250313E-16;
        internal const float  FLT_MIN     = 1.1754944E-38f;

        public static bool AreClose(double value1, double value2)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (value1 == value2)
                return true;
            var num1 = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.220446049250313E-16;
            var num2 = value1 - value2;
            return -num1 < num2 && num1 > num2;
        }

        public static bool LessThan(double value1, double value2) => value1 < value2 && !AreClose(value1, value2);

        public static bool GreaterThan(double value1, double value2) => value1 > value2 && !AreClose(value1, value2);

        public static bool LessThanOrClose(double value1, double value2) => value1 < value2 || AreClose(value1, value2);

        public static bool GreaterThanOrClose(double value1, double value2) => value1 > value2 || AreClose(value1, value2);

        public static bool IsOne(double value) => Math.Abs(value - 1.0) < 2.220446049250313E-15;

        public static bool IsZero(double value) => Math.Abs(value) < 2.220446049250313E-15;

        //public static bool AreClose(Point point1, Point point2) => DoubleUtil.AreClose(point1.X, point2.X) && DoubleUtil.AreClose(point1.Y, point2.Y);

        //public static bool AreClose(Size size1, Size size2) => DoubleUtil.AreClose(size1.Width, size2.Width) && DoubleUtil.AreClose(size1.Height, size2.Height);

        //public static bool AreClose(Vector vector1, Vector vector2) => DoubleUtil.AreClose(vector1.X, vector2.X) && DoubleUtil.AreClose(vector1.Y, vector2.Y);

        // public static bool AreClose(Rect rect1, Rect rect2)
        // {
        //     if (rect1.IsEmpty)
        //         return rect2.IsEmpty;
        //     return !rect2.IsEmpty && DoubleUtil.AreClose(rect1.X, rect2.X) && DoubleUtil.AreClose(rect1.Y, rect2.Y) && DoubleUtil.AreClose(rect1.Height, rect2.Height) && DoubleUtil.AreClose(rect1.Width, rect2.Width);
        // }

        public static bool IsBetweenZeroAndOne(double val) => GreaterThanOrClose(val, 0.0) && LessThanOrClose(val, 1.0);

        public static int DoubleToInt(double val) => 0.0 >= val ? (int)(val - 0.5) : (int)(val + 0.5);

        //public static bool RectHasNaN(Rect r) => double.IsNaN(r.X) || double.IsNaN(r.Y) || double.IsNaN(r.Height) || double.IsNaN(r.Width);
    }
}