
using System.ComponentModel.DataAnnotations;

namespace AnnouncementWebApp.Models;

/// <summary>
/// Представляє тимчасову фотографію з інформацією про шлях та час створення.
/// </summary>
public class TemporaryPhoto
{
    /// <summary>
    /// Отримує або задає шлях до тимчасової фотографії.
    /// </summary>
    [Key] public string Path { get; set; }

    /// <summary>
    /// Отримує або задає час створення тимчасової фотографії.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Ініціалізує новий екземпляр класу TemporaryPhoto з вказаними параметрами.
    /// </summary>
    /// <param name="path">Шлях до фотографії.</param>
    /// <param name="creationTime">Час створення фотографії.</param>
    public TemporaryPhoto(string path, DateTime creationTime)
    {
        Path = path;
        CreationTime = creationTime;
    }
}

