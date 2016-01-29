using System.Linq;
using Microsoft.AspNet.Mvc;
using System;
using Microsoft.AspNet.Mvc.ModelBinding;
using Sample.Context;

namespace Sample.Controllers
{
    public abstract class ControllerSupport : Controller
    {
        public DomainHelper Helper { get; set; }

        public ControllerSupport(DomainHelper helper)
        {
            this.Helper = helper;
        }

        //<summary>匿名を除く利用者を返します</summary>
        protected Actor Actor()
        {
            var actor = Helper.Actor();
            if (actor.RoleType.IsAnonymous()) throw new ValidationException(ErrorKeys.Authentication);
            return actor;
        }

        //<summary>
        // ModelStateの評価をおこないます。無効な時はValidationExceptionを発生させます。
        //</summary>
        protected void VerifyModel()
        {
            if (!ModelState.IsValid)
            {
                Warns warns = Warns.Init();
                ModelState.Where(v => v.Value.ValidationState == ModelValidationState.Invalid)
                    .ToList().ForEach(v => v.Value.Errors.ToList().ForEach(msg => warns.Add(v.Key, msg.ErrorMessage)));
                throw new ValidationException(warns);
            }

        }
    }
}
