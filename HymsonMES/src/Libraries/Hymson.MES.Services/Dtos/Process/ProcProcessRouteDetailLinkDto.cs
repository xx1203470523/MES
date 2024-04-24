/*
 *creator: Karl
 *
 *describe: 工艺路线工序节点关系明细表(前节点多条就存多条)    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:17:52
 */

using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)Dto
    /// </summary>
    public record ProcProcessRouteDetailLinkDto : BaseEntityDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 前一工艺路线工序明细ID
        /// </summary>
        public long? PreProcessRouteDetailId { get; set; }

        /// <summary>
        /// 当前工艺路线工序明细ID
        /// </summary>
        public long ProcessRouteDetailId { get; set; }

        /// <summary>
        /// 扩展字段1(暂存坐标)
        /// </summary>
        public string Extra1 { get; set; }

    }


    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)新增Dto
    /// </summary>
    public record ProcProcessRouteDetailLinkCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 前一工艺路线工序明细ID
        /// </summary>
        public long? PreProcessRouteDetailId { get; set; }

        /// <summary>
        /// 当前工艺路线工序明细ID
        /// </summary>
        public long ProcessRouteDetailId { get; set; }

        /// <summary>
        /// 扩展字段1(暂存坐标)
        /// </summary>
        public string Extra1 { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }


    }

    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)更新Dto
    /// </summary>
    public record ProcProcessRouteDetailLinkModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 前一工艺路线工序明细ID
        /// </summary>
        public long? PreProcessRouteDetailId { get; set; }

        /// <summary>
        /// 当前工艺路线工序明细ID
        /// </summary>
        public long ProcessRouteDetailId { get; set; }

        /// <summary>
        /// 扩展字段1(暂存坐标)
        /// </summary>
        public string Extra1 { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }



    }

    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)分页Dto
    /// </summary>
    public class ProcProcessRouteDetailLinkPagedQueryDto : PagerInfo
    {
        
    }
}
