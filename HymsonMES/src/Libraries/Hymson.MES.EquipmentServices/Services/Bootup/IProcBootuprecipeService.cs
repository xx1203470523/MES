/*
 *creator: Karl
 *
 *describe: 开机配方表    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
 */

using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;


namespace Hymson.MES.EquipmentServices.Services.Process
{
    /// <summary>
    /// 开机配方表 service接口
    /// </summary>
    public interface IProcBootuprecipeService
    {
        Task<List<BootupParam>> GetEquipmentBootupRecipeSetAsync(GetEquipmentBootupRecipeSetDto dto);
       
        /// <summary>
        /// 开机参数版本校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EquipmentBootupParamVersonCheckAsync(EquipmentBootupParamVersonCheckDto dto);

    }
}
