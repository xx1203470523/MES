using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public interface IManuCommonService
    {
        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="procedureBo"></param>
        /// <returns></returns>
        Task VerifySfcsLockAsync(ManuProcedureBo procedureBo);

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task VerifyContainerAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="procedureBomBo"></param>
        /// <returns></returns>
        Task VerifyBomQtyAsync(ManuProcedureBomBo procedureBomBo);

        /// <summary>
        /// 验证条码掩码规则
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<bool> CheckBarCodeByMaskCodeRuleAsync(string barCode, long materialId);

        /// <summary>
        /// 获取载具里面的条码（带验证）
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<IEnumerable<VehicleSFCResponseBo>> GetSFCsByVehicleCodesAsync(VehicleSFCRequestBo requestBo);

        /// <summary>
        /// 获取当前生产对象
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        Task<ManufactureResponseBo> GetManufactureBoAsync(ManufactureRequestBo requestBo);

        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="localizationService">国际化</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">状态(在 无效 删除 报废 锁定) 产品序列码状态为【xxxx】，不允许操作  包装验证  产品序列码已经被包装，不允许操作 </exception>
        Task<IEnumerable<ManuSfcBo>> GetManuSfcInfos(MultiSFCBo param, ILocalizationService localizationService);

        /// <summary>
        /// 校验工序是否合法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task VerifyProcedureAsync(JobRequestBo param);

    }
}
