using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode
{
    /// <summary>
    /// 生成条码接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IManuGenerateBarcodeService
    {
        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GenerateBarcodeSerialNumberAsync(BarcodeSerialNumberDto param);

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException">未找到生成规则</exception>
        Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeDto param);

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException">未找到生成规则</exception>
        Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleDto param);
    }
}
