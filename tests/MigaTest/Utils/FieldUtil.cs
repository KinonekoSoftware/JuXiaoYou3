using System.Reflection;
using Acorisoft.FutureGL.MigaTest.Reflections;

namespace Acorisoft.FutureGL.MigaTest.Utils
{
    public class FieldUtil
    {
        public static bool CheckThisField(FieldContext field, object instance)
        {
            if (field is null ||
                instance is null)
            {
                return false;
            }


            if (field.IsString)
            {
                //
                // 是否为字符串
                var s = field.GetValue(instance) as string;
                
                //
                // 检查是否有边界测试

                return Assert_String(field, s);
            }

            if (field.IsClass)
            {
                
            }

            if (field.IsNullable)
            {
                
            }
            
            
            return false;
        }

        private static bool Assert_String(FieldContext field, string value)
        {
            var null_state  = value is null;
            var empty_state = string.IsNullOrEmpty(value);


            return null_state;
        }
    }
}