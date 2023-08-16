using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.QualIpqcInspection.View
{
    public class QualIpqcInspectionView : BaseEntity
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
        /// 参数集编码
        /// </summary>
        public string ParameterGroupCode { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string ParameterGroupName { get; set; }

        /// <summary>
        /// 参数集版本
        /// </summary>
        public string ParameterGroupVersion { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

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
