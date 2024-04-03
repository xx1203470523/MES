using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 出站实体
    /// </summary>
    public record OutboundDto : QknyBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 0：不合格； 1：合格
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();

        /// <summary>
        /// 绑定的物料批次条码列表
        /// </summary>
        public List<string> BindFeedingCodeList { get; set; } = new List<string>();

        /// <summary>
        /// 不良原因
        /// </summary>
        public List<string> NgList { get; set; } = new List<string>();
    }

    /// <summary>
    /// 绑定后的极组单个出站
    /// </summary>
    public record OutboundBindJzSingleDto : OutboundDto
    {
        /// <summary>
        /// 工序类型
        /// 1-首次发生 2-中间 3-末尾工序
        /// </summary>
        public string OperationType { get; set; } = string.Empty;
    }

}
