using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（工装注册）
    /// equ_consumable
    /// @author 陈志谱
    /// @date 2023-02-18
    /// </summary>
    public class EquConsumableEntity : BaseEntity, ISite
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
        public int ManagementMode { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点Id
        /// </summary>
        long ISite.SiteId { get; }

    }
}
