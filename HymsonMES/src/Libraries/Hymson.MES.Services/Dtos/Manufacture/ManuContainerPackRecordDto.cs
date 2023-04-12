/*
 *creator: Karl
 *
 *describe: 容器装载记录    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 容器装载记录Dto
    /// </summary>
    public record ManuContainerPackRecordDto : BaseEntityDto
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
        /// 容器条码id
        /// </summary>
        public long? ContainerBarCodeId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }

       /// <summary>
        /// 操作类型;1、装载2、移除
        /// </summary>
        public bool? OperateType { get; set; }

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
    /// 容器装载记录新增Dto
    /// </summary>
    public record ManuContainerPackRecordCreateDto : BaseEntityDto
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
        /// 容器条码id
        /// </summary>
        public long? ContainerBarCodeId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }

       /// <summary>
        /// 操作类型;1、装载2、移除
        /// </summary>
        public bool? OperateType { get; set; }

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
    /// 容器装载记录更新Dto
    /// </summary>
    public record ManuContainerPackRecordModifyDto : BaseEntityDto
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
        /// 容器条码id
        /// </summary>
        public long? ContainerBarCodeId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }

       /// <summary>
        /// 操作类型;1、装载2、移除
        /// </summary>
        public bool? OperateType { get; set; }

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
    /// 容器装载记录分页Dto
    /// </summary>
    public class ManuContainerPackRecordPagedQueryDto : PagerInfo
    {
    }
}
