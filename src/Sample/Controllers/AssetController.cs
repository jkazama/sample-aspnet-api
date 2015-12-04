using Microsoft.AspNet.Mvc;

namespace Sample.Controllers
{
    //<summary>
    // 資産に関わる顧客のUI要求を処理します。
    //</summary>
    [Route("api/[controller]")]
    public class AssetController : Controller
    {
        public AssetController()
        {

        }

        //<summary>ログイン状態を確認します</summary>
        [HttpGet("cio/unprocessedOut/")]
        public IActionResult FindUnProcessedOut()
        {
            return this.Json(true);
        }
    }
}