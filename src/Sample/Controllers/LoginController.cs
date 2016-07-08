using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Context;
using Sample.Models.Constraints;
using System.ComponentModel.DataAnnotations;

namespace Sample.Controllers
{
    public class LoginController : ControllerSupport
    {
        public LoginController(DomainHelper dh) : base(dh) { }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest p)
        {
            VerifyModel();
            return this.Json(true);
        }
        public class LoginRequest
        {
            [NotNull]
            public string LoginId { get; set; }
            [NotNull]
            public string PlainPassword { get; set; }
        }

    }
}
