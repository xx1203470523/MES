using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Constants.Common
{
    /// <summary>
    /// MES更新内容
    /// </summary>
    public static class MesUpdateContent
    {
        /// <summary>
        /// 获取MES更新内容
        /// </summary>
        /// <returns></returns>
        public static string GetMesUpdateContent()
        {
            return V_20240830_0919;
        }

        public static string V_20240830_0919 = $@"
            版本：V_20240830_0919
            内容：第一次新增该接口
        ";
    }
}
