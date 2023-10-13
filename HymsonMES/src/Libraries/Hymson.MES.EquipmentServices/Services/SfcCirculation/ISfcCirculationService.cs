using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;

namespace Hymson.MES.EquipmentServices.Services.SfcCirculation
{
    /// <summary>
    /// 流转表条码绑定和解绑服务
    /// </summary>
    public interface ISfcCirculationService
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <returns></returns>
        Task SfcCirculationBindAsync(SfcCirculationBindDto sfcCirculationBindDto);

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        Task SfcCirculationUnBindAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto, SfcCirculationTypeEnum sfcCirculationTypeEnum);
        
        /// <summary>
        /// 临时用于PDA
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        Task PDASfcCirculationUnBindAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto, SfcCirculationTypeEnum sfcCirculationTypeEnum);
        
        /// <summary>
        /// 解绑并返回流转列表
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        Task <IEnumerable<ManuSfcCirculationEntity>> GetSfcCirculationUnBindAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto, SfcCirculationTypeEnum sfcCirculationTypeEnum);

        /// <summary>
        /// 组件添加
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        Task SfcCirculationModuleAddAsync(SfcCirculationBindDto sfcCirculationBindDto, SfcCirculationTypeEnum sfcCirculationTypeEnum);

        /// <summary>
        /// 组件移除
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <returns></returns>
        Task SfcCirculationModuleRemoveAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto);
        /// <summary>
        /// 获取模组/PACK绑定SFC信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        Task<IEnumerable<CirculationBindDto>> GetCirculationBindSfcsAsync(string sfc, SfcCirculationTypeEnum? sfcCirculationTypeEnum = SfcCirculationTypeEnum.Merge);
        /// <summary>
        /// CCS绑定
        /// </summary>
        /// <param name="sfcCirculationCCSBindDto"></param>
        /// <returns></returns>
        Task SfcCirculationCCSBindAsync(SfcCirculationCCSBindDto sfcCirculationCCSBindDto);

        /// <summary>
        /// CCS解绑
        /// </summary>
        /// <param name="sfcCirculationCCSUnBindDto"></param>
        /// <returns></returns>
        Task SfcCirculationCCSUnBindAsync(SfcCirculationCCSUnBindDto sfcCirculationCCSUnBindDto);
        /// <summary>
        /// 根据模组码查询需要绑定CCS位置
        /// </summary>
        /// <param name="sfc">模组码</param>
        /// <returns></returns>
        Task<CirculationBindCCSLocationDto> GetBindCCSLocationAsync(string sfc);
        /// <summary>
        /// CCS NG设定
        /// </summary>
        /// <param name="sfcCirculationCCSNgSetDto"></param>
        /// <returns></returns>
        Task SfcCirculationCCSNgSetAsync(SfcCirculationCCSNgSetDto sfcCirculationCCSNgSetDto);

        /// <summary>
        /// 根据模组条码获取绑定CCS信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<CirculationModuleCCSInfoDto> GetCirculationModuleCCSInfoAsync(string sfc);

        /// <summary>
        /// 获取需要补料的NG清单
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<NgCirculationModuleCCSInfoDto> GetReplenishNGDataAsync();

        /// <summary>
        /// CCS确认
        /// </summary>
        /// <param name="sfcCirculationCCSConfirmDto"></param>
        /// <returns></returns>
        Task SfcCirculationCCSConfirmAsync(SfcCirculationCCSConfirmDto sfcCirculationCCSConfirmDto);


        /// <summary>
        /// 补料确认
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task ReplenishNGConfirmAsync(ReplenishInputDto para);
    }
}
