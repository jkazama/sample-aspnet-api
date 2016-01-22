using Microsoft.AspNet.Mvc;
using Sample.Context;
using Sample.Models.Asset;
using Sample.Usecases;
using System;
using System.Linq;

namespace Sample.Controllers
{
    //<summary>
    // 資産に関わる顧客のUI要求を処理します。
    //</summary>
    [Route("api/[controller]")]
    public class AssetController : Controller
    {
        private readonly AssetService _service;
        public AssetController(AssetService service)
        {
            _service = service;
        }

        //<summary>未処理の振込依頼情報を検索します。</summary>
        [HttpGet("cio/unprocessedOut/")]
        public IActionResult FindUnProcessedOut()
        {
            return this.Json(_service.FindUnprocessedCashOut().Select(cio => CashOutUI.Of(cio)).ToList());
        }

        //<summary>
        // 振込出金依頼をします。
        //</summary>
        [HttpPost("cio/withdraw")]
        public IActionResult Withdraw(RegCashOut p)
        {
            return this.Json(_service.Withdraw(p));
        }

        //<summary>振込出金依頼情報の表示用Dto</summary>
        class CashOutUI
        {
            public long Id { get; set; }
            public string Currency { get; set; }
            public decimal AbsAmount { get; set; }
            public DateTime RequestDay { get; set; }
            public DateTime RequestDate { get; set; }
            public DateTime EventDay { get; set; }
            public DateTime ValueDay { get; set; }
            public ActionStatusType StatusType { get; set; }
            public DateTime UpdateDate { get; set; }
            public long? CashflowId { get; set; }

            public static CashOutUI Of(CashInOut cio)
            {
                return new CashOutUI
                {
                    Id = cio.Id,
                    Currency = cio.Currency,
                    AbsAmount = cio.AbsAmount,
                    RequestDay = cio.RequestDay,
                    RequestDate = cio.RequestDate,
                    EventDay = cio.EventDay,
                    ValueDay = cio.ValueDay,
                    StatusType = cio.StatusType,
                    UpdateDate = cio.UpdateDate,
                    CashflowId = cio.CashflowId
                };
            }
        }
    }
}