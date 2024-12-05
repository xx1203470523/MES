/*
 *creator: Karl
 *
 *describe: 马威FQC检验    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Services.Dtos.Integrated;
using Microsoft.Identity.Client;

namespace Hymson.MES.Services.Dtos.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验Dto
    /// </summary>
    public record QualFqcInspectionMavalDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }
        
        /// <summary>
        /// 工序Code
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 资源Code
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 判定结果1.合格2.不合格
        /// </summary>
        public FqcJudgmentResultsEnum JudgmentResults { get; set; }

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
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }


    /// <summary>
    /// 马威FQC检验新增Dto
    /// </summary>
    public record QualFqcInspectionMavalCreateDto : BaseEntityDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; } = 1;

        /// <summary>
        /// 判定结果1.合格2.不合格
        /// </summary>
        public FqcJudgmentResultsEnum JudgmentResults { get; set; }

        /// <summary>
        /// 不合格原因
        /// </summary>
        public string ?Remark { get; set; }

 

    }

    /// <summary>
    /// 马威FQC检验更新Dto
    /// </summary>
    public record QualFqcInspectionMavalModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 判定结果1.合格2.不合格
        /// </summary>
        public int JudgmentResults { get; set; }

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
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }



    }

    /// <summary>
    /// 马威FQC检验分页Dto
    /// </summary>
    public class QualFqcInspectionMavalPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 不合格原因
        /// </summary>
        public string? Remark {  get; set; }
        
        /// <summary>
        /// 工序Code
        /// </summary>
        public string? ProcedureCode {  get; set; }
        
        /// <summary>
        /// 资源Code
        /// </summary>
        public string? ResourceCode {  get; set; }

    }




    /// <summary>
    /// 附件dto
    /// </summary>
    public record QualFqcInspectionMavalAttachmentDto
    {
        /// <summary>
        /// 单据id
        /// </summary>
        public long FqcMavalId { get; set; }

    } 
    /// <summary>
    /// 附件保存dto
    /// </summary>
    public record QualFqcInspectionMavalSaveAttachmentDto
    {
        /// <summary>
        /// 单据id
        /// </summary>
        public long FqcMavalId { get; set; }

        /// <summary>
        /// 检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }

}
