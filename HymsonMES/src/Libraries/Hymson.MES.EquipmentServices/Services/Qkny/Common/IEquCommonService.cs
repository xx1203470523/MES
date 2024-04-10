using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.Common
{
    /// <summary>
    /// 设备通用接口
    /// </summary>
    public interface IEquCommonService
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
        /// 获取开机参数列表
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
        /// 工装寿命上报042
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ToolLifeAsync(ToolLifeDto dto);

        /// <summary>
        /// 设备过程参数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto);

        /// <summary>
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ProductParamAsync(ProductParamDto dto);

        /// <summary>
        /// 产品参数上传046
        /// 多个条码参数相同
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ProductParamSameMultSfcAsync(ProductParamSameMultSfcDto dto);
    }
}
