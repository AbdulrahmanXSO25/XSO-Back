namespace XSOBack
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]

    public class HttpPutAttribute : HttpMethodAttribute
    {
        public HttpPutAttribute()
        {
            Method = HttpMethod.PUT;
        }
    }
}