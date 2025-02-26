using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码流转查询服务
    /// </summary>
    public interface IManuSfcCirculationService
    {
        /// <summary>
        /// 获取条码绑定关系
        /// </summary>
        /// <param name="Sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationOutputDto>> GetManuSfcCirculationBySFCAsync(string Sfc);

        /// <summary>
        /// 删除绑定关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 解除条码绑定关系（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(IEnumerable<long> ids);

        /// <summary>
        /// 创建条码绑定关系
        /// </summary>
        /// <param name="craeteDto"></param>
        /// <returns></returns>
        Task CreateManuSfcCirculationAsync(ManuSfcCirculationBindDto craeteDto);
    }
}
