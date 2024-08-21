using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.OrderProcedureNum
{
    /// <summary>
    /// 工单工序数量
    /// </summary>
    public class OrderProcedureNumService : IOrderProcedureNumService
    {
        /// <summary>
        /// 步骤表
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderProcedureNumService(IManuSfcStepRepository manuSfcStepRepository,
            ICurrentSite currentSite,
            IProcMaterialRepository procMaterialRepository,
            ISysConfigRepository sysConfigRepository)
        {
            _manuSfcStepRepository = manuSfcStepRepository;
            _currentSite = currentSite;
            _procMaterialRepository = procMaterialRepository;
            _sysConfigRepository = sysConfigRepository;
        }

        /// <summary>
        /// 获取工单工序数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<OrderProcedureNumResultDto>> GetOrderProcedureNumListAsync(OrderProcedureNumDto param)
        {
            //OrderProcedureNumAllResultDto result = new OrderProcedureNumAllResultDto();

            decimal workHour = 8;
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.WorkHour });
            if(configEntities != null && configEntities.Count() > 0)
            {
                workHour = decimal.Parse(configEntities.ElementAt(0).Value);
            }

            List<OrderProcedureNumResultDto> resultList = new List<OrderProcedureNumResultDto>();

            SfcStepOrderProcedureQuery query = new SfcStepOrderProcedureQuery();
            query.SiteId = _currentSite.SiteId ?? 0;
            query.WorkPlanCode = param.WorkPlanCode;
            query.OrderCode = param.OrderCode;
            query.BeginDate = param.CreatedOn[0];
            query.EndDate = param.CreatedOn[1];
            var dbResult = await _manuSfcStepRepository.GetSfcStepOrderOpMavelAsync(query);

            if(dbResult == null)
            {
                return new PagedInfo<OrderProcedureNumResultDto>(resultList, 1, 1, 0);
            }
            List<long> matIdList = dbResult.Select(m => m.ProductId).Distinct().ToList();
            var dbMatList = await _procMaterialRepository.GetByIdsAsync(matIdList);

            foreach(var item in dbResult)
            {
                OrderProcedureNumResultDto resultItem = new OrderProcedureNumResultDto();
                resultItem.WorkPlanCode = item.WorkPlanCode;
                resultItem.DateStr = (Convert.ToDateTime(item.DateStr)).ToString("yyyy-MM-dd");
                resultItem.OrderCode = item.OrderCode;
                resultItem.ProcedureCode = item.Code;
                resultItem.ProcedureName = item.Name;
                resultItem.ProductId = item.ProductId;
                resultItem.Num = item.Num;
                resultItem.WorkHour = workHour;

                if (dbMatList != null && dbMatList.Count() > 0)
                {
                    var curMat = dbMatList.Where(m => m.Id == item.ProductId).FirstOrDefault();
                    if (curMat != null)
                    {
                        resultItem.MaterialCode = curMat.MaterialCode;
                        resultItem.MaterialName = curMat.MaterialName;
                    }
                }
                resultList.Add(resultItem);
            }

            List<string> opList = resultList.Select(m => m.ProcedureCode).Distinct().OrderBy(m => m).ToList();

            foreach(var item in opList)
            {
                var curOpList = resultList.Where(m => m.ProcedureCode == item).ToList();
                if (curOpList == null || curOpList.Count() == 0)
                {
                    continue;
                }
                var curOp = curOpList.First();
                var dataNum = curOpList.Select(m => m.DateStr).Distinct().Count();

                OrderProcedureNumResultDto sumItem = new OrderProcedureNumResultDto();
                sumItem.WorkPlanCode = curOp.WorkPlanCode;
                sumItem.WorkHour = curOp.WorkHour;
                sumItem.DateStr = $"汇总*{dataNum}";
                sumItem.OrderCode = "汇总";
                sumItem.ProcedureCode = curOp.ProcedureCode;
                sumItem.ProcedureName = curOp.ProcedureName;
                sumItem.ProductId = curOp.ProductId;
                sumItem.Num = curOpList.Select(m => m.Num).Sum();
                sumItem.WorkHour = workHour * dataNum;
                sumItem.MaterialCode = curOp.MaterialCode;
                sumItem.MaterialName = curOp.MaterialName;
                resultList.Add(sumItem);
            }

            return new PagedInfo<OrderProcedureNumResultDto>(resultList, 1, 1, resultList.Count);

            //List<OrderProcedureSumResultDto> sumList = new List<OrderProcedureSumResultDto>();
            ////根据工单工序汇总
            //List<string> orderCodeList = resultList.Select(m => m.OrderCode).Distinct().ToList();
            //List<string> operCodeList = resultList.Select(m => m.ProcedureCode).Distinct().ToList();
            //foreach (var operItem in operCodeList)
            //{
            //    var curList = resultList.Where(m => m.ProcedureCode == operItem).ToList();
            //    if(curList == null || curList.Count == 0)
            //    {
            //        continue;
            //    }
            //    int dayNum = curList.Select(m => m.DateStr).Distinct().Count();

            //    OrderProcedureSumResultDto sumModel = new OrderProcedureSumResultDto();
            //    sumModel.WorkPlanCode = curList[0].WorkPlanCode;
            //    sumModel.OrderCode = curList[0].OrderCode;
            //    sumModel.ProcedureCode = curList[0].ProcedureCode;
            //    sumModel.ProcedureName = curList[0].ProcedureName;
            //    sumModel.Num = curList.Select(m => m.Num).Sum();
            //    sumModel.WorkHour = dayNum * workHour;
            //    sumModel.MaterialCode = curList[0].MaterialCode;
            //    sumModel.MaterialName = curList[0].MaterialName;
            //    sumList.Add(sumModel);
            //}

            //result.DetailList = resultList;
            //result.SumList = sumList;
            //return result;
        }
    }
}
