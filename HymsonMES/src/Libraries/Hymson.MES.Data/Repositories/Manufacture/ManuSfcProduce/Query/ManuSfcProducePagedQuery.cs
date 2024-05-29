using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 分页参数
    /// </summary>
    public class ManuSfcProducePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        ///产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public IEnumerable<SfcStatusEnum>? Statuss { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[]? SfcArray { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResCode { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 资源类型Id
        /// </summary>
        public long? ResourceTypeId { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? Version { get; set; }
    }

    /// <summary>
    /// 条码生产信息查询 分页参数
    /// </summary>
    public class ManuSfcProduceSelectPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        ///产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public string[]? SfcArray { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 锁定状态
        /// </summary>
        public int? Lock { get; set; }

        /// <summary>
        /// 查询锁定状态不为某个状态的sfc信息，即时锁定的不能操作不查
        /// </summary>
        public int? NoLock { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? SfcStatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ManuSfcAboutInfoPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 条码 - 硬查
        /// </summary>
        public string? SfcHard { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcStatusEnum? Status { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 批量的条码
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ManuSfcAboutInfoBySfcQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }
    }

    /// <summary>
    /// 条码表 分页参数
    /// </summary>
    public class ManuSfcProduceNewPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public IEnumerable<long>? WorkOrderIds { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public YesOrNoEnum? IsUsed { get; set; }

    }

}
