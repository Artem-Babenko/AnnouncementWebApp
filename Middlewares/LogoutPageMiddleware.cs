using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Проміжний програмний шар для обробки виходу користувача (логауту).
/// </summary>
public class LogoutPageMiddleware
{
    readonly RequestDelegate next;

    /// <summary>
    /// Ініціалізує новий екземпляр класу LogoutPageMiddleware.
    /// </summary>
    /// <param name="next">Наступний проміжний програмний шар в конвеєрі.</param>
    public LogoutPageMiddleware(RequestDelegate next) => this.next = next;

    /// <summary>
    /// Обробляє вхідні HTTP-запити та виконує вирази для виходу користувача у випадку запиту "/logout" методом "GET".
    /// </summary>
    /// <param name="context">Об'єкт HttpContext, що представляє поточний запит та відповідь.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;

        if (path == "/logout" && method == "GET")
        {
            // Видалення Кукі та повернення на головну сторінку
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            response.Redirect("/");
        }
        else
        {
            await next.Invoke(context);
        }
    }
}