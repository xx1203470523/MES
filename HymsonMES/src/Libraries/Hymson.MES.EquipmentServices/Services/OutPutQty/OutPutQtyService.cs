using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Parameter.Query;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServices.Dtos.OutPutQty;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.OutPutQty
{
    /// <summary>
    /// 产出上报数量服务
    /// </summary>
    public class OutPutQtyService : IOutPutQtyService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<OutPutQtyDto> _validationOutPutQtyDtoRules;
        private readonly IManuOutputBindMaterialRepository _manuOutputBindMaterialRepository;
        private readonly IManuOutputNgRepository _manuOutputNgRepository;
        private readonly IManuOutputRepository _manuOutputRepository;
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuOutputBindMaterialRepository"></param>
        /// <param name="manuOutputNgRepository"></param>
        /// <param name="manuOutputRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="validationOutPutQtyDtoRules"></param>
        /// <param name="currentEquipment"></param> 
        public OutPutQtyService(IManuOutputBindMaterialRepository manuOutputBindMaterialRepository, IManuOutputNgRepository manuOutputNgRepository,
        IManuOutputRepository manuOutputRepository, IManuProductParameterRepository manuProductParameterRepository,
        IEquEquipmentRepository equEquipmentRepository, IProcResourceRepository procResourceRepository,
         IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IProcParameterRepository procParameterRepository, AbstractValidator<OutPutQtyDto> validationOutPutQtyDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationOutPutQtyDtoRules = validationOutPutQtyDtoRules;
            _currentEquipment = currentEquipment;

            _manuOutputBindMaterialRepository = manuOutputBindMaterialRepository;
            _manuOutputNgRepository = manuOutputNgRepository;
            _manuOutputRepository = manuOutputRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _procResourceRepository = procResourceRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _procParameterRepository = procParameterRepository;
        }

        /// <summary>
        /// 产出上报数量
        /// </summary>
        /// <param name="outPutQtyDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OutPutQtyAsync(OutPutQtyDto outPutQtyDto)
        {
            await _validationOutPutQtyDtoRules.ValidateAndThrowAsync(outPutQtyDto);

            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = outPutQtyDto.ResourceCode });
            if (procResource == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19006));
            }
            await _manuOutputRepository.InsertAsync(new ManuOutputEntity
            {
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = outPutQtyDto.LocalTime,
                OKQty = outPutQtyDto.OKQty,
                ResourceId = procResource.Id,
                SFC = outPutQtyDto.SFC,

                Id = IdGenProvider.Instance.CreateId(),
                CreatedBy = _currentEquipment.Name,
                UpdatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                SiteId = _currentEquipment.SiteId
            });

            var bindMaterialList = new List<ManuOutputBindMaterialEntity>();
            var parameterList = new List<ManuProductParameterEntity>();
            var ngList = new List<ManuOutputNgEntity>();
            //NG
            if (outPutQtyDto.NGList != null && outPutQtyDto.NGList.Any())
            {
                ngList = await NgListAsync(outPutQtyDto, procResource.Id);
            }
            //参数
            if (outPutQtyDto.ParamList != null && outPutQtyDto.ParamList.Any())
            {
                parameterList = await ParameterListAsync(outPutQtyDto, procResource.Id);
            }

            //绑定物料
            bindMaterialList = BindMaterialList(outPutQtyDto, procResource.Id);

            using var trans = TransactionHelper.GetTransactionScope();
            if (ngList != null && ngList.Any())
            {
                await _manuOutputNgRepository.InsertsAsync(ngList);
            }
            if (parameterList != null && parameterList.Any())
            {
                await _manuProductParameterRepository.InsertsAsync(parameterList);
            }
            if (bindMaterialList != null && bindMaterialList.Any())
            {
                await _manuOutputBindMaterialRepository.InsertsAsync(bindMaterialList);
            }
            trans.Complete();
        }

        /// <summary>
        /// NG列表
        /// </summary>
        /// <param name="outPutQtyDto"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private async Task<List<ManuOutputNgEntity>> NgListAsync(OutPutQtyDto outPutQtyDto, long resourceId)
        {
            var ngCodeArray = outPutQtyDto.NGList.Select(c => c.NGCode).ToArray();
            var codesQuery = new QualUnqualifiedCodeByCodesQuery
            {
                Site = _currentEquipment.SiteId,
                Codes = ngCodeArray
            };
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByCodesAsync(codesQuery);
            if (qualUnqualifiedCodes == null || !qualUnqualifiedCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19114)).WithData("Code", string.Join(',', ngCodeArray));
            }
            //如果有不存在的参数编码就提示
            var noIncludeCodes = ngCodeArray.Where(w => qualUnqualifiedCodes.Select(s => s.UnqualifiedCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19114)).WithData("Code", string.Join(',', noIncludeCodes));

            var ngList = new List<ManuOutputNgEntity>();

            if (outPutQtyDto.NGList != null && outPutQtyDto.NGList.Any())
            {
                foreach (var item in outPutQtyDto.NGList)
                {
                    var qualUnqualified = qualUnqualifiedCodes.Where(it => it.UnqualifiedCode == item.NGCode).FirstOrDefault();

                    ngList.Add(new ManuOutputNgEntity()
                    {
                        EquipmentId = _currentEquipment.Id ?? 0,
                        ResourceId = resourceId,
                        NGCode = item.NGCode,
                        NGQty = item.NGQty,
                        NGId = qualUnqualified.Id,
                        SFC = outPutQtyDto.SFC,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentEquipment.SiteId
                    });
                }
            }

            return ngList;
        }

        /// <summary>
        /// 组装参数信息
        /// </summary>
        /// <param name="outPutQtyDto"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private async Task<List<ManuProductParameterEntity>> ParameterListAsync(OutPutQtyDto outPutQtyDto, long resourceId)
        {
            var paramCodeArray = outPutQtyDto.ParamList.Select(c => c.ParamCode).ToArray();
            var codesQuery = new ProcParametersByCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                Codes = paramCodeArray
            };
            var procParameter = await _procParameterRepository.GetByCodesAsync(codesQuery);
            if (procParameter == null || !procParameter.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', paramCodeArray));
            }
            //如果有不存在的参数编码就提示
            var noIncludeCodes = paramCodeArray.Where(w => procParameter.Select(s => s.ParameterCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));
            var parameterList = new List<ManuProductParameterEntity>();

            foreach (var item in outPutQtyDto.ParamList)
            {
                var paramterEntit = procParameter.Where(it => it.ParameterCode == item.ParamCode).FirstOrDefault();
                parameterList.Add(new ManuProductParameterEntity()
                {
                    EquipmentId = _currentEquipment.Id ?? 0,
                    ResourceId = resourceId,
                    ParameterId = paramterEntit.Id,
                    LocalTime = outPutQtyDto.LocalTime,
                    ParamValue = item.ParamValue,
                    SFC = outPutQtyDto.SFC,
                    StandardUpperLimit = item.StandardUpperLimit,
                    StandardLowerLimit = item.StandardLowerLimit,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentEquipment.SiteId
                });
            }
            return parameterList;
        }


        /// <summary>
        /// 组装参数信息
        /// </summary>
        /// <param name="outPutQtyDto"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns> 
        private List<ManuOutputBindMaterialEntity> BindMaterialList(OutPutQtyDto outPutQtyDto, long resourceId)
        {
            var bindMaterialList = new List<ManuOutputBindMaterialEntity>();
            if (outPutQtyDto.BindFeedingCodes != null && outPutQtyDto.BindFeedingCodes.Any())
            {
                foreach (var item in outPutQtyDto.BindFeedingCodes)
                {
                    bindMaterialList.Add(new ManuOutputBindMaterialEntity
                    {
                        EquipmentId = _currentEquipment.Id ?? 0,
                        ResourceId = resourceId,
                        SFC = outPutQtyDto.SFC,
                        BindCode = item,
                        Type = ManuOutputBindMaterialTypeEnum.Feeding,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentEquipment.SiteId
                    });
                }
            }

            if (outPutQtyDto.BindProductCodes != null && outPutQtyDto.BindProductCodes.Any())
            {
                foreach (var item in outPutQtyDto.BindProductCodes)
                {
                    bindMaterialList.Add(new ManuOutputBindMaterialEntity
                    {
                        EquipmentId = _currentEquipment.Id ?? 0,
                        ResourceId = resourceId,
                        SFC = outPutQtyDto.SFC,
                        BindCode = item,
                        Type = ManuOutputBindMaterialTypeEnum.Product,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentEquipment.SiteId
                    });
                }
            }
            return bindMaterialList;
        }
    }
}
