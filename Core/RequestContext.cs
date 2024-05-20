using Newtonsoft.Json;
using System.Net;

namespace XSOBack
{
    public class RequestContext
    {
        public HttpListenerRequest Request { get; }
        public HttpListenerResponse Response { get; }

        private readonly List<IMiddleware> middlewares = new List<IMiddleware>();
        private int _currentMiddlewareIndex = -1;

        public RequestContext(HttpListenerContext context)
        {
            Request = context.Request;
            Response = context.Response;
        }

        public void Use(IMiddleware middleware)
        {
            middlewares.Add(middleware);
        }

        public async Task InvokeMiddleware()
        {
            _currentMiddlewareIndex++;
            if (_currentMiddlewareIndex < middlewares.Count)
            {
                await middlewares[_currentMiddlewareIndex].InvokeAsync(this, InvokeMiddleware);
            }
        }

        public void Ok(object data)
        {
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            WriteJsonResponse(data);
        }

        public void BadRequest(string message)
        {
            WriteResponse(400, message);
        }

        public void NotFound(string message)
        {
            WriteResponse(404, message);
        }

        public void Unauthorized(string message)
        {
            WriteResponse(401, message);
        }
        
        private void WriteResponse(int statusCode, string content)
        {
            Response.StatusCode = statusCode;
            using (var writer = new StreamWriter(Response.OutputStream))
            {
                writer.WriteLine(content);
            }
            Response.Close();
        }

        private void WriteJsonResponse(object data)
        {
            Response.StatusCode = 200;
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Response.ContentType = "application/json";
            using (var writer = new StreamWriter(Response.OutputStream))
            {
                writer.Write(json);
            }
            Response.Close();
        }
    }
}