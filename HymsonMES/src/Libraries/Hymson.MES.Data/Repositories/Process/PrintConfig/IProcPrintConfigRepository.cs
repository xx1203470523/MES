using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;


namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcPrintConfigRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcPrinterEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcPrinterEntity entity);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrintName"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(string PrintName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcPrinterEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcPrinterEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcPrinterEntity>> GetListAsync(ProcPrinterPagedQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ProcPrinterEntity> GetByPrintNameAsync(EntityByCodeQuery param);

        /// <summary>
        /// 根据IP查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcPrinterEntity> GetByPrintIpAsync(EntityByCodeQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcPrinterEntity>> GetBaseListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcPrinterEntity>> GetEntitiesAsync(ProcPrinterQuery Query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcPrinterEntity>> GetPagedListAsync(ProcPrinterPagedQuery PagedQuery);
    }
}
