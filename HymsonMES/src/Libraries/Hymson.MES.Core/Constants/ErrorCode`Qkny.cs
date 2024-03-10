using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Constants
{
    /// <summary>
    /// 顷刻能源错误代码15000-16000
    /// </summary>
    public static partial class ErrorCode
    {
        #region 基础通用校验

        /// <summary>
        /// 设备资源编码没有正确关联工序线体
        /// </summary>
        public const string MES45001 = "设备资源编码没有正确关联工序线体";

        /// <summary>
        /// 设备编码不能为空
        /// </summary>
        public const string MES45002 = "（EquipmentCode)设备编码字段不能为空";

        /// <summary>
        /// 资源编码不能为空
        /// </summary>
        public const string MES45003 = "（ResourceCode)资源编码字段不能为空";

        #endregion

        #region 操作员登录

        /// <summary>
        /// 设备没有维护对应的账号密码
        /// </summary>
        public const string MES45011 = "【{EquipmentCode}】设备没有维护对应的账号密码";

        /// <summary>
        /// 设备上传的账号密码错误
        /// </summary>
        public const string MES45012 = "【{EquipmentCode}】设备上传的账号密码错误";

        /// <summary>
        /// 用户名或密码不能为空
        /// </summary>
        public const string MES45013 = "用户名或密码不能为空";
        #endregion

        #region 开机参数

        /// <summary>
        /// 设备没有维护对应的开机参数
        /// </summary>
        public const string MES45021 = "设备没有维护对应的开机参数，请检查开机参数是否启用或是否关联到对应的设备组";

        /// <summary>
        /// 配方状态没有激活，请激活后在使用
        /// </summary>
        public const string MES45022 = "配方没有激活或不存在，请激活后在使用";
        #endregion


    }
}
