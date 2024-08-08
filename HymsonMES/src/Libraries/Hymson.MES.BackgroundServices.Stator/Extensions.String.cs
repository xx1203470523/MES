using System.Security.Cryptography;
using System.Text;

namespace Hymson.MES.CoreServices.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionsString
    {
        /// <summary>
        /// 序列化为数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ToLongID(this string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);

            // 将哈希值转换为整数
            var intValue = BitConverter.ToInt64(hashBytes, 0);

            // 对负数取绝对值
            if (intValue < 0) intValue = -intValue;

            return intValue;
        }

    }
}
