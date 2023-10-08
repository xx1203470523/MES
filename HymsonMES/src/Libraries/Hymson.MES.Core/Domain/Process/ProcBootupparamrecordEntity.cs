/*
 *creator: Karl
 *
 *describe: 开机参数采集表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 开机参数采集表，数据实体对象   
    /// proc_bootupparamrecord
    /// @author wxk
    /// @date 2023-07-12 04:58:46
    /// </summary>
    public class ProcBootupparamrecordEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 上限值
        /// </summary>
        public string ParamUpper { get; set; }

       /// <summary>
        /// 下限值
        /// </summary>
        public string ParamLower { get; set; }

       /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

       
    }
}
