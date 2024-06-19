using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（备件注册） 
    /// equ_sparepart
    /// @author 陈志谱
    /// @date 2023-02-13 02:27:20
    /// </summary>
    public class EquSparePartEntity : BaseEntity
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
        public TrueOrFalseEnum IsKey { get; set; }

        /// <summary>
        /// 是否标准件
        /// </summary>
        public TrueOrFalseEnum IsStandard { get; set; }

        /// <summary>
        /// 备件/工装
        /// </summary>
        public EquipmentPartTypeEnum Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

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
        public int ManagementMode { get; set; } = DbDefaultValueConstant.IntDefaultValue;

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; } = 0;

        /// <summary>
        /// 站点ID 
        /// </summary>
        public long? SiteId { get; set; }
    }
}
