namespace Acorisoft.FutureGL.MigaDB.Data.Templates.Modules
{
    public enum BlockType
    {
        
        /*
         * Basic
         */
        BasicType  = 0x00010000,
        Color      = BasicType + 0x0001,
        Degree     = BasicType + 0x0002,
        Number     = BasicType + 0x0004,
        Slider     = BasicType + 0x0008,
        SingleLine = BasicType + 0x000f,
        MultiLine  = BasicType + 0x0010,
        
        /*
         * Option
         */
        OptionType = 0x00020000,
        Switch     = OptionType + 0x0001,
        Heart      = OptionType + 0x0002,
        Star       = OptionType + 0x0004,
        Binary     = OptionType + 0x0008,
        Sequence   = OptionType + 0x000f,
        
        /*
        * Reference
        */
        ReferenceType = 0x00040000,
        Reference     = ReferenceType + 0x0001,
        Image         = ReferenceType + 0x0002,
        Video         = ReferenceType + 0x0004,
        Music         = ReferenceType + 0x0008,
        Audio         = ReferenceType + 0x000F,
        File          = ReferenceType + 0x0010,
        
        /*
         * Chart
         */
        ChartType = 0x00080000,
        Histogram = ChartType + 0x0001,
        Radar     = ChartType + 0x0002,
        
        /*
         * Group
         */
        GroupType  = 0x000F0000,
        Likability = GroupType + 0x0001,
        Rate       = GroupType + 0x0002,
        Group      = GroupType + 0x0004,
    }
}