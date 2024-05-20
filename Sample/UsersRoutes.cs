using XSOBack;

namespace Sample
{
    public class UsersRoutes : RoutesCollection
    {
        [HttpGet]
        public void GetAll(RequestContext context)
        {
            context.Ok("All users");
        }

        [HttpGet]
        public void SayHello(RequestContext context)
        {
            context.Ok("Hello World!");
        }

        [HttpGet]
        public void GetById(RequestContext context)
        {
            var userId = context.Request.QueryString["userId"];

            if (userId == null) context.BadRequest("Please provide userId in query params");

            else context.Ok($"User {userId} exists!");
        }

        [HttpPost]
        public void AddNewUser(RequestContext context)
        {
            var userName = context.Request.QueryString["userName"];

            if (userName == null) context.BadRequest("Please provide userName in query params");

            else context.Ok($"User {userName} added!");
        }
    }

}