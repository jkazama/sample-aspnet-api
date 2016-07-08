using Sample.Context;

namespace Sample.Usecases
{
    //<summary>
    // Serviceで利用されるユーティリティ処理。
    //</summary>
    public static class ServiceUtils
    {
        public static Actor ActorUser(Actor actor)
        {
            if (actor.RoleType.IsAnonymous())
                throw new ValidationException(Resources.Exception.Authentication);
            return actor;
        }
    }
}
