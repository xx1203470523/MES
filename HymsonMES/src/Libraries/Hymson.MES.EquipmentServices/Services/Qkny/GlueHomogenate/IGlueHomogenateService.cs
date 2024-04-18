using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.GlueHomogenate
{
    /// <summary>
    /// 制胶匀浆工序
    /// </summary>
    public interface IGlueHomogenateService
    {
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
        /// 投料前校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ConsumeEquBeforeCheckAsync(ConsumeEquBeforeCheckDto dto);

        /// <summary>
        /// 设备投料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ConsumeEquAsync(ConsumeEquDto dto);

        /// <summary>
        /// 上料完成(制胶匀浆)019
        /// </summary>
        /// <param name="dto"></param>
        Task FeedingCompletedAsync(FeedingCompletedDto dto);

        /// <summary>
        /// 设备产出
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<string> OutputEquAsync(QknyBaseDto dto);

        /// <summary>
        /// 批次转移
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task BatchMoveAsync(BatchMoveDto dto);

        /// <summary>
        /// 设备投料非生产投料(制胶匀浆)022
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ConsumeInNonProductionEquAsync(ConsumeInNonProductionEquDto dto);
    }
}
