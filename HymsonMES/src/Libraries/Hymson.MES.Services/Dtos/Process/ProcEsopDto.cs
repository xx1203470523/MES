/*
 *creator: Karl
 *
 *describe: ESOP    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// ESOPDto
    /// </summary>
    public record ProcEsopDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }
    }

    /// <summary>
    /// ESOP新增Dto
    /// </summary>
    public record ProcEsopCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }
    }

    /// <summary>
    /// ESOP更新Dto
    /// </summary>
    public record ProcEsopModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }
    }

    /// <summary>
    /// ESOP分页Dto
    /// </summary>
    public class ProcEsopPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        ///状态 0-未启用  1-启用
        /// </summary>
        public TrueOrFalseEnum? Status { get; set; }
    }
}
