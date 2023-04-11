/*
 *creator: Karl
 *
 *describe: 操作面板    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:56:57
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板 服务
    /// </summary>
    public class ManuFacePlateService : IManuFacePlateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        #region Repository
        /// <summary>
        /// 操作面板 仓储
        /// </summary>
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        private readonly IManuFacePlateProductionRepository _manuFacePlateProductionRepository;
        private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;
        private readonly IManuFacePlateContainerPackRepository _manuFacePlateContainerPackRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly AbstractValidator<ManuFacePlateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateModifyDto> _validationModifyRules;
        private readonly AbstractValidator<ManuFacePlateProductionCreateDto> _validationProductionCreateRules;
        private readonly AbstractValidator<ManuFacePlateProductionModifyDto> _validationProductionModifyRules;
        private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationRepairCreateRules;
        private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationRepairModifyRules;
        private readonly AbstractValidator<ManuFacePlateContainerPackCreateDto> _validationContainerPackCreateRules;
        private readonly AbstractValidator<ManuFacePlateContainerPackModifyDto> _validationContainerPackModifyRules;

        public ManuFacePlateService(ICurrentUser currentUser, ICurrentSite currentSite, IManuFacePlateRepository manuFacePlateRepository, IManuFacePlateProductionRepository manuFacePlateProductionRepository,
            IProcProcedureRepository procProcedureRepository, IProcResourceRepository procResourceRepository, IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuFacePlateContainerPackRepository manuFacePlateContainerPackRepository,
            AbstractValidator<ManuFacePlateCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateModifyDto> validationModifyRules,
            AbstractValidator<ManuFacePlateProductionCreateDto> validationProductionCreateRules, AbstractValidator<ManuFacePlateProductionModifyDto> validationProductionModifyRules,
            AbstractValidator<ManuFacePlateRepairCreateDto> validationRepairCreateRules, AbstractValidator<ManuFacePlateRepairModifyDto> validationRepairModifyRules,
            AbstractValidator<ManuFacePlateContainerPackCreateDto> validationContainerPackCreateRules, AbstractValidator<ManuFacePlateContainerPackModifyDto> validationContainerPackModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateRepository = manuFacePlateRepository;
            _manuFacePlateProductionRepository = manuFacePlateProductionRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
            _manuFacePlateContainerPackRepository = manuFacePlateContainerPackRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationProductionCreateRules = validationProductionCreateRules;
            _validationProductionModifyRules = validationProductionModifyRules;
            _validationRepairCreateRules = validationRepairCreateRules;
            _validationRepairModifyRules = validationRepairModifyRules;
            _validationContainerPackCreateRules = validationContainerPackCreateRules;
            _validationContainerPackModifyRules = validationContainerPackModifyRules;
        }
        #endregion

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuFacePlateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuFacePlateAsync(ManuFacePlateCreateDto manuFacePlateCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateCreateDto);

            //DTO转换实体
            var manuFacePlateEntity = manuFacePlateCreateDto.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuFacePlateRepository.InsertAsync(manuFacePlateEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuFacePlateAsync(long id)
        {
            await _manuFacePlateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuFacePlateAsync(long[] ids)
        {
            return await _manuFacePlateRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuFacePlatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateDto>> GetPagedListAsync(ManuFacePlatePagedQueryDto manuFacePlatePagedQueryDto)
        {
            var manuFacePlatePagedQuery = manuFacePlatePagedQueryDto.ToQuery<ManuFacePlatePagedQuery>();
            var pagedInfo = await _manuFacePlateRepository.GetPagedInfoAsync(manuFacePlatePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuFacePlateDto> manuFacePlateDtos = PrepareManuFacePlateDtos(pagedInfo);
            return new PagedInfo<ManuFacePlateDto>(manuFacePlateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuFacePlateDto> PrepareManuFacePlateDtos(PagedInfo<ManuFacePlateEntity> pagedInfo)
        {
            var manuFacePlateDtos = new List<ManuFacePlateDto>();
            foreach (var manuFacePlateEntity in pagedInfo.Data)
            {
                var manuFacePlateDto = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                manuFacePlateDtos.Add(manuFacePlateDto);
            }

            return manuFacePlateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateAsync(ManuFacePlateModifyDto manuFacePlateModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuFacePlateModifyDto);

            //DTO转换实体
            var manuFacePlateEntity = manuFacePlateModifyDto.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();

            await _manuFacePlateRepository.UpdateAsync(manuFacePlateEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateQueryDto> QueryManuFacePlateByIdAsync(long id)
        {
            ManuFacePlateQueryDto facePlatQueryDto = new ManuFacePlateQueryDto();
            var manuFacePlateEntity = await _manuFacePlateRepository.GetByIdAsync(id);
            if (manuFacePlateEntity != null)
            {
                long resourceId = 0;
                long procedureId = 0;
                facePlatQueryDto.FacePlate = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                #region 生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    var manuFacePlateProductionEntity = await _manuFacePlateProductionRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateProductionEntity != null)
                    {
                        resourceId = manuFacePlateProductionEntity.ResourceId;
                        procedureId = manuFacePlateProductionEntity.ProcedureId;

                        facePlatQueryDto.FacePlateProduction = manuFacePlateProductionEntity.ToModel<ManuFacePlateProductionDto>();
                        //颜色不为空就显示
                        facePlatQueryDto.FacePlateProduction.IsShowQualifiedColour = !string.IsNullOrEmpty(facePlatQueryDto.FacePlateProduction.QualifiedColour);
                        facePlatQueryDto.FacePlateProduction.IsShowUnqualifiedColour = !string.IsNullOrEmpty(facePlatQueryDto.FacePlateProduction.UnqualifiedColour);
                    }
                }
                #endregion

                #region 在制品维修
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    var manuFacePlateRepairEntity = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateRepairEntity != null)
                    {
                        resourceId = manuFacePlateRepairEntity.ResourceId;
                        procedureId = manuFacePlateRepairEntity.ProcedureId;
                        facePlatQueryDto.FacePlateRepair = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                    }
                }
                #endregion

                #region 容器包装
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    var manuFacePlateContainerPackEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateContainerPackEntity != null)
                    {
                        resourceId = manuFacePlateContainerPackEntity.ResourceId;
                        procedureId = manuFacePlateContainerPackEntity.ProcedureId;
                        facePlatQueryDto.FacePlateContainerPack = manuFacePlateContainerPackEntity.ToModel<ManuFacePlateContainerPackDto>();
                        //颜色不为空就显示
                        facePlatQueryDto.FacePlateContainerPack.IsShowQualifiedColour = !string.IsNullOrEmpty(facePlatQueryDto.FacePlateContainerPack.QualifiedColour);
                        facePlatQueryDto.FacePlateContainerPack.IsShowErrorsColour = !string.IsNullOrEmpty(facePlatQueryDto.FacePlateContainerPack.ErrorsColour);
                    }
                }
                #endregion

                #region 填充关联表数据
                //资源
                if (resourceId > 0)
                {
                    var procResourceEntity = await _procResourceRepository.GetResByIdAsync(resourceId);
                    if (procResourceEntity != null)
                    {   //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlatQueryDto.FacePlateRepair.ResourceName = procResourceEntity.ResName;
                            facePlatQueryDto.FacePlateRepair.ResourceCode = procResourceEntity.ResCode;
                        }
                        //生产过站
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlatQueryDto.FacePlateProduction.ResourceName = procResourceEntity.ResName;
                            facePlatQueryDto.FacePlateProduction.ResourceCode = procResourceEntity.ResCode;
                        }
                        //容器包装
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                        {
                            facePlatQueryDto.FacePlateContainerPack.ResourceName = procResourceEntity.ResName;
                            facePlatQueryDto.FacePlateContainerPack.ResourceCode = procResourceEntity.ResCode;
                        }
                    }
                }
                //工序
                if (procedureId > 0)
                {
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procedureId);
                    if (procProcedureEntity != null)
                    {
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlatQueryDto.FacePlateRepair.ProcedureName = procProcedureEntity.Name;
                            facePlatQueryDto.FacePlateRepair.ProcedureCode = procProcedureEntity.Code;
                        }
                        //生产过站
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlatQueryDto.FacePlateProduction.ProcedureName = procProcedureEntity.Name;
                            facePlatQueryDto.FacePlateProduction.ProcedureCode = procProcedureEntity.Code;
                        }
                        //容器包装
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                        {
                            facePlatQueryDto.FacePlateContainerPack.ProcedureName = procProcedureEntity.Name;
                            facePlatQueryDto.FacePlateContainerPack.ProcedureCode = procProcedureEntity.Code;
                        }
                    }
                }
                #endregion
            }
            return facePlatQueryDto;
        }

        /// <summary>
        /// 根据Code查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateQueryDto> QueryManuFacePlateByCodeAsync(string code)
        {
            ManuFacePlateQueryDto facePlateQueryDto = new ManuFacePlateQueryDto();
            var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(code);
            if (manuFacePlateEntity != null)
            {
                long resourceId = 0;
                long procedureId = 0;
                facePlateQueryDto.FacePlate = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                #region 生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    var manuFacePlateProductionEntity = await _manuFacePlateProductionRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateProductionEntity != null)
                    {
                        resourceId = manuFacePlateProductionEntity.ResourceId;
                        procedureId = manuFacePlateProductionEntity.ProcedureId;

                        facePlateQueryDto.FacePlateProduction = manuFacePlateProductionEntity.ToModel<ManuFacePlateProductionDto>();
                        //查询时Production主键使用主表的Id返回
                        facePlateQueryDto.FacePlateProduction.Id = manuFacePlateEntity.Id;
                        //颜色不为空就显示
                        facePlateQueryDto.FacePlateProduction.IsShowQualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateProduction.QualifiedColour);
                        facePlateQueryDto.FacePlateProduction.IsShowUnqualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateProduction.UnqualifiedColour);
                    }
                }
                #endregion

                #region 在制品维修
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    var manuFacePlateRepairEntity = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateRepairEntity != null)
                    {
                        resourceId = manuFacePlateRepairEntity.ResourceId;
                        procedureId = manuFacePlateRepairEntity.ProcedureId;
                        facePlateQueryDto.FacePlateRepair = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                    }
                }
                #endregion

                #region 填充关联表数据
                //资源
                if (resourceId > 0)
                {
                    var procResourceEntity = await _procResourceRepository.GetResByIdAsync(resourceId);
                    if (procResourceEntity != null)
                    {   //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlateQueryDto.FacePlateRepair.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateRepair.ResourceCode = procResourceEntity.ResCode;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlateQueryDto.FacePlateProduction.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateProduction.ResourceCode = procResourceEntity.ResCode;
                        }
                    }
                }
                //工序
                if (procedureId > 0)
                {
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procedureId);
                    if (procProcedureEntity != null)
                    {
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlateQueryDto.FacePlateRepair.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateRepair.ProcedureCode = procProcedureEntity.Code;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlateQueryDto.FacePlateProduction.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateProduction.ProcedureCode = procProcedureEntity.Code;
                        }
                    }
                }
                #endregion
            }
            return facePlateQueryDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addManuFacePlateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task AddManuFacePlateAsync(AddManuFacePlateDto addManuFacePlateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证FacePlate DTO
            await _validationCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlate);
            //DTO转换实体
            var manuFacePlateEntity = addManuFacePlateDto.FacePlate.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateEntity.SiteId = _currentSite.SiteId ?? 0;
            //生产过站
            ManuFacePlateProductionEntity manuFacePlateProductionEntity = new ManuFacePlateProductionEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
            {
                //验证Production DTO
                await _validationProductionCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateProduction);
                // manuFacePlateProduction表
                manuFacePlateProductionEntity = addManuFacePlateDto.FacePlateProduction.ToEntity<ManuFacePlateProductionEntity>();
                manuFacePlateProductionEntity.FacePlateId = manuFacePlateEntity.Id;
                manuFacePlateProductionEntity.Id = IdGenProvider.Instance.CreateId();
                manuFacePlateProductionEntity.CreatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.CreatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.SiteId = _currentSite.SiteId ?? 0;
            }
            //在制品维修
            ManuFacePlateRepairEntity manuFacePlateRepairEntity = new ManuFacePlateRepairEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
            {
                //验证在制品维修
                await _validationRepairCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateRepair);
                // manuFacePlateRepairEntity表
                manuFacePlateRepairEntity = addManuFacePlateDto.FacePlateRepair.ToEntity<ManuFacePlateRepairEntity>();
                manuFacePlateRepairEntity.FacePlateId = manuFacePlateEntity.Id;
                manuFacePlateRepairEntity.Id = IdGenProvider.Instance.CreateId();
                manuFacePlateRepairEntity.CreatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.CreatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.SiteId = _currentSite.SiteId ?? 0;
            }
            //容器包装
            ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity = new ManuFacePlateContainerPackEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
            {
                //验证容器包装
                await _validationContainerPackCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateContainerPack);
                // manuFacePlateContainerPackEntity表
                manuFacePlateContainerPackEntity = addManuFacePlateDto.FacePlateContainerPack.ToEntity<ManuFacePlateContainerPackEntity>();
                manuFacePlateContainerPackEntity.FacePlateId = manuFacePlateEntity.Id;
                manuFacePlateContainerPackEntity.Id = IdGenProvider.Instance.CreateId();
                manuFacePlateContainerPackEntity.CreatedBy = _currentUser.UserName;
                manuFacePlateContainerPackEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateContainerPackEntity.CreatedOn = HymsonClock.Now();
                manuFacePlateContainerPackEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateContainerPackEntity.SiteId = _currentSite.SiteId ?? 0;
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _manuFacePlateRepository.InsertAsync(manuFacePlateEntity);
                //生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    await _manuFacePlateProductionRepository.InsertAsync(manuFacePlateProductionEntity);
                }
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    await _manuFacePlateRepairRepository.InsertAsync(manuFacePlateRepairEntity);
                }
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    await _manuFacePlateContainerPackRepository.InsertAsync(manuFacePlateContainerPackEntity);
                }
                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateManuFacePlateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task UpdateManuFacePlateAsync(UpdateManuFacePlateDto updateManuFacePlateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlate);

            //DTO转换实体
            var manuFacePlateEntity = updateManuFacePlateDto.FacePlate.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            //生产过站
            ManuFacePlateProductionEntity manuFacePlateProductionEntity = new ManuFacePlateProductionEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
            {
                await _validationProductionModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateProduction);
                manuFacePlateProductionEntity = updateManuFacePlateDto.FacePlateProduction.ToEntity<ManuFacePlateProductionEntity>();
                manuFacePlateProductionEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.FacePlateId = manuFacePlateEntity.Id;
            }
            //在制品维修
            ManuFacePlateRepairEntity manuFacePlateRepairEntity = new ManuFacePlateRepairEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
            {
                await _validationRepairModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateRepair);
                manuFacePlateRepairEntity = updateManuFacePlateDto.FacePlateRepair.ToEntity<ManuFacePlateRepairEntity>();
                manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.FacePlateId = manuFacePlateEntity.Id;
            }
            //容器装箱
            ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity = new ManuFacePlateContainerPackEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
            {
                await _validationContainerPackModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateContainerPack);
                manuFacePlateContainerPackEntity = updateManuFacePlateDto.FacePlateContainerPack.ToEntity<ManuFacePlateContainerPackEntity>();
                manuFacePlateContainerPackEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateContainerPackEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateContainerPackEntity.FacePlateId = manuFacePlateEntity.Id;
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuFacePlateRepository.UpdateAsync(manuFacePlateEntity);
                //生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    await _manuFacePlateProductionRepository.UpdateByFacePlateIdAsync(manuFacePlateProductionEntity);
                }
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    await _manuFacePlateRepairRepository.UpdateByFacePlateRepairIdAsync(manuFacePlateRepairEntity);
                }
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    await _manuFacePlateContainerPackRepository.UpdateByFacePlateIdAsync(manuFacePlateContainerPackEntity);
                }
                //提交
                ts.Complete();
            }
        }


    }
}
