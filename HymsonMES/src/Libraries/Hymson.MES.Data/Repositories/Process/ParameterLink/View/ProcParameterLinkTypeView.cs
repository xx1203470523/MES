using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcParameterLinkTypeView :BaseEntity 
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 类型（设备/产品参数）
        /// </summary>
        public ParameterTypeEnum ParameterType { get; set; } = ParameterTypeEnum.Equipment;

        /// <summary>
        /// ID（标准参数）
        /// </summary>
        public long ParameterID { get; set; }

        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
