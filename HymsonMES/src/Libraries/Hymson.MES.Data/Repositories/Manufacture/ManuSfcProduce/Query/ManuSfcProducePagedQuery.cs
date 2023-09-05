/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除） 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SfcProduceStatusEnum? Status { get; set; }

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
        public SfcProduceStatusEnum? Status { get; set; }

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
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? SfcStatus { get; set; }
    }
}
