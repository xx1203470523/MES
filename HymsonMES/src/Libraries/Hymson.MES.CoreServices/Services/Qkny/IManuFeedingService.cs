using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Qkny
{
    /// <summary>
    /// 物料加载
    /// </summary>
    public interface IManuFeedingService
    {
        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<ManuFeedingMaterialResponseDto> CreateAsync(ManuFeedingMaterialSaveDto saveDto);

        /// <summary>
        /// 根据上料点id获取最新上料记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuFeedingEntity> GetFeedingPointNewAsync(GetFeedingPointNewQuery query);

        /// <summary>
        /// 上料物料转移
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<bool> ManuFeedingTransferAsync(ManuFeedingTransferSaveDto saveDto);

        /// <summary>
        /// 上料点条码数量更新
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateManuFeedingBarcodeQtyAsync(UpdateFeedingBarcodeQtyCommand command);

        /// <summary>
        /// 根据资源获取所有的上料信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetAllByResourceIdAsync(EntityByResourceIdQuery query);

        /// <summary>
        /// 根据条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetAllBySfcListAsync(GetManuFeedingSfcListQuery query);

    }
}
