using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using System.Reflection.Emit;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode
{
    /// <summary>
    /// 条码生成
    /// </summary>
    public class ManuGenerateBarcodeExampleService : IManuGenerateBarcodeExampleService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuGenerateBarcodeService"></param>
        public ManuGenerateBarcodeExampleService(IManuGenerateBarcodeService manuGenerateBarcodeService)
        {
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeDto param)
        {
            return await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                SiteId=_currentSite.SiteId??0,
                UserName=_currentUser.UserName,
                CodeRuleId=param.CodeRuleId,
                Count=param.Count,
                IsTest=param.IsTest
            });
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleDto param)
        {
            return await _manuGenerateBarcodeService.GenerateBarcodeListAsync(new CodeRuleBo
            {
                IsTest = param.IsTest,
                Count = param.Count,
                ProductId = param.ProductId,
                CodeType = param.CodeType,
                PackType = param.PackType,
                Base = param.Base,
                IgnoreChar = param.IgnoreChar,
                Increment = param.Increment,
                OrderLength = param.OrderLength,
                ResetType= param.ResetType,
                StartNumber = param.StartNumber,
                CodeRulesMakeList= param.CodeRulesMakeList
            }) ;
        }
    }
}
