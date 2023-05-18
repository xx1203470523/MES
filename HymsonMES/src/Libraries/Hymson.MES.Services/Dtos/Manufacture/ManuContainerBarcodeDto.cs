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
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 包装等级
        /// </summary>
        public int PackLevel { get; set; }

        /// <summary>
        /// 最大包装数
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// 最小包装数
        /// </summary>
        public decimal Minimum { get; set; }

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
    /// 创建容器条码使用的DTO
    /// </summary>
    public record CreateManuContainerBarcodeDto : BaseEntityDto
    {
        /// <summary>
        /// 包装码
        /// </summary>
        public string? ContainerCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 面板编码
        /// </summary>
        public string FacePlateCode { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

    }

    /// <summary>
    /// 容器条码表分页Dto
    /// </summary>
    public class ManuContainerBarcodePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 容器条码
        /// </summary>
        public string? BarCode { get; set; }
        /// <summary>
        /// 包装等级
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 产品编码 对应物料表编码
        /// </summary>
        public string? ProductCode { get; set; }
        /// <summary>
        /// 产品名称 对应物料表名称
        /// </summary>
        public string? ProductName { get; set; }
    }


    /// <summary>
    /// 容器条码表更新Dto
    /// </summary>
    public record UpdateManuContainerBarcodeStatusDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public int Status { get; set; }
    }
}
