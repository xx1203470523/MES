/*
 *creator: Karl
 *
 *describe: 工序配置打印表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:24:06
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 工序配置打印表Dto
    /// </summary>
    public record ProcProcedurePrintRelationDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 所属模板ID
        /// </summary>
        public long? TemplateId { get; set; }

       /// <summary>
        /// 份数
        /// </summary>
        public int? Copy { get; set; }

       /// <summary>
        /// 备注
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
    /// 工序配置打印表新增Dto
    /// </summary>
    public record ProcProcedurePrintReleationCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 所属模板ID
        /// </summary>
        public long TemplateId { get; set; }

       /// <summary>
        /// 份数
        /// </summary>
        public int? Copy { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 工序配置打印表更新Dto
    /// </summary>
    public record ProcProcedurePrintReleationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属工序ID
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 所属模板ID
        /// </summary>
        public long? TemplateId { get; set; }

       /// <summary>
        /// 份数
        /// </summary>
        public int? Copy { get; set; }

       /// <summary>
        /// 备注
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
    /// 工序配置打印表分页Dto
    /// </summary>
    public class ProcProcedurePrintReleationPagedQueryDto : PagerInfo
    {
        public long ProcedureId { get; set; }
    }

    /// <summary>
    /// 工序配置打印查询实体类
    /// </summary>
    public class ProcProcedurePrintReleationDto
    {
        /// <summary>
        /// 所属物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 所属模板ID
        /// </summary>
        public long? TemplateId { get; set; }

        /// <summary>
        /// 打印模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 份数
        /// </summary>
        public int? Copy { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }


        //TODO 模板 by wangkeming 
    }
}
