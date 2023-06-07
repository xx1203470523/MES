using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Common.ManuCommon
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public interface IManuCommonService
    {
        /// <summary>
        /// 获取生产条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<(ManuSfcProduceEntity, ManuSfcProduceBusinessEntity)> GetProduceSFCAsync(string sfc, long siteId);

        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        Task VerifySfcsLockAsync(IEnumerable<string> sfcs, long siteId);

        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
         Task VerifySfcsLockAsync(string[] sfcs, long procedureId);

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
         Task VerifyContainerAsync(string[] sfcs, long siteId);

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task VerifyBomQtyAsync(long bomId, long procedureId, string sfc, long siteId);
    }
}
