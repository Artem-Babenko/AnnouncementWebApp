
namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Middleware для обробки запитів, пов'язаних з сторінкою для кожного оголошення.
/// </summary>
public class AnnouncementPageMiddleware
{
    readonly RequestDelegate next;

    /// <summary>
    /// Ініціалізує новий екземпляр класу AnnouncementPageMiddleware.
    /// </summary>
    /// <param name="next">Наступний обробник запиту в конвеєрі.</param>
    public AnnouncementPageMiddleware(RequestDelegate next) => this.next = next;

    /// <summary>
    /// Обробляє запит та відповідає на нього відповідно до шляху та методу запиту.
    /// </summary>
    /// <param name="context">Контекст HTTP-запиту та відповіді.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;
        DatabaseContext database = new DatabaseContext();

        // Перевіряємо, чи шлях запиту відповідає "/announcement" і метод - "GET".
        if (path == "/announcement" && method == "GET")
        {
            // Отримуємо ідентифікатор користувача і шукаємо його в базі даних.
            var id = context.User.FindFirst("Id");
            if(id is not null)
            {
                var user = database.Users.ToList().FirstOrDefault(u => u.Id == id.Value);
                if(user is not null)
                {
                    // Додаєм один перегляд користувачу.
                    user.Views++;
                    database.SaveChanges();
                }
            } 
            // Відправляємо файл сторінки оголошень із веб-додатка.
            await response.SendFileAsync("wwwroot/html/announcement.html");
        }
        // Перевіряємо, чи шлях запиту відповідає "/announcement/get" і метод - "GET".
        else if (path == "/announcement/get" && method == "GET")
        {
            // Отримуємо ідентифікатор оголошення і шукаємо його в базі даних.
            var id = request.Query["id"];
            var announcement = database.Announcements.ToList().FirstOrDefault(announcement => announcement.Id == id);

            // Якщо оголошення знайдено, відправляємо його як JSON-відповідь.
            if (announcement is not null)
            {
                announcement.Views++; // додаємо перегляд
                database.SaveChanges();
                await response.WriteAsJsonAsync(announcement);
            }
        }
        // Якщо шлях та метод не відповідають жодному з вищезазначених варіантів,
        else
        {
            // передаємо обробку наступному обробнику в конвеєрі запитів.
            await next.Invoke(context);
        }
    }
}
