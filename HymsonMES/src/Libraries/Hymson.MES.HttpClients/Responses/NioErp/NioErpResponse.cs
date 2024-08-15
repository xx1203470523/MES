using Hymson.MES.HttpClients.Requests.ERP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Responses.NioErp
{
    /// <summary>
    /// ERP中关于NIO的返回
    /// </summary>
    public class NioErpResponse : BaseERPResponse
    {
        /// <summary>
        /// 数据详情
        /// </summary>
        public List<NioErpDetail> Data { get; set; } = new List<NioErpDetail>();

        /// <summary>
        /// 请购数据
        /// </summary>
        public List<AppVouchListModel> AppVouchListList { get; set; } = new List<AppVouchListModel>();

        /// <summary>
        /// 供应商存货对照信息
        /// </summary>
        public List<VenAndInvListModel> VenAndInvListList { get; set; } = new List<VenAndInvListModel>();

        /// <summary>
        /// 请购数据
        /// </summary>
        public string? AppVouchList { get; set; }

        /// <summary>
        /// 供应商存货对照信息
        /// </summary>
        public string? VenAndInvList { get; set; }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="materialCodeList"></param>
        public void InitData(List<string> materialCodeList)
        {
            if(materialCodeList == null || materialCodeList.Count == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(AppVouchList) == false)
            {
                AppVouchListList = JsonConvert.DeserializeObject<List<AppVouchListModel>>(AppVouchList);
            }
            if (string.IsNullOrEmpty(VenAndInvList) == false)
            {
                VenAndInvListList = JsonConvert.DeserializeObject<List<VenAndInvListModel>>(VenAndInvList);
            }

            foreach (var item in materialCodeList)
            {
                var curArrList = AppVouchListList.Where(m => m.CInvCode == item).ToList();
                var curVenList = VenAndInvListList.Where(m => m.cInvCode == item).ToList();

                NioErpDetail model = new NioErpDetail();
                model.MaterialCode = item;
                if (curVenList != null && curVenList.Count > 0)
                {
                    List<string> venCodeList = curVenList.Select(m => m.cVenName).ToList();
                    model.SupperialName = string.Join(";", venCodeList);
                }
                else
                {
                    model.SupperialName = "ERP没有该数据";
                }
                if (curArrList != null && curArrList.Count > 0)
                {
                    model.BuyReqCode = curArrList[0].CCode;
                    model.Num = curArrList.Select(m => m.FQuantity).Sum();
                }
                else
                {
                    model.BuyReqCode = "ERP没有该数据";
                    model.Num = 0;
                }

                Data.Add(model);
            }
        }
    }

    /// <summary>
    /// 请购数据
    /// </summary>
    public class AppVouchListModel
    {
        /// <summary>  
        /// 请购单号  
        /// </summary>  
        public string CCode { get; set; }

        /// <summary>  
        /// 请购日期  
        /// </summary>  
        public DateTime DDate { get; set; }

        /// <summary>  
        /// 存货编码  
        /// </summary>  
        public string CInvCode { get; set; }

        /// <summary>  
        /// 存货名称  
        /// </summary>  
        public string CInvName { get; set; }

        /// <summary>  
        /// 数量  
        /// </summary>  
        public decimal FQuantity { get; set; }

        /// <summary>  
        /// 到货日期  
        /// </summary>  
        public DateTime DArriveDate { get; set; }
    }

    /// <summary>
    /// 供应商存货对照信息
    /// </summary>
    public class VenAndInvListModel
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string CVenCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string cVenName { get; set; }

        /// <summary>
        /// 存货编码
        /// </summary>
        public string cInvCode { get; set; }

        /// <summary>
        /// 存货名称
        /// </summary>
        public string cInvName { get; set; }
    }

    public class NioErpDetail
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        //public string MaterialName { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        //public string SupperialCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupperialName { get; set; }

        /// <summary>
        /// 请购单号
        /// </summary>
        public string BuyReqCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        //public string Date { get; set; }

        //数量
        public decimal Num { get; set; }

        //需要到货日期
        //public string ReceiveData { get; set; }
    }
}
