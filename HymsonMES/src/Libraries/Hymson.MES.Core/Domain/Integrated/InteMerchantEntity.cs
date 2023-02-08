using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 客商信息表数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteMerchantEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :客商编号 
        /// 空值 : true  
        /// </summary>
        public string MerchantCode { get; set; }
        
        /// <summary>
        /// 描述 :数据类型(1-T100 2-人工) 
        /// 空值 : true  
        /// </summary>
        public byte SourceType { get; set; }
        
        /// <summary>
        /// 描述 :交易对象类型(1-供应商 2-客户 3-二者皆是) 
        /// 空值 : true  
        /// </summary>
        public byte MerchantType { get; set; }
        
        /// <summary>
        /// 描述 :客商简称 
        /// 空值 : true  
        /// </summary>
        public string ShortName { get; set; }
        
        /// <summary>
        /// 描述 :客商全称 
        /// 空值 : true  
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// 描述 :助记码 
        /// 空值 : true  
        /// </summary>
        public string MnemonicCode { get; set; }
        
        /// <summary>
        /// 描述 :税务登记号 
        /// 空值 : true  
        /// </summary>
        public string TaxRegistrationNumber { get; set; }
        
        /// <summary>
        /// 描述 :生命周期(1-生效 2-失效 3-合格供应商(永久有效) 4-临时供应商(3个月有效) 5-一次性供应商(1个月有效) 
        /// 空值 : true  
        /// </summary>
        public byte LifeCycle { get; set; }
        
        /// <summary>
        /// 描述 :所属据点Id 
        /// 空值 : true  
        /// </summary>
        public int? StrongholdId { get; set; }
        }
}