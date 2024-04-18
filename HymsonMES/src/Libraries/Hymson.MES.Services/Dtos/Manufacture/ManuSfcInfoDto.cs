/*
 *creator: Karl
 *
 *describe: 条码信息表    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 条码信息表Dto
    /// </summary>
    public record ManuSfcInfoDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废；
        /// </summary>
        public bool? Status { get; set; }

       /// <summary>
        /// 是否在用
        /// </summary>
        public long? IsUsed { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 条码信息表新增Dto
    /// </summary>
    public record ManuSfcInfoCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废；
        /// </summary>
        public bool? Status { get; set; }

       /// <summary>
        /// 是否在用
        /// </summary>
        public long? IsUsed { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 条码信息表更新Dto
    /// </summary>
    public record ManuSfcInfoModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

       /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废；
        /// </summary>
        public bool? Status { get; set; }

       /// <summary>
        /// 是否在用
        /// </summary>
        public long? IsUsed { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       

    }

    /// <summary>
    /// 条码信息表分页Dto
    /// </summary>
    public class ManuSfcInfoPagedQueryDto : PagerInfo
    {
        
    }
}
