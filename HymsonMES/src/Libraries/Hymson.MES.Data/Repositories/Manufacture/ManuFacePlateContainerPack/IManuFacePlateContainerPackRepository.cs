using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateContainerPack.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public interface IManuFacePlateContainerPackRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuFacePlateContainerPackEntity> manuFacePlateContainerPackEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ManuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuFacePlateContainerPackEntity ManuFacePlateContainerPackEntity);

        /// <summary>
        /// 根据FacePlateId更新
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> UpdateByFacePlateIdAsync(ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuFacePlateContainerPackEntity> manuFacePlateContainerPackEntity);

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
        Task<ManuFacePlateContainerPackEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateContainerPackEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuFacePlateContainerPackQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateContainerPackEntity>> GetManuFacePlateContainerPackEntitiesAsync(ManuFacePlateContainerPackQuery manuFacePlateContainerPackQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateContainerPackPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateContainerPackEntity>> GetPagedInfoAsync(ManuFacePlateContainerPackPagedQuery manuFacePlateContainerPackPagedQuery);

        /// <summary>
        /// 通过FacePlateId获取明细
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        Task<ManuFacePlateContainerPackEntity> GetByFacePlateIdAsync(long facePlateId);
        #endregion
    }
}
