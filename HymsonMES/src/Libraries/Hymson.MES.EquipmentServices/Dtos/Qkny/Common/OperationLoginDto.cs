using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Common
{
    /// <summary>
    /// 操作员登录
    /// </summary>
    public record OperationLoginDto : QknyBaseDto
    {
        /// <summary>
        /// 操作员账号
        /// </summary>
        public string OperatorUserID { get; set; } = string.Empty;

        /// <summary>
        /// 操作员密码
        /// </summary>
        public string OperatorPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 操作员登录
    /// </summary>
    public record OperationLoginReturnDto
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        public string AccountType { get; set; } = string.Empty;
    }
}
