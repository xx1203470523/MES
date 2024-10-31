using Azure;
using Hymson.MES.BackgroundServices.Rotor.Repositories;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 检查数据
    /// </summary>
    public class CheckDataService : ICheckDataService
    {
        /// <summary>
        /// 装箱仓储
        /// </summary>
        private IPackListRepository _packListRepository;

        /// <summary>
        /// 系统异常消息记录
        /// </summary>
        private readonly ISysAbnormalMessageRecordRepository _sysAbnormalMessageRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDataService(IPackListRepository packListRepository,
            ISysAbnormalMessageRecordRepository sysAbnormalMessageRecordRepository)
        {
            _packListRepository = packListRepository;
            _sysAbnormalMessageRecordRepository = sysAbnormalMessageRecordRepository;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public async Task Check(int rows)
        {
            //查询是否已经有
            const string SOURCE = "STATOR_CHECK";
            var recordModel = await _sysAbnormalMessageRecordRepository.GetNewBySourceAsync(SOURCE);

            string waterSfc = "E3001117AEV26071002001HA1600118";
            if(recordModel != null)
            {
                waterSfc = recordModel.Remark5;
            }

            //没有使用固定值 E3001117AEV26071002001HA1600118
            //有了则使用最新得

            //查询条码
            var barCodeList = await _packListRepository.GetStatorListAsync(waterSfc);
            if(barCodeList == null || barCodeList.Count == 0)
            {
                return;
            }

            DateTime now = DateTime.Now;
            string OperationBy = "Check";

            List<SysAbnormalMessageRecordEntity> insertList = new List<SysAbnormalMessageRecordEntity>();
            int index = 0;
            //判断是否有空的busbar
            foreach(var item in barCodeList)
            {
                ++index;
                if(string.IsNullOrEmpty(item.BusBarCode) == true)
                {
                    var op340Model = await _packListRepository.GetStatorOp340Async(item.InnerId.ToString());
                    if(op340Model == null)
                    {
                        SysAbnormalMessageRecordEntity sysLog = new SysAbnormalMessageRecordEntity();
                        sysLog.Id = IdGenProvider.Instance.CreateId();
                        sysLog.CreatedOn = DateTime.Now;
                        sysLog.UpdatedOn = sysLog.CreatedOn;
                        sysLog.CreatedBy = OperationBy;
                        sysLog.UpdatedBy = OperationBy;
                        sysLog.SiteId = 47024007283048448;
                        sysLog.Source = SOURCE;
                        sysLog.MessageType = "定子异常数据记录";
                        sysLog.Title = "定子线BusBar在追溯中没有";
                        sysLog.MessageStatus = "新建";
                        sysLog.Remark1 = item.ProductionCode;
                        sysLog.Remark2 = item.InnerId.ToString();
                        sysLog.Context = $"manu_stator_barcode表中成品码为{item.ProductionCode}的条码没有BusBarCode,innerId为{item.InnerId}，定子线op340的表中没有id为{item.InnerId}的记录";
                        insertList.Add(sysLog);
                    }
                    //index = 0; //存在元素，remark5则不用记录
                }
                if(index == barCodeList.Count)
                {
                    waterSfc = item.ProductionCode;
                }
            }

            insertList[insertList.Count - 1].Remark5 = waterSfc;
            await _sysAbnormalMessageRecordRepository.InsertRangeAsync(insertList);
            if(insertList == null)
            {
                await _sysAbnormalMessageRecordRepository.UpdateAsync(recordModel);
            }
            //有空的则写入到日志中，并将最新的条码写入到新的数据中

            //没有空，将最新的条码更新原有记录中
        }
    }
}
