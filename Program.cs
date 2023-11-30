
using AnnouncementWebApp.Models;
using AnnouncementWebApp.Extensions;
using AnnouncementWebApp.Services;

namespace AnnouncementWebApp;

class Program
{
    static void Main()
    {
        // Список тегів
        List<Tag> tags = new List<Tag>()
        {
            new Tag("Погода"),
            new Tag("Їжа"),
            new Tag("Вечірка"),
            new Tag("Екскурсія"),
            new Tag("Шоу"),
            new Tag("Університет")
        };
        
        // Створення будівальника програми з впроваженням залежностей
        var builder = WebApplication.CreateBuilder();
        // database
        builder.Services.AddEntityFrameworkSqlite().AddDbContext<DatabaseContext>();
        // фонові служби
        builder.Services.AddSingleton(new AnnouncementActiveService(checkInterval: TimeSpan.FromSeconds(30)));
        builder.Services.AddSingleton(new TemporaryPhotoService(checkInterval: TimeSpan.FromSeconds(60), lifeTimeInterval: TimeSpan.FromMinutes(60)));
        // ауторизація
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication("Cookies").AddCookie();
        var app = builder.Build();

        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();

        // Запуск фонових сервісів
        app.Services.StartAnnouncementActiveService();
        app.Services.StartTemporaryPhotoService();

        app.UseSetContentTypeHtml();

        // Обробка проміжного програмного забезпечення сторінок 
        app.UseMainPage(tags);

        app.UseAnnouncementPage();

        app.UseProfilePage();

        app.UseLoginPage();

        app.UseCreatePage(tags);

        app.UseUpdatePage(tags);

        app.UseRegistrationPage();

        app.UseLogoutPage();

        // Запуск програми
        app.Run();

    }
}