
namespace AnnouncementWebApp.Services;

/// <summary>
/// Служба для очищення тимчасових фотографій, які перевищили встановлений інтервал "lifeTimeInterval".
/// </summary>
public class TemporaryPhotoService
{
    readonly TimeSpan checkInterval;
    readonly TimeSpan lifeTimeInterval;

    /// <summary>
    /// Ініціалізує новий екземпляр класу TemporaryPhotoService з переданими параметрами.
    /// </summary>
    /// <param name="checkInterval">Інтервал періодичної перевірки фотографій на видалення.</param>
    /// <param name="lifeTimeInterval">Максимальний час, протягом якого фотографія вважається дійсною.</param>
    public TemporaryPhotoService(TimeSpan checkInterval, TimeSpan lifeTimeInterval)
    {
        this.checkInterval = checkInterval;
        this.lifeTimeInterval = lifeTimeInterval;
    }

    /// <summary>
    /// Запускає службу очищення тимчасових фотографій.
    /// </summary>
    public void Start()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    DatabaseContext database = new DatabaseContext();
                    var now = DateTime.Now;
                    var lifeThreshold = now - lifeTimeInterval;

                    var temporaryPhotos = database.TemporaryPhotos.ToList();
                    var photosToDelete = temporaryPhotos.Where(photo => photo.CreationTime <= lifeThreshold).ToList();

                    foreach (var photo in photosToDelete)
                    {
                        // Видаляємо фотографію з файлової системи
                        string filePath = $"{Directory.GetCurrentDirectory()}/wwwroot{photo.Path}";
                        File.Delete(filePath);

                        // Видаляємо фотографію з бази даних
                        database.TemporaryPhotos.Remove(photo);
                    }

                    int countDeleted = photosToDelete.Count;
                    int countTemporaryPhoto = temporaryPhotos.Count;

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("TemporaryPhotoService");
                    Console.ResetColor();
                    Console.WriteLine($": Check complited.\nTime: {now}. Deleted count: {countDeleted}/{countTemporaryPhoto}.\n");

                    database.SaveChanges();

                    await Task.Delay(checkInterval);
                }
                catch (Exception) { }
            }
        });
    }

}


