/*
 *creator: Karl
 *
 *describe: 二维载具条码明细    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-19 08:14:38
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 二维载具条码明细Dto
    /// </summary>
    public record InteVehiceFreightStackDto : BaseEntityDto
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
        /// 载具位置id
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

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
    /// 二维载具条码明细新增Dto
    /// </summary>
    public record InteVehiceFreightStackCreateDto : BaseEntityDto
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
        /// 载具位置id
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

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
    /// 二维载具条码明细更新Dto
    /// </summary>
    public record InteVehiceFreightStackModifyDto : BaseEntityDto
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
        /// 载具位置id
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

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
    /// 二维载具条码明细分页Dto
    /// </summary>
    public class InteVehiceFreightStackPagedQueryDto : PagerInfo
    {
    }
}
