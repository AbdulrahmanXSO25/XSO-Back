namespace XSOBack
{
    public interface IMiddleware
    {
        Task InvokeAsync(RequestContext context, Func<Task> next);
    }
}