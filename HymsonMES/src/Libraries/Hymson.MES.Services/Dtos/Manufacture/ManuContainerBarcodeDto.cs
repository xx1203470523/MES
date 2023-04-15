/*
 *creator: Karl
 *
 *describe: 容器条码表    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 容器条码表Dto
    /// </summary>
    public record ManuContainerBarcodeDto : BaseEntityDto
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
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 容器规格id
        /// </summary>
        public long? ContainerId { get; set; }

       /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public bool Status { get; set; }

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
    public record ManuContainerBarcodeView
    {
        /// <summary>
        /// 容器包装主表实体
        /// </summary>
        public ManuContainerBarcodeEntity manuContainerBarcodeEntity { get; set; }
        /// <summary>
        /// 容器规格实体
        /// </summary>
        public InteContainerEntity inteContainerEntity { get; set; }
        /// <summary>
        /// 该容器的包装集合
        /// </summary>
        public List<ManuContainerPackDto> manuContainerPacks { get; set; }
    }


    /// <summary>
    /// 容器条码表新增Dto
    /// </summary>
    public record ManuContainerBarcodeCreateDto : BaseEntityDto
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
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 包装码
        /// </summary>
        public string ContainerCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 容器规格id
        /// </summary>
        public long? ContainerId { get; set; }

       /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public int Status { get; set; }

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
    /// 容器条码表更新Dto
    /// </summary>
    public record ManuContainerBarcodeModifyDto : BaseEntityDto
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
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 容器规格id
        /// </summary>
        public long? ContainerId { get; set; }

       /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public bool Status { get; set; }

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
    /// 容器条码表分页Dto
    /// </summary>
    public class ManuContainerBarcodePagedQueryDto : PagerInfo
    {
    }
}
