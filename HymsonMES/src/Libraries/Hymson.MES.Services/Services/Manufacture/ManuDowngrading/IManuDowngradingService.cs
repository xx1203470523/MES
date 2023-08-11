/*
 *creator: Karl
 *
 *describe: 降级录入    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 降级录入 service接口
    /// </summary>
    public interface IManuDowngradingService
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="manuDowngradingSaveDto"></param>
        /// <returns></returns>
        Task SaveManuDowngradingAsync(ManuDowngradingSaveDto manuDowngradingSaveDto);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingDto>> GetManuDowngradingBySfcsAsync(string[] sfcs);
    }
}
