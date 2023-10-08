/*
 *creator: Karl
 *
 *describe: 开机参数表    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
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
    /// 开机参数表 服务
    /// </summary>
    public class ProcBootupparamService : IProcBootupparamService
    {

        private readonly ICurrentEquipment _currentEquipment;
        private readonly IEquEquipmentRepository _equipmentRepository;
        private readonly IProcBootuprecipeRepository _recipeRepository;
        /// <summary>
        /// 开机参数表 仓储
        /// </summary>
        private readonly IProcBootupparamRepository _procBootupparamRepository;
        private readonly IProcBootupparamrecordRepository _procBootupparamrecordRepository;
        private readonly AbstractValidator<ProcBootupparamCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBootupparamModifyDto> _validationModifyRules;
        

        public ProcBootupparamService(ICurrentEquipment currentEquipment,  
            IProcBootupparamRepository procBootupparamRepository, 
            AbstractValidator<ProcBootupparamCreateDto> validationCreateRules, 
            IEquEquipmentRepository equipmentRepository,
            IProcBootuprecipeRepository recipeRepository,
            IProcBootupparamrecordRepository procBootupparamrecordRepository,
            AbstractValidator<ProcBootupparamModifyDto> validationModifyRules)
        {
            _currentEquipment = currentEquipment;
            _equipmentRepository = equipmentRepository;
            _procBootupparamRepository = procBootupparamRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _recipeRepository = recipeRepository;
            _procBootupparamrecordRepository = procBootupparamrecordRepository;
        }
        /// <summary>
        /// 开机参数采集
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async  Task EquipmentBootupParamCollectAsync(BootupParamCollectDto dto)
        {
            var equ = await _equipmentRepository.GetByIdAsync(_currentEquipment.Id.Value);
            var recipe = await _recipeRepository.GetByCodeAsync(dto.RecipeCode);
            if(dto.ParamList!=null&&dto.ParamList.Any())
            {
                var lst = new List<ProcBootupparamrecordEntity>();
                foreach (var item in dto.ParamList)
                {
                    ProcBootupparamrecordEntity entity = new ProcBootupparamrecordEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentEquipment.SiteId,
                        EquipmentId = equ.Id,
                        ParamCode = item.ParamCode,
                        ParamLower = item.ParamLower,
                        IsDeleted =0,
                        ParamUpper = item.ParamUpper,
                        ParamValue = item.ParamValue,
                        ProductCode = dto.ProductCode,
                        RecipeId = recipe.Id,
                        Timestamp = item.Timestamp
                    };

                    lst.Add(entity);
                }
                await _procBootupparamrecordRepository.InsertsAsync(lst);
                
            }


        }
        /// <summary>
        /// 获取开机配方明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BootupParamDetail> GetEquipmentBootupRecipeDetailAsync(GetEquipmentBootupParamDetailDto dto)
        {
            var equ = await _equipmentRepository.GetByIdAsync(_currentEquipment.Id.Value);
            var recipe = await _recipeRepository.GetByCodeAsync(dto.RecipeCode);
            var lst = await _procBootupparamRepository.GetProcBootupparamEntitiesAsync(new ProcBootupparamQuery()
            {
                SiteId = _currentEquipment.SiteId,
                RecipeId = recipe.Id
            });
            BootupParamDetail bootupParamDetail = new BootupParamDetail()
            {
                ParamList = new List<BootupParamDetail.BootupParamDetailItem>(),
                Version = dto.Version,
            };
            
            foreach (var item in lst)
            {
                BootupParamDetail.BootupParamDetailItem bpi = new BootupParamDetail.BootupParamDetailItem()
                {
                    ParamCode = item.ParamId.ToString(),
                    ParamLower = item.MinValue,
                    ParamUpper = item.MaxValue,
                    ParamValue = item.ParamValue
                };
                bootupParamDetail.ParamList.Add(bpi);
            }
            return bootupParamDetail;
        }
    }
}
