using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Common
{
    public class EnumDto
    {
        public string Label { get; set; }
        public object Value { get; set; }
    }

    public static class EnumDesc {
        public static string GetEnumDescription(this Enum value)
        {
            if (value == null) {
                return "";
            }
            try
            {
                FieldInfo fi = value?.GetType().GetField(value.ToString());
                if (fi == null) {
                    return "";
                }
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return value.ToString();
                }
            }
            catch
            {
                return "";
            }
           
        }
    }

   

}
