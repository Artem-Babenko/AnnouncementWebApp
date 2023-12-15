
using AnnouncementWebApp.Extensions;
using AnnouncementWebApp.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace AnnouncementWebApp.Middlewares;

/// <summary>
/// Клас, який представляє middleware для обробки оновлення оголошень та інших пов'язаних шляхів.
/// </summary>
public class UpdatePageMiddleware
{
    private List<Tag> tags;
    private readonly RequestDelegate next;

    /// <summary>
    /// Конструктор класу CreatePageMiddleware.
    /// </summary>
    /// <param name="next">Делегат для наступного компонента middleware.</param>
    /// <param name="tags">Список тегів оголошень.</param>
    public UpdatePageMiddleware(RequestDelegate next, List<Tag> tags)
    {
        this.next = next;
        this.tags = tags;
    }

    /// <summary>
    /// Обробник запиту для сторінки оновлення оголошень та інших пов'язаних URL-шляхів.
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

        if (path == "/update" && method == "GET" && isAuthenticated)
        {
            // Відправляємо сторінку оновлення оголошень, якщо користувач авторизований.
            await response.SendFileAsync("wwwroot/html/update.html");
        }

        else if (path == "/update" && method == "POST" && isAuthenticated)
        {
            var form = request.Form;
            var id = request.Query["id"];
            Announcement? announcement = database.Announcements.ToList().FirstOrDefault(a => a.Id == id);
            if (announcement is null) return;

            announcement.PhotoLink = form["photo"];
            announcement.Name = form["name"];
            announcement.ShortDescription = form["short-description"];
            announcement.FullDescription = form["full-description"];
            announcement.Telegram = form["telegram"];
            announcement.Email = form["email"];
            announcement.Address = form["address"];
            announcement.Phone = form["phone"];
            announcement.Tag = tags.FindTag(form["tag"]).Name;
            announcement.StartDate = DateTime.ParseExact(form["startDate"], "yyyy-MM-dd", null);
            announcement.EndDate = DateTime.ParseExact(form["endDate"], "yyyy-MM-dd", null);
            announcement.StartDateString = announcement.StartDate.ToString("dd.MM.yy");
            announcement.EndDateString = announcement.EndDate.ToString("dd.MM.yy");

            // Встановлення власитвості Активне для відображення на головній сторінці.
            var now = DateTime.Now;
            if (announcement.StartDate < now && announcement.EndDate > now)
                announcement.Active = true;
            else
                announcement.Active = false;

            TemporaryPhoto? temporaryPhoto = database.TemporaryPhotos.ToList().FirstOrDefault(temporaryPhoto => temporaryPhoto.Path == form["photo"]);
            if (temporaryPhoto is not null) database.TemporaryPhotos.Remove(temporaryPhoto);

            database.SaveChanges();
            response.Redirect("/profile");
        }

        else if (path == "/update/get" && method == "GET" && isAuthenticated)
        {
            // Відправляємо інформацію про оголошення отримуючи id.
            var id = request.Query["id"];
            Announcement? announcement = database.Announcements.ToList().FirstOrDefault(announcement => announcement.Id == id);

            // При перезавантаженні сторінки, якщо користувач видалив фото оголошення, але фоче його повернути, фото оголошення видалиться з тимчасовий, якщо воно там є.
            var photo = database.TemporaryPhotos.ToList().FirstOrDefault(photo => photo.Path == announcement?.PhotoLink);
            if (photo is not null)
            {
                database.TemporaryPhotos.Remove(photo);
                database.SaveChanges();
            }
            await response.WriteAsJsonAsync(announcement);
        }

        else if (path == "/update/tags" && method == "GET" && isAuthenticated)
        {
            // Відправлення списку тегів у форматі JSON.
            await response.WriteAsJsonAsync(tags);
        }

        else if (path == "/update/add-temporary-photo" && method == "GET" && isAuthenticated)
        {
            // При видаленні фото оголошення, це фото додається до тимчасових.
            var photoPath = request.Query["path"];
            database.TemporaryPhotos.Add(new TemporaryPhoto(photoPath, DateTime.Now));
            database.SaveChanges();
        }

        else if (path == "/update/upload-photo" && method == "POST" && isAuthenticated)
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

        else if (!isAuthenticated && (path == "/update" || path == "/update/user" || path == "/update/upload-photo"))
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