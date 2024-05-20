namespace XSOBack
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute()
        {
            Method = HttpMethod.GET;
        }
    }
}