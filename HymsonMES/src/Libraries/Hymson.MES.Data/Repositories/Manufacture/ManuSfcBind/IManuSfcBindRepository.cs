/*
 *creator: Karl
 *
 *describe: 条码绑定关系表仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:11
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码绑定关系表仓储接口
    /// </summary>
    public interface IManuSfcBindRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcBindEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcBindEntity manuSfcBindEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcBindEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcBindEntity> manuSfcBindEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcBindEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcBindEntity manuSfcBindEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcBindEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcBindEntity> manuSfcBindEntitys);

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
        /// 批量删除（硬删除）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeleteTruesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcBindEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcBindEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据SFC查询绑定数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcBindEntity>> GetBySFCAsync(string sfc);

        /// <summary>
        /// 根据SFC和BindSfc查询已存在绑定关系
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="bindSfcs"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcBindEntity>> GetByBindSFCAsync(string sfc, string[] bindSfcs);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcBindQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcBindEntity>> GetManuSfcBindEntitiesAsync(ManuSfcBindQuery manuSfcBindQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcBindPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcBindEntity>> GetPagedInfoAsync(ManuSfcBindPagedQuery manuSfcBindPagedQuery);
        #endregion
    }
}
