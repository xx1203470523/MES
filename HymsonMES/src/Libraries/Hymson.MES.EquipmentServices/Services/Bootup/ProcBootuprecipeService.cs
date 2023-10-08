/*
 *creator: Karl
 *
 *describe: 开机配方表    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.Process
{
    /// <summary>
    /// 开机配方表 服务
    /// </summary>
    public class ProcBootuprecipeService : IProcBootuprecipeService
    {
        private readonly ICurrentUser _currentUser;
       
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        
        /// <summary>
        /// 开机配方表 仓储
        /// </summary>
        private readonly IProcBootuprecipeRepository _procBootuprecipeRepository;
        private readonly AbstractValidator<ProcBootuprecipeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBootuprecipeModifyDto> _validationModifyRules;

        public ProcBootuprecipeService(ICurrentUser currentUser,
            IProcBootuprecipeRepository procBootuprecipeRepository, 
            AbstractValidator<ProcBootuprecipeCreateDto> validationCreateRules, 
            ICurrentEquipment currentEquipment,
            IEquEquipmentRepository equEquipmentRepository,
            AbstractValidator<ProcBootuprecipeModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
           
            _procBootuprecipeRepository = procBootuprecipeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentEquipment = currentEquipment;
            _equEquipmentRepository = equEquipmentRepository;
        }

        public async Task EquipmentBootupParamVersonCheckAsync(EquipmentBootupParamVersonCheckDto dto)
        {
            var equ = await _equEquipmentRepository.GetByIdAsync(_currentEquipment.Id.Value);
            var lst = await _procBootuprecipeRepository.GetProcBootuprecipeEntitiesAsync(new ProcBootuprecipeQuery()
            {
                SiteId = _currentEquipment.SiteId,
                EquGroupId = equ.EquipmentGroupId
            });
            if(!lst.Any(p=>p.Version==dto.Version)) {
                throw new Exception("版本校验不通过");
            }
            
        }

        /// <summary>
        /// 获取设备启动配方集合
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<BootupParam>> GetEquipmentBootupRecipeSetAsync(GetEquipmentBootupRecipeSetDto dto)
        {
            var equ = await _equEquipmentRepository.GetByIdAsync(_currentEquipment.Id.Value);
            var lst = await _procBootuprecipeRepository.GetProcBootuprecipeEntitiesAsync(new ProcBootuprecipeQuery()
            {
                SiteId = _currentEquipment.SiteId,
                EquGroupId = equ.EquipmentGroupId
            });
            List<BootupParam> bootupParams = new List<BootupParam>();

            foreach (var kv in lst)
            {
                BootupParam bootupParam = new BootupParam()
                {
                    LastUpdateOnTime = kv.UpdatedOn??kv.CreatedOn,
                    ProductCode = kv.ProductId.ToString(),
                    RecipeCode = kv.Code,
                    Version = kv.Version,
                };
                bootupParams.Add(bootupParam);
            }
            return bootupParams;
        }
    }
}
