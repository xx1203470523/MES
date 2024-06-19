/*
 *creator: Karl
 *
 *describe: 备件库存 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquSparepartInventory
{
    /// <summary>
    ///  分页查询View
    /// </summary>
    public class EquSparepartInventoryPageView : BaseEntity
    {

        /// <summary>
        /// 备件Id
        /// </summary>
        public long SparepartId { get; set; }
        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparepartCode { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparepartName { get; set; }


        /// <summary>
        /// 备件类型编码
        /// /// </summary>
        public string SparepartGroupCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary> 
        public string SparepartGroupName { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Specifications { get; set; }

    }
}
