using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.BindContainer;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Transactions;

namespace Hymson.MES.EquipmentServices.Services.BindSFC
{
    /// <summary>
    /// 条码绑定服务
    /// </summary>
    public class BindSFCService : IBindSFCService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<BindSFCInputDto> _validationBindDtoRules;
        private readonly AbstractValidator<UnBindSFCInputDto> _validationUnBindDtoRules;
        private readonly IManuSfcBindRecordRepository _manuSfcBindRecordRepository;
        private readonly IManuSfcBindRepository _manuSfcBindRepository;
        /// <summary>
        /// 流转表
        /// </summary>
        private readonly ISfcCirculationService _sfcCirculationService;

        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationBindDtoRules"></param>
        /// <param name="currentEquipment"></param>
        /// <param name="validationUnBindDtoRules"></param>
        /// <param name="manuSfcBindRecordRepository"></param>
        /// <param name="manuSfcBindRepository"></param>
        /// <param name="currentUser"></param>
        public BindSFCService(
            ICurrentUser currentUser,
            ICurrentEquipment currentEquipment,
            IManuSfcBindRecordRepository manuSfcBindRecordRepository,
            IManuSfcBindRepository manuSfcBindRepository,
            AbstractValidator<BindSFCInputDto> validationBindDtoRules,
            AbstractValidator<UnBindSFCInputDto> validationUnBindDtoRules,
            ISfcCirculationService sfcCirculationService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcResourceRepository procResourceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository)

        {
            _validationBindDtoRules = validationBindDtoRules;
            _currentEquipment = currentEquipment;
            _validationUnBindDtoRules = validationUnBindDtoRules;
            _manuSfcBindRecordRepository = manuSfcBindRecordRepository;
            _manuSfcBindRepository = manuSfcBindRepository;
            _currentUser = currentUser;
            _sfcCirculationService = sfcCirculationService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procResourceRepository = procResourceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }


        /// <summary>
        /// 根据SFC或BindSFC查询绑定SFC
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetBindSFC(BindSFCInputDto bindSFCDto)
        {

            var manuSfcCirculationBarCodequery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Merge,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationBarCode = bindSFCDto.SFC,
                SiteId = _currentEquipment.SiteId
            };

            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodequery);

            if (!circulationBarCodeEntities.Any())
            {
                //不需要解绑
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }

            return circulationBarCodeEntities;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        public async Task BindSFCAsync(UnBindSFCInputDto bindSFCDto)
        {
            //验证参数
            await _validationBindDtoRules.ValidateAndThrowAsync(bindSFCDto);

            List<ManuSfcBindEntity> sfcBindList = new();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();

            var existsBindSfc = await _manuSfcBindRepository.GetByBindSFCAsync(bindSFCDto.SFC, bindSFCDto.BindSFCs);
            if (existsBindSfc.Any())
            {
                var bindSfcs = string.Join(",", existsBindSfc.Select(c => c.BindSFC));
                throw new CustomerValidationException(nameof(ErrorCode.MES19121)).WithData("SFC", bindSFCDto.SFC).WithData("BindSFC", bindSfcs);
            }

            foreach (var item in bindSFCDto.BindSFCs)
            {
                sfcBindList.Add(new ManuSfcBindEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bindSFCDto.SFC,
                    BindSFC = item,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    Status = ManuSfcBindStatusEnum.Bind,
                    BindingTime = HymsonClock.Now(),
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
                sfcBindRecordList.Add(new ManuSfcBindRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bindSFCDto.SFC,
                    BindSFC = item,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    OperationType = ManuSfcBindStatusEnum.Bind,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuSfcBindRepository.InsertsAsync(sfcBindList);
                //绑定记录备用
                //await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);
                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindSFCDto"></param>
        /// <returns></returns>
        public async Task UnBindSFCAsync(UnBindSFCInputDto unBindSFCDto)
        {
            //验证参数
            //await _validationUnBindDtoRules.ValidateAndThrowAsync(unBindSFCDto);
            var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(unBindSFCDto.SFC);
            if (!bindSfcs.Any())
            {
                //不需要解绑
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }

            //需要解绑的SFC
            var unBindSFCs = bindSfcs.Where(c => unBindSFCDto.BindSFCs.Where(p => p.ToUpper().Equals(c.BindSFC.ToUpper())).Any());
            List<long> idsList = new List<long>();
            List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            foreach (var item in unBindSFCs)
            {
                //更新解绑信息
                item.UnbindingTime = HymsonClock.Now();
                item.Status = ManuSfcBindStatusEnum.UnBind;
                item.UpdatedBy = _currentEquipment.Name;
                item.UpdatedOn = HymsonClock.Now();

                //解绑记录
                sfcBindRecordList.Add(new ManuSfcBindRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = unBindSFCDto.SFC,
                    BindSFC = item.BindSFC,
                    Type = 0,//预留字段
                    Location = 0,//预留
                    OperationType = ManuSfcBindStatusEnum.UnBind,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
                idsList.Add(item.Id);
            }
            //删除
            var command = new DeleteCommand
            {
                UserId = _currentEquipment.Name,
                DeleteOn = HymsonClock.Now(),
                Ids = idsList.ToArray()
            };
            await _manuSfcBindRepository.UpdatesAsync(unBindSFCs.ToList());
            //using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            //{
            //    await _manuSfcBindRepository.UpdatesAsync(unBindSFCs.ToList());
            //    //await _manuSfcBindRepository.DeleteTruesAsync(command);
            //    //绑定记录备用 如果解绑需要删除绑定数据使用
            await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);
            //    //提交
            //    ts.Complete();
            //}


        }


        /// <summary>
        /// PDA全部解绑
        /// </summary>
        /// <param name="unBindSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task UnBindPDAAsync(UnBindSFCInput unBindSFCDto)
        {
            if (string.IsNullOrEmpty(unBindSFCDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
            var request = new SfcCirculationUnBindDto
            {
                SFC = unBindSFCDto.SFC,
                IsVirtualSFC = false,
            };
            await _sfcCirculationService.SfcCirculationUnBindAsync(request, SfcCirculationTypeEnum.Merge);

            //var bindSfcs = await _manuSfcBindRepository.GetBySFCAsync(unBindSFCDto.SFC);
            //if (!bindSfcs.Any())
            //{
            //    //不需要解绑
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            //}


            ////需要解绑的SFC
            //var unBindSFCs = bindSfcs.Where(c => unBindSFCDto.BindSFCs.Where(p => p.ToUpper().Equals(c.BindSFC.ToUpper())).Any());
            //List<long> idsList = new List<long>();
            //List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            //foreach (var item in unBindSFCs)
            //{
            //    //更新解绑信息
            //    item.UnbindingTime = HymsonClock.Now();
            //    item.Status = ManuSfcBindStatusEnum.UnBind;
            //    item.UpdatedBy = _currentEquipment.Name;
            //    item.UpdatedOn = HymsonClock.Now();

            //    //解绑记录
            //    sfcBindRecordList.Add(new ManuSfcBindRecordEntity
            //    {
            //        Id = IdGenProvider.Instance.CreateId(),
            //        SiteId = _currentEquipment.SiteId,
            //        SFC = unBindSFCDto.SFC,
            //        BindSFC = item.BindSFC,
            //        Type = 0,//预留字段
            //        Location = 0,//预留
            //        OperationType = ManuSfcBindStatusEnum.UnBind,
            //        CreatedBy = _currentEquipment.Name,
            //        CreatedOn = HymsonClock.Now(),
            //        UpdatedBy = _currentEquipment.Name,
            //        UpdatedOn = HymsonClock.Now()
            //    });
            //    idsList.Add(item.Id);
            //}
            ////删除
            //var command = new DeleteCommand
            //{
            //    UserId = _currentEquipment.Name,
            //    DeleteOn = HymsonClock.Now(),
            //    Ids = idsList.ToArray()
            //};

            //using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            //{
            //    await _manuSfcBindRepository.UpdatesAsync(unBindSFCs.ToList());

            //    await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);

            //    ts.Complete();
            //}

        }


        /// <summary>
        /// 换绑
        /// </summary>
        /// <param name="BindSFCDto"></param>
        /// <returns></returns>
        public async Task SwitchBindSFCAsync(SwitchBindInputDto BindSFCDto)
        {
            //如果有传递解绑条码列表,否则解绑该SFC绑定的所有条码记录
            if (string.IsNullOrWhiteSpace(BindSFCDto.OldBindSFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19117)).WithData("SFC", "被换绑定的条码");
            }

            var manuSfcCirculationBarCodequery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Merge,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationBarCode = BindSFCDto.SFC,
                SiteId = _currentEquipment.SiteId,
                Sfcs = new string[] { BindSFCDto.OldBindSFC }
            };

            //string[] sfc = { BindSFCDto.OldBindSFC };
            //manuSfcCirculationBarCodequery.Sfcs = sfc;


            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodequery)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19150)).WithData("module", BindSFCDto.SFC).WithData("sfc", BindSFCDto.OldBindSFC);

            //if (!circulationBarCodeEntities.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            //}

            var update = circulationBarCodeEntities.FirstOrDefault();
            if (update != null)
            {
                update.SFC = BindSFCDto.NewBindSFC;
                await _manuSfcCirculationRepository.UpdateAsync(update);
            }

            //string[] BindSFCs = { BindSFCDto.OldBindSFC };
            ////验证参数
            ////await _validationUnBindDtoRules.ValidateAndThrowAsync(BindSFCDto);
            //var existsBindSfc = await _manuSfcBindRepository.GetByBindSFCAsync(BindSFCDto.SFC, BindSFCs);
            //if (!existsBindSfc.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19120)).WithData("SFC", BindSFCDto.SFC);
            //}

            //if (existsBindSfc.Any(x => x.BindSFC == BindSFCDto.OldBindSFC))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19146)).WithData("SFC", BindSFCDto.SFC);
            //}

            //var updateEntity = existsBindSfc.FirstOrDefault();

            //updateEntity.BindSFC = BindSFCDto.NewBindSFC;
            //updateEntity.UpdatedBy = _currentUser.UserName;
            //updateEntity.UpdatedOn = HymsonClock.Now();

            //List<ManuSfcBindRecordEntity> sfcBindRecordList = new();
            ////换绑记录
            //sfcBindRecordList.Add(new ManuSfcBindRecordEntity
            //{
            //    Id = IdGenProvider.Instance.CreateId(),
            //    SiteId = _currentEquipment.SiteId,
            //    SFC = BindSFCDto.SFC,
            //    BindSFC = BindSFCDto.NewBindSFC,
            //    Type = 2,//预留字段
            //    Location = 0,//预留
            //    OperationType = ManuSfcBindStatusEnum.Bind,
            //    CreatedBy = _currentEquipment.Name,
            //    CreatedOn = HymsonClock.Now(),
            //    UpdatedBy = _currentEquipment.Name,
            //    UpdatedOn = HymsonClock.Now(),
            //});

            //await _manuSfcBindRepository.UpdateAsync(updateEntity);
            //await _manuSfcBindRecordRepository.InsertsAsync(sfcBindRecordList);

        }

        /// <summary>
        /// 复投
        /// </summary>
        /// <param name="resumptionInputDto"></param>
        /// <returns></returns>
        public async Task RepeatManuSFCAsync(ResumptionInputDto resumptionInputDto)
        {
            //解绑
            var unbound = new SfcCirculationUnBindDto()
            {
                SFC = resumptionInputDto.SFC,
                IsVirtualSFC = false,
            };

            var sfcEntity = await _sfcCirculationService.GetSfcCirculationUnBindAsync(unbound, SfcCirculationTypeEnum.Merge)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19106));


            var SFCs = sfcEntity.Select(x => x.SFC).ToArray();
            // 条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = _currentEquipment.SiteId,
                Sfcs = SFCs
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15304)).WithData("sfcs", SFCs);


            var resources = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(resumptionInputDto.RepeatLocationId);

            //更新manuSfcProduce

            foreach (var item in sfcProduceEntities)
            {
                item.UpdatedBy = _currentEquipment.Name;
                item.UpdatedOn = HymsonClock.Now();

                item.Status = SfcProduceStatusEnum.lineUp;
                item.ProcedureId = resumptionInputDto.RepeatLocationId;
                item.ResourceId = resources.FirstOrDefault()?.Id;
            }
            //在制更新
            await _manuSfcProduceRepository.UpdateRangeAsync(sfcProduceEntities);


            //报废
        }

    }
}
