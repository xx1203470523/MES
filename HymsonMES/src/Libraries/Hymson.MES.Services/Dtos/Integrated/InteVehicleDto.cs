/*
 *creator: Karl
 *
 *describe: 载具注册表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 载具注册表Dto
    /// </summary>
    public record InteVehicleDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 托盘编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long VehicleTypeId { get; set; }

       /// <summary>
        /// 存放位置
        /// </summary>
        public string Position { get; set; }

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

       
    }

    /// <summary>
    /// 载具注册View
    /// </summary>
    public record InteVehicleViewDto : InteVehicleDto
    {
        public string VehicleTypeCode { get; set; } = "";

        public string VehicleTypeName { get; set; } = "";
    }

    /// <summary>
    /// 载具注册表新增Dto
    /// </summary>
    public record InteVehicleCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 托盘编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long? VehicleTypeId { get; set; }

       /// <summary>
        /// 存放位置
        /// </summary>
        public string? Position { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
        
        public InteVehicleVerifyCreateDto? InteVehicleVerify { get; set; }

        public List<InteVehicleFreightCreateDto>? InteVehicleFreights { get; set; }
    }

    /// <summary>
    /// 载具注册表更新Dto
    /// </summary>
    public record InteVehicleModifyDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long? VehicleTypeId { get; set; }

       /// <summary>
        /// 存放位置
        /// </summary>
        public string? Position { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        public InteVehicleVerifyCreateDto? InteVehicleVerify { get; set; }

        public List<InteVehicleFreightCreateDto>? InteVehicleFreights { get; set; }
    }

    /// <summary>
    /// 载具注册表分页Dto
    /// </summary>
    public class InteVehiclePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 托盘编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 托盘名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 载具类型编码
        /// </summary>
        public string? VehicleTypeCode { get; set; }

    }

    #region 载具校验
    /// <summary>
    /// 载具校验Dto
    /// </summary>
    public record InteVehicleVerifyDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 载具id
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

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

    }


    /// <summary>
    /// 载具校验新增Dto
    /// </summary>
    public record InteVehicleVerifyCreateDto : BaseEntityDto
    {
        ///// <summary>
        ///// 载具id
        ///// </summary>

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

    }
    #endregion

    #region 载具装载
    /// <summary>
    /// 载具装载Dto
    /// </summary>
    public record InteVehicleFreightDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 载具id
        /// </summary>
        public long VehicleId { get; set; }

        /// <summary>
        /// x位置
        /// </summary>
        public int Column { get; set; } 

        /// <summary>
        /// y位置
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 状态;0-禁用 1-启用
        /// </summary>
        public bool? Status { get; set; }

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
        /// 已绑数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 扩展条码集合，用于一个位置多个条码的情况
        /// </summary>
        public List<InteVehicleFreightStackEntity> Stacks { get; set; } = new List<InteVehicleFreightStackEntity>();

    }
    public class InteVehicleStackView
    {
        /// <summary>
        /// 载具实体
        /// </summary>
        public InteVehicleEntity Vehicle { get; set; }
        /// <summary>
        /// 载具类型信息
        /// </summary>
        public InteVehicleTypeEntity VehicleType { get; set; }

        /// <summary>
        /// 条码信息集合
        /// </summary>
        public List<InteVehicleFreightDto> Stacks { get; set; }

        /// <summary>
        /// 载具容量
        /// </summary>
        public int Capacity { get; set; }
    }



    /// <summary>
    /// 载具装载新增Dto
    /// </summary>
    public record InteVehicleFreightCreateDto : BaseEntityDto
    {
        /// <summary>
        /// x位置
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// y位置
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 状态;0-禁用 1-启用
        /// </summary>
        public bool? Status { get; set; }

    }
    /// <summary>
    /// 载具操作
    /// </summary>
    public class InteVehicleOperationDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>
        /// 操作方式 0 绑定，1解绑，2清盘
        /// </summary>
        public VehicleOperationEnum OperationType;

    }
    /// <summary>
    /// 绑盘
    /// </summary>
    public class InteVehicleBindOperationDto:InteVehicleOperationDto
    {
        /// <summary>
        /// 位置ID
        /// </summary>
        public long LocationId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        

    }
    /// <summary>
    /// 载具解绑操作
    /// </summary>
    public class InteVehicleUnbindOperationDto:InteVehicleOperationDto
    {
        /// <summary>
        /// 位置ID
        /// </summary>
        public long LocationId { get; set; }
        /// <summary>
        /// 待解绑条码Id集合
        /// </summary>
        public List<long> StackIds { get; set; } = new List<long>();
       
    }
    /// <summary>
    /// 清盘
    /// </summary>
    public class InteVehicleClearOperationDto : InteVehicleOperationDto
    {
        
    }

    /// <summary>
    /// PDA获取托盘绑定信息视图
    /// </summary>
    public class InteVehicleFreightRecordView
    {
        /// <summary>
        /// 二维载具条码明细Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 位置Id
        /// </summary>
        public long? LocationId { get; set; }

        /// <summary>
        /// 装载条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string? Position { get; set;}

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Qty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string? Unit { get; set; }
    }

    #endregion
}
