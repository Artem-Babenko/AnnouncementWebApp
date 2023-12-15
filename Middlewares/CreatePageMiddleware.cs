using AnnouncementWebApp.Extensions;
using AnnouncementWebApp.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Клас, який представляє middleware для обробки створення оголошень та інших пов'язаних шляхів.
/// </summary>
public class CreatePageMiddleware
{
    private List<Tag> tags;
    private readonly RequestDelegate next;

    /// <summary>
    /// Конструктор класу CreatePageMiddleware.
    /// </summary>
    /// <param name="next">Делегат для наступного компонента middleware.</param>
    /// <param name="tags">Список тегів оголошень.</param>
    public CreatePageMiddleware(RequestDelegate next, List<Tag> tags)
    {
        this.next = next;
        this.tags = tags;
    }

    /// <summary>
    /// Обробник запиту для сторінки створення оголошень та інших пов'язаних URL-шляхів.
    /// </summary>
    /// <param name="context">Об'єкт HttpContext для обробки запиту.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        var method = request.Method;
        var response = context.Response;
        DatabaseContext database = new DatabaseContext();

        bool isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;

        if (path == "/create" && method == "GET" && isAuthenticated)
        {
            // Відправляємо сторінку створення оголошень, якщо користувач авторизований.
            await response.SendFileAsync("wwwroot/html/create.html");
        }

        else if (path == "/create" && method == "POST" && isAuthenticated)
        {
            // Обробляємо POST-запит для створення нового оголошення.
            var form = request.Form;
            var id = context.User.FindFirst("Id");
            var user = database.Users.ToList().FirstOrDefault(u => u.Id == id?.Value);

            Announcement announcement = new Announcement(
                id: Guid.NewGuid().ToString(),
                photoLink: form["photo"],
                name: form["name"],
                shortDescription: form["short-description"],
                fullDescription: form["full-description"],
                owner: form["owner"],
                ownerId: user?.Id ?? "null",
                tag: tags.FindTag(form["tag"]).Name,
                startDate: DateTime.ParseExact(form["startDate"], "yyyy-MM-dd", null),
                endDate: DateTime.ParseExact(form["endDate"], "yyyy-MM-dd", null), 
                telegram: form["telegram"],
                email: form["email"],
                address: form["address"],
                phone: form["phone"]
            );

            // Встановлення власитвості Активне для відображення на головній сторінці.
            var now = DateTime.Now;
            if (announcement.StartDate < now && announcement.EndDate > now)
                announcement.Active = true;
            else
                announcement.Active = false;

            // При створення оголошення, його фотографія видаляється зі списку тимчасових файлів
            TemporaryPhoto? temporaryPhoto = database.TemporaryPhotos.ToList().FirstOrDefault(temporaryPhoto => temporaryPhoto.Path == form["photo"]);
            if(temporaryPhoto is not null) database.TemporaryPhotos.Remove(temporaryPhoto);

            database.Announcements.Add(announcement);
            database.SaveChanges();

            response.Redirect("/");
        }

        else if (path == "/create/user" && method == "GET" && isAuthenticated)
        {
            // Отримуємо інформацію про користувача та надсилаємо її у форматі JSON.
            var id = context.User.FindFirst("Id");
            var user = database.Users.ToList().FirstOrDefault(u => u.Id == id?.Value);
            await response.WriteAsJsonAsync(user);
        }

        else if(path == "/create/tags" && method == "GET" && isAuthenticated)
        {
            // Відправлення списку тегів у форматі JSON.
            await response.WriteAsJsonAsync(tags);
        }

        else if (path == "/create/upload-photo" && method == "POST" && isAuthenticated)
        {
            // Завантажуємо фотографії.
            IFormFileCollection files = request.Form.Files;
            var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/photos";
            Directory.CreateDirectory(uploadPath);

            string fileName = "";
            foreach (var file in files)
            {
                fileName = Guid.NewGuid().ToString() + file.FileName;

                string fullPath = $"{uploadPath}/{fileName}";

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Код для зменшення розміру фото
                using (var image = Image.Load(fullPath))
                {
                    int targetWidth = 1600;
                    int targetHeight = 900;

                    if (image.Width > targetWidth || image.Height > targetHeight)
                    {
                        double scale = Math.Min((double)targetWidth / image.Width, (double)targetHeight / image.Height);
                        int newWidth = (int)(image.Width * scale);
                        int newHeight = (int)(image.Height * scale);

                        image.Mutate(operation => operation.Resize(new ResizeOptions
                        {
                            Size = new Size(newWidth, newHeight),
                            Mode = ResizeMode.Max,
                        }));
                    }

                    image.Save(fullPath, new JpegEncoder());
                }

                // Додавання тимчасового фото.
                database.TemporaryPhotos.Add(new TemporaryPhoto($"/photos/{fileName}", DateTime.Now));

            }
            database.SaveChanges();
            await response.WriteAsJsonAsync(new { fileName = $"/photos/{fileName}" });
        }

        else if (!isAuthenticated && (path == "/create" || path == "/create/user" || path == "/create/upload-photo"))
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