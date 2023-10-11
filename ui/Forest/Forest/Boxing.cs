namespace Acorisoft.FutureGL.Forest
{
    public class Boxing
    {
        #region DoubleBoxing

        public static readonly object[] DoubleValues = new object[]
        {
            0d,
            1d,
            2d,
            3d,
            4d,
            5d,
            6d,
            7d,
            8d,
            9d,
            10d,
        };

        public static object Box(double value)
        {
            return value switch
            {
                0 => DoubleValues[0],
                1 => DoubleValues[1],
                2 => DoubleValues[2],
                _ => value,
            };
        }

        #endregion

        #region IntBoxing

        public static readonly object[] IntValues = new object[]
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
        };

        public static object Box(int value)
        {
            return value switch
            {
                0 => IntValues[0],
                1 => IntValues[1],
                2 => IntValues[2],
                3 => IntValues[3],
                4 => IntValues[4],
                5 => IntValues[5],
                6 => IntValues[6],
                7 => IntValues[7],
                8 => IntValues[8],
                9 => IntValues[9],
                10 => IntValues[10],
                _ => value,
            };
        }

        #endregion

        public static readonly object ThicknessZero = new Thickness(0);
        public static readonly object ThicknessOne  = new Thickness(1);
        
        #region Boolean
        
        /// <summary>
        /// True的装箱值。
        /// </summary>
        public static readonly object True  = true;
        
        /// <summary>
        /// False的装箱值。
        /// </summary>
        public static readonly object False = false;

        /// <summary>
        /// 获取布尔类型的装箱值。
        /// </summary>
        /// <param name="value">指定要装箱的值</param>
        /// <remarks>使用该方法来避免过多的装箱值类型。</remarks>
        /// <returns>返回布尔类型的装箱值</returns>
        public static object Box(bool value) => value ? True : False;

        #endregion
    }
}