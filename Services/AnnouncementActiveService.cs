
namespace AnnouncementWebApp.Services;

/// <summary>
/// Сервіс для відстеження активних оголошень.
/// </summary>
public class AnnouncementActiveService
{
    readonly TimeSpan checkInterval;
    /// <summary>
    /// Ініціалізує новий екземпляр класу AnnouncementActiveService зі списком оголошень
    /// та інтервалом часу для перевірки та оновлення активності оголошень.
    /// </summary>
    /// <param name="checkInterval">Інтервал часу для перевірки та оновлення активності.</param>
    public AnnouncementActiveService(TimeSpan checkInterval)
    {
        this.checkInterval = checkInterval;
    }

    /// <summary>
    /// Запускає фонову задачу, яка безперервно перевіряє та оновлює активність оголошень.
    /// </summary>
    public void Start()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    DatabaseContext db = new DatabaseContext();
                    var now = DateTime.Now;
                    
                    var announcements = db.Announcements;

                    var activeAnnouncements = announcements.Where(announcement => now >= announcement.StartDate && now <= announcement.EndDate).ToList();

                    var notActiveAnnouncements = announcements.Where(announcement => now < announcement.StartDate || now > announcement.EndDate).ToList();

                    // Проходимося по всіх оголошеннях
                    foreach(var announcement in activeAnnouncements)
                    {
                        announcement.Active = true;
                    }
                    foreach(var announcement in notActiveAnnouncements)
                    {
                        announcement.Active = false;
                    }

                    // Виводимо інформацію про активні оголошення
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("AnnouncementActiveService");
                    Console.ResetColor();
                    Console.WriteLine($": Active announcements have been checked. \nTime {now}. Active: {activeAnnouncements.Count}/{announcements.Count()}.\n");

                    db.SaveChanges();

                    // Затримуємо виконання циклу на вказану тривалість
                    await Task.Delay(checkInterval);
                }
                catch (Exception)
                { }
            }
        });
    }
}
