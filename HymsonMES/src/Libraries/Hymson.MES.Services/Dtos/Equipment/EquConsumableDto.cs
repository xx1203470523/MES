using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// Dto（工装注册）
    /// </summary>
    public record EquConsumableDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工装编码
        /// </summary>
        public string ConsumableCode { get; set; } = "";

        /// <summary>
        /// 工装名称
        /// </summary>
        public string ConsumableName { get; set; } = "";

        /// <summary>
        /// 工装类型ID
        /// </summary>
        public long ConsumableTypeId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

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
    /// 新增Dto（工装注册）
    /// </summary>
    public record EquConsumableCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 工装编码
        /// </summary>
        public string ConsumableCode { get; set; } = "";

        /// <summary>
        /// 工装名称
        /// </summary>
        public string ConsumableName { get; set; } = "";

        /// <summary>
        /// 工装类型ID
        /// </summary>
        public long ConsumableTypeId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

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
        /// 设备ID集合
        /// </summary>
        public IEnumerable<long> EquipmentIDs { get; set; }
    }

    /// <summary>
    /// 更新Dto（工装注册）
    /// </summary>
    public record EquConsumableModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工装名称
        /// </summary>
        public string ConsumableName { get; set; } = "";

        /// <summary>
        /// 工装类型ID
        /// </summary>
        public long ConsumableTypeId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

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
        /// 设备ID集合
        /// </summary>
        public IEnumerable<long> EquipmentIDs { get; set; }
    }

    /// <summary>
    /// 分页Dto（工装注册）
    /// </summary>
    public class EquConsumablePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string SiteCode { get; set; } = "";
    }

}
