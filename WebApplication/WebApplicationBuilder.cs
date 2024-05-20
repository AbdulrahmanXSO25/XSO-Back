using System.Reflection;

namespace XSOBack
{
    public class WebApplicationBuilder()
    {
        private List<Route> _routes = new List<Route>();

        private List<IMiddleware> _middlewares = new List<IMiddleware>();

        public void RegisterRoutesCollection(Type routesCollectionType)
        {
            if (!typeof(RoutesCollection).IsAssignableFrom(routesCollectionType))
            {
                throw new ArgumentException("Type must be derived from RoutesCollection", nameof(routesCollectionType));
            }

            string basePath = routesCollectionType.Name.Replace("Routes", "");

            var routesCollectionInstance = Activator.CreateInstance(routesCollectionType);

            foreach (MethodInfo method in routesCollectionType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                HttpMethodAttribute httpMethodAttr = method.GetCustomAttribute<HttpMethodAttribute>(true);
                if (httpMethodAttr != null &&
                    method.ReturnType == typeof(void) &&
                    method.GetParameters().Length == 1 &&
                    method.GetParameters()[0].ParameterType == typeof(RequestContext))
                {
                    string routePath = $"/{basePath}/{method.Name}";
                    Delegate methodDelegate = Delegate.CreateDelegate(typeof(RequestHandler), routesCollectionInstance, method);
                    Route newRoute = new Route(httpMethodAttr.Method, routePath, (RequestHandler)methodDelegate);
                    _routes.Add(newRoute);
                }
            }

        }

        public void RegisterMiddleware(IMiddleware middleware)
        {
            _middlewares.Add(middleware);
        }

        public WebApplication Build()
        {
            var router = new Router();

            foreach (var route in _routes)
            {
                router.RegisterHandler(route);
            }

            foreach (var middleware in _middlewares)
            {
                router.UseGlobalMiddleware(middleware);
            }

            router.UseGlobalMiddleware(new LoggingMiddleware());

            return new WebApplication(router);
        }
    }
}