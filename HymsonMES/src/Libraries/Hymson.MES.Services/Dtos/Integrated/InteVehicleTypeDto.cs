/*
 *creator: Karl
 *
 *describe: 载具类型维护    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 载具类型维护Dto
    /// </summary>
    public record InteVehicleTypeDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 类型编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public EnableEnum Status { get; set; }

       /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 列
        /// </summary>
        public int Arrange { get; set; }

        /// <summary>
        /// 单元数量
        /// </summary>
        public int UnitNumber { get; set; }

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
    /// 载具类型维护新增Dto
    /// </summary>
    public record InteVehicleTypeCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 类型编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public EnableEnum Status { get; set; }

       /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 列
        /// </summary>
        public int Arrange { get; set; }

        /// <summary>
        /// 单元数量
        /// </summary>
        public int UnitNumber { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 载具类型验证
        /// </summary>
        public IList<InteVehicleTypeVerifyCreateDto> VehicleTypeVerifyList { get; set; }
    }

    /// <summary>
    /// 载具类型维护更新Dto
    /// </summary>
    public record InteVehicleTypeModifyDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       ///// <summary>
       // /// 类型编码
       // /// </summary>
       // public string Code { get; set; }

       /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public EnableEnum Status { get; set; }

       /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

       /// <summary>
        /// 列
        /// </summary>
        public int Arrange { get; set; }

        /// <summary>
        /// 单元数量
        /// </summary>
        public int UnitNumber { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 载具类型验证
        /// </summary>
        public IList<InteVehicleTypeVerifyCreateDto> VehicleTypeVerifyList { get; set; }
    }

    /// <summary>
    /// 载具类型维护分页Dto
    /// </summary>
    public class InteVehicleTypePagedQueryDto : PagerInfo
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public EnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 载具类型验证新增Dto
    /// </summary>
    public record InteVehicleTypeVerifyCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 验证类型;1-物料  2-物料组
        /// </summary>
        public VehicleTypeVerifyTypeEnum Type { get; set; }

        /// <summary>
        /// 物料或者物料组id
        /// </summary>
        public long VerifyId { get; set; }
    }

    /// <summary>
    /// 载具类型验证Dto
    /// </summary>
    public record InteVehicleTypeVerifyDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 载具类型id
        /// </summary>
        public long VehicleTypeId { get; set; }

        /// <summary>
        /// 验证类型;1-物料  2-物料组
        /// </summary>
        public VehicleTypeVerifyTypeEnum? Type { get; set; }

        /// <summary>
        /// 物料或者物料组id
        /// </summary>
        public long? VerifyId { get; set; }

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


        public string? VerifyCode { get; set; }

        public string? Version { get; set; }

        public string? Name { get; set; }
    }


}
