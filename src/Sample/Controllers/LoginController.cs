using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Sample.Context;
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
            //HttpContext.Authentication
            return this.Json(true);
        }
        public class LoginRequest
        {
            [Required]
            public string LoginId { get; set; }
            [Required]
            public string PlainPassword { get; set; }
        }

    }
}
