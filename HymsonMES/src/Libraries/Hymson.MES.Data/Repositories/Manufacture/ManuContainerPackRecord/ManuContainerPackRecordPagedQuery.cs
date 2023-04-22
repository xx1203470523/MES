/*
 *creator: Karl
 *
 *describe: 容器装载记录 分页查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器装载记录 分页参数
    /// </summary>
    public class ManuContainerPackRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 容器条码id
        /// </summary>
        public long? ContainerBarCodeId { get; set; }

        /// <summary>
        /// 装载条码
        /// </summary>
        public string? LadeBarCode { get; set; }
    }
}
