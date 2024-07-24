using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.WhMaterialReturn
{
    public class MaterialReturnDto
    {
        /// <summary>
        /// 需要退的条码列表
        /// </summary>
        public IEnumerable<string> MaterialBarCodes { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 退仓类型1：实仓，2：虚仓
        /// </summary>
        public ManuReturnTypeEnum Type { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }
    }
}
