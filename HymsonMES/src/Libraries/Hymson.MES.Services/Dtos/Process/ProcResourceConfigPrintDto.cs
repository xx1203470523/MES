/*
 *creator: Karl
 *
 *describe: 资源配置打印机    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-09 04:14:52
 */

using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 资源配置打印机Dto
    /// </summary>
    public record ProcResourceConfigPrintDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 所属资源ID
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 打印机ID
        /// </summary>
        public long PrintId { get; set; }

       /// <summary>
        /// 备注
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

    public record ProcResourceConfigPrintViewDto : ProcResourceConfigPrintDto
    {
        /// <summary>
        /// PrintName
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印机IP
        /// </summary>
        public string PrintIp { get; set; }
    }

    /// <summary>
    /// 资源配置打印机分页Dto
    /// </summary>
    public class ProcResourceConfigPrintPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 资源配置打印机新增Dto
    /// </summary>
    public record ProcResourceConfigPrintCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 打印机ID
        /// </summary>
        public long PrintId { get; set; }
    }

    /// <summary>
    /// 资源配置打印机更新Dto
    /// </summary>
    public record ProcResourceConfigPrintModifyDto : BaseEntityDto
    {
       /// <summary>
        /// 打印机ID
        /// </summary>
        public long PrintId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
       // [Required(ErrorMessage = "操作类型不可为空")]
       // [Range(1, 3)]
        public int OperationType { get; set; }
    }
}
