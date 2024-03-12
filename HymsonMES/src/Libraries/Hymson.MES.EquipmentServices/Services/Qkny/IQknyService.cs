using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny
{
    /// <summary>
    /// 顷刻能源服务
    /// </summary>
    public interface IQknyService
    {
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OperatorLoginAsync(OperationLoginDto dto);

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task HeartbeatAsync(HeartbeatDto dto);

        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task StateAsync(StateDto dto);

        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AlarmAsync(AlarmDto dto);

        /// <summary>
        /// CCD文件上传完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CcdFileUploadCompleteAsync(CCDFileUploadCompleteDto dto);

        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<GetRecipeListReturnDto>> GetRecipeListAsync(GetRecipeListDto dto);

        /// <summary>
        /// 获取开机参数明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto);

        /// <summary>
        /// 开机参数校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task RecipeAsync(RecipeDto dto);

        /// <summary>
        /// 原材料上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task FeedingAsync(FeedingDto dto);

        /// <summary>
        /// 半成品上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task HalfFeedingAsync(HalfFeedingDto dto);

        /// <summary>
        /// AGV叫料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AgvMaterialAsync(AgvMaterialDto dto);

        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto);

        /// <summary>
        /// 获取配方明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<FormulaDetailGetReturnDto> FormulaDetailGetAsync(FormulaDetailGetDto dto);

        /// <summary>
        /// 配方校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task FormulaVersionExamineAsync(FormulaVersionExamineDto dto);

        /// <summary>
        /// 设备过程参数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto);
    }
}
