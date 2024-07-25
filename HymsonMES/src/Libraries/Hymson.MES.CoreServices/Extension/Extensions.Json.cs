using System.Text.Json;

namespace Hymson.MES.CoreServices.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionsJson
    {
        /// <summary>
        /// 序列化为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSerializeLower(this object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCasePropertyNamingPolicy()
            });
        }

        /// <summary>
        /// 序列化为字符串
        /// </summary>
        /// <param name="stringObj"></param>
        /// <returns></returns>
        public static T? ToDeserializeLower<T>(this string stringObj)
        {
            return JsonSerializer.Deserialize<T>(stringObj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCasePropertyNamingPolicy()
            });
        }

    }

    /// <summary>
    /// 转小写
    /// </summary>
    public class LowerCasePropertyNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string ConvertName(string name)
        {
            // 如果名称为空或只有一个字符，直接返回原名称
            if (string.IsNullOrEmpty(name) || name.Length < 2) return name;

            // 将首字母转换为小写
            return char.ToLowerInvariant(name[0]) + name[1..];
        }
    }

}
