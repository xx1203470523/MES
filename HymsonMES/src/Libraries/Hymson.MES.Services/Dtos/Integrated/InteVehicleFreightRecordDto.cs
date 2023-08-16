/*
 *creator: Karl
 *
 *describe: 载具装载记录    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-24 04:45:45
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 载具装载记录Dto
    /// </summary>
    public record InteVehicleFreightRecordDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具编码
        /// </summary>
        public long VehicleId { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 状态;0-绑定 1-解绑
        /// </summary>
        public bool? OperateType { get; set; }
       
    }


    /// <summary>
    /// 载具装载记录新增Dto
    /// </summary>
    public record InteVehicleFreightRecordCreateDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具编码
        /// </summary>
        public long VehicleId { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 状态;0-绑定 1-解绑
        /// </summary>
        public bool? OperateType { get; set; }

       
       
    }

    /// <summary>
    /// 载具装载记录更新Dto
    /// </summary>
    public record InteVehicleFreightRecordModifyDto : BaseEntityDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具编码
        /// </summary>
        public long VehicleId { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 状态;0-绑定 1-解绑
        /// </summary>
        public bool? OperateType { get; set; }

       
    }

    /// <summary>
    /// 载具装载记录分页Dto
    /// </summary>
    public class InteVehicleFreightRecordPagedQueryDto : PagerInfo
    {
    }
}
