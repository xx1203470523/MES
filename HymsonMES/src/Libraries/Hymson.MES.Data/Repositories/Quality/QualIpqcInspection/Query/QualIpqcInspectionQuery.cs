using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IPQC检验项目 查询参数
    /// </summary>
    public class QualIpqcInspectionQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验类型;1、首检 2、尾检 3、巡检
        /// </summary>
        public IPQCTypeEnum Type { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string ParameterGroupCode { get; set; }

        /// <summary>
        /// 生成条件单位
        /// </summary>
        public GenerateConditionUnitEnum GenerateConditionUnit { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long? MaterialId { get; set; }
        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
