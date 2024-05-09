using Hymson.MES.Core.Domain.Parameter;

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
        /// 获取表名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<string> GetParamTableName(long siteId, long equipmentId);
    }
}
