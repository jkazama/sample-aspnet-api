using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Sample.Usecases;
using Sample.Models.Account;
using Sample.Context;

namespace Sample.Controllers
{
    //<summary>
    // 口座に関わる顧客のUI要求を処理します。
    //</summary>
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountService _service;
        private readonly DomainHelper _dh;

        public AccountController(AccountService service, DomainHelper dh)
        {
            _service = service;
            _dh = dh;
        }

        //<summary>ログイン状態を確認します</summary>
        [HttpGet("loginStatus")]
        public IActionResult LoginStatus()
        {
            return this.Json(true);
        }

        //<summary>口座ログイン情報を取得します</summary>
        [HttpGet("loginAccount")]
        public IActionResult LoadLoginAccount()
        {
            return this.Json(LoginAccount.Of(_service.GetAccount(_dh.Actor().Id)));
        }

        //<summary>クライアント利用用途に絞ったパラメタ</summary>
        class LoginAccount
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<string> Authorities { get; set; }

            public static LoginAccount Of(Account m)
            {
                return new LoginAccount
                {
                    Id = m.Id,
                    Name = m.Name,
                    Authorities = new List<string> { "ROLE_USER" }
                };
            }
        }
    }
}