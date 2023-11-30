using System;
using System.ComponentModel.DataAnnotations;

namespace AnnouncementWebApp.Models;

/// <summary>
/// Представляє об'єкт користувача.
/// </summary>
public class User
{
    /// <summary>
    /// Індетифікатор користувача.
    /// </summary>
    [Key] public string Id { get; set; }

    /// <summary>
    /// Отримує або задає ім'я користувача.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Отримує або задає прізвище користувача.
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Отримує або задає електронну пошту користувача.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Отримує або задає пароль користувача.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Отримує або задає роль користувача в системі.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Кількісь переглядів які зробив користувач.
    /// </summary>
    public int Views { get; set; }

    /// <summary>
    /// Дата реєстрації користувача.
    /// </summary>
    public string RegistrationDate { get; set; }

#pragma warning disable CS8618
    private User() { }
#pragma warning restore CS8618 

    /// <summary>
    /// Ініціалізує новий об'єкт користувача з вказаними даними.
    /// </summary>
    /// <param name="name">Ім'я користувача.</param>
    /// <param name="surname">Прізвище користувача.</param>
    /// <param name="email">Електронна пошта користувача.</param>
    /// <param name="password">Пароль користувача.</param>
    /// <param name="role">Роль користувача в системі.</param>
    public User(string id, string name, string surname, string email, string password, string role, DateTime registrationDate, int views = 0)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
        Role = role;
        RegistrationDate = registrationDate.ToString("dd.MM.yy"); ;
        Views = views;
    }
}