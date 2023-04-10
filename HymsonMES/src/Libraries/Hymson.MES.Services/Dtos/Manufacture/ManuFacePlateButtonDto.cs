/*
 *creator: Karl
 *
 *describe: 操作面板按钮    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:58:19
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 操作面板按钮Dto
    /// </summary>
    public record ManuFacePlateButtonDto : BaseEntityDto
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
        /// 面板Id
        /// </summary>
        public long? FacePlateId { get; set; }

       /// <summary>
        /// 序列号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentage { get; set; }

       /// <summary>
        /// 热键
        /// </summary>
        public HotkeyEnum Hotkeys { get; set; }

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
    /// 操作面板按钮新增Dto
    /// </summary>
    public record ManuFacePlateButtonCreateDto : BaseEntityDto
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
        /// 面板Id
        /// </summary>
        public long? FacePlateId { get; set; }

       /// <summary>
        /// 序列号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentage { get; set; }

       /// <summary>
        /// 热键
        /// </summary>
        public HotkeyEnum Hotkeys { get; set; }

       ///// <summary>
       // /// 创建人
       // /// </summary>
       // public string CreatedBy { get; set; }

       ///// <summary>
       // /// 创建时间
       // /// </summary>
       // public DateTime CreatedOn { get; set; }

       ///// <summary>
       // /// 更新人
       // /// </summary>
       // public string UpdatedBy { get; set; }

       ///// <summary>
       // /// 更新时间
       // /// </summary>
       // public DateTime UpdatedOn { get; set; }

       ///// <summary>
       // /// 删除标识
       // /// </summary>
       // public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 操作面板按钮更新Dto
    /// </summary>
    public record ManuFacePlateButtonModifyDto : BaseEntityDto
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
        /// 面板Id
        /// </summary>
        public long? FacePlateId { get; set; }

       /// <summary>
        /// 序列号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentage { get; set; }

       /// <summary>
        /// 热键
        /// </summary>
        public HotkeyEnum Hotkeys { get; set; }

       ///// <summary>
       // /// 创建人
       // /// </summary>
       // public string CreatedBy { get; set; }

       ///// <summary>
       // /// 创建时间
       // /// </summary>
       // public DateTime CreatedOn { get; set; }

       ///// <summary>
       // /// 更新人
       // /// </summary>
       // public string UpdatedBy { get; set; }

       ///// <summary>
       // /// 更新时间
       // /// </summary>
       // public DateTime UpdatedOn { get; set; }

       ///// <summary>
       // /// 删除标识
       // /// </summary>
       // public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 操作面板按钮分页Dto
    /// </summary>
    public class ManuFacePlateButtonPagedQueryDto : PagerInfo
    {
    }
}
