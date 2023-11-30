
namespace AnnouncementWebApp.Models;

/// <summary>
/// Представляє тег.
/// </summary>
public class Tag
{
    /// <summary>
    /// Отримує або задає назву тегу.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Ініціалізує новий об'єкт тегу з вказаною назвою.
    /// </summary>
    /// <param name="name">Назва тегу.</param>
    public Tag(string name)
    {
        Name = name;
    }
}