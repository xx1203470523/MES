/*
 *creator: Karl
 *
 *describe: 托盘装载信息表仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘装载信息表仓储接口
    /// </summary>
    public interface IManuTrayLoadRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTrayLoadEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuTrayLoadEntity manuTrayLoadEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuTrayLoadEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuTrayLoadEntity> manuTrayLoadEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuTrayLoadEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuTrayLoadEntity manuTrayLoadEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuTrayLoadEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuTrayLoadEntity> manuTrayLoadEntitys);

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
        Task<ManuTrayLoadEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTrayLoadEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuTrayLoadQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuTrayLoadEntity>> GetManuTrayLoadEntitiesAsync(ManuTrayLoadQuery manuTrayLoadQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuTrayLoadPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuTrayLoadEntity>> GetPagedInfoAsync(ManuTrayLoadPagedQuery manuTrayLoadPagedQuery);
        #endregion
    }
}
