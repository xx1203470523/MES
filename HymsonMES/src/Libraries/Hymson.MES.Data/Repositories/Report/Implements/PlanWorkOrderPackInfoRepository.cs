using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public partial class PlanWorkOrderPackInfoRepository : BaseRepository, IPlanWorkOrderPackInfoRepository
{
    public PlanWorkOrderPackInfoRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

    public async Task<IEnumerable<PackTestView>> GetTestListAsync(PackTestQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var sqlTemplete = sqlBuilder.AddTemplate(packTestSql);

        sqlBuilder.AddParameters(query);

        if (!string.IsNullOrEmpty(query.SFC))
        {
            sqlBuilder.Where("mss.CirculationBarCode = @SFC");
        }

        if (!string.IsNullOrEmpty(query.WorkOrderCode))
        {
            sqlBuilder.Where("pwo.OrderCode  = @WorkOrderCode");
        }

        sqlBuilder.Where("CirculationBarCode  LIKE 'ES%' ");
        sqlBuilder.Where("sfc LIKE 'YT%' ");
        sqlBuilder.Where("mss.isdeleted = 0");

        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<PackTestView>(sqlTemplete.RawSql,sqlTemplete.Parameters);
    }

    public async Task<IEnumerable<PackTraceView>> GetTraceListAsync(PackTraceQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var sqlTemplete = sqlBuilder.AddTemplate(packTraceSql);

        sqlBuilder.AddParameters(query);

        if (!string.IsNullOrEmpty(query.SFC))
        {
            sqlBuilder.Where("mss.CirculationBarCode = @SFC");
        }

        if (!string.IsNullOrEmpty(query.WorkOrderCode))
        {
            sqlBuilder.Where("pwo.OrderCode  = @WorkOrderCode");
        }

        sqlBuilder.Where("CirculationBarCode  LIKE 'ES%' ");
        sqlBuilder.Where("sfc LIKE 'YT%' ");
        sqlBuilder.Where("mss.isdeleted = 0");

        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<PackTraceView>(sqlTemplete.RawSql, sqlTemplete.Parameters);
    }
}

/// <summary>
/// SQL
/// </summary>
public partial class PlanWorkOrderPackInfoRepository
{
    public const string packTestSql = @$"
WITH RECURSIVE CirculationCTE AS (
    -- 初始层级0
    SELECT DISTINCT CirculationBarCode, SFC, 0 AS lv
    FROM manu_sfc_circulation mss
    LEFT JOIN plan_work_order pwo ON pwo.id = mss.workOrderId
    /**where**/
    UNION ALL
    -- 递归部分，查询每一层的CirculationBarCode与其相关联的SFC
    SELECT m.CirculationBarCode, m.SFC, c.lv + 1
    FROM manu_sfc_circulation m
    JOIN CirculationCTE c ON c.SFC = m.CirculationBarCode
    WHERE c.lv < 2  -- 限制递归深度，总层级数为3
      AND m.isdeleted = 0
),
LatestParameters AS (
    SELECT 
        SFC,
        ParameterId,
        ParamValue,
        ROW_NUMBER() OVER (PARTITION BY SFC, ParameterId ORDER BY CreatedOn DESC) AS rn
    FROM manu_product_parameter
    WHERE SFC IN (SELECT DISTINCT CirculationBarCode FROM CirculationCTE)  
),
FilteredParameters AS (
    SELECT SFC, ParameterId, ParamValue
    FROM LatestParameters
    WHERE rn = 1
)
SELECT DISTINCT 
    cte.circulationbarcode SFC,
    MAX(CASE WHEN fp.ParameterId = 22207802265960449 THEN fp.ParamValue else 0 END) AS column1, -- '电压值',
    MAX(CASE WHEN fp.ParameterId = 22207802265960448 THEN fp.ParamValue else 0 END) AS column2, -- '电阻值'
    MAX(CASE WHEN fp.ParameterId = 22996387792723971 THEN fp.ParamValue else 0 END) AS column3, --  '正极对液冷板绝缘测试',
    MAX(CASE WHEN fp.ParameterId = 22996387792723977 THEN fp.ParamValue else 0 END) AS column4, --  '负极对液冷板绝缘测试',
    MAX(CASE WHEN fp.ParameterId = 23154324836229122 THEN fp.ParamValue else 0 END) AS column5, --  '正极对液冷板耐压测试',
    MAX(CASE WHEN fp.ParameterId = 22996387792723974 THEN fp.ParamValue else 0 END) AS column6, --  '负极对液冷板耐压测试',
    '' AS column7, --  'BAMU版本号',
    MAX(CASE WHEN fp.ParameterId IN (
        23046644484317184, 23046644484317185, 23046644484317186, 23046644484317187, 23046644484317188, 23046644484317189,
        23046644484317190, 23046644484317191, 23046644484317192, 23046644484317193, 23046644484317194, 23046644484317195,
        23046644484317196, 23046644484317197, 23046644484317198, 23046644484317199, 23046644484317200, 23046644484317201,
        23046644484317202, 23046644484317203, 23046644484317204, 23046644484317205, 23046644484317206, 23046644484317207,
        23046644484317208, 23046644484317209, 23046644484317210, 23046644484317211, 23046644484317212, 23046644484317213,
        23046644484317214, 23046644484317215, 23046644484317216, 23046644484317217, 23046644484317218, 23046644484317219,
        23046644484317220, 23046644484317221, 23046644484317222, 23046644484317223, 23046644484317224, 23046644484317225,
        23046644484317226, 23046644484317227, 23046644484317228, 23046644484317229, 23046644484317230, 23046644484317231,
        25556357165158400, 25556357165158401, 34089819198873600, 34089819198873601
    ) THEN fp.ParamValue END) AS column8, --  '采集电压最高值',
    MIN(CASE WHEN fp.ParameterId IN (
        23046644484317184, 23046644484317185, 23046644484317186, 23046644484317187, 23046644484317188, 23046644484317189,
        23046644484317190, 23046644484317191, 23046644484317192, 23046644484317193, 23046644484317194, 23046644484317195,
        23046644484317196, 23046644484317197, 23046644484317198, 23046644484317199, 23046644484317200, 23046644484317201,
        23046644484317202, 23046644484317203, 23046644484317204, 23046644484317205, 23046644484317206, 23046644484317207,
        23046644484317208, 23046644484317209, 23046644484317210, 23046644484317211, 23046644484317212, 23046644484317213,
        23046644484317214, 23046644484317215, 23046644484317216, 23046644484317217, 23046644484317218, 23046644484317219,
        23046644484317220, 23046644484317221, 23046644484317222, 23046644484317223, 23046644484317224, 23046644484317225,
        23046644484317226, 23046644484317227, 23046644484317228, 23046644484317229, 23046644484317230, 23046644484317231,
        25556357165158400, 25556357165158401, 34089819198873600, 34089819198873601
    ) THEN fp.ParamValue END) AS column9, --  '采集电压最低值',
    MAX(CASE WHEN fp.ParameterId IN (
        22996387792723980, 22996387793772544, 22996387793772545, 22996387793772546, 22996387793772547, 22996387793772548,
        22996387793772549, 22996387793772550, 22996387793772551, 22996387793772552, 22996387793772553, 22996387793772554,
        22996387793772555, 22996387793772556, 22996387793772557, 22996387793772558, 22996387793772559, 22996387793772560,
        22996387793772561, 22996387793772562, 22996387793772563, 22996387793772564, 22996387793772565, 22996387793772566,
        22996387793772567, 22996387793772568, 22996387793772569, 22996387793772570, 22996387793772571
    ) THEN fp.ParamValue END) AS column10, --  '采集最高温度',
    MIN(CASE WHEN fp.ParameterId IN (
        22996387792723980, 22996387793772544, 22996387793772545, 22996387793772546, 22996387793772547, 22996387793772548,
        22996387793772549, 22996387793772550, 22996387793772551, 22996387793772552, 22996387793772553, 22996387793772554,
        22996387793772555, 22996387793772556, 22996387793772557, 22996387793772558, 22996387793772559, 22996387793772560,
        22996387793772561, 22996387793772562, 22996387793772563, 22996387793772564, 22996387793772565, 22996387793772566,
        22996387793772567, 22996387793772568, 22996387793772569, 22996387793772570, 22996387793772571
    ) THEN fp.ParamValue END) AS column11, --  '采集最低温度',
    MAX(CASE WHEN fp.ParameterId = 22996387792723978 THEN fp.ParamValue else 0 END) AS column12, --  '压差',
    MAX(CASE WHEN fp.ParameterId = 22996387792723979 THEN fp.ParamValue else 0 END) AS column13, --  '温差',
    MAX(CASE WHEN fp.ParameterId = 22948098810617856 THEN fp.ParamValue else 0 END) AS column14 --  '漏率'
FROM CirculationCTE cte
LEFT JOIN FilteredParameters fp ON cte.circulationbarcode = fp.SFC
where cte.circulationbarcode like 'ES%'
GROUP BY cte.circulationbarcode
ORDER BY cte.circulationbarcode;";

    public const string packTraceSql = @$"
-- 追溯Sheet1
WITH RECURSIVE CirculationCTE AS (
    -- 初始层级0
    SELECT distinct CirculationBarCode, SFC, 0 AS lv
    FROM manu_sfc_circulation mss
    LEFT JOIN plan_work_order pwo ON pwo.id = mss.workOrderId
    /**where**/
    UNION ALL
    
    -- 递归部分，查询每一层的CirculationBarCode与其相关联的SFC
    SELECT m.CirculationBarCode, m.SFC, c.lv + 1
    FROM manu_sfc_circulation m
    JOIN CirculationCTE c ON c.SFC = m.CirculationBarCode
    WHERE c.lv < 2  -- 限制递归深度，lv < 2 则总层级数为3
   AND m.isdeleted = 0
)
SELECT DISTINCT  c1.CirculationBarCode AS Pack, -- 'Pack编码'
c1.SFC AS Module, -- '模组编码'
    (
        SELECT ParamValue
        FROM manu_product_parameter
        WHERE SFC = c1.SFC 
          AND ParameterId = 22207802265960449
        ORDER BY CreatedOn DESC
        LIMIT 1  -- 如果是SQL Server用TOP 1
    ) AS moduleVoltage, -- '模组电压值',
    (
        SELECT ParamValue
        FROM manu_product_parameter
        WHERE SFC = c1.SFC 
          AND ParameterId = 22207802265960448
        ORDER BY CreatedOn DESC
        LIMIT 1  -- 如果是SQL Server用TOP 1
    ) AS moduleInternalResistance, -- '模组电阻值'
c2.SFC AS cell , -- '电芯编码'
    (
        SELECT ParamValue
        FROM manu_product_parameter
        WHERE SFC = c2.SFC 
          AND ParameterId = 13598208638709760 AND paramValue < 100
        ORDER BY CreatedOn DESC
        LIMIT 1  -- 如果是SQL Server用TOP 1
    ) AS cellVoltage , -- '电芯电压值',
    (
        SELECT ParamValue
        FROM manu_product_parameter
        WHERE SFC = c2.SFC 
          AND ParameterId = 13598223492837376 AND paramValue < 100
        ORDER BY CreatedOn DESC
        LIMIT 1  -- 如果是SQL Server用TOP 1
    ) AS cellInternalResistance  -- '电芯电阻值'
FROM CirculationCTE c1
LEFT JOIN CirculationCTE c2 ON c1.SFC = C2.CirculationBarCode 
WHERE 
c1.CirculationBarCode LIKE 'E%' AND c1.SFC LIKE 'Y%'  AND c2.SFC LIKE '0%'
ORDER BY c1.CirculationBarCode,c1.SFC,c2.SFC

";
}