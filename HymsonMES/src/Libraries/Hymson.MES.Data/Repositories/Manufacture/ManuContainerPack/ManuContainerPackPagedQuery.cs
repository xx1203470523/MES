/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除） 分页查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 分页参数
    /// </summary>
    public class ManuContainerPackPagedQuery : PagerInfo
    {
        /// <summary>
        /// 容器Id
        /// </summary>
        public long? BarCodeId { get; set; }
        /// <summary>
        /// 容器条码
        /// </summary>
        public string? BarCode { get; set; }
    }
}
