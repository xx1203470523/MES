/*
 *creator: Karl
 *
 *describe: 托盘条码记录表    Dto | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:02
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 托盘条码记录表Dto
    /// </summary>
    public record ManuTraySfcRecordDto : BaseEntityDto
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
        /// 托盘条码
        /// </summary>
        public string Tray { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 条码信息
        /// </summary>
        public long SFCIinfoId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int? Seq { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

       /// <summary>
        /// 状态;0-解绑 1-绑定
        /// </summary>
        public bool OperationType { get; set; }

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
    /// 托盘条码记录表新增Dto
    /// </summary>
    public record ManuTraySfcRecordCreateDto : BaseEntityDto
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
        /// 托盘条码
        /// </summary>
        public string Tray { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 条码信息
        /// </summary>
        public long SFCIinfoId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int? Seq { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

       /// <summary>
        /// 状态;0-解绑 1-绑定
        /// </summary>
        public bool OperationType { get; set; }

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
    /// 托盘条码记录表更新Dto
    /// </summary>
    public record ManuTraySfcRecordModifyDto : BaseEntityDto
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
        /// 托盘条码
        /// </summary>
        public string Tray { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 条码信息
        /// </summary>
        public long SFCIinfoId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int? Seq { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal LoadQty { get; set; }

       /// <summary>
        /// 状态;0-解绑 1-绑定
        /// </summary>
        public bool OperationType { get; set; }

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
    /// 托盘条码记录表分页Dto
    /// </summary>
    public class ManuTraySfcRecordPagedQueryDto : PagerInfo
    {
    }
}
