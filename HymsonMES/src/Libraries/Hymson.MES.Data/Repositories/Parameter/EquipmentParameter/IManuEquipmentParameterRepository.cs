using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Repositories.Parameter.Query;

namespace Hymson.MES.Data.Repositories.Parameter
{
    /// <summary>
    /// 产品过程参数
    /// </summary>
    public interface IManuEquipmentParameterRepository
    {

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<EquipmentParameterEntity> list);

        /// <summary>
        /// 获取建表SQL语句
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        string PrepareEquipmentParameterBySequenceTableSql(int sequence);

        /// <summary>
        /// 按设备Id查询设备参数
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>

        Task<PagedInfo<EquipmentParameterEntity>> GetParametesByEqumentIdEntitiesAsync(ManuEquipmentParameterPagedQuery pagedQuery);
    }
}
