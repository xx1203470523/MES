using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 备件类型Dto
    /// </summary>
    public record EquSparePartTypeDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; }

       /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdateBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 备件类型新增Dto
    /// </summary>
    public record EquSparePartTypeCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; }

       /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdateBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 备件类型更新Dto
    /// </summary>
    public record EquSparePartTypeModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; }

       /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdateBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool? IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 备件类型分页Dto
    /// </summary>
    public class EquSparePartTypePagedQueryDto : PagerInfo
    {
        ///// <summary>
        ///// 描述 :站点编码 
        ///// 空值 : false  
        ///// </summary>
        //public string SiteCode { get; set; }
    }
}
