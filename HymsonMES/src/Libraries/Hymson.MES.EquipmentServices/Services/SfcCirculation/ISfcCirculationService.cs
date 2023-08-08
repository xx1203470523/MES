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
        /// 组件添加
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        Task SfcCirculationModuleAddAsync(SfcCirculationBindDto sfcCirculationBindDto,SfcCirculationTypeEnum sfcCirculationTypeEnum);

        /// <summary>
        /// 组件移除
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <returns></returns>
        Task SfcCirculationModuleRemoveAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto);

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
    }
}
