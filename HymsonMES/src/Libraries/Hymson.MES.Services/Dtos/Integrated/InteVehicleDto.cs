/*
 *creator: Karl
 *
 *describe: 载具注册表    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

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
        public EnableEnum Status { get; set; }

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
        public EnableEnum Status { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long VehicleTypeId { get; set; }

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
        public EnableEnum Status { get; set; }

       /// <summary>
        /// 载具类型id
        /// </summary>
        public long VehicleTypeId { get; set; }

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
        public EnableEnum? Status { get; set; }

        ///// <summary>
        ///// 载具类型id
        ///// </summary>
        //public long VehicleTypeId { get; set; }

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
        //public long VehicleId { get; set; }

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
        /// 位置号
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

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

    }


    /// <summary>
    /// 载具装载新增Dto
    /// </summary>
    public record InteVehicleFreightCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 位置号
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 状态;0-禁用 1-启用
        /// </summary>
        public bool? Status { get; set; }

    }
    #endregion
}
