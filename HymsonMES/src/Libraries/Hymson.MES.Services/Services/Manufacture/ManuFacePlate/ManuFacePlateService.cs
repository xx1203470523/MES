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
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;
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

        /// <summary>
        /// 操作面板 仓储
        /// </summary>
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        private readonly IManuFacePlateProductionRepository _manuFacePlateProductionRepository;
        private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly AbstractValidator<ManuFacePlateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateModifyDto> _validationModifyRules;
        private readonly AbstractValidator<ManuFacePlateProductionCreateDto> _validationProductionCreateRules;
        private readonly AbstractValidator<ManuFacePlateProductionModifyDto> _validationProductionModifyRules;

        public ManuFacePlateService(ICurrentUser currentUser, ICurrentSite currentSite, IManuFacePlateRepository manuFacePlateRepository, IManuFacePlateProductionRepository manuFacePlateProductionRepository,
            IProcProcedureRepository procProcedureRepository, IProcResourceRepository procResourceRepository, IManuFacePlateRepairRepository manuFacePlateRepairRepository,
        AbstractValidator<ManuFacePlateCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateModifyDto> validationModifyRules,
            AbstractValidator<ManuFacePlateProductionCreateDto> validationProductionCreateRules, AbstractValidator<ManuFacePlateProductionModifyDto> validationProductionModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateRepository = manuFacePlateRepository;
            _manuFacePlateProductionRepository = manuFacePlateProductionRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationProductionCreateRules = validationProductionCreateRules;
            _validationProductionModifyRules = validationProductionModifyRules;
        }

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
        public async Task<QueryManuFacePlateDto> QueryManuFacePlateByIdAsync(long id)
        {
            QueryManuFacePlateDto queryProcDto = new QueryManuFacePlateDto();
            var manuFacePlateEntity = await _manuFacePlateRepository.GetByIdAsync(id);
            if (manuFacePlateEntity != null)
            {
                long resourceId = 0;
                long procedureId = 0;
                queryProcDto.FacePlate = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                #region 生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    var manuFacePlateProductionEntity = await _manuFacePlateProductionRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateProductionEntity != null)
                    {
                        resourceId = manuFacePlateProductionEntity.ResourceId;
                        procedureId = manuFacePlateProductionEntity.ProcedureId;

                        queryProcDto.FacePlateProduction = manuFacePlateProductionEntity.ToModel<ManuFacePlateProductionDto>();
                        //查询时Production主键使用主表的Id返回
                        queryProcDto.FacePlateProduction.Id = manuFacePlateEntity.Id;
                        //颜色不为空就显示
                        queryProcDto.FacePlateProduction.IsShowQualifiedColour = !string.IsNullOrEmpty(queryProcDto.FacePlateProduction.QualifiedColour);
                        queryProcDto.FacePlateProduction.IsShowUnqualifiedColour = !string.IsNullOrEmpty(queryProcDto.FacePlateProduction.UnqualifiedColour);
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

                        queryProcDto.FacePlateRepair = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                        //查询时Repair主键使用主表的Id返回
                        queryProcDto.FacePlateRepair.Id = manuFacePlateEntity.Id;
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
                            queryProcDto.FacePlateRepair.ResourceName = procResourceEntity.ResName;
                            queryProcDto.FacePlateRepair.ResourceCode = procResourceEntity.ResCode;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            queryProcDto.FacePlateProduction.ResourceName = procResourceEntity.ResName;
                            queryProcDto.FacePlateProduction.ResourceCode = procResourceEntity.ResCode;
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
                            queryProcDto.FacePlateRepair.ProcedureName = procProcedureEntity.Name;
                            queryProcDto.FacePlateRepair.ProcedureCode = procProcedureEntity.Code;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            queryProcDto.FacePlateProduction.ProcedureName = procProcedureEntity.Name;
                            queryProcDto.FacePlateProduction.ProcedureCode = procProcedureEntity.Code;
                        }
                    }
                }
                #endregion
            }
            return queryProcDto;
        }

        /// <summary>
        /// 根据Code查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<QueryManuFacePlateDto> QueryManuFacePlateByCodeAsync(string code)
        {
            QueryManuFacePlateDto queryProcDto = new QueryManuFacePlateDto();
            var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(code);
            if (manuFacePlateEntity != null)
            {
                long resourceId = 0;
                long procedureId = 0;
                queryProcDto.FacePlate = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                #region 生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    var manuFacePlateProductionEntity = await _manuFacePlateProductionRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateProductionEntity != null)
                    {
                        resourceId = manuFacePlateProductionEntity.ResourceId;
                        procedureId = manuFacePlateProductionEntity.ProcedureId;

                        queryProcDto.FacePlateProduction = manuFacePlateProductionEntity.ToModel<ManuFacePlateProductionDto>();
                        //查询时Production主键使用主表的Id返回
                        queryProcDto.FacePlateProduction.Id = manuFacePlateEntity.Id;
                        //颜色不为空就显示
                        queryProcDto.FacePlateProduction.IsShowQualifiedColour = !string.IsNullOrEmpty(queryProcDto.FacePlateProduction.QualifiedColour);
                        queryProcDto.FacePlateProduction.IsShowUnqualifiedColour = !string.IsNullOrEmpty(queryProcDto.FacePlateProduction.UnqualifiedColour);
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

                        queryProcDto.FacePlateRepair = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                        //查询时Repair主键使用主表的Id返回
                        queryProcDto.FacePlateRepair.Id = manuFacePlateEntity.Id;
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
                            queryProcDto.FacePlateRepair.ResourceName = procResourceEntity.ResName;
                            queryProcDto.FacePlateRepair.ResourceCode = procResourceEntity.ResCode;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            queryProcDto.FacePlateProduction.ResourceName = procResourceEntity.ResName;
                            queryProcDto.FacePlateProduction.ResourceCode = procResourceEntity.ResCode;
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
                            queryProcDto.FacePlateRepair.ProcedureName = procProcedureEntity.Name;
                            queryProcDto.FacePlateRepair.ProcedureCode = procProcedureEntity.Code;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            queryProcDto.FacePlateProduction.ProcedureName = procProcedureEntity.Name;
                            queryProcDto.FacePlateProduction.ProcedureCode = procProcedureEntity.Code;
                        }
                    }
                }
                #endregion
            }
            return queryProcDto;
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
            //验证Production DTO
            await _validationProductionCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateProduction);

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
            await _validationProductionModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateProduction);

            //DTO转换实体
            var manuFacePlateEntity = updateManuFacePlateDto.FacePlate.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            //生产过站
            ManuFacePlateProductionEntity manuFacePlateProductionEntity = new ManuFacePlateProductionEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
            {
                manuFacePlateProductionEntity = updateManuFacePlateDto.FacePlateProduction.ToEntity<ManuFacePlateProductionEntity>();
                manuFacePlateProductionEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.FacePlateId = manuFacePlateEntity.Id;
            }
            //在制品维修
            ManuFacePlateRepairEntity manuFacePlateRepairEntity = new ManuFacePlateRepairEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
            {
                manuFacePlateRepairEntity = updateManuFacePlateDto.FacePlateRepair.ToEntity<ManuFacePlateRepairEntity>();
                manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.FacePlateId = manuFacePlateEntity.Id;
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
                //提交
                ts.Complete();
            }
        }


    }
}
