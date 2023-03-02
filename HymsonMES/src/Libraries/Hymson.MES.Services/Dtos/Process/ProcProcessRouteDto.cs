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
        public int Status { get; set; }

       /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

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

    public class CustomProcessRouteDto
    {
        /// <summary>
        /// 描述 :基础信息
        /// 空值 : false  
        /// </summary>
        public ProcProcessRouteDto Info { get; set; }

        /// <summary>
        /// 集合（节点）
        /// </summary>
        public IEnumerable<ProcProcessRouteDetailNodeViewDto> Nodes { get; set; }

        /// <summary>
        /// 集合（连线）
        /// </summary>
        public IEnumerable<ProcProcessRouteDetailLinkDto> Links { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CustomProcessRouteDto()
        {
            Nodes = new List<ProcProcessRouteDetailNodeViewDto>();
            Links = new List<ProcProcessRouteDetailLinkDto>();
        }
    }

    public class FlowDynamicDataDto
    {
        /// <summary>
        /// 集合（节点）
        /// </summary>
        public IEnumerable<FlowDynamicNodeDto> Nodes { get; set; }

        /// <summary>
        /// 集合（连线）
        /// </summary>
        public IEnumerable<FlowDynamicLinkDto> Links { get; set; }
    }

    /// <summary>
    /// 节点对象
    /// </summary>
    public class FlowDynamicNodeDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 工序明细ID
        /// </summary>
        //[JsonConverter(typeof(ValueToStringConverter))]
        public long ProcedureBomId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public string ProcessType { get; set; }

        /// <summary>
        /// 抽检类型
        /// </summary>
        public string CheckType { get; set; }

        /// <summary>
        /// 抽检比例
        /// </summary>
        public int CheckRate { get; set; }

        /// <summary>
        /// 是否报工
        /// </summary>
        public bool IsWorkReport { get; set; }

        /// <summary>
        /// 是否首工序
        /// </summary>
        public bool IsFirstProcess { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Extra1 { get; set; }
    }

    /// <summary>
    /// 连线对象
    /// </summary>
    public class FlowDynamicLinkDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 工序明细ID（起点）
        /// </summary>
       // [JsonConverter(typeof(ValueToStringConverter))]
        public long PreProcessRouteDetailId { get; set; }

        /// <summary>
        /// 工序明细ID（终点）
        /// </summary>
       // [JsonConverter(typeof(ValueToStringConverter))]
        public long ProcessRouteDetailId { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Extra1 { get; set; }
    }


    /// <summary>
    /// 工艺路线表新增Dto
    /// </summary>
    public record ProcProcessRouteCreateDto : BaseEntityDto
    {
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
        public int Status { get; set; }

       /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

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
        /// 工序集合
        /// </summary>
        public FlowDynamicDataDto DynamicData { get; set; }
    }

    /// <summary>
    /// 工艺路线表更新Dto
    /// </summary>
    public record ProcProcessRouteModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public bool IsCurrentVersion { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 工序集合
        /// </summary>
        public FlowDynamicDataDto DynamicData { get; set; }
    }

    /// <summary>
    /// 工艺路线表分页Dto
    /// </summary>
    public class ProcProcessRoutePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工艺路线代码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public int? IsCurrentVersion { get; set; }
    }
}
