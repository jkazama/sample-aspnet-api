using Microsoft.AspNetCore.Mvc;
using Sample.Context;
using Sample.Models.Asset;
using Sample.Usecases;

namespace Sample.Controllers.Admin
{
    //<summary>
    // 資産に関わる社内のUI要求を処理します。
    //</summary>
    [Route("api/admin/asset")]
    public class AssetAdminController : ControllerSupport
    {
        private AssetAdminService _service;
        public AssetAdminController(DomainHelper dh, AssetAdminService service) : base(dh)
        {
            this._service = service;
        }

        //<summary>未処理の振込依頼情報を検索します。</summary>
        [HttpGet("cio/")]
        public IActionResult FindCashInOut(FindCashInOut p)
        {
            return this.Json(_service.FindCashInOut(p));
        }

    }
}
