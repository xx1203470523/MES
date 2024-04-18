using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using System.Reflection.Emit;
using Hymson.MES.Core.Constants.Manufacture;
using System.Reflection;
using Hymson.MES.Core.Attribute;

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
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        public ManuGenerateBarcodeExampleService(IManuGenerateBarcodeService manuGenerateBarcodeService,ICurrentSite currentSite,ICurrentUser currentUser)
        {
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _currentSite = currentSite;
            _currentUser = currentUser;
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
                CodeMode= param.CodeMode,
                PackType = param.PackType,
                Base = param.Base,
                IgnoreChar = param.IgnoreChar,
                Increment = param.Increment,
                OrderLength = param.OrderLength,
                ResetType= param.ResetType,
                StartNumber = param.StartNumber,
                CodeRulesMakeList= param.CodeRulesMakeList,
                SiteId=_currentSite.SiteId??0
            }) ;
        }

        /// <summary>
        /// 生成通配符列表供前端渲染下拉使用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BarcodeWildcardItemDto> GetGenerateBarcodeWildcardItemDtos()
        {
            var dtos = new List<BarcodeWildcardItemDto>();
            var fieldInfos = typeof(GenerateBarcodeWildcard).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var fieldInfo in fieldInfos)
            {
                var barcodeWildcardItemDto = new BarcodeWildcardItemDto();
                var descriptionAttribute = fieldInfo.GetCustomAttribute<GenerateBarcodeWildcardDescriptionAttribute>();
                if (descriptionAttribute == null) continue;
                barcodeWildcardItemDto.Description = descriptionAttribute.Description;
                barcodeWildcardItemDto.CodeTypes = descriptionAttribute.CodeTypes;
                barcodeWildcardItemDto.CodeModes = descriptionAttribute.CodeModes;
                var obj = fieldInfo.GetValue(fieldInfo.Name);
                if (obj == null) continue;
                barcodeWildcardItemDto.Key = obj?.ToString();
                dtos.Add(barcodeWildcardItemDto);
            }
            return dtos;
        }
    }
}
