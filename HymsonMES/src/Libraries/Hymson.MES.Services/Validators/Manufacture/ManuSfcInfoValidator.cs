/*
 *creator: Karl
 *
 *describe: 条码信息表    验证规则 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using FluentValidation;
using Hymson.MES.Services.Dtos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 条码信息表 更新 验证
    /// </summary>
    internal class ManuSfcInfoCreateValidator: AbstractValidator<ManuSfcInfoCreateDto>
    {
        public ManuSfcInfoCreateValidator()
        {

        }
    }

    /// <summary>
    /// 条码信息表 修改 验证
    /// </summary>
    internal class ManuSfcInfoModifyValidator : AbstractValidator<ManuSfcInfoModifyDto>
    {
        public ManuSfcInfoModifyValidator()
        {

        }
    }
}
