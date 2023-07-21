/*
 *creator: Karl
 *
 *describe: 单位表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-29 02:13:40
 */

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 单位表 查询参数
    /// </summary>
    public class InteUnitQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :单位编码
        /// 空值 : false  
        /// </summary>
        public string? UnitCode { get; set; }

        /// <summary>
        /// 单位编码列表
        /// </summary>
        public string[] UnitCodes { get; set; }
    }
}
