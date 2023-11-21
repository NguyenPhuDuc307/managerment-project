namespace NetCore.BackendServer.Services;

public interface IViewRenderService
{
    Task<string> RenderToStringAsync(string viewName, object model);
}