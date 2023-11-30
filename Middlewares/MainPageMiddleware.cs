
using AnnouncementWebApp.Models;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Проміжний програмний шар для обробки запитів, пов'язаних з головною сторінкою та оголошеннями.
/// </summary>
public class MainPageMiddleware
{
    /// <summary>
    /// Наступний RequestDelegate в конвеєрі проміжних програмних шарів.
    /// </summary>
    private readonly RequestDelegate next;

    private readonly List<Tag> tags;

    /// <summary>
    /// Ініціалізує новий екземпляр класу MainPageMiddleware.
    /// </summary>
    /// <param name="next">Наступний проміжний програмний шар в конвеєрі.</param>
    public MainPageMiddleware(RequestDelegate next, List<Tag> tags)
    {
        this.tags = tags;
        this.next = next;
    }

    /// <summary>
    /// Обробляє вхідні HTTP-запити та надає відповіді на основі шляху та методу запиту.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext, що представляє поточний запит та відповідь.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;
        DatabaseContext database = new DatabaseContext();
        int announcementsPerPage = 25;

        if (path == "/" && method == "GET")
        {
            // Завантаження головної сторінки
            await context.Response.SendFileAsync("wwwroot/html/index.html");
        }

        else if (path == "/main/user" && method == "GET")
        {
            var message = "null";
            var id = context.User.FindFirst("Id");
            if (id != null) message = "have";
            // Якщо користувач авторизований то повертаємо стово "have" (для відображення повідомлення реєстрація, профіль та вхід
            await response.WriteAsJsonAsync(new { message });
        }

        else if (path == "/main/announcements" && method == "GET")
        {
            int page = Convert.ToInt32(request.Query["page"]);
            string sortMethod = request.Query["sort"];
            string selectedTag = request.Query["tag"];
            string search = request.Query["search"];

            int startAnnouncement = (page - 1) * announcementsPerPage;

            List<Announcement> announcements = database.Announcements.ToList();

            // Лишаємо тільки активні оголошення
            announcements = announcements.Where(announcement => announcement.Active == true).ToList();
            
            // Лишаємо тільки ті які рівні тегу пошуку.
            if (selectedTag != "Всі")
                announcements = announcements.Where(announcement => announcement.Tag == selectedTag).ToList();

            // Лишаємо тільки ті, які містять вміст пошуку у назві або описі
            if (!string.IsNullOrEmpty(search))
            {
                /*announcements = announcements.Where(announcement => announcement.Name.Contains(search) || announcement.ShortDescription.Contains(search) || announcement.FullDescription.Contains(search) || announcement.Owner.Contains(search) || announcement.Tag.Contains(search)).ToList();*/
                announcements = announcements.Where(announcement =>
                    announcement.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.ShortDescription.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.FullDescription.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.Address.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.Email.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.Telegram.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.Phone.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.Owner.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    announcement.Tag.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            
            // Кількість сортованих сторінок оголошень
            int totalPageCount = (int)Math.Ceiling((double)announcements.Count / announcementsPerPage);

            // Сортування за методом(За датою, За назвою, За популярністю)
            if (sortMethod == "За назвою")
            {
                announcements = announcements.OrderBy(announcement => announcement.Name).ToList();
            }
            else if (sortMethod == "За датою")
            {
                announcements = announcements.OrderByDescending(announcement => announcement.StartDate).ToList();
            }
            else if (sortMethod == "За популярністю")
            {
                announcements = announcements.OrderByDescending(announcement => announcement.Views).ToList();
            }

            // Берем діапазон
            List<Announcement> announcementsToResponse = announcements
                .Skip(startAnnouncement)
                .Take(announcementsPerPage)
                .ToList();

            var responseObj = new
            {
                Announcements = announcementsToResponse,
                TotalPages = totalPageCount
            };

            // Повертає оголошення у діапазоні відносно сторінки виклику.
            await response.WriteAsJsonAsync(responseObj);
        }

        else if (path == "/main/tags" && method == "GET")
        {
            // Повертає теги для оголошень
            await response.WriteAsJsonAsync(tags);
        }

        else
        {
            //Перехід до наступного програмного шару, якщо це не головна сторінка
            await next.Invoke(context);
        }
    }
}
