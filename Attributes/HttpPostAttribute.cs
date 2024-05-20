namespace XSOBack
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute()
        {
            Method = HttpMethod.POST;
        }
    }
}