using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.Equipment.EquSpotcheck
{
    /// <summary>
    /// 数据实体（设备点检快照任务项目）   
    /// equ_spotcheck_task_snapshot_item
    /// @author JAM
    /// @date 2024-05-21 02:08:20
    /// </summary>
    public class EquSpotcheckTaskSnapshotItemEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务ID;equ_spotcheck_task表的Id
        /// </summary>
        public long SpotCheckTaskId { get; set; }

        /// <summary>
        /// 点检项目ID;equ_spotcheck_item的Id
        /// </summary>
        public long SpotCheckItemId { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 数值类型;文本/数值
        /// </summary>
        public EquSpotcheckDataTypeEnum? DataType { get; set; }

        /// <summary>
        /// 点检方式
        /// </summary>
        public EquSpotcheckItemMethodEnum? CheckType { get; set; }

        /// <summary>
        /// 作业方法
        /// </summary>
        public string? CheckMethod { get; set; }

        /// <summary>
        /// 单位ID;inte_unit表的Id
        /// </summary>
        public long? UnitId { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 部件
        /// </summary>
        public string Components { get; set; }

        /// <summary>
        /// 规格下限;值来源于点检模板
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 规格值（规格中心）;值来源于点检模板
        /// </summary>
        public decimal? ReferenceValue { get; set; }

        /// <summary>
        /// 规格上限;值来源于点检模板
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 描述;项目描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}
