using Hymson.Infrastructure.Exceptions;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode
{
    /// <summary>
    /// 生成条码接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IManuGenerateBarcodeExampleService
    {
        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeDto param);

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleDto param);


        /// <summary>
        /// 生成通配符列表供前端渲染下拉使用
        /// </summary>
        /// <returns></returns>
        IEnumerable<BarcodeWildcardItemDto> GetGenerateBarcodeWildcardItemDtos();
    }
}
