﻿using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode
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
        Task<IEnumerable<string>> GenerateBarCodeSerialNumberAsync(BarCodeSerialNumberBo param);

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException">未找到生成规则</exception>
        Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeBo param);

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException">未找到生成规则</exception>
        Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleBo param);
    }
}

