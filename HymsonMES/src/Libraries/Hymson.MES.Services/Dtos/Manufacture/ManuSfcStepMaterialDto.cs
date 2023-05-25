/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码    Dto | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 出站绑定的物料批次条码Dto
    /// </summary>
    public record ManuSfcStepMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 步骤ID
        /// </summary>
        public long StepId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 批次条码
        /// </summary>
        public string MaterialBarcode { get; set; }

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
    /// 出站绑定的物料批次条码新增Dto
    /// </summary>
    public record ManuSfcStepMaterialCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 步骤ID
        /// </summary>
        public long StepId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 批次条码
        /// </summary>
        public string MaterialBarcode { get; set; }

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
    /// 出站绑定的物料批次条码更新Dto
    /// </summary>
    public record ManuSfcStepMaterialModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 步骤ID
        /// </summary>
        public long StepId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 批次条码
        /// </summary>
        public string MaterialBarcode { get; set; }

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
    /// 出站绑定的物料批次条码分页Dto
    /// </summary>
    public class ManuSfcStepMaterialPagedQueryDto : PagerInfo
    {
    }
}
