using Microsoft.AspNetCore.Mvc;
using Sample.Context;
using System.Linq;

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
            if (actor.RoleType.IsAnonymous()) throw new ValidationException(Resources.Exception.Authentication);
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
                ModelState.ToList().ForEach(v => v.Value.Errors.ToList().ForEach(msg =>
                {
                    warns.Add(FieldName(v.Key), msg.ErrorMessage);
                }));
                throw new ValidationException(warns);
            }
        }
        private string FieldName(string field)
        {
            return field.Substring(field.IndexOf('.') + 1);
        }
    }
}
