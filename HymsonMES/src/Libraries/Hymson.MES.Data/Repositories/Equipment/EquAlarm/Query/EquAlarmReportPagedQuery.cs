using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Equipment
{
    public class EquAlarmReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
        /// <summary>
        /// 工序编码集合
        /// </summary>
        public string[]? ProcedureCodes { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string? EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResName { get; set; }
        /// <summary>
        /// 状态;1：触发 2、恢复
        /// </summary>
        public EquipmentAlarmStatusEnum? Status { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime[]? TriggerTimes { get; set; }
    }


    /// <summary>
    /// <para>@层级：仓储层</para>
    /// <para>@类型：查询对象</para>
    /// <para>@描述：设备报警信息;标准查询对象</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-28</para>
    /// </summary>
    public class EquAlarmQuery : QueryAbstraction
    {

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 主键组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }


        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 站点Id组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }


        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备Id组
        /// </summary>
        public IEnumerable<long>? EquipmentIds { get; set; }


        /// <summary>
        /// 故障代码
        /// </summary>
        public string? FaultCode { get; set; }

        /// <summary>
        /// 故障代码模糊条件
        /// </summary>
        public string? FaultCodeLike { get; set; }


        /// <summary>
        /// 故障信息
        /// </summary>
        public string? AlarmMsg { get; set; }

        /// <summary>
        /// 故障信息模糊条件
        /// </summary>
        public string? AlarmMsgLike { get; set; }


        /// <summary>
        /// 状态;1、开启 2、恢复
        /// </summary>
        public long? Status { get; set; }

        /// <summary>
        /// 状态;1、开启 2、恢复组
        /// </summary>
        public IEnumerable<long>? Statuss { get; set; }


        /// <summary>
        /// 传输时间开始日期
        /// </summary>
        public DateTime? LocalTimeStart { get; set; }

        /// <summary>
        /// 传输时间结束日期
        /// </summary>
        public DateTime? LocalTimeEnd { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }


        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }


        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }


        /// <summary>
        /// 更新时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 更新时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }

    }

    /// <summary>
    /// <para>@层级：仓储层</para>
    /// <para>@类型：分页查询对象</para>
    /// <para>@描述：设备报警信息;标准分页查询对象</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-2-28</para>
    /// </summary>
    public class EquAlarmPagedQuery : PagerInfo
    {
        /// <summary>
        /// 排序
        /// </summary>
        new public string Sorting { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 主键组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }


        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 站点Id组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }


        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 设备Id组
        /// </summary>
        public IEnumerable<long>? EquipmentIds { get; set; }


        /// <summary>
        /// 故障代码
        /// </summary>
        public string? FaultCode { get; set; }

        /// <summary>
        /// 故障代码模糊条件
        /// </summary>
        public string? FaultCodeLike { get; set; }


        /// <summary>
        /// 故障信息
        /// </summary>
        public string? AlarmMsg { get; set; }

        /// <summary>
        /// 故障信息模糊条件
        /// </summary>
        public string? AlarmMsgLike { get; set; }


        /// <summary>
        /// 状态;1、开启 2、恢复
        /// </summary>
        public long? Status { get; set; }

        /// <summary>
        /// 状态;1、开启 2、恢复组
        /// </summary>
        public IEnumerable<long>? Statuss { get; set; }


        /// <summary>
        /// 传输时间开始日期
        /// </summary>
        public DateTime? LocalTimeStart { get; set; }

        /// <summary>
        /// 传输时间结束日期
        /// </summary>
        public DateTime? LocalTimeEnd { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }


        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }


        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }


        /// <summary>
        /// 更新时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 更新时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }

    }
}
