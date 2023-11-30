namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Проміжний програмний шар для встановлення типу контенту HTTP-відповідей на "text/html; charset=utf-8".
/// </summary>
public class ContentTypeHtmlMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// Ініціалізує новий екземпляр класу ContentTypeHtmlMiddleware.
    /// </summary>
    /// <param name="next">Наступний проміжний програмний шар в конвеєрі.</param>
    public ContentTypeHtmlMiddleware(RequestDelegate next) => this.next = next;

    /// <summary>
    /// Встановлює тип контенту HTTP-відповіді на "text/html; charset=utf-8" і передає запит на обробку наступному проміжному програмному шару в конвеєрі.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext, що представляє поточний запит та відповідь.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        await next.Invoke(context);
    }
}