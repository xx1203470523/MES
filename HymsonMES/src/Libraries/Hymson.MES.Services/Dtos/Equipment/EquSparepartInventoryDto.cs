/*
 *creator: Karl
 *
 *describe: 备件库存    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.EquSparepartInventory
{

    /// <summary>
    /// 获取备件
    /// </summary>
    public record VerifyQtyParamDto
    {
        /// <summary>
        /// 库存数量
        /// </summary>
        public decimal InventoryQty { get; set; }

        /// <summary>
        /// 备件数量
        /// </summary>
        public long SparepartQty { get; set; }
        /// <summary>
        /// 操作数量
        /// </summary>
        public int OperateQty { get; set; }
        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }
        /// <summary>
        /// 操作数量
        /// </summary>
        public EquOperationTypeEnum OperateType { get; set; }
    }

    /// <summary>
    /// 获取备件
    /// </summary>
    public record GetEquSparePartsParamDto
    {
        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int VerifyQty { get; set; } = 0;
    }
    /// <summary>
    /// 备件信息
    /// </summary>
    public record GetEquSparePartsDto
    {
        /// <summary>
        /// 备件Id equ_sparepartId
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparepartName { get; set; }

        /// <summary>
        /// 备件类型Id 
        /// </summary>
        public long? SparepartGroupId { get; set; }

        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparepartGroupCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparepartGroupName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal InventoryQty { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal SparepartQty { get; set; }
    }

    /// <summary>
    /// 获取备件
    /// </summary>
    public record GetOutboundChooseEquSparePartsParamDto
    {
        /// <summary>
        /// 备件编码
        /// </summary>
        public IEnumerable<long> SparepartIds { get; set; }
    }

    /// <summary>
    /// 备件信息
    /// </summary>
    public record GetOutboundChooseEquSparePartsDto
    {
        /// <summary>
        /// 备件Id equ_sparepartId
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparepartName { get; set; }

        /// <summary>
        /// 备件类型Id 
        /// </summary>
        public long? SparepartGroupId { get; set; }

        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparepartGroupCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparepartGroupName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    ///  分页查询View
    /// </summary>
    public record EquSparepartInventoryPageDto : BaseEntityDto
    {

        /// <summary>
        /// 备件ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 备件编码 
        /// </summary>
        public long SparepartId { get; set; }
        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>  
        /// 备件名称 
        /// </summary>
        public string SparepartName { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparepartGroupCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparepartGroupName { get; set; } 

        /// <summary>
        /// 备件名称
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>     
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 备件库存Dto
    /// </summary>
    public record EquSparepartInventoryDto : BaseEntityDto
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
        /// 备件Id equ_sparepartId
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// 入出库参数
    /// </summary>
    public record OperationEquSparepartInventoryDto
    {
        /// <summary>
        /// 参数集
        /// </summary>
        public IEnumerable<EquSparepartInventoryCreateDto> Param { get; set; }

        /// <summary>
        ///  工作中心
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        ///  领用人
        /// </summary>
        public string? Recipients { get; set; }

        /// <summary>
        ///  操作类型
        /// </summary>
        public EquOperationTypeEnum OperationType { get; set; }
    }

    /// <summary>
    /// 备件库存新增Dto
    /// </summary>
    public record EquSparepartInventoryCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 备件Code SparepartCode
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件Id equ_sparepartId
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 备件库存更新Dto
    /// </summary>
    public record EquSparepartInventoryModifyDto : BaseEntityDto
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
        /// 备件Id equ_sparepartId
        /// </summary>
        public long SparepartId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }



    }

    /// <summary>
    /// 备件库存分页Dto
    /// </summary>
    public class EquSparepartInventoryPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string? SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? SparepartName { get; set; }
        /// <summary>
        /// 备件编码
        /// </summary>
        public string? SparePartsGroupCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string? Specifications { get; set; }
    }



}
