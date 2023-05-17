/*
 *creator: Karl
 *
 *describe: 托盘条码关系仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘条码关系仓储接口
    /// </summary>
    public interface IManuTraySfcRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTraySfcRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuTraySfcRelationEntity manuTraySfcRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuTraySfcRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuTraySfcRelationEntity> manuTraySfcRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuTraySfcRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuTraySfcRelationEntity manuTraySfcRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuTraySfcRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuTraySfcRelationEntity> manuTraySfcRelationEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuTraySfcRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTraySfcRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据TrayLoadIdD获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTraySfcRelationEntity>> GetByTrayLoadIdAsync(long trayLoadId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuTraySfcRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTraySfcRelationEntity>> GetManuTraySfcRelationEntitiesAsync(ManuTraySfcRelationQuery manuTraySfcRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuTraySfcRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuTraySfcRelationEntity>> GetPagedInfoAsync(ManuTraySfcRelationPagedQuery manuTraySfcRelationPagedQuery);

        /// <summary>
        /// 根据托盘条码查询装载信息
        /// </summary>
        /// <param name="manuTraySfcRelationQuery"></param>
        /// <returns></returns> 
        Task<IEnumerable<ManuTraySfcRelationEntity>> GetManuTraySfcRelationByTrayCodeAsync(ManuTraySfcRelationByTrayCodeQuery manuTraySfcRelationByTrayCode);
        #endregion
    }
}
