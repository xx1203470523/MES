/*
 *creator: Karl
 *
 *describe: 设备参数组详情    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-08-02 02:08:48
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 设备参数组详情，数据实体对象   
    /// proc_equipment_group_param_detail
    /// @author Karl
    /// @date 2023-08-02 02:08:48
    /// </summary>
    public class ProcEquipmentGroupParamDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 开机配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParamId { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }

       /// <summary>
        /// 小数位数
        /// </summary>
        public int? DecimalPlaces { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       
    }
}
