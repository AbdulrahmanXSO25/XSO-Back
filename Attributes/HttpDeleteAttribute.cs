namespace XSOBack
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public HttpDeleteAttribute()
        {
            Method = HttpMethod.DELETE;
        }
    }
}