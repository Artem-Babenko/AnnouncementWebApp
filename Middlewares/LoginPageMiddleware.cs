
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Клас, який представляє проміжний компонент для обробки сторінки входу (логіну).
/// </summary>
public class LoginPageMiddleware
{
    readonly RequestDelegate next;

    /// <summary>
    /// Конструктор класу, який ініціалізує об'єкт middleware.
    /// </summary>
    /// <param name="next">Наступний обробник запитів.</param>
    public LoginPageMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Метод, який обробляє запити, які надходять до middleware.
    /// </summary>
    /// <param name="context">Контекст HTTP-запиту.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;
        DatabaseContext database = new DatabaseContext();

        // Перевіряємо, чи запит відповідає сторінці для входу за умовами GET.
        if (path == "/login" && method == "GET")
        {
            await context.Response.SendFileAsync("wwwroot/html/login.html");
        }

        // Перевіряємо, чи запит відповідає сторінці для входу за умовами POST.
        else if (path == "/login" && method == "POST")
        {
            var login = await request.ReadFromJsonAsync<LoginModel>();
            if (login is null) return;

            var email = login.Email;
            var password = login.Password;

            // перевірка чи є користувач
            var user = database.Users.ToList().FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user is null)
            {
                response.StatusCode = 404;
                return;
            }
            else // якщо користувача знайдено то
            {
                // Створюємо клейми користувача та створюємо ідентичність на основі цих клеймів.
                var claims = new List<Claim>()
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                // Виконуємо процедуру аутентифікації користувача.
                await context.SignInAsync(principal);

                response.StatusCode = 200;
            }
        }

        // Якщо запит не відповідає сторінці входу, передаємо управління наступному middleware.
        else
        {
            await next.Invoke(context);
        }
    }
}

public class LoginModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}