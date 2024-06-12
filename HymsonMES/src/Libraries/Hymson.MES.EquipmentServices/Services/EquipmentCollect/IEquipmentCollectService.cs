using Hymson.MES.EquipmentServices.Dtos;
using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.EquipmentServices.Services.EquipmentCollect
{
    /// <summary>
    /// 设备信息收集服务接口
    /// @author Czhipu
    /// @date 2023-05-16 04:51:15
    /// </summary>
    public interface IEquipmentCollectService
    {
        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentHeartbeatAsync(EquipmentHeartbeatDto request);

        /// <summary>
        /// 设备状态监控
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentStateAsync(EquipmentStateDto request);

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentAlarmAsync(EquipmentAlarmDto request);

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentDownReasonAsync(EquipmentDownReasonDto request);



        /// <summary>
        /// 设备过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentProcessParamAsync(EquipmentProcessParamDto request);

        /// <summary>
        /// 设备产品过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCDto request);

        /// <summary>
        /// 设备产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamDto request);

        /// <summary>
        /// 设备产品NG录入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentProductNgAsync(EquipmentProductNgDto request);



        /// <summary>
        /// 获取开机配方列表
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <returns></returns>
        Task<List<BootupParam>> GetEquipmentBootupRecipeSetAsync(GetEquipmentBootupRecipeSetDto dto);
        /// <summary>
        /// 获取指定配方的开机参数
        /// </summary>
        /// <param name="RecipeCode"></param>
        /// <returns></returns>
        Task<BootupParamDetail> GetEquipmentBootupRecipeDetailAsync(GetEquipmentBootupParamDetailDto dto);
        /// <summary>
        /// 开机配方参数采集
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EquipmentBootupParamCollectAsync(BootupParamCollectDto dto);
        /// <summary>
        /// 开机参数版本校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EquipmentBootupParamVersonCheckAsync(EquipmentBootupParamVersonCheckDto dto);

        /// <summary>
        /// 条码生产记录
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<ManuSfcStatusOutputDto> GetManuSfcStatusByProcedureAsync(ManuSfcStatusQueryDto queryDto);
    }
}
