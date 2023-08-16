using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IPQC检验项目）   
    /// qual_ipqc_inspection
    /// @author xiaofei
    /// @date 2023-08-08 10:19:34
    /// </summary>
    public class QualIpqcInspectionEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验类型;1、首检 2、尾检 3、巡检
        /// </summary>
        public IPQCTypeEnum Type { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 全检参数idqual_inspection_parameter_group 的id
        /// </summary>
        public long InspectionParameterGroupId { get; set; }

        /// <summary>
        /// 生成条件
        /// </summary>
        public int GenerateCondition { get; set; }

        /// <summary>
        /// 生成条件单位;1、小时 2、班次 3、批次 4、罐 5、卷
        /// </summary>
        public GenerateConditionUnitEnum GenerateConditionUnit { get; set; }

        /// <summary>
        /// 管控时间
        /// </summary>
        public int? ControlTime { get; set; }

        /// <summary>
        /// 管控时间单位;1、时 2、分
        /// </summary>
        public ControlTimeUnitEnum? ControlTimeUnit { get; set; }

        /// <summary>
        /// 物料id proc_material 的 id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 工序id  proc_procedure的id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }
}
