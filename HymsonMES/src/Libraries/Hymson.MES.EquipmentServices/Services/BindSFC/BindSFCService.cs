using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Mysqlx;
using System.Drawing;
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

        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;


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
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository)

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
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }


        /// <summary>
        /// 根据SFC或BindSFC查询绑定SFC
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<BindSFCOutputDto> GetBindSFC(BindSFCInputDto bindSFCDto)
        {
            if (string.IsNullOrEmpty(bindSFCDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }

            var manuSfcCirculationBarCodequery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Merge,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationBarCode = bindSFCDto.SFC,
                SiteId = 123456
            };

            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodequery);

            if (!circulationBarCodeEntities.Any())
            {
                //不需要解绑
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }

            var circulateFirst = circulationBarCodeEntities.FirstOrDefault();

            //汇总表找出NG状态
            //查询已有汇总信息
            //var sfcQuery = new List<string>();
            //sfcQuery.AddRange(circulationBarCodeEntities.Select(x => x.SFC));
            //sfcQuery.Add(bindSFCDto.SFC);

            ManuSfcSummaryQuery manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                //SiteId = _currentEquipment.SiteId,
                SiteId = 123456,
                SFCS = new string[] { bindSFCDto.SFC },
                ProcedureIds = new long[] { circulateFirst.ProcedureId }, //当前工序 
                WorkOrderId = circulateFirst.WorkOrderId, //当前工单
                QualityStatus = (int)TrueOrFalseEnum.No, //NG的
            };

            //获取汇总表信息
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);

            var NG = RepairOutTypeEnum.OK;
            List<ManuSfcCirculationSummaryEntity> circulateSummary = new();

            if (manuSfcSummaryEntities.Any(x => x.SFC == bindSFCDto.SFC))
            {
                //OK确认处理
                if (bindSFCDto.OperateType == RepairOperateTypeEnum.OK)
                {
                    //汇总表处理
                    foreach (var item in manuSfcSummaryEntities)
                    {
                        item.QualityStatus = 1;
                    }
                    //步骤NG表处理

                    //更新汇总表
                    await _manuSfcSummaryRepository.UpdatesAsync(manuSfcSummaryEntities.ToList());

                }
                //查询逻辑
                else
                {
                    NG = RepairOutTypeEnum.NG;

                    foreach (var item in circulationBarCodeEntities)
                    {
                        circulateSummary.Add(new ManuSfcCirculationSummaryEntity
                        {
                            manuSfcCirculationEntity = item,
                            NGState = manuSfcSummaryEntities.Any(x => x.SFC == item.SFC) ? RepairOutTypeEnum.NG : RepairOutTypeEnum.OK,//单个NG状态
                        });
                    }
                }
            }
            else
            {
                foreach (var item in circulationBarCodeEntities)
                {
                    circulateSummary.Add(new ManuSfcCirculationSummaryEntity
                    {
                        manuSfcCirculationEntity = item,
                        NGState = RepairOutTypeEnum.OK,//单个NG状态
                    });
                }
            }

            var result = new BindSFCOutputDto
            {
                Data = circulateSummary,
                NGState = NG, //总的NG状态
            };

            return result;
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
                    SiteId = 123456,
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
                    SiteId = 123456,
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
                    SiteId = 123456,
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

            //设置测试Site信息
            Dictionary<string, object> siteInfo = new()
            {
                { "SiteId", 123456 },
                {"SiteName","单元测试站点"}
            };


            await _sfcCirculationService.PDASfcCirculationUnBindAsync(request, SfcCirculationTypeEnum.Merge);

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

            //校验是否加工中
            var SFCs = new string[] { BindSFCDto.SFC };
            //条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            {
                SiteId = 123456,
                Sfcs = SFCs
            });

            if (!sfcProduceEntities.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES15304)).WithData("sfcs", SFCs);

            //加工中,未出站
            if (sfcProduceEntities.Any(x => x.Status == SfcProduceStatusEnum.Activity))
                throw new CustomerValidationException(nameof(ErrorCode.MES19152)).WithData("SFC", SFCs);


            // 流转信息（多条码）
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery
            {
                SiteId = 123456,
                Sfc = new string[] { BindSFCDto.OldBindSFC, BindSFCDto.NewBindSFC },
                CirculationTypes = new SfcCirculationTypeEnum[] { SfcCirculationTypeEnum.Merge },
                IsDisassemble = TrueOrFalseEnum.No
            });

            //新条码有绑定记录
            if (sfcCirculationEntities.Any(x => x.SFC == BindSFCDto.NewBindSFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19138)).WithData("SFCS", BindSFCDto.NewBindSFC);
            }


            //var manuSfcCirculationBarCodequery = new ManuSfcCirculationBarCodeQuery
            //{
            //    CirculationType = SfcCirculationTypeEnum.Merge,
            //    IsDisassemble = TrueOrFalseEnum.No,
            //    CirculationBarCode = BindSFCDto.SFC,
            //    SiteId = 123456,
            //    Sfcs = new string[] { BindSFCDto.OldBindSFC, BindSFCDto.NewBindSFC }
            //};

            ////查询流转条码绑定记录
            //var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodequery)
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES19150)).WithData("module", BindSFCDto.SFC).WithData("sfc", BindSFCDto.OldBindSFC);


            var update = sfcCirculationEntities.FirstOrDefault(x => x.SFC == BindSFCDto.OldBindSFC && x.CirculationBarCode == BindSFCDto.SFC)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19153)).WithData("SFC", BindSFCDto.OldBindSFC); ;
            int row = 0;

            //if (update != null)
            //{
            //换绑
            update.SFC = BindSFCDto.NewBindSFC;
            update.UpdatedBy = _currentUser.UserName;
            update.UpdatedOn = HymsonClock.Now();


            //换绑记录
            var sfcBindRecord = new ManuSfcBindRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = 123456,
                SFC = BindSFCDto.SFC,
                BindSFC = BindSFCDto.OldBindSFC,
                Type = 1,//预留字段
                Location = 0,//预留
                OperationType = ManuSfcBindStatusEnum.Bind,
                CreatedBy = "PDA",
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = "PDA",
                UpdatedOn = HymsonClock.Now(),
            };


            row += await _manuSfcCirculationRepository.UpdateAsync(update);
            await _manuSfcBindRecordRepository.InsertAsync(sfcBindRecord);
            //}
            if (row == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19151));
            }
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

            var sfcEntity = await _sfcCirculationService.GetSfcCirculationUnBindAsync(unbound, SfcCirculationTypeEnum.Merge);


            if (!sfcEntity.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }

            //实际这里只处理模组在制即可
            //var SFCs = sfcEntity.Select(x => x.SFC).ToArray();
            // 条码在制表
            //var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
            //{
            //    SiteId = 123456,
            //    Sfcs = SFCs
            //});

            var sfcProduceEntities = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                SiteId = 123456,
                Sfc = resumptionInputDto.SFC
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES15304)).WithData("sfcs", resumptionInputDto.SFC);


            //复投位的资源
            var resources = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(resumptionInputDto.RepeatLocationId);


            //更新汇总表
            var summaryQuery = new ManuSfcSummaryQuery()
            {
                SFCS = new string[] { resumptionInputDto.SFC },
                QualityStatus = 0,//0 不合格，代表NG状态
                SiteId = _currentEquipment.SiteId,
            };

            //查询模组的NG记录
            var manuSfcSummaries = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(summaryQuery);
            IEnumerable<ManuSfcSummaryEntity> InsertsRecord = manuSfcSummaries;
            if (manuSfcSummaries.Any())
            {
                foreach (var item in manuSfcSummaries) { item.QualityStatus = 1; }
            }


            sfcProduceEntities.UpdatedBy = "PDA";
            sfcProduceEntities.UpdatedOn = HymsonClock.Now();
            sfcProduceEntities.Status = SfcProduceStatusEnum.lineUp;
            sfcProduceEntities.ProcedureId = resumptionInputDto.RepeatLocationId;
            sfcProduceEntities.ResourceId = resources.FirstOrDefault()?.Id;
            //更新在制
            //foreach (var item in sfcProduceEntities)
            //{
            //    item.UpdatedBy = "PDA";
            //    item.UpdatedOn = HymsonClock.Now();
            //    item.Status = SfcProduceStatusEnum.lineUp;
            //    item.ProcedureId = resumptionInputDto.RepeatLocationId;
            //    item.ResourceId = resources.FirstOrDefault()?.Id;
            //}

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
 

                //解绑
                if (sfcEntity.Any())
                {
                    foreach (var entity in sfcEntity)
                    {
                        entity.IsDisassemble = TrueOrFalseEnum.Yes;
                        entity.DisassembledBy = "PDA";
                        entity.DisassembledOn = HymsonClock.Now();
                        entity.UpdatedBy = "PDA";
                        entity.UpdatedOn = HymsonClock.Now();
                        //manuSfcCirculationEntities.Add(entity);
                    }
                    await _manuSfcCirculationRepository.UpdateRangeAsync(sfcEntity);
                }

                //ng记录
                await _manuSfcSummaryRepository.InsertsRecordAsync(InsertsRecord.ToList());

                //更新汇总
                await _manuSfcSummaryRepository.UpdatesAsync(manuSfcSummaries.ToList());

                //在制更新
                await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntities);


                //报废 暂时不废


                ts.Complete();
            }
        }

    }
}
