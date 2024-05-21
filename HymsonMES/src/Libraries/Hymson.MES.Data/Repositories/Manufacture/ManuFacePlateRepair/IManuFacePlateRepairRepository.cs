/*
 *creator: Karl
 *
 *describe: 在制品维修面板仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 在制品维修仓储接口
    /// </summary>
    public interface IManuFacePlateRepairRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFacePlateRepairEntity manuFacePlateRepairEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateRepairEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuFacePlateRepairEntity> manuFacePlateRepairEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuFacePlateRepairEntity manuFacePlateRepairEntity);

        /// <summary>
        /// 根据FacePlateId更新
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        Task<int> UpdateByFacePlateRepairIdAsync(ManuFacePlateRepairEntity manuFacePlateRepairEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuFacePlateRepairEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuFacePlateRepairEntity> manuFacePlateRepairEntitys);

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
        Task<ManuFacePlateRepairEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateRepairEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuFacePlateRepairQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateRepairEntity>> GetManuFacePlateRepairEntitiesAsync(ManuFacePlateRepairQuery manuFacePlateRepairQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateRepairPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateRepairEntity>> GetPagedInfoAsync(ManuFacePlateRepairPagedQuery manuFacePlateRepairPagedQuery);
        /// <summary>
        /// 通过FacePlateId获取明细
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        Task<ManuFacePlateRepairEntity> GetByFacePlateIdAsync(long facePlateId);
        #endregion

        #region 维修记录

        /// <summary>
        /// 根据ProductBadId批量获取维修明细数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcRepairDetailEntity>> ManuSfcRepairDetailByProductBadIdAsync(ManuSfcRepairDetailByProductBadIdQuery query);


        /// <summary>
        /// 根据SFC获取维修记录数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<ManuSfcRepairRecordEntity> GetManuSfcRepairBySFCAsync(GetManuSfcRepairBySfcQuery query);


        /// <summary>
        /// 新增维修记录
        /// </summary>
        /// <param name="manuSfcRepairRecordEntity"></param>
        /// <returns></returns> 
        Task<int> InsertRecordAsync(ManuSfcRepairRecordEntity manuSfcRepairRecordEntity);

        /// <summary>
        /// 批量新增维修记录
        /// </summary>
        /// <param name="manuSfcRepairRecordEntitys"></param>
        /// <returns></returns> 
        Task<int> InsertsRecordAsync(List<ManuSfcRepairRecordEntity> manuSfcRepairRecordEntitys);


        /// <summary>
        /// 新增维修明细
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        Task<int> InsertDetailAsync(ManuSfcRepairDetailEntity manuSfcRepairDetailEntity);

        /// <summary>
        /// 批量新增维修明细
        /// </summary>
        /// <param name="manuFacePlateRepairEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsDetailAsync(List<ManuSfcRepairDetailEntity> manuSfcRepairDetailEntities);

        /// <summary>
        /// 批量修改维修明细
        /// </summary>
        /// <param name="manuFacePlateRepairEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateDetailsAsync(List<ManuSfcRepairDetailEntity> manuSfcRepairDetailEntities);

        /// <summary>
        /// 获取维修详情
        /// </summary>
        /// <param name="manuSfcRepairDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcRepairDetailEntity>> GetManuSfcRepairDetailEntitiesAsync(ManuSfcRepairDetailQuery manuSfcRepairDetailQuery);
        
        /// <summary>
        /// 获取维修记录
        /// </summary>
        /// <param name="manuFacePlateRepairQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcRepairRecordEntity>> GetManuSfcRepairRecordEntitiesAsync(ManuSfcRepairRecordQuery manuSfcRepairRecordQuery);
        #endregion
    }
}
