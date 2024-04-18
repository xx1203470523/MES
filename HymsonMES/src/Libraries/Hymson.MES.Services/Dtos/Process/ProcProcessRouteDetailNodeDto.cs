/*
 *creator: Karl
 *
 *describe: 工艺路线工序节点明细表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:17:40
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 工艺路线工序节点明细表Dto
    /// </summary>
    public record ProcProcessRouteDetailNodeDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 序号
        /// 手动排序号  20230703 海龙说加上这个做排序使用，重复也不管
        /// </summary>
        public string ManualSortNumber { get; set; }

        /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 抽检类型
        /// </summary>
        public ProcessRouteInspectTypeEnum? CheckType { get; set; }

       /// <summary>
        /// 抽检比例
        /// </summary>
        public int? CheckRate { get; set; }

       /// <summary>
        /// 是否报工
        /// </summary>
        public int? IsWorkReport { get; set; }

       /// <summary>
        /// 包装等级
        /// </summary>
        public string PackingLevel { get; set; }

       /// <summary>
        /// 是否首工序
        /// </summary>
        public int? IsFirstProcess { get; set; }

        /// <summary>
        /// 扩展字段1(暂存坐标)
        /// </summary>
        public string Extra1 { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";
    }


    /// <summary>
    /// 工艺路线工序节点明细表新增Dto
    /// </summary>
    public record ProcProcessRouteDetailNodeCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public long SerialNo { get; set; }

       /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 抽检类型
        /// </summary>
        public string CheckType { get; set; }

       /// <summary>
        /// 抽检比例
        /// </summary>
        public int? CheckRate { get; set; }

       /// <summary>
        /// 是否报工
        /// </summary>
        public bool? IsWorkReport { get; set; }

       /// <summary>
        /// 包装等级
        /// </summary>
        public string PackingLevel { get; set; }

       /// <summary>
        /// 是否首工序
        /// </summary>
        public bool? IsFirstProcess { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

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
    /// 工艺路线工序节点明细表更新Dto
    /// </summary>
    public record ProcProcessRouteDetailNodeModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

       /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public long SerialNo { get; set; }

       /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 抽检类型
        /// </summary>
        public string CheckType { get; set; }

       /// <summary>
        /// 抽检比例
        /// </summary>
        public int? CheckRate { get; set; }

       /// <summary>
        /// 是否报工
        /// </summary>
        public bool? IsWorkReport { get; set; }

       /// <summary>
        /// 包装等级
        /// </summary>
        public string PackingLevel { get; set; }

       /// <summary>
        /// 是否首工序
        /// </summary>
        public bool? IsFirstProcess { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

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
    /// 工艺路线工序节点明细表分页Dto
    /// </summary>
    public class ProcProcessRouteDetailNodePagedQueryDto : PagerInfo
    {
        
    }

    public record ProcProcessRouteDetailNodeViewDto : ProcProcessRouteDetailNodeDto
    {
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
        public int ProcessType { get; set; }
    }
}
