using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// Dto（备件类型）
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
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";
        /*
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
        */
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        //public bool? IsDeleted { get; set; }


    }

    /// <summary>
    /// 新增Dto（备件类型）
    /// </summary>
    public record EquSparePartTypeCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; } = "";

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 备件ID集合
        /// </summary>
        public IEnumerable<long> SparePartIDs { get; set; }
    }

    /// <summary>
    /// 更新Dto（备件类型）
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
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备件ID集合
        /// </summary>
        public IEnumerable<long> SparePartIDs { get; set; }
    }

    /// <summary>
    /// 分页Dto（备件类型）
    /// </summary>
    public class EquSparePartTypePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string SiteCode { get; set; } = "";

        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; } = "";

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;
    }
}
