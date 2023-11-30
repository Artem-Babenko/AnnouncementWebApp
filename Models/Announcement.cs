using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AnnouncementWebApp.Models;
/// <summary>
/// Представляє оголошення, яке використовується для відображення на сайті.
/// </summary>
public class Announcement
{
    /// <summary>
    /// Індетифікатор оголошення.
    /// </summary>
    [Key]public string Id { get; set; }

    /// <summary>
    /// Отримує або задає посилання на фото оголошення.
    /// </summary>
    public string PhotoLink { get; set; }

    /// <summary>
    /// Отримує або задає назву оголошення.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Отримує або задає короткий опис оголошення.
    /// </summary>
    public string ShortDescription { get; set; }

    /// <summary>
    /// Отримує або задає повний опис оголошення.
    /// </summary>
    public string FullDescription { get; set; }

    /// <summary>
    /// Отримує або задає власника оголошення.
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// Отримує або задає індетифікатор власника оголошення.
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Отримує або задає тег оголошення.
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Отримує або задає дату початку оголошення.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Початкова дата, конвертована у формат "dd.mm.yy". Для виведення у форматі JSON.
    /// </summary>
    public string StartDateString { get; set; }

    /// <summary>
    /// Отримує або задає дату закінчення оголошення.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Кінцева дата, конвертована у формат "dd.mm.yy". Для виведення у форматі JSON.
    /// </summary>
    public string EndDateString { get; set; }

    /// <summary>
    /// Отримує або задає контакт під назвою "Telegram".
    /// </summary>
    public string Telegram { get; set; }

    /// <summary>
    /// Отримує або задає контакт під назвою "Пошта".
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Отримує або задає контакт під назвою "Адреса".
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Отримує або задає контакт під назвою "Телефон".
    /// </summary>
    public string Phone { get; set; }


    /// <summary>
    /// Отримує або задає кількість переглядів оголошення.
    /// </summary>
    public int Views { get; set; }

    /// <summary>
    /// Отримує або задає True, якщо оголошення активне.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Ініціалізує новий об'єкт оголошення з вказаними даними.
    /// </summary>
    /// <param name="id">Унікальний ідентифікатор оголошення.</param>
    /// <param name="photoLink">Посилання на фото оголошення.</param>
    /// <param name="name">Назва оголошення.</param>
    /// <param name="shortDescription">Короткий опис оголошення.</param>
    /// <param name="fullDescription">Докладний опис оголошення.</param>
    /// <param name="owner">Власник оголошення.</param>
    /// <param name="ownerId">Унікальний ідентифікатор власника.</param>
    /// <param name="tag">Тег, пов'язаний з оголошенням.</param>
    /// <param name="startDate">Дата початку оголошення.</param>
    /// <param name="endDate">Дата закінчення оголошення.</param>
    /// <param name="telegram">Контактна інформація для зв'язку через Telegram.</param>
    /// <param name="email">Контактна інформація для зв'язку через email.</param>
    /// <param name="address">Адреса, пов'язана з оголошенням.</param>
    /// <param name="phone">Номер телефону, пов'язаний з оголошенням.</param>
    /// <param name="views">Кількість переглядів оголошення (за замовчуванням 0).</param>
    /// <returns>Об'єкт оголошення з вказаними даними.</returns>
    public Announcement(string id, string photoLink, string name, string shortDescription, string fullDescription, string owner, string ownerId, string tag, DateTime startDate, DateTime endDate, string telegram, string email, string address, string phone, int views = 0)
    {
        Id = id;
        PhotoLink = photoLink;
        Name = name;
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        Owner = owner;
        OwnerId = ownerId;
        Tag = tag;
        StartDate = startDate;
        EndDate = endDate + TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59));
        Telegram = telegram;
        Email = email;
        Address = address;
        Phone = phone;
        Views = views;
        StartDateString = DateTimeToStringDate(startDate);
        EndDateString = DateTimeToStringDate(endDate);
        Active = AnnouncementIsActive(startDate, endDate);
    }

#pragma warning disable CS8618
    private Announcement() { }
#pragma warning restore CS8618 


    /// <summary>
    /// Парсує рядок у форматі "dd.mm.yy" і повертає відповідний об'єкт DateTime.
    /// </summary>
    /// <param name="date">Рядок, що містить дату у форматі "dd.mm.yy".</param>
    /// <returns>Об'єкт DateTime, що представляє розпарсену дату.</returns>
    /// <exception cref="ArgumentException">Викидається, якщо рядок не відповідає вказаному формату.</exception>
    public static DateTime StringDateToDateTime(string date)
    {
        DateTime dateTime;
        if (DateTime.TryParseExact(date, "dd.MM.yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
        {
            // Парсинг пройшов успішно
            return dateTime;
        }
        else
        {
            // Помилка парсингу, обробити помилку або повернути значення за замовчуванням
            throw new ArgumentException("Невірний формат дати. Очікується формат: dd.mm.yy");
        }
    }

    /// <summary>
    /// Конвертує об'єкт DateTime у рядок у форматі "dd.mm.yy".
    /// </summary>
    /// <param name="dateTime">Об'єкт DateTime для конвертації.</param>
    /// <returns>Рядок, що представляє дату у форматі "dd.mm.yy".</returns>
    public static string DateTimeToStringDate(DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yy");
    }

    /// <summary>
    /// Визначає, чи активне оголошення на підставі дат початку і закінчення.
    /// </summary>
    /// <param name="startDate">Дата початку оголошення.</param>
    /// <param name="endDate">Дата закінчення оголошення.</param>
    /// <returns>
    /// true, якщо оголошення активне у поточний момент часу і знаходиться між датами початку і закінчення;
    /// в іншому випадку - false.
    /// </returns>
    public static bool AnnouncementIsActive(DateTime startDate, DateTime endDate)
    {
        DateTime now = DateTime.Now;
        return now >= startDate && now <= endDate;
    }
}