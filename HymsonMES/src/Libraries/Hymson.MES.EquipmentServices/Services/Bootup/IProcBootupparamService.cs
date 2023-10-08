/*
 *creator: Karl
 *
 *describe: 开机参数表    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
 */

using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;


namespace Hymson.MES.EquipmentServices.Services.Process
{
    /// <summary>
    /// 开机参数表 service接口
    /// </summary>
    public interface IProcBootupparamService
    {
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
    }
}
