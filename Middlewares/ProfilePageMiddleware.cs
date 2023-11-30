
using AnnouncementWebApp.Models;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Клас, який представляє middleware для обробки сторінки профілю та інших пов'язаних шляхів.
/// </summary>
public class ProfilePageMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// Конструктор класу ProfilePageMiddleware.
    /// </summary>
    /// <param name="next">Делегат для наступного компонента middleware.</param>
    public ProfilePageMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Обробник запиту для сторінки профілю та інших пов'язаних шляхів.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext для обробки запиту.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;
        DatabaseContext database = new DatabaseContext();
        List<Announcement> announcementsInProfile = new List<Announcement>();
        bool isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;

        if (isAuthenticated)
        {
            // Знайдіть всі оголошення, де OwnerId дорівнює id.
            var id = context.User.FindFirst("Id");
            if (id is not null)
            announcementsInProfile = database.Announcements.ToList().Where(a => a.OwnerId == id.Value).ToList();
            // Тепер у "announcements" містяться всі оголошення з відповідним OwnerId.
        }

        if (path == "/profile" && method == "GET" && isAuthenticated)
        {
            // Надсилаємо сторінку профілю, якщо користувач авторизований.
            await response.SendFileAsync("wwwroot/html/profile.html");
        }
        else if (path == "/profile/user" && method == "GET" && isAuthenticated)
        {
            // Отримуємо інформацію про користувача
            var id = context.User.FindFirst("Id");
            var user = database.Users.ToList().FirstOrDefault(u => u.Id == id?.Value);
            // Надсилаємо інформацію про користувача.
            await response.WriteAsJsonAsync(user);
        }
        else if (path == "/profile/user/announcements" && method == "GET" && isAuthenticated)
        {
            await response.WriteAsJsonAsync(announcementsInProfile);
        }
        else if (path == "/profile/user/announcements" && method == "DELETE" && isAuthenticated)
        {
            Announcement? announcementData = await request.ReadFromJsonAsync<Announcement>();
            if (announcementData is null) return;

            Announcement? announcement = announcementsInProfile.FirstOrDefault(a => a.Id == announcementData.Id);
            if (announcement is null) return;

            // Видалення фото оголошення з серверу
            string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot{announcement.PhotoLink}";
            File.Delete(filePath);

            database.Announcements.Remove(announcement);
            database.SaveChanges();


            await response.WriteAsJsonAsync(announcement.Id);
        }
        else if (!isAuthenticated && (path == "/profile" || path == "/profile/user"))
        {
            // Якщо користувач не авторизований, перенаправляємо його на сторінку входу і передаємо returnUrl.
            string returnUrl = path;
            response.Redirect($"/login?returnUrl={returnUrl}");
        }
        else
        {
            // Якщо шлях не відповідає жодному з умов вище, передаємо обробку наступному middleware.
            await next.Invoke(context);
        }
    }
}