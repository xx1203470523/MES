using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.CCDFileUploadComplete;
using Hymson.MES.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.CCDFileUploadComplete
{
    /// <summary>
    ///CCD文件上传完成验证
    /// </summary>
    internal class CCDFileUploadCompleteValidator : AbstractValidator<CCDFileUploadCompleteDto>
    {
        public CCDFileUploadCompleteValidator()
        {
            //RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);
            //RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.SFCs).NotEmpty().Must(it => !it.Any()).WithErrorCode(ErrorCode.MES19107);
        }
    }
}
