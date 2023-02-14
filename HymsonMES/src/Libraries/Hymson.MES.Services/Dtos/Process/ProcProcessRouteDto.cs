/*
 *creator: Karl
 *
 *describe: 工艺路线表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
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
    /// 工艺路线表Dto
    /// </summary>
    public record ProcProcessRouteDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 是否当前版本
        /// </summary>
        public bool? IsCurrentVersion { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

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

    public class CustomProcessRoute
    {
        /// <summary>
        /// 描述 :基础信息
        /// 空值 : false  
        /// </summary>
        public ProcProcessRouteDto Info { get; set; }

        /// <summary>
        /// 集合（节点）
        /// </summary>
        public IEnumerable<ProcProcessRouteDetailNodeDto> Nodes { get; set; }

        /// <summary>
        /// 集合（连线）
        /// </summary>
        public IEnumerable<ProcProcessRouteDetailLinkDto> Links { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CustomProcessRoute()
        {
            Nodes = new List<ProcProcessRouteDetailNodeDto>();
            Links = new List<ProcProcessRouteDetailLinkDto>();
        }
    }

    /// <summary>
    /// 工艺路线表新增Dto
    /// </summary>
    public record ProcProcessRouteCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 是否当前版本
        /// </summary>
        public bool? IsCurrentVersion { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

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
    /// 工艺路线表更新Dto
    /// </summary>
    public record ProcProcessRouteModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 是否当前版本
        /// </summary>
        public bool? IsCurrentVersion { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

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
    /// 工艺路线表分页Dto
    /// </summary>
    public class ProcProcessRoutePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; } = "";

        /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public int IsCurrentVersion { get; set; }
    }
}
