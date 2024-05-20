namespace XSOBack
{
    public class Route
    {
        public HttpMethod Method { private set; get; }

        public string Path { private set; get; }

        public RequestHandler Handler { private set; get; }

        public Route(HttpMethod method, string path, RequestHandler handler)
        {
            Method = method;
            Path = path;
            Handler = handler;
        }
    }
}