using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// Dto（备件注册）
    /// </summary>
    public record EquSparePartDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; } = "";

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; } = "";

        /// <summary>
        /// 备件类型ID
        /// </summary>
        public long SparePartTypeId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

        /// <summary>
        /// 是否关键备件
        /// </summary>
        public int IsKey { get; set; } = 0;

        /// <summary>
        /// 是否标准件
        /// </summary>
        public int IsStandard { get; set; } = 0;

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 图纸编号
        /// </summary>
        public string BluePrintNo { get; set; } = "";

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// 管理方式
        /// </summary>
        public bool? ManagementMode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 新增Dto（备件注册）
    /// </summary>
    public record EquSparePartCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; } = "";

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; } = "";

        /// <summary>
        /// 备件类型ID
        /// </summary>
        public long SparePartTypeId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

        /// <summary>
        /// 是否关键备件
        /// </summary>
        public int IsKey { get; set; } = 0;

        /// <summary>
        /// 是否标准件
        /// </summary>
        public int IsStandard { get; set; } = 0;

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 图纸编号
        /// </summary>
        public string BluePrintNo { get; set; } = "";

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// 管理方式
        /// </summary>
        public bool? ManagementMode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";


        /// <summary>
        /// ID集合（设备组）
        /// </summary>
        public IEnumerable<long> EquipmentGroupIDs { get; set; }
    }

    /// <summary>
    /// 更新Dto（备件注册）
    /// </summary>
    public record EquSparePartModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode { get; set; } = "";

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName { get; set; } = "";

        /// <summary>
        /// 备件类型ID
        /// </summary>
        public long SparePartTypeId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

        /// <summary>
        /// 是否关键备件
        /// </summary>
        public int IsKey { get; set; } = 0;

        /// <summary>
        /// 是否标准件
        /// </summary>
        public int IsStandard { get; set; } = 0;

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 图纸编号
        /// </summary>
        public string BluePrintNo { get; set; } = "";

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// 管理方式
        /// </summary>
        public int ManagementMode { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";


        /// <summary>
        /// ID集合（设备组）
        /// </summary>
        public IEnumerable<long> EquipmentGroupIDs { get; set; }
    }

    /// <summary>
    /// 分页Dto（备件注册）
    /// </summary>
    public class EquSparePartPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }
    }

}
