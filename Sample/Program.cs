using XSOBack;

namespace Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new WebApplicationBuilder();

            builder.RegisterRoutesCollection(typeof(UsersRoutes));

            builder.RegisterMiddleware(new RateLimitMiddleware(5, TimeSpan.FromMinutes(1)));

            var app = builder.Build();

            app.SetPort(5000);

            app.Start();
        }
    }
}