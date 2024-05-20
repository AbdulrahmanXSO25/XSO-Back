namespace XSOBack
{
    public abstract class HttpMethodAttribute : Attribute
    {
        public HttpMethod Method { get; set; }
    }
}