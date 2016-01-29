using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Sample.Usecases;
using Sample.Models.Account;
using Sample.Context;
using Microsoft.AspNet.Authorization;

namespace Sample.Controllers
{
    //<summary>
    // 口座に関わる顧客のUI要求を処理します。
    //</summary>
    //[Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerSupport
    {
        private readonly AccountService _service;

        public AccountController(AccountService service, DomainHelper dh) : base(dh)
        {
            _service = service;
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
            return this.Json(LoginAccount.Of(_service.GetAccount(Actor().Id)));
        }

        //<summary>クライアント利用用途に絞ったパラメタ</summary>
        class LoginAccount
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public IList<string> Authorities { get; set; }

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