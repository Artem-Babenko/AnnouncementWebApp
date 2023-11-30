using AnnouncementWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Xml.Linq;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Клас, який представляє middleware для обробки сторінки реєстрації користувачів.
/// </summary>
public class RegistrationPageMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// Конструктор класу RegistrationPageMiddleware.
    /// </summary>
    /// <param name="next">Делегат для наступного компонента middleware.</param>
    public RegistrationPageMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Обробник запиту для сторінки реєстрації користувачів та інших пов'язаних URL-шляхів.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext для обробки запиту.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;
        DatabaseContext database = new DatabaseContext();

        if (path == "/registration" && method == "GET")
        {
            // Відправляємо сторінку реєстрації користувачів при GET-запиті.
            await response.SendFileAsync("wwwroot/html/registration.html");
        }
        else if (path == "/registration" && method == "POST")
        {
            var registrationData = await request.ReadFromJsonAsync<RegistrationModel>();
            if (registrationData is null) return;

            var users = database.Users.ToList();

            var userWithSameEmail = users.FirstOrDefault(u => u.Email == registrationData.Email);
            if (userWithSameEmail != null)
            {
                // якщо пошта зареєстрована
                response.StatusCode = 400;
                return;
            }
            else
            {
                // Створюємо нового користувача та додаємо його до списку користувачів.
                var newUser = new User(Guid.NewGuid().ToString(), registrationData.Name, registrationData.Surname, registrationData.Email, registrationData.Password, "user", DateTime.Now);
                database.Users.Add(newUser);

                // Створюємо ідентичність та ролі для користувача та встановлюємо автентифікацію.
                var claims = new List<Claim>()
                {
                    new Claim("Id", newUser.Id),
                    new Claim(ClaimTypes.Name, newUser.Name),
                    new Claim(ClaimTypes.Surname, newUser.Surname),
                    new Claim(ClaimTypes.Email, newUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, newUser.Role)
                };
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);
                await context.SignInAsync(principal);

                database.SaveChanges();
                response.StatusCode = 200;
            }
        }
        else
        {
            // Якщо шлях не відповідає жодному з умов вище, передаємо обробку наступному middleware.
            await next.Invoke(context);
        }
    }
}

class RegistrationModel
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";  
}